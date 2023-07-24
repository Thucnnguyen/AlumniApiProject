using AlumniProject.Dto;
using AlumniProject.Entity;

namespace AlumniProject.Service
{
    public interface IEventService
    {
        Task<int> CreateEvent(Events events);
        Task<Events> UpdateEvents(Events events);
        Task DeleteEvents(int id, int schoolId);
        Task<PagingResultDTO<Events>> GetEventsByAlumniId(int pageNo, int pageSize, int alumniId, int schoolId);
        Task<PagingResultDTO<Events>> GetEventsBySchoolIdIdWithoutCondition(int pageNo, int pageSize, int schoolId);
        //Task<PagingResultDTO<Events>> GetEventsBySchoolIdIdWithCondition(int pageNo, int pageSize, int schoolId);

        Task<Events> GetEventsById(int eventsId);
        Task<IEnumerable<Events>> GetLatestEvent(int size, int alumniId, int schoolId);
        Task<IEnumerable<Events>> GetLatestEvent(int size, int schoolId);

        Task<PagingResultDTO<Events>> GetEventParticipant(int pageNo, int pageSize, int alumniId);

    }
}
