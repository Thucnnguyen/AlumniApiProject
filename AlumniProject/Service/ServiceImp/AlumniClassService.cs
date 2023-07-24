using AlumniProject.Data.Repostitory;
using AlumniProject.Dto;
using AlumniProject.Entity;
using AlumniProject.ExceptionHandler;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;

namespace AlumniProject.Service.ServiceImp;

public class AlumniClassService : IClassService
{
    private readonly IAlumniClassRepo repo;
    
    private readonly Lazy<IGradeService> gradeService;
    private readonly Lazy<IAlumniToClassService> alumniToClassService;
    public AlumniClassService(IAlumniClassRepo repo, Lazy<IGradeService> gradeService, Lazy<IAlumniToClassService> alumniToClassService)
    {
        this.repo = repo;
        this.gradeService = gradeService;
        this.alumniToClassService = alumniToClassService;
    }

    public async Task<int> CountClassByGradeId(int gradeId)
    {
        var isExistedGrade = await gradeService.Value.IsExistedGrade(gradeId);
        if (!isExistedGrade)
        {
            throw new NotFoundException("Grade not found with id: " + gradeId);
        }

        var size = await repo.CountByCondition(c => c.GradeId == gradeId && c.Archived == true);
        return size;
    }

    public async Task<int> CreateClass(AlumniClass alumniClass)
    {
        await ValidateClass(alumniClass.GradeId,alumniClass.Name);

        alumniClass.CreatedAt = DateTime.Now;
        int newClassId =  await repo.CreateAsync(alumniClass);
        return newClassId;
    }

    public async Task<AlumniClass> GetClassById(int id)
    {
        var classById = await repo.GetByIdAsync(c=> c.Id == id, c =>c.Archived == true);
        if(classById == null)
        {
            throw new NotFoundException("Class not found with id: "+id);
        }
        return classById;
    }

    public async Task<PagingResultDTO<AlumniClassDTO>> GetClassPagingResults(int pageNo, int pageSize, int gradeId)
    {
        var isExistedGrade = await gradeService.Value.IsExistedGrade(gradeId);
        if (!isExistedGrade)
        {
            throw new NotFoundException("Grade not found with id: " + gradeId);
        }
        var ClassList = await repo.GetAllByConditionAsync(pageNo, pageSize, g => g.GradeId == gradeId, g => g.Archived == true);
        List<AlumniClassDTO> ClassDtoList = new List<AlumniClassDTO>();

        foreach(var Class in ClassList.Items)
        {
            var count = await alumniToClassService.Value.CountAlumniByClassid(Class.Id);
            var classDto = new AlumniClassDTO()
            {
                CreatedDate = Class.CreatedAt,
                Name = Class.Name,
                Id = Class.Id,
                NumberOfAlumni = count
            };
            ClassDtoList.Add(classDto);
        }
        
        var pageResult = new PagingResultDTO<AlumniClassDTO>()
        {
            CurrentPage = ClassList.CurrentPage,
            PageSize = ClassList.PageSize,
            TotalItems = ClassList.PageSize,
            Items = ClassDtoList
        };
        return pageResult;
    }

    public async Task<AlumniClass> UpdateClass(AlumniClass updatedAlumniClass)
    {
        
        AlumniClass classById = await GetClassById(updatedAlumniClass.Id);
        var nameExisted = await repo.FindOneByCondition(c => c.Name == updatedAlumniClass.Name && c.GradeId == classById.GradeId&& c.Archived == true);
        if (nameExisted != null)
        {
            throw new BadRequestException("Name is exist");
        }
        classById.Name = updatedAlumniClass.Name;
        await repo.UpdateAsync(classById);
        return classById;
    }
    public async Task DeleteClassById(int id,int schoolID)
    {
        AlumniClass classes = await GetClassById(id);
        var grade = await gradeService.Value.GetGradeById(classes.GradeId);
        if (grade != null && grade.SchoolId == schoolID)
        {
            classes.Archived = false;
            await repo.UpdateAsync(classes);
        }
        else
        {
            throw new ConflictException("You not have permission");
        }
    }
    private async Task ValidateClass(int gradeId,String name)
    {
        var grade= await gradeService.Value.GetGradeById(gradeId);
        if (grade == null)
        {
            throw new BadRequestException("grade  not exist");
        }
        var nameExisted = await repo.FindOneByCondition(c => c.Name == name , c => c.GradeId == gradeId);
        if (nameExisted != null)
        {
            throw new ConflictException("Name is exist");
        }
    }

    public async Task<IEnumerable<AlumniClass>> GetClassByGradeId(int gradeId)
    {
        var isExistedGrade = await gradeService.Value.IsExistedGrade(gradeId);
        if (!isExistedGrade)
        {
            throw new NotFoundException("Grade not found with id: " + gradeId);
        }

        var classesList = await repo.GetAllByConditionAsync(c => c.GradeId == gradeId && c.Archived == true);
        return classesList;
    }
}
