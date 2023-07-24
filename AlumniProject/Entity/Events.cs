using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace AlumniProject.Entity;

public class Events
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Title { get; set; }
    public string ImageUrl { get; set; } = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRYqO2-ugiq4JXp-w6pbniXhJceIg8_9Bu3uA&usqp=CAU";
    [StringLength(int.MaxValue)]
    public string Desciption { get; set; }
    public bool IsOffline { get; set; }
    public string location { get; set; }= String.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public bool IsPublicSchool { get; set; }
    public int HostId { get; set; }
    public int? GradeId { get; set; }
    public bool Archived { get; set; } = true;
    public int SchoolId { get; set; }
    public virtual Grade? Grade { get; set; }
    [JsonIgnore]
    public virtual Alumni Host { get; set; }
    public virtual School School { get; set; }

    public DateTime CreatedAt { get; set; }
    public virtual ICollection<EventParticipant> Participants { get; set; }
}
