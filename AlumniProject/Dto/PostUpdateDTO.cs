using System.ComponentModel.DataAnnotations;

namespace AlumniProject.Dto
{
    public class PostUpdateDTO
    {
        [Required(ErrorMessage ="Id is required")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Content is required")]
        public string Content { get; set; }
    }
}
