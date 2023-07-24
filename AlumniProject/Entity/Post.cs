using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlumniProject.Entity;

public class Post
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Content { get; set; }
    public int AlumniId {get; set; }
    public int SchoolId { get; set; }
    public bool Archived { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public virtual Alumni Alumni { get; set; }
    public virtual School School { get; set; }
}
