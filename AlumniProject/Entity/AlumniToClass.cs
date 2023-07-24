namespace AlumniProject.Entity;

public class AlumniToClass
{
    public int Id { get; set; }
    public int AlumniId { get; set; }
    public int ClassId { get; set; }

    public virtual Alumni Alumni { get; set; }
    public virtual AlumniClass Class { get; set; }
    public bool Archived { get; set; } = true;

}
