using AlumniProject.Entity;

namespace AlumniProject.Service
{
    public interface IEducationService 
    {
        Task<int> CreateEducation(Education education);
        Task<Education> UpdateEducation(Education education);
        Task<IEnumerable<Education>> GetEducationByAlumniId(int alumniId);
        Task DeleteEducation(int id);
    }
}
