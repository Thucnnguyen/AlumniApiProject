using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AlumniProject.Entity;

public class News
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Title { get; set; }
    [StringLength(int.MaxValue)]
    public string Content { get; set; }
    public string NewsImageUrl { get; set; } = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRYqO2-ugiq4JXp-w6pbniXhJceIg8_9Bu3uA&usqp=CAU";
    public bool IsPublic { get; set; } = true;
    public int AlumniId { get; set; }
    public int SchoolId { get; set; }
    public bool Archived { get; set; } =true;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public virtual Alumni Alumni { get; set; }
    public virtual School School { get; set; }
    public virtual ICollection<NewsTagNew> NewsTagNews { get; set; }
}
