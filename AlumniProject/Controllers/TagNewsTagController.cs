using AlumniProject.Dto;
using AlumniProject.ExceptionHandler;
using AlumniProject.Service;
using AlumniProject.Ultils;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AlumniProject.Controllers
{
    [Route("")]
    [ApiController]
    public class TagNewsTagController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly INewsTageNewsService _newsTageNewsService;

        public TagNewsTagController(IMapper mapper, INewsTageNewsService newsTageNewsService)
        {
            _mapper = mapper;
            _newsTageNewsService = newsTageNewsService;

        }

        [HttpGet("alumni/api/news/{newId}/tagNews"),Authorize(Roles = "alumni,tenant")]
        public async Task<ActionResult<IEnumerable<TagDTO>>> GetTagByNewsId([FromRoute]int newId)
        {
            try
            {
                var tag = await _newsTageNewsService.GetTagNewsByNewsId(newId);
                var tagDTO = tag.Select(t => _mapper.Map<TagDTO>(t)).ToList();
                return Ok(tagDTO);
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
        [HttpDelete("tenant/api/newsTagNews/{newsTagNewsId}"), Authorize(Roles = "tenant")]
        public async Task<ActionResult<string>> DeleteNewsTagNewsById([FromRoute] int newsTagNewsId)
        {
            try
            {
                await _newsTageNewsService.DeleteNewsTagNews(newsTagNewsId);
                return Ok("Delete newsTagNews success with id:"+newsTagNewsId);
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
        [HttpPost("tenant/api/newsTagNews"), Authorize(Roles = "tenant")]
        public async Task<ActionResult<int>> CreateNewsTagNewsById([FromBody] NewsTagNewsAddDTO newsTagNewsAddDTO)
        {
            try
            {
                var listId = await _newsTageNewsService.CreateNewsTagNews(newsTagNewsAddDTO.NewsId, newsTagNewsAddDTO.TagIds);
                return Ok(listId);
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
