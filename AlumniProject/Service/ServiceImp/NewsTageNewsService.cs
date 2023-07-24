using AlumniProject.Data.Repostitory;
using AlumniProject.Entity;
using AlumniProject.ExceptionHandler;

namespace AlumniProject.Service.ServiceImp
{
    public class NewsTageNewsService : INewsTageNewsService
    {
        private readonly ITagNewsService tagNewsService;
        private readonly INewsService newsService;
        private readonly INewsTagNewRepo newsTagNewRepo;
        public NewsTageNewsService(ITagNewsService tagNewsService, INewsService newsService, INewsTagNewRepo newsTagNewRepo)
        {
            this.tagNewsService = tagNewsService;
            this.newsService = newsService;
            this.newsTagNewRepo = newsTagNewRepo ;
        }

        public async Task<int> CreateNewsTagNews(NewsTagNew newsTagNew)
        {
            var tag = await tagNewsService.GetTagsNewsById(newsTagNew.TagsId) ;
            if(tag == null)
            {
                throw new NotFoundException("Tag not found with id: "+ newsTagNew.TagsId);
            }
            var News = await newsService.GetNewsById(newsTagNew.NewsId);
            if (News == null)
            {
                throw new NotFoundException("News not found with id: " + newsTagNew.NewsId);
            }
            var id = await newsTagNewRepo.CreateAsync(newsTagNew);
            return id;
        }

        public async Task<List<int>> CreateNewsTagNews(int newsId, List<int> tagIds)
        {
            List<int> listId = new List<int>();
            var News = await newsService.GetNewsById(newsId);
            if (News == null)
            {
                throw new NotFoundException("News not found with id: " + newsId);
            }

            foreach(var tagId in tagIds)
            {
                var tag = await tagNewsService.GetTagsNewsById(tagId);
                if(tag != null)
                {
                    var newsTagNew = newsTagNewRepo.FindOneByCondition(n => n.NewsId == newsId && n.TagsId == tagId && n.Archived == true);
                    if(newsTagNew == null)
                    {
                        var id = await newsTagNewRepo.CreateAsync(new NewsTagNew()
                        {
                            NewsId=newsId,
                            TagsId= tagId,
                        });
                        listId.Add(id);
                    }
                }
            }
            return listId;
        }

        public async Task DeleteNewsTagNews(int id)
        {
            NewsTagNew newsTagNew = await newsTagNewRepo.GetByIdAsync( n => n.Id == id);
            if(newsTagNew == null)
            {
                throw new NotFoundException("newsTagNew not found with id: " + id);
            }
            newsTagNew.Archived = false;
            await newsTagNewRepo.UpdateAsync(newsTagNew);
        }

        public async Task<IEnumerable<TagsNew>> GetTagNewsByNewsId(int newsId)
        {
            var newsTagNewList = await newsTagNewRepo.GetAllByConditionAsync(n => n.NewsId == newsId && n.Archived == true);
            var tags = new List<TagsNew>();

            foreach (var newsTagNew in newsTagNewList)
            {
                var tag = await tagNewsService.GetTagsNewsById(newsTagNew.TagsId);
                tags.Add(tag);
            }

            return tags;
        }
    }
}
