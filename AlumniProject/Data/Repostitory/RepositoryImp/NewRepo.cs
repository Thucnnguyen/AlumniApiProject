using AlumniProject.Entity;

namespace AlumniProject.Data.Repostitory.RepositoryImp;

public class NewRepo : RepositoryBase<News>, INewRepo
{
    public NewRepo(AlumniDbContext context) : base(context)
    {
    }
}
