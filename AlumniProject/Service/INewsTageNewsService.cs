using AlumniProject.Entity;

namespace AlumniProject.Service
{
    public interface INewsTageNewsService
    {
        Task<int> CreateNewsTagNews(NewsTagNew newsTagNew);
        Task<List<int>> CreateNewsTagNews(int newsId, List<int> tagIds);

        Task DeleteNewsTagNews(int id);
        Task<IEnumerable<TagsNew>> GetTagNewsByNewsId(int id);
    }
}
