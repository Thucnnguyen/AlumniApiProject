using AlumniProject.Data.Repostitory;
using AlumniProject.Dto;
using AlumniProject.Entity;
using AlumniProject.ExceptionHandler;
using System.Security.Cryptography.X509Certificates;

namespace AlumniProject.Service.ServiceImp
{
    public class fundService : IFundService
    {
        private readonly Lazy<ISchoolService> schoolService;
        private readonly IFundRepo fundRepo;
        public fundService(Lazy<ISchoolService> schoolService, IFundRepo fundRepo)
        {
            this.schoolService = schoolService;
            this.fundRepo = fundRepo;
        }

        public async Task<int> CreateFund(Fund fund)
        {
            var school = await schoolService.Value.GetSchoolById(fund.schoolId);
            if (school == null)
            {
                throw new NotFoundException("School not found with ID: " + fund.schoolId);
            }
            var fundId = await fundRepo.CreateAsync(fund);
            return fundId;
        }

        public async Task DeleteFund(int id)
        {
            Fund fund = await GetFundById(id);
            fund.Archived = false;
            await fundRepo.UpdateAsync(fund);
        }

        public Task<PagingResultDTO<Fund>> GetAllFundBySchoolId(int pageNo, int pageSize, int schoolId)
        {
            var FundList = fundRepo.GetAllByConditionAsync(pageNo, pageSize, f => f.schoolId == schoolId && f.Archived == true);
            return FundList;
        }

        public async Task<Fund> GetFundById(int id)
        {
            var fund = await fundRepo.FindOneByCondition(f => f.Id == id && f.Archived == true);
            if (fund == null)
            {
                throw new NotFoundException("Fund not found with id: " + id);
            }
            return fund;
        }

        public async Task<Fund> UpdateFund(Fund updatedFund)
        {
            Fund fund = await GetFundById(updatedFund.Id);
            fund.Title = updatedFund.Title;
            fund.TargetBalance = updatedFund.TargetBalance;
            fund.BackgroundImage = updatedFund.BackgroundImage;
            fund.Description = updatedFund.Description;
            fund.StartTime = updatedFund.StartTime;
            fund.EndTime = updatedFund.EndTime;
            await fundRepo.UpdateAsync(fund);
            return fund;
        }
    }
}
