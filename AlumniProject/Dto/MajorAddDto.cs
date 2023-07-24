using System.ComponentModel.DataAnnotations;

namespace AlumniProject.Dto;

public class MajorAddDto
{
    [Required(ErrorMessage ="JobTitle is required")]
    public string JobTitle { get; set; } = string.Empty;
    [Required(ErrorMessage = "Company is required")]
    public string Company { get; set; } = string.Empty;
    [Required(ErrorMessage = "StartDate is required")]
    public DateTime StartDate { get; set; }
    [Required(ErrorMessage = "EndDate is required")]
    public DateTime EndDate { get; set; }
}
