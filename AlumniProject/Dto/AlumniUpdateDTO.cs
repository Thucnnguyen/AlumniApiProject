using System.ComponentModel.DataAnnotations;

namespace AlumniProject.Dto;

public class AlumniUpdateDTO
{
    [Required(ErrorMessage = "Bio is required")]
    public string? Bio { get; set; }
    [Required(ErrorMessage = "FullName is required")]
    public string FullName { get; set; }
    [Required(ErrorMessage = "Avatar_url is required")]
    public string? Avatar_url { get; set; } = String.Empty;
    public string? CoverImage_url { get; set; } = String.Empty;
    public string Phone { get; set; }
    [Required(ErrorMessage = "FaceBook_url is required")]
    public string? FaceBook_url { get; set; }
    [Required(ErrorMessage = "DateOfBirth is required")]
    public DateTime? DateOfBirth { get; set; }
}
