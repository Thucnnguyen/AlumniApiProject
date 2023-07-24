using System.ComponentModel.DataAnnotations;

namespace AlumniProject.Dto
{
    public class AccessRequestAddDTO
    {

        [Required(ErrorMessage = "AlumniClassId is required")]
        public int AlumniClassId { get; set; }
    }
}
