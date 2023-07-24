using System.ComponentModel.DataAnnotations;

namespace AlumniProject.Dto;

public class AlumniClassUpdateDTO
{
    [Required(ErrorMessage = "Id is required")]
    public int Id { get; set; }
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; }
}
