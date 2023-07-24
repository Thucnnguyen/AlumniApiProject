using AlumniProject.Entity;

namespace AlumniProject.Data.Repostitory.RepositoryImp;

public class AlumniClassRepo : RepositoryBase<AlumniClass>, IAlumniClassRepo
{
    public AlumniClassRepo(AlumniDbContext context) : base(context)
    {
    }
}
