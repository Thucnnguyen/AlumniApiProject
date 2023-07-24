using AlumniProject.Dto;
using AlumniProject.Entity;
using AlumniProject.ExceptionHandler;
using AlumniProject.Service;
using AlumniProject.Ultils;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AlumniProject.Controllers
{
    [Route("")]
    [ApiController]
    public class SchoolController : ControllerBase
    {
        private readonly ISchoolService _schoolService;
        private readonly IAlumniService _alumniService;
        private readonly IMapper mapper;
        private readonly TokenUltil tokenUltil;


        public SchoolController(ISchoolService schoolService, IMapper mapper, IAlumniService alumniService)
        {
            this._schoolService = schoolService;
            this.mapper = mapper;
            tokenUltil = new TokenUltil();
            this._alumniService = alumniService;
        }
        [HttpGet("tenant/api/schools/isExistRequest"), Authorize(Roles = "alumni,tenant")]
        public async Task<ActionResult<bool>> checkIsAlumniHasCreateSchool()
        {
            try
            {
                var alumniId = tokenUltil.GetClaimByType(User, Constant.AlumniId).Value;
                var alumni = await _alumniService.GetById(int.Parse(alumniId));
                var check = alumni.schoolId == null ? false : true;
                return Ok(check);
            }
            catch (Exception e)
            {

                if (e is NotFoundException)
                {
                    return Ok(false);
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

        [HttpGet("tenant/api/schools/isAcceptRequest"), Authorize(Roles = "alumni,tenant")]
        public async Task<ActionResult<string>> checkIsAlumniHasAcceptSchool()
        {
            try
            {
                var alumniId = tokenUltil.GetClaimByType(User, Constant.AlumniId).Value;
                var alumni = await _alumniService.GetById(int.Parse(alumniId));
                if (alumni.schoolId == null)
                {
                    throw new NotFoundException("Don't have any request");
                }
                var check = await _schoolService.CheckHasAnyRequestAcceptByAlumniId(alumni.schoolId.Value);
                return Ok(check.ToString());
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
        [HttpGet("tenant/api/schools/id"), Authorize(Roles = "alumni,tenant")]
        public async Task<ActionResult<int>> GetSchoolId()
        {
            try
            {

                var schoolId = tokenUltil.GetClaimByType(User, Constant.SchoolId).Value;
                return Ok(schoolId);
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

        [HttpGet("alumni/api/schools/subDomain")]
        public async Task<ActionResult<SchoolSubDomainDTO>> GetSchoolByDomain([FromQuery] string subDomain)
        {
            var school = await _schoolService.GetSchoolBySubDomain(subDomain);
            if (school == null)
            {
                return NotFound("School not found with SubDomain: " + subDomain);
            }
            var schoolDto = mapper.Map<SchoolSubDomainDTO>(school);
            var cardItem = school.CardInSchool.Select(c => mapper.Map<CardItemDTO>(c)).ToList();
            var attr = school.Attribute.Select(c => mapper.Map<AttributeDTO>(c)).ToList();
            schoolDto.CardInSchool = cardItem;
            schoolDto.Attributes = attr;
            return Ok(schoolDto);
        }
        [HttpGet("admin/api/schools"), Authorize(Roles = "admin")]
        public async Task<ActionResult<PagingResultDTO<SchoolDTO>>> GetAllSchool(
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
                var SchoolsList = await _schoolService.GetSchools(pageNo, pageSize);
                List<SchoolDTO> SchoolListDto = SchoolsList.Items.Select(s => mapper.Map<SchoolDTO>(s)).ToList();
                foreach (var s in SchoolListDto)
                {
                    var tenant = await _alumniService.GetTenantBySchoolId(s.id);
                    s.HostEmail = tenant != null ? tenant.Email : null;
                }
                var result = new PagingResultDTO<SchoolDTO>()
                {
                    Items = SchoolListDto,
                    CurrentPage = SchoolsList.CurrentPage,
                    PageSize = SchoolsList.PageSize,
                    TotalItems = SchoolsList.TotalItems
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

        [HttpGet("alumni/api/schools"), Authorize(Roles = "tenant,alumni")]
        public async Task<ActionResult<SchoolDTO>> GetSchoolById()
        {
            try
            {
                var schoolId = tokenUltil.GetClaimByType(User, Constant.SchoolId).Value;
                var school = await _schoolService.GetSchoolById(int.Parse(schoolId));
                if (school == null)
                {
                    return NotFound("School not found with ID: " + int.Parse(schoolId));
                }
                return Ok(mapper.Map<SchoolDTO>(school));
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
        [HttpGet("admin/api/schools/count")]
        public async Task<ActionResult<int>> countSchoolInRange([FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            try
            {
                var count = await _schoolService.CountSchoolRequestInRange(from, to);
                return Ok(count);
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
        [HttpGet("admin/api/schools/count/status")]
        public async Task<ActionResult<int>> countSchoolInRangeandStatus([FromQuery] DateTime from, [FromQuery] DateTime to, [Range(1,3), FromQuery]int status)
        {
            try
            {
                var count = await _schoolService.CountSchoolRequestInRangeAndStatus(from, to,status);
                return Ok(count);
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
        [HttpGet("admin/api/schools/statistics"), Authorize(Roles = "admin")]
        public async Task<ActionResult<IDictionary<int, int>>> staticsSchoolInRange([FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            try
            {
                var statics = await _schoolService.CalculateStaticInrange(from, to);
                return Ok(statics);
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
        [HttpPost("alumni/api/schools"), Authorize(Roles = "alumni")]
        public async Task<ActionResult<int>> AddSchool([FromBody] SchoolAddDTO schoolAddDTO)
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
                if (int.Parse(schoolId) != -1)
                {
                    return Conflict("An account can only create one account");
                }
                var schoolIdNew = await _schoolService.AddSchool(mapper.Map<School>(schoolAddDTO));
                var alumniId = tokenUltil.GetClaimByType(User, Constant.AlumniId).Value;
                Alumni alumni = await _alumniService.GetById(int.Parse(alumniId));

                alumni.schoolId = schoolIdNew;
                alumni.IsOwner = true;
                await _alumniService.UpdateAlumni(alumni);
                return Ok(schoolIdNew);
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
        [HttpDelete("alumni/api/schools/{schoolId}"), Authorize(Roles = "alumni")]
        public async Task<ActionResult<string>> DeleteBySchoolID([FromRoute] int schoolId)
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


                await _alumniService.DeleteById(schoolId);
                return Ok("Delete successful with id: " + schoolId);
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
        [HttpPut("tenant/api/schools"), Authorize(Roles = "tenant")]
        public async Task<ActionResult<SchoolDTO>> UpdateSchool(SchoolUpdateDto schoolUpdateDto)
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

                School schoolUpdate = mapper.Map<School>(schoolUpdateDto);
                schoolUpdate.Id = int.Parse(schoolId);
                var school = await _schoolService.UpdateSchool(schoolUpdate);
                return Ok(mapper.Map<SchoolDTO>(school));
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
        [HttpPut("admin/api/schools/status"), Authorize(Roles = "admin")]

        public async Task<ActionResult<SchoolDTO>> UpdateSchoolStatus(UpdateSchoolStatusDTO updateSchoolStatusDTO)
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
                var school = await _schoolService.UpdateSchoolStatus(updateSchoolStatusDTO.Id, updateSchoolStatusDTO.RequestStatus);
                return Ok(mapper.Map<SchoolDTO>(school));
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
