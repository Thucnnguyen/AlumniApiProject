using AlumniProject.Dto;
using AlumniProject.Entity;

namespace AlumniProject.Service;

public interface ISchoolService
{
    Task<bool> CheckRequestExistByAlumniId(int schoolId);   
    Task<bool> CheckHasAnyRequestAcceptByAlumniId(int schoolId);   
    Task<int> AddSchool(School school);
    Task<School> UpdateSchool(School school);
    Task<School> UpdateSchoolStatus(int schoolId,int status);
    Task DeleteSchool(School school);
    Task<School> GetSchoolById(int schoolId);
    Task<School> GetSchoolBySubDomain(string schoolSubDomain);

    Task<bool> IsExistedSchool(int id);
    Task<int> CountSchoolRequestInRange(DateTime from, DateTime to);
    Task<int> CountSchoolRequestInRangeAndStatus(DateTime from, DateTime to,int status);

    Task<Dictionary<int,int>> CalculateStaticInrange(DateTime from, DateTime to);


    Task<PagingResultDTO<School>> GetSchools(int pageNo,int pageSize);
}
    