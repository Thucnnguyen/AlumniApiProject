using AlumniProject.Data.Repostitory;
using AlumniProject.Dto;
using AlumniProject.Entity;
using AlumniProject.ExceptionHandler;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Diagnostics;

namespace AlumniProject.Service.ServiceImp
{
    public class EventService : IEventService
    {
        private readonly Lazy<IAlumniService> _alumniService;
        private readonly Lazy<IClassService> _classService;
        private readonly Lazy<IGradeService> _gradeService;
        private readonly Lazy<IAlumniToClassService> _alumniToClassService;
        private readonly Lazy<IEventParticipantService> _eventParticipantService;
        private readonly RedisService _redisService;

        private IEventRepo _repo;
        public EventService(Lazy<IAlumniService> alumniService, IEventRepo repo,
            Lazy<IGradeService> gradeService,
            Lazy<IAlumniToClassService> alumniToClassService,
            Lazy<IClassService> classService,
            Lazy<IEventParticipantService> eventParticipantService, RedisService redisService)
        {
            _alumniService = alumniService;
            _repo = repo;
            _gradeService = gradeService;
            _alumniToClassService = alumniToClassService;
            _classService = classService;
            _eventParticipantService = eventParticipantService;
            _redisService = redisService;
        }

        public async Task<int> CreateEvent(Events events)
        {
            var alumni = await _alumniService.Value.GetById(events.HostId);
            if (alumni == null)
            {
                throw new NotFoundException("Alumni not found with id: " + events.HostId);
            }
            Grade grade = new Grade();
            if (events.GradeId.HasValue)
            {
                grade = await _gradeService.Value.GetGradeById(events.GradeId.Value);
                if (grade == null)
                {
                    throw new NotFoundException("Grade not found with id: " + events.GradeId);
                }
                events.SchoolId = grade.SchoolId;
            }
            var EventId = await _repo.CreateAsync(events);
            _redisService.PublishMessage<Events>("eventQueues", events);

            return EventId;
        }

        public async Task DeleteEvents(int id,int schoolId)
        {
            var events = await _repo.GetByIdAsync(e => e.Id == id && e.Archived == true && e.SchoolId == schoolId);
            if (events == null)
            {
                throw new NotFoundException("Event not found with id: " + id);
            }
            events.Archived = false;
            await _repo.UpdateAsync(events);
            _redisService.PublishMessage<Events>("eventQueues",events);


        }

        public async Task<Events> GetEventsById(int eventsId)
        {
            var cachedEvent = await _redisService.GetObjectAsync<Events>("Event:" + eventsId.ToString());
            if (cachedEvent != null)
            {
                return cachedEvent;
            }
            var events = await _repo.GetByIdAsync(e => e.Id == eventsId && e.Archived == true);
            if (events == null)
            {
                throw new NotFoundException("Event not found with id: " + eventsId);
            }
            await _redisService.SetObjectAsync<Events>("Event:" + events.Id, events);
            return events;
        }

        public async Task<PagingResultDTO<Events>> GetEventsByAlumniId(int pageNo, int pageSize, int alumniId, int schoolId)
        {
            var cachedEvent = await _redisService.GetObjectAsync<PagingResultDTO<Events>>("Event:alumniId:" + alumniId + ":" + pageNo + "" + pageSize);
            if (cachedEvent != null)
            {
                return cachedEvent;
            }
            var classId = await _alumniToClassService.Value.GetClassIdByAlumniId(alumniId);
            var gradeIdList = new List<int?>();

            foreach (var c in classId)
            {
                var classes = await _classService.Value.GetClassById(c);
                gradeIdList.Add(classes.GradeId);
            }
            if (gradeIdList.Count > 0)
            {
                var eventsList = (await _repo.GetAllByConditionAsync( e =>
                                (gradeIdList.Contains(e.GradeId) ||
                                (e.SchoolId == schoolId && e.IsPublicSchool == true)) &&
                                e.Archived == true))
                                .OrderByDescending(e => e.Id)
                                .Skip((pageNo - 1) * pageSize)  
                                .Take(pageSize)
                                .ToList();

                var count = await _repo.CountByCondition(e =>
                                (gradeIdList.Contains(e.GradeId) ||
                                (e.SchoolId == schoolId && e.IsPublicSchool == true)) &&
                                e.Archived == true);
                var eventListPage = new PagingResultDTO<Events>()
                {
                    CurrentPage = pageNo,
                    Items = eventsList,
                    PageSize = pageSize,
                    TotalItems = count
                };
                await _redisService.SetObjectAsync("Event:alumniId:" + alumniId+":"+pageNo+""+pageSize, eventListPage);
                return eventListPage;
            }
            return new PagingResultDTO<Events>();
        }

