using AlumniProject.Entity;

namespace AlumniProject.Data.Repostitory.RepositoryImp
{
    public class FundRepo : RepositoryBase<Fund>, IFundRepo
    {
        public FundRepo(AlumniDbContext context) : base(context)
        {
        }
    }
}
