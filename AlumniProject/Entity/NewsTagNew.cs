using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AlumniProject.Entity;

public class NewsTagNew
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int NewsId { get; set; }
    public int TagsId { get; set; }
    public bool Archived { get; set; } = true;
    public virtual News News { get; set; }
    public virtual TagsNew Tags { get; set; }
}
