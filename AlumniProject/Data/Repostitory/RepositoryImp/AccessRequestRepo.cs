using AlumniProject.Entity;

namespace AlumniProject.Data.Repostitory.RepositoryImp;

public class AccessRequestRepo : RepositoryBase<AccessRequest>, IAccessRequestRepo
{
    public AccessRequestRepo(AlumniDbContext context) : base(context)
    {
    }
}
