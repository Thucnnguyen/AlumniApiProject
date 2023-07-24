using System.ComponentModel.DataAnnotations;

namespace AlumniProject.Dto
{
    public class GradeAddDTO
    {
        public string Code { get; set; }
        [Required(ErrorMessage = "StartYear is required")]
        public int StartYear { get; set; }
        [Required(ErrorMessage = "EndYear is required")]
        public int EndYear { get; set; }
        
    }
}
