using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AlumniProject.Entity;

public class EventParticipant
{

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int EventId { get; set; }
    public int AlumniId { get; set; }
    public bool Archived { get; set; } = true;


    public virtual Events Event { get; set; }
    public virtual Alumni Alumni { get; set; }
}
