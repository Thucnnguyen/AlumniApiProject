namespace AlumniProject.Entity;

public class Fund
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int TargetBalance { get; set; }
    public string BackgroundImage { get; set; } = String.Empty;
    public bool Archived { get; set; } = true;
    public DateTime CreatedAt { get; set; }
    public int schoolId { get; set; }
    public virtual School School { get; set; }
}
