using System.ComponentModel.DataAnnotations;

namespace AlumniProject.Dto
{
    public class ClassAddDto
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "GradeId is required")]
        public int GradeId { get; set; }
    }
}
