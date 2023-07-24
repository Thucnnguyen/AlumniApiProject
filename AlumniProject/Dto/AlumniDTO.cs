using System.ComponentModel.DataAnnotations;

namespace AlumniProject.Dto;

public class AlumniDTO
{

    public int Id { get; set; }
    public string Bio { get; set; }
    public string FullName { get; set; }
    public string Avatar_url { get; set; }
    public string CoverImage_url { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string FaceBook_url { get; set; }
    public DateTime DateOfBirth { get; set; }
}
