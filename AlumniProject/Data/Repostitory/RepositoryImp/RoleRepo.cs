using AlumniProject.Entity;

namespace AlumniProject.Data.Repostitory.RepositoryImp;

public class RoleRepo : RepositoryBase<Role>, IRoleRepo
{
    public RoleRepo(AlumniDbContext context) : base(context)
    {
    }
}
