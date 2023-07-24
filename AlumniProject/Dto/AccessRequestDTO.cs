namespace AlumniProject.Dto
{
    public class AccessRequestDTO
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string RequestStatus { get; set; }
        public int AlumniClassId { get; set; }
        public string AlumniClassName { get; set; }

        public string CreatedAt { get; set; }
    }
}
