using AlumniProject.Data.Repostitory;
using AlumniProject.Dto;
using AlumniProject.Entity;
using AlumniProject.ExceptionHandler;
using AutoMapper;
using System.Drawing;

namespace AlumniProject.Service.ServiceImp
{
    public class NewsService : INewsService
    {
        private readonly Lazy<IAlumniService> _alumniService;
        private readonly Lazy<ISchoolService> _schoolService;
        private readonly Lazy<ITagNewsService> _tagNewsService;
        private readonly Lazy<INewsTageNewsService> _newsTageNewsService;
        private readonly IMapper _mapper;
        private readonly INewRepo _repo;

        public NewsService(INewRepo repo, Lazy<IAlumniService> alumniService, Lazy<ISchoolService> schoolService, Lazy<ITagNewsService> tagNewsService,
            Lazy<INewsTageNewsService> newsTageNewsService, IMapper mapper)
        {
            _repo = repo;
            _alumniService = alumniService;
            _schoolService = schoolService;
            _tagNewsService = tagNewsService;
            _newsTageNewsService = newsTageNewsService;
            _mapper = mapper;
        }

        public async Task<int> CreateNews(News news, List<int> tagIds)
        {
            var alumni = await _alumniService.Value.GetById(news.AlumniId);
            if (alumni == null)
            {
                throw new NotFoundException("Alumni not found with id: " + news.AlumniId);
            }
            var school = await _schoolService.Value.GetSchoolById(news.SchoolId);
            if (school == null)
            {
                throw new NotFoundException("School not found with id: " + news.SchoolId);
            }
            var NewsId = await _repo.CreateAsync(news);
            foreach (var tagId in tagIds)
            {
                var t = await _tagNewsService.Value.GetTagsNewsById(tagId);
                if (t != null)
                {
                    await _newsTageNewsService.Value.CreateNewsTagNews(new NewsTagNew()
                    {
                        NewsId = NewsId,
                        TagsId = tagId
                    });
                }
            }
            return NewsId;
        }

        public async Task DeleteNews(int newsId, int schoolId)
        {
            News news = await _repo.GetByIdAsync(n => n.Id == newsId && n.Archived == true && n.SchoolId == schoolId);
            if (news == null)
            {
                throw new NotFoundException("NewsId not found id: " + newsId);
            }
            news.Archived = false;
            await _repo.UpdateAsync(news);
        }

        public async Task<IEnumerable<News>> GetLatestNewsBySchoolId(int size, int schoolId)
        {
            var school = await _schoolService.Value.GetSchoolById(schoolId);
            if (school == null)
            {
                throw new NotFoundException("School not found with id: " + schoolId);
            }
            var latestNews = (await _repo.GetAllByConditionAsync(n => n.SchoolId == schoolId && n.Archived == true && n.IsPublic == true))
                .OrderByDescending(n => n.CreatedAt)
                .Take(size)
                .ToList();
            return latestNews;
        }

        public async Task<News> GetNewsById(int id)
        {
            var news = await _repo.GetByIdAsync(n => n.Id == id && n.Archived == true);
            if (news == null)
            {
                throw new NotFoundException("News not found with Id: " + id);
            }
            return news;
        }

        public async Task<PagingResultDTO<NewsDTO>> GetNewsBySchoolIdWithCondition(int pageSize, int pageNo, int schoolId)
        {
            var count = await _repo.CountByCondition(n => n.IsPublic == true
            && n.SchoolId == schoolId
            && n.Archived == true);
            var NewsList = (await _repo.GetAllByConditionAsync(n => n.IsPublic == true
            && n.SchoolId == schoolId
            && n.Archived == true))
            .OrderByDescending(n => n.Id)
            .Skip((pageNo - 1) * pageSize)
            .Take(pageSize).ToList();

            List<NewsDTO> newsDTOs = new List<NewsDTO>();

            foreach (var news in NewsList)
            {
                var tags = await _newsTageNewsService.Value.GetTagNewsByNewsId(news.Id);
                var tagDTOs = tags.Select(t => _mapper.Map<TagDTO>(t));
                var alumni = await _alumniService.Value.GetById(news.AlumniId);

                NewsDTO newsDTO = new NewsDTO()
                {
                    Id = news.Id,
                    alumniName = alumni.FullName,
                    Content = news.Content,
                    IsPublic = news.IsPublic,
                    NewsImageUrl = news.NewsImageUrl,
                    tags = tagDTOs,
                    Title = news.Title
                };
                newsDTOs.Add(newsDTO);
            }

            var pageResult = new PagingResultDTO<NewsDTO>()
            {
                Items = newsDTOs,
                CurrentPage = pageNo,
                PageSize = pageSize,
                TotalItems = count,
            };
            return pageResult;
        }

        public async Task<PagingResultDTO<NewsDTO>> GetNewsBySchoolIdWithoutCondition(int pageSize, int pageNo, int schoolId)
        {
            var count = await _repo.CountByCondition(n => n.SchoolId == schoolId && n.Archived == true);
            var NewsList = (await _repo.GetAllByConditionAsync(n => n.SchoolId == schoolId && n.Archived == true))
            .OrderByDescending(n => n.Id)
            .Skip((pageNo - 1) * pageSize)
            .Take(pageSize).ToList();
            List<NewsDTO> newsDTOs = new List<NewsDTO>();
            foreach (var news in NewsList)
            {
                var tags = await _newsTageNewsService.Value.GetTagNewsByNewsId(news.Id);
                var tagDTOs = tags.Select(t => _mapper.Map<TagDTO>(t));
                var alumni = await _alumniService.Value.GetById(news.AlumniId);
                NewsDTO newsDTO = new NewsDTO()
                {
                    Id = news.Id,
                    alumniName = alumni.FullName,
                    Content = news.Content,
                    IsPublic = news.IsPublic,
                    NewsImageUrl = news.NewsImageUrl,
                    tags = tagDTOs,
                    Title = news.Title
                };
                newsDTOs.Add(newsDTO);
            }

            var pageResult = new PagingResultDTO<NewsDTO>()
            {
                Items = newsDTOs,
                CurrentPage = pageNo,
                PageSize = pageSize,
                TotalItems = count,
            };
            return pageResult;
        }

        public async Task<NewsDTO> GetNewsDtoById(int id)
        {
            var news = await _repo.GetByIdAsync(n => n.Id == id && n.Archived == true);
            if (news == null)
            {
                throw new NotFoundException("News not found with Id: " + id);
            }
            var tags = await _newsTageNewsService.Value.GetTagNewsByNewsId(news.Id);
            var tagDTOs = tags.Select(t => _mapper.Map<TagDTO>(t));
            var alumni = await _alumniService.Value.GetById(news.AlumniId);

            NewsDTO newsDTO = new NewsDTO()
            {
                Id = news.Id,
                alumniName = alumni.FullName,
                Content = news.Content,
                IsPublic = news.IsPublic,
                NewsImageUrl = news.NewsImageUrl,
                tags = tagDTOs,
                Title = news.Title
            };
            return newsDTO;
        }

        public async Task<News> UpdateNews(News newsUpdate)
        {
            News news = await _repo.GetByIdAsync(n => n.Id == newsUpdate.Id && n.Archived == true);
            if (news == null)
            {
                throw new NotFoundException("News not found with id: " + newsUpdate.Id);
            }
            news.Title = newsUpdate.Title;
            news.Content = newsUpdate.Content;
            news.NewsImageUrl = newsUpdate.NewsImageUrl;
            await _repo.UpdateAsync(news);
            return news;
        }
    }
}
