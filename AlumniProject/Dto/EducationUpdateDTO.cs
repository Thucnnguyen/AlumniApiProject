using System.ComponentModel.DataAnnotations;

namespace AlumniProject.Dto;

public class EducationUpdateDTO
{
    [Required(ErrorMessage = "Id is required")]
    public int Id { get; set; }
    [Required(ErrorMessage = "School is required")]
    public string School { get; set; }
    [Required(ErrorMessage = "Degree is required")]
    public string Degree { get; set; }
    [Required(ErrorMessage = "StartDate is required")]
    public DateTime StartDate { get; set; }
    [Required(ErrorMessage = "EndDate is required")]
    public DateTime EndDate { get; set; }
}
