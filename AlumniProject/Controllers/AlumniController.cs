using AlumniProject.Data;
using AlumniProject.Dto;
using AlumniProject.Entity;
using AlumniProject.ExceptionHandler;
using AlumniProject.Service;
using AlumniProject.Service.ServiceImp;
using AlumniProject.Ultils;
using AutoMapper;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;

namespace AlumniProject.Controllers
{
    [Route("")]
    [ApiController]
    public class AlumniController : ControllerBase
    {
        private readonly IAlumniService service;
        private readonly IConfiguration _configuration;
        private readonly IAlumniToClassService alumniToClassService;
        private readonly IGradeService _gradeService;
        private readonly IRoleService _roleService;
        private readonly IMapper mapper;
        private readonly TokenUltil tokenUltil;

        public AlumniController(IAlumniService alumniService, IMapper mapper, IConfiguration configuration, IGradeService gradeService, IRoleService roleService, IAlumniToClassService alumniToClassService)
        {
            this.service = alumniService;
            this.mapper = mapper;
            this._configuration = configuration;
            this._gradeService = gradeService;
            this._roleService = roleService;
            tokenUltil = new TokenUltil();
            this.alumniToClassService = alumniToClassService;
        }


        [HttpGet("alumni/api/alumnis"), Authorize(Roles = ("tenant,alumni"))]
        public async Task<ActionResult<AlumniDTO>> GetAlumniById()
        {
            var id = tokenUltil.GetClaimByType(User, Constant.AlumniId).Value;

            var alumni = await service.GetById(int.Parse(id));
            if (alumni == null)
            {
                return NotFound("Alumni not found with ID: " + id);
            }
            return Ok(mapper.Map<AlumniDTO>(alumni));
        }
        [HttpGet("admin/api/alumnis/lastLogin"), Authorize(Roles = ("admin"))]
        public async Task<ActionResult<int>> Count([FromQuery] DateTime from, [FromQuery] DateTime to)
        {

            var count = await service.CountLastLoginInRage(from, to);
            return Ok(count);
        }
        [HttpGet("admin/api/alumnis/statistics"), Authorize(Roles = ("admin"))]
        public async Task<ActionResult<Dictionary<int, int>>> staticsOfLastLogin([FromQuery] DateTime from, [FromQuery] DateTime to)
        {

            var statics = await service.CalculateAccessCountsByHour(from, to);
            return Ok(statics);
        }
        [HttpGet("tenant/api/alumnis/filter"), Authorize(Roles = ("tenant"))]
        public async Task<ActionResult<PagingResultDTO<AlumniDTO>>> GetAlumnisBySchoolId(
            [FromQuery] string? searchText,
            [FromQuery, Range(1, int.MaxValue), Required(ErrorMessage = "pageNo is Required")] int pageNo = 1,
            [FromQuery, Range(1, int.MaxValue), Required(ErrorMessage = "pageSize is Required")] int pageSize = 10
            )
        {
            var schoolId = tokenUltil.GetClaimByType(User, Constant.SchoolId).Value;

            if (string.IsNullOrEmpty(searchText))
            {
                var alumni = await service.GetAlumniBySchoolId(pageNo, pageSize, int.Parse(schoolId));
                var alumniDtoList = alumni.Items.Select(a => mapper.Map<AlumniDTO>(a)).ToList();
                return Ok(new PagingResultDTO<AlumniDTO>()
                {
                    Items = alumniDtoList,
                    TotalItems = alumni.TotalItems,
                    CurrentPage = pageNo,
                    PageSize = pageSize
                });
            }
            else
            {
                var alumni = await service.SearchAlumniByEmailOrNameOrPhone(pageNo, pageSize, searchText, int.Parse(schoolId));
                var alumniDtoList = alumni.Items.Select(a => mapper.Map<AlumniDTO>(a)).ToList();
                return Ok(new PagingResultDTO<AlumniDTO>()
                {
                    Items = alumniDtoList,
                    TotalItems = alumni.TotalItems,
                    CurrentPage = pageNo,
                    PageSize = pageSize
                });
            }
        }
        [HttpGet("alumni/api/alumnis/filter"), Authorize(Roles = ("alumni"))]
        public async Task<ActionResult<PagingResultDTO<AlumniDTO>>> alumniGetAlumnisBySchoolId(
            [FromQuery] int classID,
            [FromQuery] string? searchText,
            [FromQuery, Range(1, int.MaxValue), Required(ErrorMessage = "pageNo is Required")] int pageNo = 1,
            [FromQuery, Range(1, int.MaxValue), Required(ErrorMessage = "pageSize is Required")] int pageSize = 10
            )
        {

            if (string.IsNullOrEmpty(searchText))
            {
                var schoolId = tokenUltil.GetClaimByType(User, Constant.SchoolId).Value;
                var alumni = await service.GetAlumniBySchoolId(pageNo, pageSize, int.Parse(schoolId));
                var alumniDtoList = alumni.Items.Select(a => mapper.Map<AlumniDTO>(a)).ToList();
                return Ok(new PagingResultDTO<AlumniDTO>()
                {
                    Items = alumniDtoList,
                    TotalItems = alumni.TotalItems,
                    CurrentPage = pageNo,
                    PageSize = pageSize
                });
            }
            else
            {
                var alumni = await service.SearchAlumniByEmailOrNameOrPhoneAndClassId(pageNo, pageSize, searchText, classID);
                var alumniDtoList = alumni.Items.Select(a => mapper.Map<AlumniDTO>(a)).ToList();
                return Ok(new PagingResultDTO<AlumniDTO>()
                {
                    Items = alumniDtoList,
                    TotalItems = alumni.TotalItems,
                    CurrentPage = pageNo,
                    PageSize = pageSize
                });
            }
        }
        //[HttpGet("alumni/api/alumnis/search"), Authorize(Roles = ("tenant,alumni"))]
        //public async Task<ActionResult<PagingResultDTO<AlumniDTO>>> SearchAlumniByNameOrEmailOrPhone(
        //    [FromQuery] string searchText,
        //    [FromQuery, Range(1, int.MaxValue), Required(ErrorMessage = "pageNo is Required")] int pageNo = 1,
        //    [FromQuery, Range(1, int.MaxValue), Required(ErrorMessage = "pageSize is Required")] int pageSize = 10
        //    )
        //{
        //    var schoolId = tokenUltil.GetClaimByType(User, Constant.SchoolId).Value;

