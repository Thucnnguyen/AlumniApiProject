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

namespace AlumniProject.Controllers
{
    [Route("")]
    [ApiController]
    public class EventParticipantController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IEventParticipantService _eventParticipantService;
        private readonly TokenUltil tokenUltil;

        public EventParticipantController(IMapper mapper, IEventParticipantService eventParticipantService)
        {
            _mapper = mapper;
            _eventParticipantService = eventParticipantService;
            tokenUltil = new TokenUltil();
        }

        [HttpPost("alumni/api/events")]
        [Authorize(Roles = "alumni,tenant")]
        public async Task<ActionResult<int>> EnrollEvent([FromQuery] int eventId)
        {
            try
            {
                var alumniId = tokenUltil.GetClaimByType(User, Constant.AlumniId).Value;
                var schoolId = tokenUltil.GetClaimByType(User, Constant.SchoolId).Value;

                var eventParticipant = new EventParticipant();

                eventParticipant.EventId = eventId;
                eventParticipant.AlumniId = int.Parse(alumniId);
                var eventParticipantId = await _eventParticipantService.createEventParticipant(eventParticipant);

                return Ok(eventParticipantId);
            }
            catch (Exception e)
            {
                if (e is NotFoundException)
                {
                    return NotFound(e.Message);
                }
                else if (e is ConflictException)
                {
                    return Conflict(e.Message);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
                }
            }
        }
        
        [HttpDelete("alumni/api/events/{eventId}")]
        [Authorize(Roles = "alumni,tenant")]
        public async Task<ActionResult<string>> DeleteEventParticipant([FromRoute] int eventId)
        {
            try
            {
                var participantId = tokenUltil.GetClaimByType(User, Constant.AlumniId).Value;

                await _eventParticipantService.DeleteEventParticipant(eventId, int.Parse(participantId));

                return Ok($"Deleted event participant with ID: {participantId}");
            }
            catch (Exception e)
            {
                if (e is NotFoundException)
                {
                    return NotFound(e.Message);
                }
                else if (e is UnauthorizedAccessException)
                {
                    return Unauthorized(e.Message);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
                }
            }
        }
        [HttpGet("alumni/api/events/{eventId}/participants")]
        [Authorize(Roles = "alumni,tenant")]
        public async Task<ActionResult<bool>> GetEventParticipant([FromRoute] int eventId)
        {
            try
            {
                var alumniId = tokenUltil.GetClaimByType(User, Constant.AlumniId).Value;
                var isParticipant = await _eventParticipantService.isAccess(eventId, int.Parse(alumniId));

                if (isParticipant == false)
                {
                    return NotFound($"Event participant with AlumniId: {alumniId} and EventId: {eventId} not found.");
                }

                return Ok(isParticipant);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}