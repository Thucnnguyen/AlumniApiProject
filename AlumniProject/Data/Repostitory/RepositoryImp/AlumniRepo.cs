using AlumniProject.Entity;
using Microsoft.EntityFrameworkCore;

namespace AlumniProject.Data.Repostitory.RepositoryImp;

public class AlumniRepo : RepositoryBase<Alumni> , IAlumniRepo
{
    public AlumniRepo(AlumniDbContext alumniDbContext):base(alumniDbContext)
    {

    }

    
    
}
