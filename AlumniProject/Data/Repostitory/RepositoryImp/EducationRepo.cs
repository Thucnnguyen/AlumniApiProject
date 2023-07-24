using AlumniProject.Entity;

namespace AlumniProject.Data.Repostitory.RepositoryImp
{
    public class EducationRepo : RepositoryBase<Education>, IEducationRepo
    {
        public EducationRepo(AlumniDbContext context) : base(context)
        {
        }
    }
}
