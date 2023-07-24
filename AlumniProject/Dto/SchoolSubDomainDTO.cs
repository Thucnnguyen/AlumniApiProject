using AlumniProject.Entity;

namespace AlumniProject.Dto
{
    public class SchoolSubDomainDTO
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SubDomain { get; set; }
        public string BackGround1 { get; set; }
        public string BackGround2 { get; set; }
        public string BackGround3 { get; set; }
        public string Icon { get; set; }
        public string ProvinceName { get; set; }
        public string CityName { get; set; }
        public string Address { get; set; }
        public string Theme { get; set; }
        public DateTime EndTime { get; set; }
        public int Duration { get; set; }
        public int RequestStatus { get; set; } = 1;
        public virtual ICollection<CardItemDTO> CardInSchool { get; set; }
        public virtual ICollection<AttributeDTO> Attributes { get; set; }

    }
}
