using AlumniProject.Data.Repostitory;
using AlumniProject.Dto;
using AlumniProject.Entity;
using AlumniProject.ExceptionHandler;
using AlumniProject.Ultils;
using Microsoft.Extensions.Logging;

namespace AlumniProject.Service.ServiceImp
{
    public class SchoolService : ISchoolService
    {
        private readonly ISchoolRepo repository;
        private readonly ICardItem cardItemRepo;
        private readonly IAttributeRepo attributeRepo;

        private readonly Lazy<IAlumniService> alumniService;
        private readonly RedisService _redisService;

        public SchoolService(ISchoolRepo schoolRepo, Lazy<IAlumniService> alumniService, RedisService redisService, ICardItem cardItem, IAttributeRepo attributeRepo)
        {
            this.repository = schoolRepo;
            this.alumniService = alumniService;
            _redisService = redisService;
            this.cardItemRepo = cardItem;
            this.attributeRepo = attributeRepo;
        }
        public async Task<int> AddSchool(School school)
        {
            var ExistName = await repository.FindOneByCondition(s => s.Name == school.Name);
            if (ExistName != null)
            {
                throw new ConflictException("School's Name is existed");
            }
            else
            {
                var ExistSubdomain = await repository.FindOneByCondition(s => s.SubDomain == school.SubDomain);
                if (ExistSubdomain != null)
                {
                    throw new ConflictException("School's Subdomain is existed");
                }
                else
                {
                    school.StartTime = DateTime.Now;
                    school.EndTime = DateTime.Now.AddMonths(school.Duration);
                    int NewSchoolId = await repository.CreateAsync(school);
                    return NewSchoolId;
                }
            }
        }

        public async Task<Dictionary<int, int>> CalculateStaticInrange(DateTime from, DateTime to)
        {
            Dictionary<int, int> RequestCountsByMonth = new Dictionary<int, int>();
            var listSchool = await repository.GetAllByConditionAsync(s => from <= s.CreatedAt && s.CreatedAt <= to);
            foreach (var school in listSchool)
            {
                int month = school.CreatedAt.Month;
                if (RequestCountsByMonth.ContainsKey(month))
                {
                    RequestCountsByMonth[month]++;
                }
                else
                {
                    RequestCountsByMonth.Add(month, 1);
                }
            }
            return RequestCountsByMonth;
        }

        public async Task<bool> CheckRequestExistByAlumniId(int schoolId)
        {
            var school = await GetSchoolById(schoolId);
            if(school == null)
            {
                throw new NotFoundException("");
            }
            return school.RequestStatus.Equals(StatusEnum.processing);
        }
        public async Task<bool> CheckHasAnyRequestAcceptByAlumniId(int schoolId)
        {
            var school = await GetSchoolById(schoolId);
            if (school == null)
            {
                throw new NotFoundException("You not have any request");
            }
            return school.RequestStatus == (int)StatusEnum.accept;
        }
        public async Task<int> CountSchoolRequestInRange(DateTime from, DateTime to)
        {
            var count = await repository.CountByCondition(s => from <= s.CreatedAt && s.CreatedAt <= to);
            return count;
        }

        public async Task DeleteSchool(School deletedSchool)
        {
            School school = await GetSchoolById(deletedSchool.Id);
            school.Archived = false;
            await repository.UpdateAsync(school);
        }

        public async Task<School> GetSchoolById(int schoolId)
        {
            var school = await repository.GetByIdAsync(s => s.Id == schoolId, s => s.Archived == true);
            return school;
        }

        public async Task<School> GetSchoolBySubDomain(string schoolSubDomain)
        {
            var school = await repository.FindOneByCondition(s => s.SubDomain == schoolSubDomain && s.Archived == true && s.RequestStatus == 2);
            var card = await cardItemRepo.GetAllByConditionAsync(c => c.schoolId == school.Id);
            var attribute = await attributeRepo.GetAllByConditionAsync(c => c.schoolId == school.Id);
            school.CardInSchool = card.ToList();
            school.Attribute = attribute.ToList();
            return school;
        }