        public async Task<PagingResultDTO<Events>> GetEventsBySchoolIdIdWithoutCondition(int pageNo, int pageSize, int schoolId)
        {
            var cachedEvent = await _redisService.GetObjectAsync<PagingResultDTO<Events>>("Event:SchoolId:" + schoolId+":"+pageNo+""+pageSize);
            if (cachedEvent != null)
            {
                return cachedEvent;
            }
            var eventsList = (await _repo.GetAllByConditionAsync( e => e.SchoolId == schoolId && e.Archived == true))
                .OrderByDescending(e =>  e.Id)
                .Skip((pageNo-1)*pageSize)
                .Take(pageSize)
                .ToList();
            var count = await _repo.CountByCondition(e => e.SchoolId == schoolId && e.Archived == true);

            var eventsListDto = new PagingResultDTO<Events>()
            {
                CurrentPage = pageNo,
                PageSize = pageSize,
                Items = eventsList,
                TotalItems = count
            };
            await _redisService.SetObjectAsync("Event:SchoolId:" + schoolId + ":" + pageNo + "" + pageSize, eventsListDto);
            //var result = new PagingResultDTO<Events>()
            //{
            //    TotalItems = total,
            //    CurrentPage = pageNo,
            //    PageSize = pageSize,
            //    Items = allEvents.Skip((pageNo - 1) * pageSize).Take(pageSize).ToList()
            //};
            return eventsListDto;
        }

        public async Task<Events> UpdateEvents(Events updateEvents)
        {
            Events events = await _repo.GetByIdAsync(e => e.Id == updateEvents.Id && e.Archived == true);
            if (events == null)
            {
                throw new NotFoundException("Events not found with id: " + updateEvents.Id);
            }
            events.Desciption = updateEvents.Desciption;
            events.location = updateEvents.location;
            events.Title = updateEvents.Title;
            events.ImageUrl = updateEvents.ImageUrl;
            events.IsOffline = updateEvents.IsOffline;
            events.StartTime = updateEvents.StartTime;
            events.EndTime = updateEvents.EndTime;
            events.IsPublicSchool = updateEvents.IsPublicSchool;
            await _repo.UpdateAsync(events);
            _redisService.PublishMessage<Events>("eventQueues",events);

            return events;
        }

        public async Task<IEnumerable<Events>> GetLatestEvent(int size, int alumniId, int schoolId)
        {
            var classId = await _alumniToClassService.Value.GetClassIdByAlumniId(alumniId);
            var gradeIdList = new List<int?>();
            foreach (var c in classId)
            {
                var classes = await _classService.Value.GetClassById(c);
                gradeIdList.Add(classes.GradeId);
            }
            if (gradeIdList.Count > 0)
            {
                var eventsList = await _repo.GetAllByConditionAsync(e => (gradeIdList.Contains(e.GradeId) || (e.SchoolId == schoolId && e.IsPublicSchool) == true) && e.Archived == true);
                return eventsList.OrderByDescending(e => e.CreatedAt).Take(size);
            }
            return new List<Events>();
        }

        public async Task<PagingResultDTO<Events>> GetEventParticipant(int pageNo, int pageSize, int alumniId)
        {
            PagingResultDTO<EventParticipant> eventParticipant = await _eventParticipantService.Value.GetEventParticipantByAlumniId(pageNo, pageSize, alumniId);
            List<Events> events = new List<Events>();
            foreach (var e in eventParticipant.Items)
            {
                var ev = await GetEventsById(e.EventId);
                events.Add(ev);
            }
            PagingResultDTO<Events> result = new PagingResultDTO<Events>()
            {
                TotalItems = eventParticipant.TotalItems,
                CurrentPage = eventParticipant.CurrentPage,
                PageSize = eventParticipant.PageSize,
                Items = events
            };
            return result;
        }

        public async Task<IEnumerable<Events>> GetLatestEvent(int size, int schoolId)
        {
            var eventsList = await _repo.GetAllByConditionAsync(e => ((e.SchoolId == schoolId && e.IsPublicSchool) == true) && e.Archived == true);
            return eventsList.OrderByDescending(e => e.CreatedAt).Take(size);
        }


    }
}
