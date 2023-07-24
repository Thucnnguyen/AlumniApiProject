using AlumniProject.Dto;
using AlumniProject.Entity;

namespace AlumniProject.Service
{
    public interface INewsService
    {
        Task<int> CreateNews(News news,List<int>tagIds);
        Task<News> UpdateNews(News news);
        Task DeleteNews(int id, int newsId);
        Task<PagingResultDTO<NewsDTO>> GetNewsBySchoolIdWithoutCondition(int pageSize, int pageNo,int schoolId);
        Task<PagingResultDTO<NewsDTO>> GetNewsBySchoolIdWithCondition(int pageSize, int pageNo, int schoolId);
        Task<News> GetNewsById(int id);
        Task<NewsDTO> GetNewsDtoById(int id);

        Task<IEnumerable<News>> GetLatestNewsBySchoolId(int size, int schoolId);

    }
}
