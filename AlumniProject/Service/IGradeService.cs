using AlumniProject.Dto;
using AlumniProject.Entity;

namespace AlumniProject.Service;

public interface IGradeService
{
    public Task<Grade> GetGradeById(int id);
    public Task<int> CreateGrade(Grade grade);  
    public Task<Grade> UpdateGrade(Grade grade);
    public Task<int> DupplicateGrade (int id);
    public Task<PagingResultDTO<GradeDTO>> GetGradePagingResults(int pageNo, int pageSize,int schoolId);
    public Task<IEnumerable<Grade>> GetAllGradesBySchoolId(int schoolID);
    public Task<IEnumerable<Grade>> GetAllGradesByCode(int schoolID,string searchText);

    public Task<bool> IsExistedGrade(int gradeId);
    public Task DeleteGrade(int gradeId);

}
