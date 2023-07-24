using AlumniProject.Dto;
using AlumniProject.Entity;
using AlumniProject.ExceptionHandler;
using AlumniProject.Service;
using AlumniProject.Ultils;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AlumniProject.Controllers
{
    [Route("")]
    [ApiController]
    public class GradeController : ControllerBase
    {
        private readonly IGradeService service;
        private readonly IMapper mapper;
        private readonly TokenUltil tokenUltil;
        private readonly IClassService classService;

        public GradeController(IGradeService gradeService, IMapper mapper)
        {
            this.service = gradeService;
            this.mapper = mapper;
            tokenUltil = new TokenUltil();
        }


        [HttpGet("tenant/api/grades"),Authorize(Roles ="tenant")]
        public async Task<ActionResult<PagingResultDTO<GradeDTO>>> GetGrades(
            [FromQuery, Range(1, int.MaxValue)] int pageNo = 1,
            [FromQuery, Range(1, int.MaxValue)] int pageSize= 10
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
                var GradeList = await service.GetGradePagingResults( pageNo, pageSize, int.Parse(schoolId));
                var GradeDTOList = new PagingResultDTO<GradeDTO>
                {
                    Items = GradeList.Items.Select(g => mapper.Map<GradeDTO>(g)).ToList(),
                    CurrentPage = GradeList.CurrentPage,
                    PageSize = GradeList.PageSize,
                    TotalItems = GradeList.TotalItems
                };
                return Ok(GradeDTOList);
            }
            catch(Exception e)
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
        [HttpGet("alumni/api/grades")]
        public async Task<ActionResult<IEnumerable<GradeDTO>>> GetGradesForAlumni(
            [FromQuery] int SchoolId
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
                var GradeList = await service.GetAllGradesBySchoolId( SchoolId);
                var GradeDTOList = GradeList.Select(g =>mapper.Map<GradeDTO>(g));
                return Ok(GradeDTOList);
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

        [HttpGet("alumni/api/school/grades"), Authorize(Roles = "alumni")]
        public async Task<ActionResult<IEnumerable<GradeDTO>>> GetGradesForAlumniBySchoolId(
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
                //var GradeList = await service.GetAllGradesBySchoolId(int.Parse(schoolId));
                var GradeList = await service.GetGradePagingResults(pageNo, pageSize, int.Parse(schoolId));
                return Ok(GradeList);
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

        [HttpGet("tenant/api/grades/code"), Authorize(Roles = "tenant")]
        public async Task<ActionResult<IEnumerable<GradeDTO>>> GetGradesByCode(
           [FromQuery] string searchText
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

                var GradeList = await service.GetAllGradesByCode(int.Parse(schoolId),searchText);
                var GradeDTOList = GradeList.Select(g => mapper.Map<GradeDTO>(g));
                return Ok(GradeDTOList);
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
        [HttpGet("alumni/api/grades/code"), Authorize(Roles = "alumni")]
        public async Task<ActionResult<IEnumerable<GradeDTO>>> GetGradesByCodeForAlumni(
           [FromQuery] string? searchText 
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

                var GradeList = await service.GetAllGradesByCode(int.Parse(schoolId), searchText != null? searchText : "");
                var GradeDTOList = GradeList.Select(g => mapper.Map<GradeDTO>(g));
                return Ok(GradeDTOList);
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
        [HttpPost("tenant/api/grades"), Authorize(Roles = "tenant")]
        public async Task<ActionResult<int>> CreateGrade([FromBody] GradeAddDTO gradeAddDTO)
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
                Grade grade = mapper.Map<Grade>(gradeAddDTO);
                grade.SchoolId = int.Parse(schoolId);
                var gradeId = await service.CreateGrade(grade);
                return Ok(gradeId);
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
        [HttpPost("tenant/api/grades/{gradeId}/duplicates"), Authorize(Roles = "tenant")]
        public async Task<ActionResult<int>> DupplicatedGrade([FromRoute] int gradeId)
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
                var gradeNewId = await service.DupplicateGrade(gradeId);
                return Ok(gradeNewId);
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

        [HttpPut("tenant/api/grades"), Authorize(Roles = "tenant")]
        public async Task<ActionResult<GradeDTO>> UpdateGrade([FromBody]GradeUpdateDTO gradeUpdateDTO)
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
                var gradeUpdate = await service.UpdateGrade(mapper.Map<Grade>(gradeUpdateDTO));
                return Ok(mapper.Map<GradeDTO>(gradeUpdate));
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

        [HttpDelete("tenant/api/grades/{gradeId}"), Authorize(Roles = "tenant")]
        public async Task<ActionResult<string>> DeleteGrade([FromRoute] int gradeId)
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
                await service.DeleteGrade(gradeId);
                return Ok("Deleted successful grade with id :" +gradeId);
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
        [HttpPost("tenant/api/grades/file")]
        public async Task<ActionResult<int>> AddClassFromFile(IFormFile file)
        {
            var schoolId = tokenUltil.GetClaimByType(User, Constant.SchoolId).Value;

            await ReadExcelFile(file,int.Parse(schoolId));
            return Ok();
        }
        private async Task ReadExcelFile(IFormFile file,int schoolId)
        {
            if (file.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" &&
                Path.GetExtension(file.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                using (var stream = new MemoryStream())
                {
                    file.CopyTo(stream);
                    ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.Commercial;
                    using (var excelPackage = new ExcelPackage(stream))
                    {
                        ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[0];

                        int rowCount = worksheet.Dimension.Rows;
                        int colCount = worksheet.Dimension.Columns;



                        for (int row = 2; row <= rowCount; row++)
                        {

                            var code = worksheet.Cells[row, 2].Value?.ToString() ?? "";
                            var endYear = worksheet.Cells[row, 3].Value?.ToString() ?? "";
                            var grade = new Grade
                            {
                                Code = code,
                                EndYear = Int32.Parse(endYear),
                                StartYear = Int32.Parse(endYear) - 3,
                                CreatedAt = DateTime.Now,
                                SchoolId = schoolId,
                            };
                            var gradeId = await service.CreateGrade(grade);
                            List<AlumniClass> classes = new List<AlumniClass>();
                            for (int col = 4; col <= colCount; col++)
                            {
                                var cellValue = worksheet.Cells[row, col].Value?.ToString() ?? "";
                                if (cellValue != "")
                                {
                                    //classes.Add(new AlumniClass
                                    //{
                                    //    Name = cellValue,
                                    //    GradeId = gradeId,
                                    //    CreatedAt = DateTime.Now,
                                    //});
                                    await classService.CreateClass(new AlumniClass
                                    {
                                        Name = cellValue,
                                        GradeId = gradeId,
                                        CreatedAt = DateTime.Now,
                                    });
                                }
                                //await classService.CreateClassRange(classes);
                            }

                        }
                    }
                }
            }
            else
            {
            }
        }
    }
}
