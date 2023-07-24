using System.ComponentModel.DataAnnotations;

namespace AlumniProject.Dto
{
    public class TagUpdateDTO
    {
        [Required(ErrorMessage = "Id is required")]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "TagName is required")]
        public string TagName { get; set; }
    }
}
