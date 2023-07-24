using AlumniProject.Dto;
using AlumniProject.Entity;
using AlumniProject.ExceptionHandler;
using AlumniProject.Service;
using AlumniProject.Ultils;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AlumniProject.Controllers
{
    [Route("")]
    [ApiController]
    public class RequestToSchoolController : ControllerBase
    {
        private readonly IAlumniRequestService _alumniRequestService;
        private readonly IAlumniService _alumniService;
        private readonly IClassService _classService;

        private readonly IMapper mapper;
        private readonly TokenUltil tokenUltil;
        public RequestToSchoolController(IAlumniRequestService alumniRequestService, IMapper mapper, IAlumniService alumniService, IClassService classService)
        {
            this._alumniRequestService = alumniRequestService;
            this.mapper = mapper;
            tokenUltil = new TokenUltil();
            _alumniService = alumniService;
            _classService = classService;
        }

        [HttpGet("tenant/api/accessReqeuest"), Authorize(Roles = "tenant")]
        public async Task<ActionResult<PagingResultDTO<AccessRequestDTO>>> GetAccessRequestBySchoolId(
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
                var AccessRequestList = await _alumniRequestService.GetAccessRequestsByScchoolId(pageNo, pageSize, int.Parse(schoolId));
                if (AccessRequestList == null)
                {
                    return NoContent();
                }

                List<AccessRequestDTO> AccessRequestDtoList = new List<AccessRequestDTO>();

                foreach (var accessRequest in AccessRequestList.Items)
                {
                    AccessRequestDTO newAccessReqquest = mapper.Map<AccessRequestDTO>(accessRequest);
                    newAccessReqquest.AlumniClassName = (await _classService.GetClassById(newAccessReqquest.AlumniClassId)).Name;
                    AccessRequestDtoList.Add(newAccessReqquest);
                }

                var AccessRequestDtoPage = new PagingResultDTO<AccessRequestDTO>()
                {
                    CurrentPage = AccessRequestList.CurrentPage,
                    PageSize = AccessRequestList.PageSize,
                    TotalItems = AccessRequestList.TotalItems,
                    Items = AccessRequestDtoList
                };
                return Ok(AccessRequestDtoPage);
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

        [HttpPost("alumni/api/accessReqeuest"), Authorize(Roles = "alumni")]
        public async Task<ActionResult<int>> CreateAccessRequest([FromBody] AccessRequestAddDTO accessRequestAddDTO)
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

                var alumni = await _alumniService.GetById(int.Parse(alumniId));
                var request = mapper.Map<AccessRequest>(accessRequestAddDTO);

                request.FullName = alumni.FullName;
                request.Phone = alumni.Phone;
                request.DateOfBirth = alumni.DateOfBirth;
                request.Email = alumni.Email;



                var accessrequestId = await _alumniRequestService.CreateAlumniRequest(request, int.Parse(schoolId));
                return Ok(accessrequestId);
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

        [HttpPut("tenant/api/accessReqeuest"), Authorize(Roles = "tenant")]
        public async Task<ActionResult<AccessRequestDTO>> updateAccessRequestStatus([FromBody] AccessRequestUpdateDTO accessRequestUpdateDTO)
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
                var updateStatus = await _alumniRequestService.UpdateAccessRequest(accessRequestUpdateDTO.Id, accessRequestUpdateDTO.RequestStatus);
                return Ok(mapper.Map<AccessRequestDTO>(updateStatus));
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
        [HttpDelete("tenant/api/accessReqeuest/{requestId}"), Authorize(Roles = "tenant")]
        public async Task<ActionResult<string>> deleteAccessRequestStatus([FromRoute] int requestId)
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
                await _alumniRequestService.DeleteAccessRequest(requestId);
                return Ok("AccessRequest deleted successful with id: " + requestId);
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
