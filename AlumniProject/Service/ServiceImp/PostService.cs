using AlumniProject.Data.Repostitory;
using AlumniProject.Dto;
using AlumniProject.Entity;
using AlumniProject.ExceptionHandler;

namespace AlumniProject.Service.ServiceImp
{
    public class PostService : IPostService
    {
        private readonly IPostRepo _postRepo;
        private readonly ISchoolService _schoolService;
        private readonly IAlumniService _alumniService;
        public PostService(IPostRepo postRepo, IAlumniService alumniService,ISchoolService schoolService)
        {
            _postRepo = postRepo;
            _alumniService = alumniService;
            _schoolService = schoolService;
        }

        public async Task<int> CreatePost(Post post)
        {
            var alumni = await _alumniService.GetById(post.AlumniId);
            if(alumni == null)
            {
                throw new NotFoundException("Alumni not found with id:" + post.AlumniId);
            }
            var school = await _schoolService.GetSchoolById(post.SchoolId);
            if(school == null)
            {
                throw new NotFoundException("School not found with id:" + post.SchoolId);
            }
            var postId= await _postRepo.CreateAsync(post);
            return postId;
        }

        public async Task DeletePost(int postId)
        {
            Post post = await GetPostById(postId);
            post.Archived = false;
            await _postRepo.UpdateAsync(post);
            
        }

        public async Task<Post> GetPostById(int postId)
        {
            var post = await _postRepo.GetByIdAsync(p => p.Id == postId && p.Archived ==true);
            if(post == null)
            {
                throw new NotFoundException("Post not found with id:" + postId);
            }
            return post;
        }

        public Task<PagingResultDTO<Post>> GetPostPaging(int pageNo, int pageSize, int SchoolId)
        {
            var postsList = _postRepo.GetAllByConditionAsync(pageNo, pageSize, p => p.SchoolId == SchoolId && p.Archived == true);
            return postsList;
        }

        public async Task<Post> UpdatePost(Post updatedPost)
        {
            Post post = await GetPostById(updatedPost.Id);
            post.Content = updatedPost.Content;
            await _postRepo.UpdateAsync(post);
            return post;
        }
    }
}
