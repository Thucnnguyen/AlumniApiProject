using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AlumniProject.Entity
{
    public class Attributes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string Descption { get; set; }
        public string img { get; set; }
        public int schoolId { get; set; }

        public virtual School School { get; set; }
    }
}
