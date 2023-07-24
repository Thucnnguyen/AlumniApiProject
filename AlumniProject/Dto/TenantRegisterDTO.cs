using System.ComponentModel.DataAnnotations;

namespace AlumniProject.Dto
{
    public class TenantRegisterDTO
    {
        [Required(ErrorMessage = "Email is required")]
        public string email { get; set; }
        [Required(ErrorMessage = "Bio is required")]
        public string Bio { get; set; }
        [Required(ErrorMessage = "FullName is required")]
        public string FullName { get; set; }
        public string Avatar_url { get; set; } =String.Empty;
        public string CoverImage_url { get; set; } = String.Empty;
        [Required(ErrorMessage = "Phone is required")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "FaceBook_url is required")]
        public string FaceBook_url { get; set; }
        [Required(ErrorMessage = "DateOfBirth is required")]
        public DateTime DateOfBirth { get; set; }
    }
}
