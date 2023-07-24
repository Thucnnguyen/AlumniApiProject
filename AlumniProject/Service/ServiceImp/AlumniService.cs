using AlumniProject.Data.Repostitory;
using AlumniProject.Dto;
using AlumniProject.Entity;
using AlumniProject.ExceptionHandler;

namespace AlumniProject.Service.ServiceImp
{
    public class AlumniService : IAlumniService
    {

        private readonly IAlumniRepo _repo;
        private readonly Lazy<IAlumniToClassService> _alumniToClassService;
        public AlumniService(IAlumniRepo repo,Lazy<IAlumniToClassService> alumniToClass )
        {
            _repo = repo;
            _alumniToClassService = alumniToClass;
        }

        public async Task<int> AddAlumni(Alumni newAlumni)
        {
            var alumni = await _repo.FindOneByCondition(a => a.Email == newAlumni.Email && a.Archived == true);
            if (alumni != null)
            {
                throw new ConflictException("Alumni was existed with email: "+newAlumni.Email);
            }
            var newAlumniId = await _repo.CreateAsync(newAlumni);
            return newAlumniId;
        }

        public async Task<Dictionary<int, int>> CalculateAccessCountsByHour(DateTime from, DateTime to)
        {
            Dictionary<int, int> accessCountsByHour = new Dictionary<int, int>();
            var listAlumni = await _repo.GetAllByConditionAsync(a => from <= a.LastLogin && a.LastLogin <= to);
            foreach(var alumni in listAlumni)
            {
                int hour = alumni.LastLogin.Hour;
                if (accessCountsByHour.ContainsKey(hour))
                {
                    accessCountsByHour[hour]++;
                }
                else
                {
                    accessCountsByHour.Add(hour, 1);
                }
            }
            return accessCountsByHour;
        }

        public async Task<int> CountLastLoginInRage(DateTime from, DateTime to)
        {
            var count = await _repo.CountByCondition(a => from <= a.LastLogin &&  a.LastLogin <= to);
            return count;
        }

        public async Task<bool> DeleteById(int id)
        {
            var existAlumni = GetById(id);
            if(existAlumni == null)
            {
                return false;
            }
             await _repo.DeleteByIdAsync(id);
            return true;
        }

        public async Task<PagingResultDTO<Alumni>> GetAll(int pageNo, int pageSize)
        {
            var AlumniList = await _repo.GetAllByConditionAsync(pageNo,pageSize,null);
            return AlumniList;
        }

        public async Task<PagingResultDTO<Alumni>> SearchAlumniByEmailOrNameOrPhoneAndClassId(int pageNo, int pageSize, string searchText, int classid)
        {
            var alumni = await _alumniToClassService.Value.GetAlumniByClassId(classid);
            var alumniList = alumni.Where(a => a.Email.ToLower().Contains(searchText.Trim().ToLower()) || a.Phone.ToLower().Contains(searchText.Trim().ToLower()) || a.FullName.ToLower().Contains(searchText.Trim().ToLower()));
            var alumniListResult = alumniList.Skip((pageNo - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            var alumniListDto = new PagingResultDTO<Alumni>()
            {
                CurrentPage = pageNo,
                Items = alumniListResult,
                PageSize = pageSize,
                TotalItems = alumniList.Count()
            };

            return alumniListDto;
        }

        public async Task<Alumni> GetAlumniByEmail(string email)
        {
            var alumni = await _repo.FindOneByCondition(a => a.Email == email && a.Archived == true);
            if (alumni == null)
            {
                throw new NotFoundException("Alumni not found with email: " + email);
            }
            return alumni;
        }

        public async Task<PagingResultDTO<Alumni>> GetAlumniBySchoolId(int pageNo,int pageSize, int schoolId)
        {
            var alumni = await _repo.GetAllByConditionAsync(pageNo, pageSize, a => a.schoolId == schoolId && a.Archived == true);
            return alumni;
        }

        public async Task<Alumni> GetById(int id)
        {
            var alumni = await _repo.GetByIdAsync(a => a.Id == id, a=> a.Archived == true);
            if(alumni == null)
            {
                throw new NotFoundException("Alumni not found with id: " + id);
            }
            return alumni;
        }

        public async Task<Alumni> GetById(int id, int schoolId)
        {
            var alumni = await _repo.FindOneByCondition(a => a.schoolId == schoolId && a.Id == id);
            return alumni;
        }

        public async Task<Alumni> GetTenantBySchoolId(int schoolId)
        {
            var alumni = await _repo.FindOneByCondition(a => a.schoolId == schoolId && a.IsOwner == true && a.Archived == true);
            return alumni;
        }

        public async Task<PagingResultDTO<Alumni>> SearchAlumniByEmailOrNameOrPhone(int pageNo,int pageSize,string searchText, int schoolId)
        {
            var alumni = await _repo.GetAllByConditionAsync(pageNo, pageSize, a => (a.schoolId == schoolId && a.Archived == true) 
            && (a.Email.Contains(searchText.Trim()) || a.Phone.Contains(searchText.Trim()) || a.FullName.Contains(searchText.Trim())));
            return alumni;
        }

        public async Task<Alumni> UpdateAlumni(Alumni alumni)
        {

            Alumni updateAlumni = await GetById(alumni.Id);
            updateAlumni.Bio = alumni.Bio;
            updateAlumni.FaceBook_url = alumni.FaceBook_url;
            updateAlumni.FullName = alumni.FullName;
            updateAlumni.DateOfBirth = alumni.DateOfBirth;
            updateAlumni.Avatar_url = alumni.Avatar_url;
            updateAlumni.CoverImage_url = alumni.CoverImage_url;
            updateAlumni.Phone = alumni.Phone;
            updateAlumni.Bio = alumni.Bio;
            await _repo.UpdateAsync(updateAlumni);
            
            return updateAlumni;
        }
    }
}
