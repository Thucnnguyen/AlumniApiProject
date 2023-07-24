using AlumniProject.Entity;

namespace AlumniProject.Data.Repostitory.RepositoryImp
{
    public class AttributeRepo : RepositoryBase<Attributes>, IAttributeRepo
    {
        public AttributeRepo(AlumniDbContext context) : base(context)
        {
        }
    }
}
