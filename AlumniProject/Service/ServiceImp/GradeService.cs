using AlumniProject.Data.Repostitory;
using AlumniProject.Dto;
using AlumniProject.Entity;
using AlumniProject.ExceptionHandler;
using System.Diagnostics;

namespace AlumniProject.Service.ServiceImp;

public class GradeService : IGradeService
{
    private readonly IGradeRepo repo;
    private readonly Lazy<IClassService> classService;

    public GradeService(IGradeRepo repo, Lazy<IClassService> classService)
    {
        this.repo = repo;
        this.classService = classService;
    }

    public async Task<int> CreateGrade(Grade grade)
    {
        var existGradeCode = await repo.FindOneByCondition(g => g.Code == grade.Code && !string.IsNullOrEmpty(g.Code) && g.SchoolId == grade.SchoolId);
        if (existGradeCode != null)
        {
            throw new ConflictException("Code is exist");
        }
        else
        {
            var existGradeTime = await repo.FindOneByCondition(g => g.StartYear == grade.StartYear && g.SchoolId == grade.SchoolId);
            if (existGradeTime != null)
            {
                throw new ConflictException("Grade is exist");
            }
            grade.CreatedAt = DateTime.Now;
            var gradeId = await repo.CreateAsync(grade);
            return gradeId;
        }
    }

    public async Task DeleteGrade(int gradeId)
    {
        Grade grade = await GetGradeById(gradeId);
        grade.Archived = false;
        await repo.UpdateAsync(grade);
    }

    public async Task<int> DupplicateGrade(int id)
    {
        var grade = await GetGradeById(id);

        Grade duplicatedGrade = new Grade()
        {
            Code = string.Empty,
            StartYear = grade.StartYear + 1,
            EndYear = grade.EndYear + 1,
            SchoolId = grade.SchoolId,
            CreatedAt = DateTime.Now,
        };
        await CheckGradeIsExisted(grade, id, duplicatedGrade);

        var duplicatedGradeId = await repo.CreateAsync(duplicatedGrade);
        return duplicatedGradeId;
    }

    public async Task<IEnumerable<Grade>> GetAllGradesByCode(int schoolID, string searchText)
    {
        var grade = await repo.GetAllByConditionAsync(g => g.SchoolId == schoolID && g.Code.ToLower().Contains(searchText.ToLower()) && g.Archived == true);
        return grade;
    }

    public async Task<IEnumerable<Grade>> GetAllGradesBySchoolId(int schoolID)
    {
        var grades = await repo.GetAllByConditionAsync(g => g.SchoolId == schoolID && g.Archived == true);
        return grades;
    }

    public async Task<Grade> GetGradeById(int id)
    {
        var grade = await repo.GetByIdAsync(g => g.Id == id && g.Archived == true);
        if (grade == null)
        {
            throw new NotFoundException("Grade not found with id: " + id);
        }
        return grade;
    }

    public async Task<PagingResultDTO<GradeDTO>> GetGradePagingResults(int pageNo, int pageSize, int schoolId)
    {
        var gradeListResult = await repo.GetAllByConditionAsync(pageNo, pageSize, s => s.SchoolId == schoolId && s.Archived == true);
        List<GradeDTO> gradeDtoList = new List<GradeDTO>();

        foreach (var g in gradeListResult.Items)
        {
            var numberOfClasses = await classService.Value.CountClassByGradeId(g.Id);


            var gradeDto = new GradeDTO()
            {
                Code = g.Code,
                StartYear = g.StartYear,
                EndYear = g.EndYear,
                CreatedAt = g.CreatedAt,
                Id = g.Id,
                NumberOfClass = numberOfClasses
            };

            gradeDtoList.Add(gradeDto);
        }
        var GradeDtoResult = new PagingResultDTO<GradeDTO>()
        {
            Items = gradeDtoList,
            CurrentPage = gradeListResult.CurrentPage,
            PageSize = gradeListResult.PageSize,
            TotalItems = gradeListResult.TotalItems

        };
        return GradeDtoResult;
    }

    public async Task<bool> IsExistedGrade(int gradeId)
    {
        var Existed = await GetGradeById(gradeId);
        if (Existed != null)
        {
            return true;
        }
        return false;
    }

    public async Task<Grade> UpdateGrade(Grade updateGrade)
    {
        Grade existGrade = await GetGradeById(updateGrade.Id);
        await CheckGradeIsExisted(existGrade, updateGrade.Id, updateGrade);
        existGrade.StartYear = updateGrade.StartYear;
        existGrade.EndYear = updateGrade.EndYear;
        existGrade.Code = updateGrade.Code;

        await repo.UpdateAsync(existGrade);
        return existGrade;
    }

    private async Task CheckGradeIsExisted(Grade grade, int gradeId, Grade updateGrade)
    {
        if (grade == null)
        {
            throw new NotFoundException("Grade not found with id: " + gradeId);
        }
        var existGradeTime = await repo.FindOneByCondition(g => g.StartYear == updateGrade.StartYear && g.SchoolId == updateGrade.SchoolId && g.Archived == true);
        if (existGradeTime != null)
        {
            throw new ConflictException("Grade is exist");
        }
        if (!string.IsNullOrEmpty(updateGrade.Code))
        {
            var existGradeCode = await repo.FindOneByCondition(g => g.Code == updateGrade.Code && g.SchoolId == updateGrade.SchoolId);
            if (existGradeCode != null)
            {
                throw new ConflictException("Code is exist");
            }
        }

    }
}