        //    var alumni = await service.SearchAlumniByEmailOrNameOrPhone(pageNo, pageSize, searchText, int.Parse(schoolId));
        //    var alumniDtoList = alumni.Items.Select(a => mapper.Map<AlumniDTO>(a)).ToList();
        //    return Ok(new PagingResultDTO<AlumniDTO>()
        //    {
        //        Items = alumniDtoList,
        //        TotalItems = alumni.TotalItems,
        //        CurrentPage = pageNo,
        //        PageSize = pageSize
        //    });
        //}
        [HttpGet("tenant/api/alumnis"), Authorize(Roles = ("tenant"))]
        public async Task<ActionResult<AlumniDTO>> GetAlumnisByAlumniId([FromQuery] int alumniId)
        {
            try
            {
                var schoolId = tokenUltil.GetClaimByType(User, Constant.SchoolId).Value;

                var alumni = await service.GetById(alumniId);

                return Ok(mapper.Map<AlumniDTO>(alumni));
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
        [HttpGet("alumni/api/alumnis/info"), Authorize(Roles = ("alumni"))]
        public async Task<ActionResult<AlumniDTO>> GetAlumnisById([FromQuery] int alumniId)
        {
            try
            {
                var schoolId = tokenUltil.GetClaimByType(User, Constant.SchoolId).Value;

                var alumni = await service.GetById(alumniId, int.Parse(schoolId));

                return Ok(mapper.Map<AlumniDTO>(alumni));
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
        [HttpGet("alumni/api/class/{classId}/alumnis"), Authorize(Roles = ("alumni"))]
        public async Task<ActionResult<PagingResultDTO<AlumniDTO>>> GetAlumnisByClassId(
            [FromRoute] int classId,
            [FromQuery, Range(1, int.MaxValue), Required(ErrorMessage = "pageNo is Required")] int pageNo = 1,
            [FromQuery, Range(1, int.MaxValue), Required(ErrorMessage = "pageSize is Required")] int pageSize = 10
            )
        {
            try
            {
                //var schoolId = tokenUltil.GetClaimByType(User, Constant.SchoolId).Value;
                var alumni = await alumniToClassService.GetAlumniByClassId(pageNo, pageSize, classId);
                var countTotalAlumni = await alumniToClassService.GetAlumniByClassId(classId);
                var alumniDto = alumni.Items.Select(s => mapper.Map<AlumniDTO>(s)).ToList();
                var alumniDtoPage = new PagingResultDTO<AlumniDTO>()
                {
                    CurrentPage = pageNo,
                    PageSize = pageSize,
                    Items = alumniDto,
                    TotalItems = countTotalAlumni.Count()
                };
                return Ok(alumniDtoPage);
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
        [HttpGet("tenant/api/class/{classId}/alumnis"), Authorize(Roles = ("tenant"))]
        public async Task<ActionResult<PagingResultDTO<AlumniDTO>>> GetAlumnisByClassIdForTenant(
            [FromRoute] int classId,
            [FromQuery, Range(1, int.MaxValue), Required(ErrorMessage = "pageNo is Required")] int pageNo = 1,
            [FromQuery, Range(1, int.MaxValue), Required(ErrorMessage = "pageSize is Required")] int pageSize = 10
            )
        {
            try
            {
                var schoolId = tokenUltil.GetClaimByType(User, Constant.SchoolId).Value;
                var alumni = await alumniToClassService.GetAlumniByClassId(pageNo, pageSize, classId);
                var alumniDto = alumni.Items.Select(s => mapper.Map<AlumniDTO>(s)).ToList();
                var alumniDtoPage = new PagingResultDTO<AlumniDTO>()
                {
                    CurrentPage = pageNo,
                    PageSize = pageSize,
                    Items = alumniDto,
                    TotalItems = alumniDto.Count()
                };
                return Ok(alumniDtoPage);
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
        [HttpGet("tenant/api/alumnis/{alumniId}/gradeCode/className")]
        public async Task<ActionResult<Dictionary<string, List<string>>>> GetGradeCodeAndClassNameByClassIdForTenant(
            [FromRoute] int alumniId
            )
        {
            try
            {
                var alumniInfo = await alumniToClassService.GetClassNameAndGradeNameByAlumniId(alumniId);
                return Ok(alumniInfo);
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
        [HttpDelete("tenant/api/alumnis/{alumniId}"), Authorize(Roles = ("tenant"))]
        public async Task<ActionResult<PagingResultDTO<AlumniDTO>>> DeleteAlumnisByAlumniId([FromRoute] int alumniId)
        {
            try
            {
                var schoolId = tokenUltil.GetClaimByType(User, Constant.SchoolId).Value;

                var alumni = await service.GetById(alumniId);

                return Ok(alumni);
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

        [HttpPost("api/alumnis/login")]
        public async Task<ActionResult<string>> login([FromBody] GoogleSignInRequest signInRequest)
        {
            var tokenHelper = new TokenHelper(_configuration, _gradeService, _roleService);

            try
            {
                var tokenValue = tokenHelper.DecodeJwtToken(signInRequest.Token);
                if (!string.IsNullOrEmpty(tokenValue.Email))
                {
                    var alumni = await service.GetAlumniByEmail(tokenValue.Email);
                    var alumniToken = await tokenHelper.CreateToken(alumni);
                    alumni.LastLogin = DateTime.Now;
                    await service.UpdateAlumni(alumni);
                    return Ok(alumniToken);
                }
                else
                {
                    throw new BadRequestException("token not valid");
                }



            }
            catch (Exception e)
            {
                if (e is NotFoundException)
                {
                    var tokenValue = tokenHelper.DecodeJwtToken(signInRequest.Token);
                    //return NotFound(e.Message);
                    Alumni newAlumni = new Alumni()
                    {
                        FullName = tokenValue.FullName != null ? tokenValue.FullName : "",
                        Email = tokenValue.Email,
                        Phone = "",
                        Avatar_url = tokenValue.Avatar_url != null ? tokenValue.Avatar_url : "",
                        RoleId = (int)RoleEnum.alumni,
                    };

                    var newAlumniID = await service.AddAlumni(newAlumni);
                    var alumni = await service.GetById(newAlumniID);
                    var alumniToken = await tokenHelper.CreateToken(alumni);
                    return alumniToken;

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

        [HttpPut("alumni/api/alumnis"), Authorize(Roles = "alumni,tenant")]
        public async Task<ActionResult<AlumniDTO>> UpdateAlumni([FromBody] AlumniUpdateDTO alumniUpdateDTO)
        {
            try
            {
                var alumniIdClaims = tokenUltil.GetClaimByType(User, Constant.AlumniId).Value;
                var alumniId = int.Parse(alumniIdClaims);
                var alumni = await service.GetById(alumniId);
                if (alumni == null)
                {
                    return NotFound("Alumni not found with ID: " + alumniId);
                }
                Alumni alumniUpdate = mapper.Map<Alumni>(alumniUpdateDTO);
                alumniUpdate.Id = alumniId;
                var updateAlumni = await service.UpdateAlumni(alumniUpdate);
                return Ok(mapper.Map<AlumniDTO>(updateAlumni));
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
