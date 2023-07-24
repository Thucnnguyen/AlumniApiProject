using AlumniProject.Entity;

namespace AlumniProject.Service;

public interface IMajorService
{
    Task<int> CreateMajor(Major major);
    Task<Major> GetMajorById(int id);
    Task<Major> UpdateMajor(Major major);
    Task DeleteMajor(int id);
    Task<IEnumerable<Major>> GetMajorByAlumniId(int alumniId);
}
