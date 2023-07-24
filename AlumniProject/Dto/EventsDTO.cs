using System.ComponentModel.DataAnnotations;

namespace AlumniProject.Dto
{
    public class EventsDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string Desciption { get; set; }
        public bool IsOffline { get; set; }
        public string location { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        //public bool PublicPartictipant { get; set; }
        public bool IsPublicSchool { get; set; }
        public string HostName { get; set; }
    }
}
