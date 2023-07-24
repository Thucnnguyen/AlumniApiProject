using AlumniProject.Data.Repostitory;
using AlumniProject.Entity;
using AlumniProject.ExceptionHandler;

namespace AlumniProject.Service.ServiceImp;

public class EducationService : IEducationService
{
    private readonly IEducationRepo repo;
    private readonly IAlumniService alumniService;

    public EducationService(IEducationRepo repo, IAlumniService alumniService)
    {
        this.repo = repo;
        this.alumniService = alumniService;
    }

    public async Task<int> CreateEducation(Education education)
    {
        await VefifyEducation(education.AlumniId);
        var educationId = await repo.CreateAsync(education);
        return educationId;
    }

    public async Task DeleteEducation(int id)
    {
        var deleteEdu = await repo.GetByIdAsync(e => e.Id == id, e => e.Archived == true);
        if(deleteEdu == null)
        {
            throw new NotFoundException("Education not found with id: " + id);
        }
        await repo.DeleteByIdAsync(id);
    }

    public async Task<IEnumerable< Education>> GetEducationByAlumniId(int alumniId)
    {
        await VefifyEducation(alumniId);
        var EducationList = await repo.GetAllByConditionAsync(e => e.AlumniId == alumniId, e => e.Archived==true);
        return EducationList;
    }

    public async Task<Education> UpdateEducation(Education updateEducation)
    {
        var education = await repo.GetByIdAsync(e => e.Id==updateEducation.Id, e => e.Archived== true);
        if(education == null)
        {
            throw new NotFoundException("Education not found with id: " + updateEducation.Id);
        }
        education.StartDate = updateEducation.StartDate;
        education.EndDate = updateEducation.EndDate;
        education.Degree = updateEducation.Degree;
        education.School = updateEducation.School;
        await repo.UpdateAsync(education);
        return education;
    }

    private async Task VefifyEducation(int alumniId)
    {
        var alumni = await alumniService.GetById(alumniId);
        if(alumni == null)
        {
            throw new NotFoundException("Alumni not found with id: "+alumniId);
        }
    }
}
