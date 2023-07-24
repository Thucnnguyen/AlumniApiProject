using AlumniProject.Entity;

namespace AlumniProject.Data.Repostitory.RepositoryImp
{
    public class TagNewRepo : RepositoryBase<TagsNew>, ITagnewRepo
    {
        public TagNewRepo(AlumniDbContext context) : base(context)
        {
        }
    }
}