        public async Task<PagingResultDTO<School>> GetSchools(int pageNo, int pagesize)
        {
            var schoolList = await repository.GetAllByConditionAsync(pageNo, pagesize, s => s.Archived == true);
            return schoolList;
        }



        public async Task<bool> IsExistedSchool(int schoolId)
        {
            var existedSchool = await GetSchoolById(schoolId);
            if (existedSchool != null)
            {
                return true;
            }
            return false;
        }

        public async Task<School> UpdateSchool(School school)
        {
            School schoolToUpdate = await repository.FindOneByCondition(s => s.Id == school.Id && s.RequestStatus == 2 && s.Archived == true);
            if (schoolToUpdate != null)
            {
                schoolToUpdate.Name = school.Name;
                schoolToUpdate.Description = school.Description;
                schoolToUpdate.Theme = school.Theme;
                schoolToUpdate.BackGround1 = school.BackGround1;
                schoolToUpdate.BackGround2 = school.BackGround2;
                schoolToUpdate.BackGround3 = school.BackGround3;
                schoolToUpdate.ProvinceName = school.ProvinceName;
                schoolToUpdate.CityName = school.CityName;
                schoolToUpdate.Address = school.Address;
                schoolToUpdate.Icon = school.Icon;
                schoolToUpdate.SubDomain = school.SubDomain;
                await repository.UpdateAsync(schoolToUpdate);
                return schoolToUpdate;
            }
            else
            {
                throw new NotFoundException("School not found with ID:" + school.Id);
            }
        }

        public async Task<School> UpdateSchoolStatus(int schoolId, int status)
        {
            School schoolToUpdate = await repository.FindOneByCondition(s => s.Id == schoolId && s.Archived == true);
            if (schoolToUpdate != null)
            {
                schoolToUpdate.RequestStatus = status;
                schoolToUpdate.EndTime = DateTime.Now.AddMonths(schoolToUpdate.Duration);
                await repository.UpdateAsync(schoolToUpdate);
                if (status == (int)StatusEnum.accept)
                {
                    Alumni tenant = await alumniService.Value.GetTenantBySchoolId(schoolId);
                    if (tenant != null)
                    {
                        tenant.RoleId = (int)RoleEnum.tenant;
                        await alumniService.Value.UpdateAlumni(tenant);
                        var schoolEmail = new SchoolEmailDTO();
                        schoolEmail.SchoolName = schoolToUpdate.Name;
                        schoolEmail.Email = tenant.Email;
                        schoolEmail.Status = status;
                        _redisService.PublishMessage<SchoolEmailDTO>("SchoolRequestQueue", schoolEmail);
                    }

                }
                else if (status == (int)StatusEnum.deny)
                {
                    Alumni tenant = await alumniService.Value.GetTenantBySchoolId(schoolId);
                    if (tenant != null)
                    {
                        tenant.IsOwner = false;
                        tenant.schoolId = null;
                        await alumniService.Value.UpdateAlumni(tenant);
                        var schoolEmail = new SchoolEmailDTO();
                        schoolEmail.SchoolName = schoolToUpdate.Name;
                        schoolEmail.Email = tenant.Email;
                        schoolEmail.Status = status;
                        _redisService.PublishMessage<SchoolEmailDTO>("SchoolRequestQueue", schoolEmail);
                    }
                }
                return schoolToUpdate;
            }
            else
            {
                throw new NotFoundException("School not found with ID:" + schoolId);
            }
        }

        public async Task<int> CountSchoolRequestInRangeAndStatus(DateTime from, DateTime to, int status)
        {
            var count = await repository.CountByCondition(s => from <= s.CreatedAt && s.CreatedAt <= to && s.RequestStatus == status);
            return count;
        }
    }
}
