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
using System.Data;

namespace AlumniProject.Controllers
{
    [Route("api")]
    [ApiController]
    public class PostController : ControllerBase
    {
        //private readonly IPostService _postService;
        //private readonly IMapper _mapper;
        //private readonly TokenUltil tokenUltil;

        //public PostController(IPostService postService, IMapper mapper)
        //{
        //    _postService = postService;
        //    _mapper = mapper;
        //    tokenUltil = new TokenUltil();
        //}

        //[HttpGet("posts"), Authorize(Roles = "tenant,alumni")]
        //public async Task<ActionResult<PagingResultDTO<PostDTO>>> GetPostsBySchoolId(
        //    [FromQuery, Range(1, int.MaxValue), Required(ErrorMessage = "PageNo is required")] int pageNo = 1,
        //    [FromQuery, Range(1, int.MaxValue), Required(ErrorMessage = "PageSize is required")] int pageSize = 10
        //)
        //{
        //    try
        //    {
        //        var errorMessages = ModelState.Values
        //            .SelectMany(v => v.Errors)
        //            .Select(e => e.ErrorMessage)
        //            .ToList();

        //        if (errorMessages.Any())
        //        {
        //            return BadRequest(string.Join(", ", errorMessages));
        //        }
        //        var schoolId = tokenUltil.GetClaimByType(User, Constant.SchoolId).Value;

        //        var postList = await _postService.GetPostPaging(pageNo, pageSize, int.Parse(schoolId));
        //        if (postList == null)
        //        {
        //            return NoContent();
        //        }

        //        var postDtoList = new PagingResultDTO<PostDTO>()
        //        {
        //            CurrentPage = postList.CurrentPage,
        //            PageSize = postList.PageSize,
        //            TotalItems = postList.TotalItems,
        //            Items = postList.Items.Select(p => _mapper.Map<PostDTO>(p)).ToList()
        //        };

        //        return Ok(postDtoList);
        //    }
        //    catch (Exception e)
        //    {
        //        if (e is NotFoundException)
        //        {
        //            return NotFound(e.Message);
        //        }
        //        else if (e is BadRequestException)
        //        {
        //            return BadRequest(e.Message);
        //        }
        //        else
        //        {
        //            return Conflict(e.Message);
        //        }
        //    }
        //}

        //[HttpPost("posts"),Authorize(Roles = "alumni,tenant")]
        //public async Task<ActionResult<int>> CreatePost([FromBody] PostAddDTO postAddDTO)
        //{
        //    try
        //    {
        //        var errorMessages = ModelState.Values
        //            .SelectMany(v => v.Errors)
        //            .Select(e => e.ErrorMessage)
        //            .ToList();

        //        if (errorMessages.Any())
        //        {
        //            return BadRequest(string.Join(", ", errorMessages));
        //        }
        //        var schoolId = tokenUltil.GetClaimByType(User, Constant.SchoolId).Value;
        //        var alumniId = tokenUltil.GetClaimByType(User, Constant.AlumniId).Value;
        //        Post post = _mapper.Map<Post>(postAddDTO);
        //        post.AlumniId = int.Parse(alumniId);
        //        post.SchoolId = int.Parse(schoolId);
        //        var postId = await _postService.CreatePost(post);
        //        return Ok(postId);
        //    }
        //    catch (Exception e)
        //    {
        //        if (e is NotFoundException)
        //        {
        //            return NotFound(e.Message);
        //        }
        //        else if (e is BadRequestException)
        //        {
        //            return BadRequest(e.Message);
        //        }
        //        else
        //        {
        //            return Conflict(e.Message);
        //        }
        //    }
        //}

        //[HttpPut("posts"), Authorize(Roles = "alumni,tenant")]
        //public async Task<ActionResult<PostDTO>> UpdatePost([FromBody] PostUpdateDTO postUpdateDTO)
        //{
        //    try
        //    {
        //        var errorMessages = ModelState.Values
        //            .SelectMany(v => v.Errors)
        //            .Select(e => e.ErrorMessage)
        //            .ToList();

        //        if (errorMessages.Any())
        //        {
        //            return BadRequest(string.Join(", ", errorMessages));
        //        }

        //        var updatedPost = _mapper.Map<Post>(postUpdateDTO);
        //        var post = await _postService.UpdatePost(updatedPost);
        //        return Ok(_mapper.Map<PostDTO>(post));
        //    }
        //    catch (Exception e)
        //    {
        //        if (e is NotFoundException)
        //        {
        //            return NotFound(e.Message);
        //        }
        //        else if (e is BadRequestException)
        //        {
        //            return BadRequest(e.Message);
        //        }
        //        else
        //        {
        //            return Conflict(e.Message);
        //        }
        //    }
        //}

        //[HttpDelete("posts"), Authorize(Roles = "alumni,tenant")]
        //public async Task<ActionResult<string>> DeletePost([FromQuery] int postId)
        //{
        //    try
        //    {
        //        var errorMessages = ModelState.Values
        //            .SelectMany(v => v.Errors)
        //            .Select(e => e.ErrorMessage)
        //            .ToList();

        //        if (errorMessages.Any())
        //        {
        //            return BadRequest(string.Join(", ", errorMessages));
        //        }

        //        await _postService.DeletePost(postId);
        //        return Ok($"Post deleted successfully with ID: {postId}");
        //    }
        //    catch (Exception e)
        //    {
        //        if (e is NotFoundException)
        //        {
        //            return NotFound(e.Message);
        //        }
        //        else if (e is BadRequestException)
        //        {
        //            return BadRequest(e.Message);
        //        }
        //        else
        //        {
        //            return Conflict(e.Message);
        //        }
        //    }
        //}
    }
}
