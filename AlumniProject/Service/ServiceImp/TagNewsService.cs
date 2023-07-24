using AlumniProject.Data.Repostitory;
using AlumniProject.Entity;
using AlumniProject.ExceptionHandler;

namespace AlumniProject.Service.ServiceImp
{
    public class TagNewsService : ITagNewsService
    {

        private readonly ITagnewRepo _tagnewRepo;
        private readonly ISchoolService _schoolService;
        public TagNewsService(ITagnewRepo tagnewRepo, ISchoolService schoolService)
        {
            _tagnewRepo = tagnewRepo;
            _schoolService = schoolService;
        }

        public async Task<int> CreateTagNes(TagsNew tagsNew)
        {
            var school = await _schoolService.GetSchoolById(tagsNew.SchoolId);
            if(school == null)
            {
                throw new NotFoundException("School not found with Id: " + tagsNew.SchoolId);
            }
            var tagNesId = await _tagnewRepo.CreateAsync(tagsNew);
            return tagNesId;
        }

        public async Task DeteleTagNews(int tagId)
        {
            TagsNew tagsNew = await GetTagsNewsById(tagId);
            if(tagsNew == null)
            {
                throw new NotFoundException("TagsNews not found with Id: " + tagId);
            }
            tagsNew.Archived = false;
            await _tagnewRepo.UpdateAsync(tagsNew);
        }

        public async Task<TagsNew> GetTagsNewsById(int tagId)
        {
            TagsNew tagsNew = await _tagnewRepo.GetByIdAsync(t => t.Id == tagId && t.Archived == true);
            if (tagsNew == null)
            {
                throw new NotFoundException("TagsNews not found with Id: " + tagId);
            }
            return tagsNew;
        }

        public async Task<IEnumerable<TagsNew>> GetTagsNewsBySchoolId(int SchoolId)
        {
            var TagsNewsList = await _tagnewRepo.GetAllByConditionAsync(t => t.SchoolId == SchoolId && t.Archived == true);
            return TagsNewsList;
        }

        public async Task<TagsNew> UpdateTagNews(TagsNew updatedTagsNew)
        {
            TagsNew tagsNew = await _tagnewRepo.GetByIdAsync(t => t.Id == updatedTagsNew.Id && t.Archived == true);
            if (tagsNew == null)
            {
                throw new NotFoundException("TagsNews not found with Id: " + tagsNew.SchoolId);
            }
            tagsNew.TagName = updatedTagsNew.TagName;
            await _tagnewRepo.UpdateAsync(tagsNew);
            return tagsNew;
        }
    }
}
