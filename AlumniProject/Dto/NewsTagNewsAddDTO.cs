using System.ComponentModel.DataAnnotations;

namespace AlumniProject.Dto
{
    public class NewsTagNewsAddDTO
    {
        [Required(ErrorMessage ="NewsId is required")]
        public int NewsId { get; set; }
        [Required(ErrorMessage = "TagIds is required")]

        public List<int> TagIds { get; set; }
    }
}
