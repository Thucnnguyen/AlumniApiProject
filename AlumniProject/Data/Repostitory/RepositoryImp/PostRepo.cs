using AlumniProject.Entity;

namespace AlumniProject.Data.Repostitory.RepositoryImp;

public class PostRepo : RepositoryBase<Post>, IPostRepo
{
    public PostRepo(AlumniDbContext context) : base(context)
    {
    }
}
