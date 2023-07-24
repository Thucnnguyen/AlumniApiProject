using System.ComponentModel.DataAnnotations;

namespace AlumniProject.Dto
{
    public class TagAddDTO
    {
        
        
        [Required(ErrorMessage = "TagName is required")]
        public string TagName { get; set; }
    }
}
