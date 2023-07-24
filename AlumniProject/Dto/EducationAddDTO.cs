using System.ComponentModel.DataAnnotations;

namespace AlumniProject.Dto
{
    public class EducationAddDTO
    {
        
        [Required(ErrorMessage ="School is required")]
        public string School { get; set; }
        [Required(ErrorMessage = "Degree is required")]
        public string Degree { get; set; }
        [Required(ErrorMessage = "StartDate is required")]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "EndDate is required")]
        public DateTime EndDate { get; set; }

    }
}
