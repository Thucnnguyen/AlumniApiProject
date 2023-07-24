using AlumniProject.Dto;
using AlumniProject.Entity;

namespace AlumniProject.Service
{
    public interface IEventParticipantService
    {
        Task<int> createEventParticipant(EventParticipant eventParticipant);
        Task<bool> isAccess(int eventId,int alumniId);
        Task DeleteEventParticipant(int EventId ,int alumniId);
        Task<PagingResultDTO<EventParticipant>> GetEventParticipantByAlumniId(int pageNo,int pageSize,int alumniId);

    }
}
