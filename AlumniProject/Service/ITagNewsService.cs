using AlumniProject.Entity;

namespace AlumniProject.Service
{
    public interface ITagNewsService
    {
        Task<int> CreateTagNes(TagsNew tagsNew);
        Task<TagsNew> UpdateTagNews(TagsNew tagsNew);
        Task DeteleTagNews(int tagId);    
        Task<IEnumerable<TagsNew>> GetTagsNewsBySchoolId(int SchoolId);
        Task<TagsNew> GetTagsNewsById(int tagId);

    }
}
