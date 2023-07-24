using AlumniProject.Dto;
using AlumniProject.Entity;
using AlumniProject.ExceptionHandler;
using AlumniProject.Service;
using AlumniProject.Ultils;
using AutoMapper;
using Firebase.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace AlumniProject.Controllers
{
    [Route("")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _newsService;
        private readonly IAlumniService _alumniService;
        private readonly IMapper _mapper;
        private readonly TokenUltil tokenUltil;

        public NewsController(INewsService newsService, IMapper mapper,IAlumniService alumniService)
        {
            _newsService = newsService;
            _mapper = mapper;
            tokenUltil = new TokenUltil();
            _alumniService = alumniService;
        }

        [HttpGet("tenant/api/news"), Authorize(Roles = "tenant")]
        public async Task<ActionResult<PagingResultDTO<NewsDTO>>> GetNewsTenant(
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

                var nesDtoList = await _newsService.GetNewsBySchoolIdWithoutCondition(pageSize, pageNo, int.Parse(schoolId));
                return Ok(nesDtoList);
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
        [HttpGet("tenant/api/news/{newsId}"), Authorize(Roles = "tenant")]
        public async Task<ActionResult<PagingResultDTO<NewsDTO>>> GetNewsDetail(
                [FromRoute] int newsId
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

                var news = await _newsService.GetNewsDtoById(newsId);
                return Ok(news);
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
        [HttpGet("alumni/api/news/{newsId}"), Authorize(Roles = "alumni")]
        public async Task<ActionResult<PagingResultDTO<NewsDTO>>> GetNewsDetailForAlumni(
                [FromRoute] int newsId
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

                var news = await _newsService.GetNewsDtoById(newsId);
                return Ok(news);
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
        [HttpGet("alumni/api/news/latest"), Authorize(Roles = "tenant,alumni")]
        public async Task<ActionResult<IEnumerable<NewsDTO>>> GetNewsAlumni(
             [FromQuery, Range(1, int.MaxValue), Required(ErrorMessage = "Size is Required")] int size = 1
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
                var EventsList = await _newsService.GetLatestNewsBySchoolId(size, int.Parse(schoolId));
                var EventsDTOList = EventsList.Select(e => _mapper.Map<NewsDTO>(e)).ToList();

                return Ok(EventsDTOList);
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
        [HttpGet("alumni/api/news"), Authorize(Roles = "tenant,alumni")]
        public async Task<ActionResult<PagingResultDTO<NewsDTO>>> GetNewsAlumni(
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
                var NewsListDto = await _newsService.GetNewsBySchoolIdWithCondition(pageSize, pageNo, int.Parse(schoolId));
                return Ok(NewsListDto);
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
        [HttpPost("tenant/api/news"), Authorize(Roles = "tenant")]
        public async Task<ActionResult<int>> CreateNews([FromBody] NewsAddDTO newsAddDTO)
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
                var alumniId = tokenUltil.GetClaimByType(User, Constant.AlumniId).Value;
                News news = _mapper.Map<News>(newsAddDTO);
                news.AlumniId = int.Parse(alumniId);
                news.SchoolId = int.Parse(schoolId);
                var newsId = await _newsService.CreateNews(news, newsAddDTO.tagIds);
                return Ok(newsId);
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
        [HttpPut("tenant/api/news"), Authorize(Roles = "tenant")]
        public async Task<ActionResult<NewsDTO>> UpdateNews([FromBody] NewsUpdateDTO newsUpdateDTO)
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
                var news = await _newsService.UpdateNews(_mapper.Map<News>(newsUpdateDTO));
                return Ok(_mapper.Map<NewsDTO>(news));
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
        [HttpDelete("tenant/api/news/{newsId}"), Authorize(Roles = "tenant")]
        public async Task<ActionResult<string>> deleteNews([FromRoute] int newsId)
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

                await _newsService.DeleteNews(newsId, int.Parse(schoolId));
                return Ok("Deleted news successful with id: " + newsId);
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
