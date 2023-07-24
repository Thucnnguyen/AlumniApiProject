using AlumniProject.Dto;
using AlumniProject.Entity;

namespace AlumniProject.Service
{
    public interface IAlumniService
    {
        Task<PagingResultDTO<Alumni>> GetAll(int pageNo, int pageSize);
        Task<Alumni> GetById(int id);
        Task<Alumni> GetById(int id,int schoolId);
        Task<bool> DeleteById(int id);
        Task<Alumni> UpdateAlumni(Alumni alumni);
        Task<int> AddAlumni(Alumni alumni);
        Task<Alumni> GetAlumniByEmail(string email); 
        Task<Alumni> GetTenantBySchoolId(int schoolId);
        Task<PagingResultDTO<Alumni>> GetAlumniBySchoolId(int pageNo, int pageSize,int schoolId);
        Task<PagingResultDTO<Alumni>> SearchAlumniByEmailOrNameOrPhoneAndClassId(int pageNo, int pageSize, string searchText, int classid);
        Task<PagingResultDTO<Alumni>> SearchAlumniByEmailOrNameOrPhone(int pageNo,int pageSize,string searchText, int schoolId);
        Task<int> CountLastLoginInRage(DateTime from, DateTime to);
        Task<Dictionary<int, int>> CalculateAccessCountsByHour(DateTime from, DateTime to);
    }
}
