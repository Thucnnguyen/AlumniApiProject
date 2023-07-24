using System.ComponentModel.DataAnnotations;

namespace AlumniProject.Dto
{
    public class EventUpdateDTO
    {
        [Required(ErrorMessage = "Id is required")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "ImageUrl is required")]
        public string ImageUrl { get; set; }
        [Required(ErrorMessage = "Desciption is required")]
        public string Desciption { get; set; }

        [Required(ErrorMessage = "IsOffline is required")]
        public bool IsOffline { get; set; }
        [Required(ErrorMessage = "location is required")]
        public string Location { get; set; } = String.Empty;

        [Required(ErrorMessage = "StartTime is required")]

        public DateTime StartTime { get; set; }
        [Required(ErrorMessage = "EndTime is required")]
        public DateTime EndTime { get; set; }
        [Required(ErrorMessage = "IsPublicSchool is required")]
        public bool IsPublicSchool { get; set; }
        
    }
}
