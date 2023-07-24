using AlumniProject.Entity;
using System.ComponentModel.DataAnnotations;

namespace AlumniProject.Dto
{
    public class NewsDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string NewsImageUrl { get; set; }
        public bool IsPublic { get; set; } = false;
        public string alumniName { get; set; }
        public IEnumerable<TagDTO> tags { get; set; }
    }
}
