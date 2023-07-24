using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AlumniProject.Entity;

public class School
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Name { get; set; }
    [StringLength(int.MaxValue)]
    public string Description { get; set; } = String.Empty;
    public string SubDomain { get; set; }
    public string BackGround1 { get; set; } =String.Empty;
    public string BackGround2 { get; set; } = String.Empty;
    public string BackGround3 { get; set; } = String.Empty;
    public string Icon { get; set; }
    public string ProvinceName { get; set; }
    public string CityName { get; set; }
    public string Address { get; set; }
    public string Theme { get; set; } = String.Empty;
    public int RequestStatus { get; set; } = 1;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string EvidenceUrl { get; set; }
    public int Duration { get; set; }
    public bool Archived { get; set; }=true;
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public virtual ICollection<Alumni> alumni { get; set; }
    public virtual ICollection<Grade> Grade { get; set; }
    public virtual ICollection<Post> Post { get; set; }
    public virtual ICollection<News> New { get; set; }
    public virtual ICollection<Events> Event { get; set; }
    public virtual ICollection<Fund> Fund { get; set; }
    public virtual ICollection<CardInSchool> CardInSchool { get; set; }
    public virtual ICollection<Attributes> Attribute { get; set; }

    public virtual ICollection<AccessRequest> AccessRequests { get; set; }
}
