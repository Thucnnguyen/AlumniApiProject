using System.ComponentModel.DataAnnotations;

namespace AlumniProject.Dto
{
    public class AccessRequestUpdateDTO
    {
        [Required(ErrorMessage = "Id is required")]
        public int Id { get; set; }
        [Range(1,3,ErrorMessage = "RequestStatus must in rage 1-3 (1 is processing, 2 is accept,3 is deny)"), Required(ErrorMessage = "RequestStatus is required")]
        public int RequestStatus { get; set; }
    }
}
