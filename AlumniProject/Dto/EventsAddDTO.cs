using System.ComponentModel.DataAnnotations;

namespace AlumniProject.Dto
{
    public class EventsAddDTO
    {
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "ImageUrl is required")]
        public string ImageUrl { get; set; }
        [Required(ErrorMessage = "Desciption is required")]
        public string Desciption { get; set; }
        [Required(ErrorMessage = "Desciption is required")]
        public string location { get; set; }
        [Required(ErrorMessage = "IsOffline is required")]
        public bool IsOffline { get; set; }
        [Required(ErrorMessage = "StartTime is required")]

        public DateTime StartTime { get; set; }
        [Required(ErrorMessage = "EndTime is required")]
        public DateTime EndTime { get; set; }
        [Required(ErrorMessage = "IsPublicSchool is required")]
        public bool IsPublicSchool { get; set; }
        [Required(ErrorMessage = "GradeId is required")]
        public int? GradeId { get; set; }
    }
}
