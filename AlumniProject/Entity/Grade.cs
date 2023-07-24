using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AlumniProject.Entity;

public class Grade
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Code { get; set; }= String.Empty;
    public int StartYear { get; set; }
    public int EndYear { get; set; }
    public int SchoolId { get; set; }
    public bool Archived { get; set; }= true;
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public virtual School School { get; set; }
}
