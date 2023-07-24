using System.ComponentModel.DataAnnotations;

namespace AlumniProject.Dto
{
    public class PostAddDTO
    {

      
        [Required(ErrorMessage = "Content is required")]
        public string Content { get; set; }
        
    }
}
