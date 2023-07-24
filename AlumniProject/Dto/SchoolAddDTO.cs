using System.ComponentModel.DataAnnotations;

namespace AlumniProject.Dto;

public class SchoolAddDTO 
{

    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; }
    [Required(ErrorMessage = "SubDomain is required")]
    public string SubDomain { get; set; }
    [Required(ErrorMessage = "Icon is required")]
    public string Icon { get; set; }
    [Required(ErrorMessage = "ProvinceName is required")]
    public string ProvinceName { get; set; }
    [Required(ErrorMessage = "CityName is required")]
    public string CityName { get; set; }
    [Required(ErrorMessage = "Address is required")]
    public string Address { get; set; }
    [Required(ErrorMessage = "Duration is required")]
    public int Duration { get; set; }
    [Required(ErrorMessage = "EvidenceUrl is required")]
    public string EvidenceUrl { get; set; }

}
