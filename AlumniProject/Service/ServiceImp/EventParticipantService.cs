using AlumniProject.Data.Repostitory;
using AlumniProject.Dto;
using AlumniProject.Entity;
using AlumniProject.ExceptionHandler;

namespace AlumniProject.Service.ServiceImp
{
    public class EventParticipantService : IEventParticipantService
    {
        private readonly Lazy<IAlumniService> _alumniService;
        private readonly Lazy<IEventService> _eventService;
        private readonly IEventParticipantRepo _eventParticipantRepo;
        public EventParticipantService(Lazy<IAlumniService> alumniService, Lazy<IEventService> eventServic, IEventParticipantRepo eventParticipantRepo)
        {
            _alumniService = alumniService;
            _eventService = eventServic;
            _eventParticipantRepo = eventParticipantRepo;
        }

        public async Task<int> createEventParticipant(EventParticipant eventParticipant)
        {
            var alumni = await _alumniService.Value.GetById(eventParticipant.AlumniId);
            if (alumni == null)
            {
                throw new NotFoundException("Alumni not found with id: " + eventParticipant.AlumniId);
            }
            var events = await _eventService.Value.GetEventsById(eventParticipant.EventId);
            if (events == null)
            {
                throw new NotFoundException("Alumni not found with id: " + eventParticipant.EventId);
            }
            var check = await isAccess(eventParticipant.EventId, eventParticipant.AlumniId);
            if (check)
            {
                throw new ConflictException("Alumni is access event");
            }
            var eventParticipantId = await _eventParticipantRepo.CreateAsync(eventParticipant);
            return eventParticipantId;
        }

        public async Task DeleteEventParticipant(int EventId, int alumniId)
        {
            EventParticipant eventParticipant = await _eventParticipantRepo.FindOneByCondition(e => e.EventId == EventId && e.AlumniId == alumniId && e.Archived == true);
            if(eventParticipant != null)
            {
                eventParticipant.Archived = false;
                await _eventParticipantRepo.UpdateAsync(eventParticipant);
            }
        }

        public async Task<PagingResultDTO<EventParticipant>> GetEventParticipantByAlumniId(int pageNo,int pageSize,int alumniId)
        {
            PagingResultDTO<EventParticipant> eventParticipant = await _eventParticipantRepo.GetAllByConditionAsync(pageNo,pageSize,e => e.AlumniId == alumniId && e.Archived == true);
            return eventParticipant;
        }

        public async Task<bool> isAccess(int eventId, int alumniId)
        {
            var eventParticipant = await _eventParticipantRepo.FindOneByCondition(ep => ep.EventId == eventId && ep.AlumniId == alumniId && ep.Archived == true);
            if(eventParticipant == null)
            {
                return false;
            }
            return true; 
        }
    }
}
