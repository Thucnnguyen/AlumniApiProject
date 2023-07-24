using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace AlumniProject.Entity
{
    public class CardInSchool
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Img { get; set; }
        public string Description { get; set; }
        public int schoolId { get; set; }

        [JsonIgnore]
        public  School School { get; set; }
    }
}
