using AlumniProject.Entity;

namespace AlumniProject.Data.Repostitory.RepositoryImp;

public class MajorRepo : RepositoryBase<Major>, IMajorRepo
{
    public MajorRepo(AlumniDbContext context) : base(context)
    {
    }
}
