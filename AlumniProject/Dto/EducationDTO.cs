namespace AlumniProject.Dto
{
    public class EducationDTO
    {
        public int Id { get; set; }
        public string School { get; set; }
        public string Degree { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
