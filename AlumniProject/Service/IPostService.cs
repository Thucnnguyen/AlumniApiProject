using AlumniProject.Dto;
using AlumniProject.Entity;

namespace AlumniProject.Service
{
    public interface IPostService
    {
        Task<int> CreatePost(Post post);
        Task<Post> UpdatePost(Post post);
        Task DeletePost(int postId);
        Task<PagingResultDTO<Post>> GetPostPaging(int pageNo, int pageSize, int SchoolId);
        Task<Post> GetPostById(int postId);
    }
}
