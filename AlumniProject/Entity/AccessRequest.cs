using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AlumniProject.Entity;

public class AccessRequest
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Phone { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Email { get; set; }
    public int RequestStatus { get; set; } =1;
    public int AlumniClassId { get; set; }
    public int SchoolId { get; set; } 
    public bool Archived { get; set; } =true;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public virtual AlumniClass AlumniClass { get; set; }
    public virtual School School { get; set; }
}
