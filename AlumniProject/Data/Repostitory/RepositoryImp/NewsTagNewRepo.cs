using AlumniProject.Entity;

namespace AlumniProject.Data.Repostitory.RepositoryImp
{
    public class NewsTagNewRepo : RepositoryBase<NewsTagNew>, INewsTagNewRepo
    {
        public NewsTagNewRepo(AlumniDbContext context) : base(context)
        {
        }
    }
}
