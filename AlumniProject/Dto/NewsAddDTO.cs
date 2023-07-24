using System.ComponentModel.DataAnnotations;

namespace AlumniProject.Dto
{
    public class NewsAddDTO
    {
        [Required(ErrorMessage ="Title is required")]
        public string Title { get; set; }
        [ Required(ErrorMessage = "Content is required")]
        public string Content { get; set; }
        [Required(ErrorMessage = "NewsImageUrl is required")]
        public string NewsImageUrl { get; set; }
        public List<int> tagIds { get; set; }
        
        
    }
}
