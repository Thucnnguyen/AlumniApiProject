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
    public class TagNewsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ITagNewsService _tagNewsService;
        private readonly TokenUltil tokenUltil;

        public TagNewsController(IMapper mapper, ITagNewsService tagNewsService)
        {
            _mapper = mapper;
            _tagNewsService = tagNewsService;
            tokenUltil = new TokenUltil();
        }


        [HttpGet("tenant/api/tagnews"), Authorize(Roles = "tenant")]
        public async Task<ActionResult<IEnumerable<TagDTO>>> GetTagNews()
        {
            try
            {
                var schoolId = tokenUltil.GetClaimByType(User, Constant.SchoolId).Value;
                var tagList = await _tagNewsService.GetTagsNewsBySchoolId(int.Parse(schoolId));
                var tagDtoList  = tagList.Select(t => _mapper.Map<TagDTO>(t)).ToList();
                return Ok(tagDtoList);
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

        [HttpPost("tenant/api/tagnews"), Authorize(Roles = "tenant")]
        public async Task<ActionResult<IEnumerable<int>>> CreateNews([FromBody] TagAddDTO tagAddDTO)
        {
            try
            {
                var schoolId = tokenUltil.GetClaimByType(User, Constant.SchoolId).Value;
                TagsNew tag = _mapper.Map<TagsNew>(tagAddDTO);
                tag.SchoolId = int.Parse(schoolId);
                var tagId = await _tagNewsService.CreateTagNes(tag);
                return Ok(tagId);
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

        [HttpPut("tenant/api/tagnews"), Authorize(Roles = "tenant")]
        public async Task<ActionResult<IEnumerable<TagDTO>>> UpdateTagNews([FromBody] TagUpdateDTO tagUpdateDTO)
        {
            try
            {
                var tag = _mapper.Map<TagsNew>(tagUpdateDTO);
                var tagUpdate = await _tagNewsService.UpdateTagNews(tag);
                return Ok(_mapper.Map<TagDTO>(tagUpdate));
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
        [HttpDelete("tenant/api/tagnews/{tagId}"), Authorize(Roles = "tenant")]
        public async Task<ActionResult<string>> DeleteTagNews([FromRoute] int tagId)
        {
            try
            {
                await _tagNewsService.DeteleTagNews(tagId);
                return Ok("Deleted Tagnews successful with id: "+tagId);
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
