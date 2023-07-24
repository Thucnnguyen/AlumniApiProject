using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AlumniProject.Entity;

public class Education
{

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Degree { get; set; }
    public string School { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; } 
    public bool Archived { get; set; } = true;

    public int AlumniId { get; set; }
    public virtual Alumni Alumni { get; set; }

}
