using AlumniProject.Entity;

namespace AlumniProject.Data.Repostitory.RepositoryImp;

public class EventRepo : RepositoryBase<Events>, IEventRepo
{
    public EventRepo(AlumniDbContext context) : base(context)
    {
    }
}
