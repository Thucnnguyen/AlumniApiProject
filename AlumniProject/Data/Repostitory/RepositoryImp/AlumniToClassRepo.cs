using AlumniProject.Entity;

namespace AlumniProject.Data.Repostitory.RepositoryImp
{
    public class AlumniToClassRepo : RepositoryBase<AlumniToClass>, IAlumniToClassRepo
    {
        public AlumniToClassRepo(AlumniDbContext context) : base(context)
        {
        }
    }
}
