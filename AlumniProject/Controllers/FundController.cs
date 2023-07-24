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
    [Route("")]
    [ApiController]
    public class FundController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IFundService _fundService;
        private readonly TokenUltil tokenUltil;

        public FundController(IMapper mapper, IFundService fundService)
        {
            _mapper = mapper;
            _fundService = fundService;
            tokenUltil = new TokenUltil();
        }

        [HttpGet("tenant/api/funds"), Authorize(Roles = "tenant")]
        public async Task<ActionResult<PagingResultDTO<FundDTO>>> GetFundsTenant(
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
                var funds = await _fundService.GetAllFundBySchoolId(pageNo, pageSize, int.Parse(schoolId));
                var fundDtos = funds.Items.Select(f => _mapper.Map<FundDTO>(f)).ToList();
                var result = new PagingResultDTO<FundDTO>()
                {
                    CurrentPage = funds.CurrentPage,
                    Items = fundDtos,
                    PageSize = funds.PageSize,
                    TotalItems = funds.TotalItems
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
        [HttpGet("alumni/api/funds"), Authorize(Roles = "alumni,tenant")]
        public async Task<ActionResult<PagingResultDTO<FundDTO>>> GetFundsalumni(
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
                var funds = await _fundService.GetAllFundBySchoolId(pageNo, pageSize, int.Parse(schoolId));
                var fundDtos = funds.Items.Select(f => _mapper.Map<FundDTO>(f)).ToList();
                var result = new PagingResultDTO<FundDTO>()
                {
                    CurrentPage = funds.CurrentPage,
                    Items = fundDtos,
                    PageSize = funds.PageSize,
                    TotalItems = funds.TotalItems
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
        [HttpPost("tenant/api/funds"), Authorize(Roles = "tenant")]
        public async Task<ActionResult<int>> CreateFunds(
            [FromBody] FundAddDTO fundAddDTO
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
                Fund fund = _mapper.Map<Fund>(fundAddDTO);
                fund.schoolId = int.Parse(schoolId);
                var fundId = await _fundService.CreateFund(fund);
                return Ok(fundId);
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
        [HttpPut("tenant/api/funds"), Authorize(Roles = "tenant")]
        public async Task<ActionResult<FundDTO>> UpdateFunds(
            [FromBody] FundUpdateDTO fundUpdateDTO
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
                var fund = _mapper.Map<Fund>(fundUpdateDTO);
                var fundUpdate = await _fundService.UpdateFund(fund);
                return Ok(_mapper.Map<Fund>(fundUpdate));
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
        [HttpDelete("tenant/api/funds"), Authorize(Roles = "tenant")]
        public async Task<ActionResult<FundDTO>> DeleteFunds(
           [FromQuery] int fundId
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

                await _fundService.DeleteFund(fundId);
                return Ok("Delete successful with id: " + fundId);
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
