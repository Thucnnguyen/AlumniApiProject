using AlumniProject.Dto;
using AlumniProject.Entity;
using AlumniProject.ExceptionHandler;
using AlumniProject.Service;
using AlumniProject.Ultils;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace AlumniProject.Controllers
{
    [Route("")]
    [ApiController]
    public class EducationController : ControllerBase
    {
        private readonly IEducationService service;
        private readonly IMapper mapper;
        private readonly TokenUltil tokenUltil;
        public EducationController(IEducationService service, IMapper mapper)
        {
            this.service = service;
            this.mapper = mapper;
            tokenUltil = new TokenUltil();
        }


        [HttpGet("alumni/api/educations"), Authorize(Roles = "tenant,alumni")]
        public async Task<ActionResult<List<EducationDTO>>> GetEducationByAlumniId()
        {
            try
            {
                var alumniId = tokenUltil.GetClaimByType(User, Constant.AlumniId).Value;
                var EducationList = await service.GetEducationByAlumniId(int.Parse(alumniId));
                var EducationDtoList = EducationList
                    .Select(e => mapper.Map<EducationDTO>(e))
                    .ToList();
                return Ok(EducationDtoList);
            }
            catch (Exception e)
            {
                if (e is NotFoundException)
                {
                    return NotFound(e.Message);
                }
                else
                {
                    return BadRequest(e.Message);
                }
            }
        }
        [HttpGet("alumni/api/alumni/{alumniId}/educations"), Authorize(Roles = "alumni")]
        public async Task<ActionResult<List<EducationDTO>>> GetEducationByid(
            [FromRoute] int alumniId
            )
        {
            try
            {
                var EducationList = await service.GetEducationByAlumniId(alumniId);
                var EducationDtoList = EducationList
                    .Select(e => mapper.Map<EducationDTO>(e))
                    .ToList();
                return Ok(EducationDtoList);
            }
            catch (Exception e)
            {
                if (e is NotFoundException)
                {
                    return NotFound(e.Message);
                }
                else
                {
                    return BadRequest(e.Message);
                }
            }
        }
        [HttpGet("tenant/api/alumni/{alumniId}/educations"), Authorize(Roles = "tenant")]
        public async Task<ActionResult<List<EducationDTO>>> GetEducationByidFortenant(
            [FromRoute] int alumniId
            )
        {
            try
            {
                var EducationList = await service.GetEducationByAlumniId(alumniId);
                var EducationDtoList = EducationList
                    .Select(e => mapper.Map<EducationDTO>(e))
                    .ToList();
                return Ok(EducationDtoList);
            }
            catch (Exception e)
            {
                if (e is NotFoundException)
                {
                    return NotFound(e.Message);
                }
                else
                {
                    return BadRequest(e.Message);
                }
            }
        }
        [HttpPost("alumni/api/educations"), Authorize(Roles = "tenant,alumni")]
        public async Task<ActionResult<int>> CreateEducation([FromBody] EducationAddDTO educationAddDTO)
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
                Education education = mapper.Map<Education>(educationAddDTO);
                education.AlumniId = int.Parse(alumniId);
                var EducationId = await service.CreateEducation(education);
                return Ok(EducationId);
            }
            catch (Exception e)
            {
                if (e is NotFoundException)
                {
                    return NotFound(e.Message);
                }
                else
                {
                    return BadRequest(e.Message);
                }
            }
        }
        [HttpPut("alumni/api/educations"),Authorize(Roles = "tenant,alumni")]
        public async Task<ActionResult<EducationDTO>> UpdateEducation([FromBody] EducationUpdateDTO educationUpdateDTO)
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
                var Education = await service.UpdateEducation(mapper.Map<Education>(educationUpdateDTO));
                return Ok(mapper.Map<EducationDTO>(Education));
            }
            catch (Exception e)
            {
                if (e is NotFoundException)
                {
                    return NotFound(e.Message);
                }
                else
                {
                    return BadRequest(e.Message);
                }
            }
        }
        [HttpDelete("alumni/api/educations/{educationId}"), Authorize(Roles = "tenant,alumni")]
        public async Task<ActionResult<string>> DeleteEducation([FromRoute]int educationId)
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
                await service.DeleteEducation(educationId);
                return Ok("Deleted education successful with id: "+educationId);
            }
            catch (Exception e)
            {
                if (e is NotFoundException)
                {
                    return NotFound(e.Message);
                }
                else
                {
                    return BadRequest(e.Message);
                }
            }
        }
    }
}
