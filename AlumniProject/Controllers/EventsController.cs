using AlumniProject.Dto;
using AlumniProject.Entity;
using AlumniProject.ExceptionHandler;
using AlumniProject.Service;
using AlumniProject.Service.ServiceImp;
using AlumniProject.Ultils;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace AlumniProject.Controllers
{
    [Route("")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;
        private readonly IAlumniService _alumniService;
        private readonly IMapper _mapper;
        private readonly TokenUltil tokenUltil;

        public EventsController(IEventService eventService, IMapper mapper, IAlumniService alumniService)
        {
            _eventService = eventService;
            _mapper = mapper;
            tokenUltil = new TokenUltil();
            _alumniService = alumniService;
        }

        [HttpGet("tenant/api/events"), Authorize(Roles = "tenant")]
        public async Task<ActionResult<PagingResultDTO<EventsDTO>>> GetEventsBySchoolIdTenant(
            [FromQuery, Range(1, int.MaxValue), Required(ErrorMessage = "pageNo is Required")] int pageNo = 1,
            [FromQuery, Range(1, int.MaxValue), Required(ErrorMessage = "pageSize is Required")] int pageSize = 10
            )
        {
            try
            {
                var errorMessages = ModelState.Values
                   .SelectMany(v => v.Errors)
                   .Select(e => e.ErrorMessage)
                   .ToList();
                if (errorMessages.Any())
                {
                    return BadRequest(string.Join(", ", errorMessages));
                }

                var schoolId = tokenUltil.GetClaimByType(User, Constant.SchoolId).Value;

                var events = await _eventService.GetEventsBySchoolIdIdWithoutCondition(pageNo, pageSize, int.Parse(schoolId));
                List<EventsDTO> eventsDTO = new List<EventsDTO>();
                foreach (Events e in events.Items)
                {
                    EventsDTO newEventDto = _mapper.Map<EventsDTO>(e);
                    newEventDto.HostName = (await _alumniService.GetById(e.HostId)).FullName;
                    eventsDTO.Add(newEventDto);
                }
                var result = new PagingResultDTO<EventsDTO>()
                {
                    CurrentPage = events.CurrentPage,
                    PageSize = events.PageSize,
                    TotalItems = events.TotalItems,
                    Items = eventsDTO
                };
                return Ok(result);
            }
            catch (Exception e)
            {
                if (e is NotFoundException)
                {
                    return NotFound(e.Message);
                }
                else if (e is BadRequestException)
                {
                    return BadRequest(e.Message);
                }
                else
                {
                    return Conflict(e.Message);
                }
            }
        }
        [HttpGet("alumni/api/events"), Authorize(Roles = "alumni,tenant")]
        public async Task<ActionResult<PagingResultDTO<EventsDTO>>> GetEventsByAlumniId(
            [FromQuery, Range(1, int.MaxValue), Required(ErrorMessage = "pageNo is Required")] int pageNo = 1,
            [FromQuery, Range(1, int.MaxValue), Required(ErrorMessage = "pageSize is Required")] int pageSize = 10
            )
        {
            try
            {

                var errorMessages = ModelState.Values
                   .SelectMany(v => v.Errors)
                   .Select(e => e.ErrorMessage)
                   .ToList();
                if (errorMessages.Any())
                {
                    return BadRequest(string.Join(", ", errorMessages));
                }
                var alumniId = tokenUltil.GetClaimByType(User, Constant.AlumniId).Value;
                var schoolId = tokenUltil.GetClaimByType(User, Constant.SchoolId).Value;
                if(int.Parse(schoolId) == -1)
                {
                    return Ok(new PagingResultDTO<EventsDTO>());
                }
                if (User.IsInRole("alumni"))
                {

                    var events = await _eventService.GetEventsByAlumniId(pageNo, pageSize, int.Parse(alumniId), int.Parse(schoolId));
                    if(events.Items == null)
                    {
                        return Ok(new PagingResultDTO<EventsDTO>());
                    }
                    List<EventsDTO> eventsDTO = new List<EventsDTO>();
                    foreach (Events e in events.Items)
                    {
                        EventsDTO newEventDto = _mapper.Map<EventsDTO>(e);
                        newEventDto.HostName = (await _alumniService.GetById(e.HostId)).FullName;
                        eventsDTO.Add(newEventDto);
                    }
                    var result = new PagingResultDTO<EventsDTO>()
                    {
                        CurrentPage = events.CurrentPage,
                        PageSize = events.PageSize,
                        TotalItems = events.TotalItems,
                        Items = eventsDTO
                    };
                    return Ok(result);
                }
                else if (User.IsInRole("tenant"))
                {
                    var events = await _eventService.GetEventsBySchoolIdIdWithoutCondition(pageNo, pageSize, int.Parse(schoolId));

                    var eventsDTO = events.Items.Select(e => _mapper.Map<EventsDTO>(e)).ToList();
                    var result = new PagingResultDTO<EventsDTO>()
                    {
                        CurrentPage = events.CurrentPage,
                        PageSize = events.PageSize,
                        TotalItems = events.TotalItems,
                        Items = eventsDTO
                    };
                    return Ok(result);
                }
                return Ok(new PagingResultDTO<EventsDTO>());
            }
            catch (Exception e)
            {
                if (e is NotFoundException)
                {
                    return NotFound(e.Message);
                }
                else if (e is BadRequestException)
                {
                    return BadRequest(e.Message);
                }
                else
                {
                    return Conflict(e.Message);
                }
            }
        }
        
        [HttpDelete("tenant/api/events/{eventsId}"), Authorize(Roles = "tenant")]
        public async Task<ActionResult<string>> deleteNews([FromRoute] int eventsId)
        {
            try
            {
                var errorMessages = ModelState.Values
                 .SelectMany(v => v.Errors)
                 .Select(e => e.ErrorMessage)
                 .ToList();
                if (errorMessages.Any())
                {
                    return BadRequest(string.Join(", ", errorMessages));
                }
                var schoolId = tokenUltil.GetClaimByType(User, Constant.SchoolId).Value;

                await _eventService.DeleteEvents(eventsId,int.Parse(schoolId));
                return Ok("Deleted news successful with id: " + eventsId);
            }
            catch (Exception e)
            {
                if (e is NotFoundException)
                {
                    return NotFound(e.Message);
                }
                else if (e is BadRequestException)
                {
                    return BadRequest(e.Message);
                }
                else
                {
                    return Conflict(e.Message);
                }
            }
        }

        [HttpGet("alumni/api/events/latest"), Authorize(Roles = "alumni,tenant")]
        public async Task<ActionResult<List<EventsDTO>>> GetEventsByAlumniIDLastest(
            [FromQuery, Range(1, int.MaxValue), Required(ErrorMessage = "size is Required")] int size = 1
            )
        {
            try
            {
                var errorMessages = ModelState.Values
                   .SelectMany(v => v.Errors)
                   .Select(e => e.ErrorMessage)
                   .ToList();
                if (errorMessages.Any())
                {
                    return BadRequest(string.Join(", ", errorMessages));
                }
                var alumniId = tokenUltil.GetClaimByType(User, Constant.AlumniId).Value;
                var schoolId = tokenUltil.GetClaimByType(User, Constant.SchoolId).Value;
                
                if (User.IsInRole("alumni"))
                {
                    var events = await _eventService.GetLatestEvent(size, int.Parse(alumniId), int.Parse(schoolId));
                    var eventsDTO = events.Select(e => _mapper.Map<EventsDTO>(e)).ToList();
                    return Ok(eventsDTO);
                }
                if (User.IsInRole("tenant"))
                {
                    var events = await _eventService.GetLatestEvent(size, int.Parse(schoolId));
                    var eventsDTO = events != null ? events.Select(e => _mapper.Map<EventsDTO>(e)).ToList() : new List<EventsDTO>();
                    return Ok(eventsDTO);
                }
                return Ok(new List<EventsDTO>());
            }
            catch (Exception e)
            {
                if (e is NotFoundException)
                {
                    return NotFound(e.Message);
                }
                else if (e is BadRequestException)
                {
                    return BadRequest(e.Message);
                }
                else
                {
                    return Conflict(e.Message);
                }
            }
        }
        [HttpGet("alumni/api/eventsParticipant"), Authorize(Roles = "tenant,alumni")]
        public async Task<ActionResult<PagingResultDTO<EventsDTO>>> GetEventsParticipantByAlumniId(
            [FromQuery, Range(1, int.MaxValue), Required(ErrorMessage = "pageNo is Required")] int pageNo = 1,
            [FromQuery, Range(1, int.MaxValue), Required(ErrorMessage = "pageSize is Required")] int pageSize = 10
            )
        {
            try
            {

                var errorMessages = ModelState.Values
                   .SelectMany(v => v.Errors)
                   .Select(e => e.ErrorMessage)
                   .ToList();
                if (errorMessages.Any())
                {
                    return BadRequest(string.Join(", ", errorMessages));
                }

                var alumniId = tokenUltil.GetClaimByType(User, Constant.AlumniId).Value;
                var events = await _eventService.GetEventParticipant(pageNo, pageSize, int.Parse(alumniId));
                var eventsDTO = events.Items.Select(e => _mapper.Map<EventsDTO>(e)).ToList();
                var result = new PagingResultDTO<EventsDTO>()
                {
                    CurrentPage = events.CurrentPage,
                    PageSize = events.PageSize,
                    TotalItems = events.TotalItems,
                    Items = eventsDTO
                };
                return Ok(result);
            }
            catch (Exception e)
            {
                if (e is NotFoundException)
                {
                    return NotFound(e.Message);
                }
                else if (e is BadRequestException)
                {
                    return BadRequest(e.Message);
                }
                else
                {
                    return Conflict(e.Message);
                }
            }
        }
        [HttpPost("tenant/api/events"), Authorize(Roles = "tenant")]
        public async Task<ActionResult<int>> CreateEvents(
            [FromBody] EventsAddDTO eventsAddDTO
            )
        {
            try
            {
                var errorMessages = ModelState.Values
                   .SelectMany(v => v.Errors)
                   .Select(e => e.ErrorMessage)
                   .ToList();
                if (errorMessages.Any())
                {
                    return BadRequest(string.Join(", ", errorMessages));
                }
                var alumniId = tokenUltil.GetClaimByType(User, Constant.AlumniId).Value;
                var schoolId = tokenUltil.GetClaimByType(User, Constant.SchoolId).Value;

                Events events = _mapper.Map<Events>(eventsAddDTO);
                events.HostId = int.Parse(alumniId);
                var eventId = await _eventService.CreateEvent(events);
                return Ok(eventId);
            }
            catch (Exception e)
            {
                if (e is NotFoundException)
                {
                    return NotFound(e.Message);
                }
                else if (e is BadRequestException)
                {
                    return BadRequest(e.Message);
                }
                else
                {
                    return Conflict(e.Message);
                }
            }
        }
        [HttpPut("tenant/api/events"), Authorize(Roles = "tenant")]
        public async Task<ActionResult<PagingResultDTO<EventsDTO>>> UpdateEvents(
            [FromBody] EventUpdateDTO eventsUpdateDTO
            )
        {
            try
            {
                var errorMessages = ModelState.Values
                   .SelectMany(v => v.Errors)
                   .Select(e => e.ErrorMessage)
                   .ToList();
                if (errorMessages.Any())
                {
                    return BadRequest(string.Join(", ", errorMessages));
                }
                var events = _mapper.Map<Events>(eventsUpdateDTO);
                var eventsUpdate = await _eventService.UpdateEvents(events);
                return Ok(_mapper.Map<EventsDTO>(eventsUpdate));
            }
            catch (Exception e)
            {
                if (e is NotFoundException)
                {
                    return NotFound(e.Message);
                }
                else if (e is BadRequestException)
                {
                    return BadRequest(e.Message);
                }
                else
                {
                    return Conflict(e.Message);
                }
            }
        }
        [HttpGet("alumni/api/events/{eventId}")]
        [Authorize(Roles = "tenant,alumni")]
        public async Task<ActionResult<EventsDTO>> GetEventById(int eventId)
        {
            try
            {
                var errorMessages = ModelState.Values
                   .SelectMany(v => v.Errors)
                   .Select(e => e.ErrorMessage)
                   .ToList();
                if (errorMessages.Any())
                {
                    return BadRequest(string.Join(", ", errorMessages));
                }

                var eventDetails = await _eventService.GetEventsById(eventId);
                if (eventDetails == null)
                {
                    return NotFound($"Event with ID {eventId} not found.");
                }

                var eventDTO = _mapper.Map<EventsDTO>(eventDetails);
                eventDTO.HostName = (await _alumniService.GetById(eventDetails.HostId)).FullName;
                return Ok(eventDTO);
            }
            catch (Exception e)
            {
                if (e is NotFoundException)
                {
                    return NotFound(e.Message);
                }
                else if (e is BadRequestException)
                {
                    return BadRequest(e.Message);
                }
                else
                {
                    return Conflict(e.Message);
                }
            }

        }
        [HttpGet("teanant/api/events/{eventId}")]
        [Authorize(Roles = "tenant")]
        public async Task<ActionResult<EventsDTO>> GetEventByIdForTenant(int eventId)
        {
            try
            {
                var errorMessages = ModelState.Values
                   .SelectMany(v => v.Errors)
                   .Select(e => e.ErrorMessage)
                   .ToList();
                if (errorMessages.Any())
                {
                    return BadRequest(string.Join(", ", errorMessages));
                }

                var eventDetails = await _eventService.GetEventsById(eventId);
                if (eventDetails == null)
                {
                    return NotFound($"Event with ID {eventId} not found.");
                }

                var eventDTO = _mapper.Map<EventsDTO>(eventDetails);
                eventDTO.HostName = (await _alumniService.GetById(eventDetails.HostId)).FullName;
                return Ok(eventDTO);
            }
            catch (Exception e)
            {
                if (e is NotFoundException)
                {
                    return NotFound(e.Message);
                }
                else if (e is BadRequestException)
                {
                    return BadRequest(e.Message);
                }
                else
                {
                    return Conflict(e.Message);
                }
            }

        }
    }

}
