using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AlumniProject.Entity;

public class Alumni
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    //public string Token { get; set; } = string.Empty;
    public DateTime LastLogin { get; set; }
    public bool IsOwner { get; set; }
    [StringLength(int.MaxValue)]
    public string Bio { get; set; } = string.Empty;
    public string FullName { get; set; }
    public string Avatar_url { get; set; } = "";
    public string CoverImage_url { get; set; }= "";
    public string Email { get; set; }
    public string Phone { get; set; }
    public int RoleId { get; set; }
    public string? FaceBook_url { get; set; }
    public DateTime DateOfBirth { get; set; } = DateTime.Now;
    public bool Archived { get; set; } =true;
    public int? schoolId { get; set; }

    public virtual ICollection<AlumniToClass> AlumniToClasses { get; set; }
    public virtual ICollection<Major> Major { get; set; }
    public virtual Role Role { get; set; }
    public virtual ICollection<Education> Education { get; set; }
    public virtual ICollection<EventParticipant> EventParticipants { get; set; }
    public virtual ICollection<Events> Events { get; set; }
    public virtual School School { get; set; }  
}
