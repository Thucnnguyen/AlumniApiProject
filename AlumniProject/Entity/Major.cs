using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlumniProject.Entity;

public class Major
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string JobTitle { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int AlumniId { get; set; }
    public bool Archived { get; set; } = true;

    public virtual Alumni Alumni { get; set; }


}
