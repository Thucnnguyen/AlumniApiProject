using AlumniProject.Dto;
using AlumniProject.Entity;

namespace AlumniProject.Service
{
    public interface IFundService
    {
        Task<int> CreateFund(Fund fund);
        Task<Fund> UpdateFund(Fund fund);
        Task DeleteFund(int id);
        Task<Fund> GetFundById(int id);
        Task<PagingResultDTO<Fund>> GetAllFundBySchoolId(int pageNo, int pageSize, int schoolId);
    }
}
