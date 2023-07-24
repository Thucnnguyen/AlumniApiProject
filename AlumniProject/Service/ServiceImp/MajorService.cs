using AlumniProject.Data.Repostitory;
using AlumniProject.Entity;
using AlumniProject.ExceptionHandler;

namespace AlumniProject.Service.ServiceImp
{
    public class MajorService : IMajorService
    {
        private readonly IMajorRepo repo;
        private readonly IAlumniService alumniService;
        public MajorService(IMajorRepo repo, IAlumniService alumniService)
        {
            this.repo = repo;
            this.alumniService = alumniService;
        }

        public async Task<int> CreateMajor(Major major)
        {
            var majorId = await repo.CreateAsync(major);
            return majorId;
        }

        public async Task DeleteMajor(int id)
        {
            Major deletedMajor = await GetMajorById(id);
            deletedMajor.Archived = false;
            await repo.UpdateAsync(deletedMajor);
        }

        public async Task<IEnumerable<Major>> GetMajorByAlumniId(int alumniId)
        {
            var alumni= await alumniService.GetById(alumniId);
            if(alumni == null)
            {
                throw new NotFoundException("Major not found with Id: " + alumniId);
            }
            var majorsList = await repo.GetAllByConditionAsync(m => m.AlumniId == alumniId && m.Archived == true);
            return majorsList;
        }

        public async Task<Major> GetMajorById(int id)
        {
            var major = await repo.FindOneByCondition(m => m.Id == id,m => m.Archived==true);
            if (major == null)
            {
                throw new NotFoundException("Major not found with Id: "+ id);
            }
            return major;
        }

        public async Task<Major> UpdateMajor(Major updateMajor)
        {
            var major = await GetMajorById(updateMajor.Id);
            major.JobTitle = updateMajor.JobTitle;
            major.StartDate = major.StartDate;
            major.EndDate = major.EndDate;
            major.Company = major.Company;
            await repo.UpdateAsync(major);
            return major;
        }
    }
}
