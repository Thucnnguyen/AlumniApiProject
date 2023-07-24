using System.ComponentModel.DataAnnotations;

namespace AlumniProject.Dto;

public class SchoolUpdateDto
{
    
    [Required(ErrorMessage = "Name is required")]

    public string Name { get; set; }
    [Required(ErrorMessage = "Description is required")]
    public string Description { get; set; }
    [Required(ErrorMessage = "SubDomain is required")]
    public string SubDomain { get; set; }
    [Required(ErrorMessage = "BackGround1 is required")]
    public string BackGround1 { get; set; }
    [Required(ErrorMessage = "BackGround2 is required")]
    public string BackGround2 { get; set; }
    [Required(ErrorMessage = "BackGround3 is required")]
    public string BackGround3 { get; set; }
    [Required(ErrorMessage = "Icon is required")]
    public string Icon { get; set; }
    [Required(ErrorMessage = "ProvinceName is required")]
    public string ProvinceName { get; set; }
    [Required(ErrorMessage = "CityName is required")]
    public string CityName { get; set; }
    [Required(ErrorMessage = "Address is required")]
    public string Address { get; set; }
    [Required(ErrorMessage = "Theme is required")]
    public string Theme { get; set; }
}
