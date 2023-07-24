using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlumniProject.Entity;

public class TagsNew
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int SchoolId { get; set; }
    public string TagName  { get; set; }
    public bool Archived { get; set; } = true;

    public virtual ICollection<NewsTagNew> NewsTagNews { get; set; }
    public virtual School School { get; set; }
}
