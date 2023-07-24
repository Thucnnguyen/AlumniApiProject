using System.ComponentModel.DataAnnotations;

namespace AlumniProject.Dto
{
    public class UpdateSchoolStatusDTO
    {
        [Required(ErrorMessage ="Id is required")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Id is required"),Range(1,3,ErrorMessage ="Status must be in range 1 - 3")]
        public int RequestStatus { get; set; }
    }
}
