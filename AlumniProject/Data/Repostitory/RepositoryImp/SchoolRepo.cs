using AlumniProject.Entity;

namespace AlumniProject.Data.Repostitory.RepositoryImp;

public class SchoolRepo : RepositoryBase<School>, ISchoolRepo
{
    public SchoolRepo(AlumniDbContext context) : base(context)
    {
    }
}
