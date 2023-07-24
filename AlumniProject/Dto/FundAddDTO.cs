using System.ComponentModel.DataAnnotations;

namespace AlumniProject.Dto
{
    public class FundAddDTO
    {
        
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }
        [Required(ErrorMessage = "StartTime is required")]
        public DateTime StartTime { get; set; }
        [Required(ErrorMessage = "EndTime is required")]
        public DateTime EndTime { get; set; }
        [Required(ErrorMessage = "TargetBalance is required")]
        public int TargetBalance { get; set; }
        [Required(ErrorMessage = "BackgroundImage is required")]
        public string BackgroundImage { get; set; } = String.Empty;
        
    }
}
