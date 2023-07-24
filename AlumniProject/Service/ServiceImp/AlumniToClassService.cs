using AlumniProject.Data.Repostitory;
using AlumniProject.Dto;
using AlumniProject.Entity;
using AlumniProject.ExceptionHandler;
using System.Drawing.Printing;

namespace AlumniProject.Service.ServiceImp
{
    public class AlumniToClassService : IAlumniToClassService
    {
        private readonly Lazy<IClassService> _classService;
        private readonly Lazy<IGradeService> _gradeService;
        private readonly Lazy<IAlumniService> _alumniService;
        private readonly IAlumniToClassRepo _repo;
        public AlumniToClassService(Lazy<IClassService> classService, Lazy<IAlumniService> alumniService, IAlumniToClassRepo alumniToClassRepo, Lazy<IGradeService> gradeService)
        {
            this._classService = classService;
            this._alumniService = alumniService;
            this._repo = alumniToClassRepo;
            _gradeService = gradeService;
        }
        public async Task<int> CountAlumniByClassid(int classId)
        {
            var alumniClass = await _classService.Value.GetClassById(classId);
            if(alumniClass == null)
            {
                throw new NotFoundException("class not found with id: "+classId);
            }
            var count = await _repo.CountByCondition(c => c.ClassId == classId && c.Archived == true);
            return count;
        }

        public async Task<int> CreateAlumniToClass(AlumniToClass alumniToClass)
        {
            var alumniClass = await _classService.Value.GetClassById(alumniToClass.ClassId);
            if (alumniClass == null)
            {
                throw new NotFoundException("class not found with id: " + alumniToClass.ClassId);
            }
            var alumni = await _alumniService.Value.GetById(alumniToClass.ClassId);
            if (alumni == null)
            {
                throw new NotFoundException("Alumni not found with id: " + alumniToClass.AlumniId);
            }
            var alumniToClassId = await _repo.CreateAsync(alumniToClass);
            return alumniToClassId;
        }

        public async Task DeleteAlumniToClass(int id)
        {
            AlumniToClass alumniToClass = await _repo.GetByIdAsync(c => c.ClassId == id && c.Archived == true);
            if (alumniToClass == null)
            {
                throw new NotFoundException("alumniToClass not found with id: " + id);
            }
            alumniToClass.Archived = false;
            await _repo.UpdateAsync(alumniToClass);
        }

        public async Task<PagingResultDTO<Alumni>> GetAlumniByClassId(int pageNo,int pageSize, int classId)
        {
            var alumniToClassList = await _repo.GetAllByConditionAsync(c => c.ClassId == classId);
            List<Alumni> alumniList = new List<Alumni>();
            foreach(AlumniToClass a in alumniToClassList)
            {
                var alum = await _alumniService.Value.GetById(a.AlumniId);
                if(alum != null)
                {
                    alumniList.Add(alum);
                }
            }

            var totalItems = alumniList.Count;
            var skipAmount = (pageNo - 1) * pageSize;
            var pagedItems = alumniList.Skip(skipAmount).Take(pageSize).ToList();

            var alumniPage = new PagingResultDTO<Alumni>()
            {
                CurrentPage = pageNo,
                TotalItems = totalItems,
                PageSize = pageSize,
                Items = pagedItems.OrderBy(i => i.Id).ToList()
            };

            return alumniPage;
        }

        public async Task<List<Alumni>> GetAlumniByClassId(int classId)
        {
            var alumniToClassList = await _repo.GetAllByConditionAsync(c => c.ClassId == classId);
            List<Alumni> alumniList = new List<Alumni>();
            foreach (AlumniToClass a in alumniToClassList)
            {
                var alum = await _alumniService.Value.GetById(a.AlumniId);
                if (alum != null)
                {
                    alumniList.Add(alum);
                }
            }

            return alumniList;
        }

        public async Task<IEnumerable<int>> GetClassIdByAlumniId(int alumniId)
        {
            var classes = await _repo.GetAllByConditionAsync(c => c.AlumniId == alumniId && c.Archived == true);
            var classesId = classes.Select(classes => classes.ClassId).ToList().Distinct();
            return classesId;
        }

        public async Task<Dictionary<string, List<string>>> GetClassNameAndGradeNameByAlumniId(int alumniId)
        {
            var classes = await _repo.GetAllByConditionAsync(c => c.AlumniId == alumniId && c.Archived == true);
            var classesId = classes.Select(classes => classes.ClassId).ToList().Distinct();
            Dictionary<string, List<string>> result = new Dictionary<string, List<string>>();
            foreach (var id in classesId)
            {
                var clas = await _classService.Value.GetClassById(id);
                var grade = await _gradeService.Value.GetGradeById(clas.GradeId);
                if (result.ContainsKey(grade.Code))
                {
                    result[grade.Code].Add(clas.Name);
                }
                else
                {
                    result.Add(grade.Code, new List<string> { clas.Name });
                }
            }
            return result;
        }

        /* public Task<Alumni> GetAlumniByClassId(int classId)
         {
             var alumni = _alumniService.GetById(classId);
             if (alumni == null)
             {
                 throw new NotFoundException("Alumni not found with id: " + classId);
             }
             return alumni;
         }*/
    }
}
