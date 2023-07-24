using AlumniProject.Dto;
using AlumniProject.Entity;

namespace AlumniProject.Service;

public interface IClassService 
{
    Task<int> CreateClass(AlumniClass alumniClass);
    Task<AlumniClass> GetClassById(int id);
    Task<IEnumerable<AlumniClass>> GetClassByGradeId(int gradeId);

    Task< AlumniClass> UpdateClass(AlumniClass alumniClass);
    Task<int> CountClassByGradeId(int GradeId);
    Task DeleteClassById(int Id,int SchoolId);

    Task<PagingResultDTO<AlumniClassDTO>> GetClassPagingResults(int pageNo,int pageSize,int GradeId);

}
