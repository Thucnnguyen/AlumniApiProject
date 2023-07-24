using AlumniProject.Entity;

namespace AlumniProject.Data.Repostitory.RepositoryImp
{
    public class CardItemRepo : RepositoryBase<CardInSchool>, ICardItem
    {
        public CardItemRepo(AlumniDbContext context) : base(context)
        {
        }
    }
}
