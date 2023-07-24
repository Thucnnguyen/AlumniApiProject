namespace AlumniProject.Dto
{
    public class FundDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int TargetBalance { get; set; }
        public string BackgroundImage { get; set; } = String.Empty;
    }
}
