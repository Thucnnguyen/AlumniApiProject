using AlumniProject.Entity;

namespace AlumniProject.Data.Repostitory.RepositoryImp;

public class EventParticipantRepo : RepositoryBase<EventParticipant>, IEventParticipantRepo
{
    public EventParticipantRepo(AlumniDbContext context) : base(context)
    {
    }
}
