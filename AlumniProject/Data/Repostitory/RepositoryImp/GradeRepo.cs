using AlumniProject.Entity;

namespace AlumniProject.Data.Repostitory.RepositoryImp;

public class GradeRepo : RepositoryBase<Grade>, IGradeRepo
{
    public GradeRepo(AlumniDbContext context) : base(context)
    {
    }
}
