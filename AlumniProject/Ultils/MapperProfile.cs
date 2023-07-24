

using AlumniProject.Dto;
using AlumniProject.Entity;
using AutoMapper;

namespace AlumniProject.Ultils;

public class MapperProfile : Profile
{
	public MapperProfile()
	{
		CreateMap<AlumniDTO, Alumni>().ReverseMap();
        CreateMap<TenantRegisterDTO, Alumni>().ReverseMap();
        CreateMap<AlumniUpdateDTO, Alumni>().ReverseMap();
        CreateMap<AlumniUpdateDTO, AlumniDTO>().ReverseMap();
        CreateMap<School, SchoolAddDTO>().ReverseMap();
        CreateMap<School, SchoolUpdateDto>().ReverseMap();
        CreateMap<School, SchoolDTO>().ReverseMap();
        CreateMap<School, SchoolSubDomainDTO>().ReverseMap();
        CreateMap<Grade, GradeDTO>().ReverseMap();
        CreateMap<Grade, GradeAddDTO>().ReverseMap();
        CreateMap<Grade, GradeUpdateDTO>().ReverseMap();
        CreateMap<AlumniClass, ClassAddDto>().ReverseMap();
        CreateMap<AlumniClass, AlumniClassDTO>().ReverseMap();
        CreateMap<AlumniClass, AlumniClassUpdateDTO>().ReverseMap();
        CreateMap<Education, EducationAddDTO>().ReverseMap();
        CreateMap<Education, EducationDTO>().ReverseMap();
        CreateMap<Education, EducationUpdateDTO>().ReverseMap();
        CreateMap<Major, MajorAddDto>().ReverseMap();
        CreateMap<Major, MajorDTO>().ReverseMap();
        CreateMap<Major, MajorUpdateDTO>().ReverseMap();
        CreateMap<AccessRequest, AccessRequestDTO>().ReverseMap();
        CreateMap<AccessRequest, AccessRequestAddDTO>().ReverseMap();
        CreateMap<AccessRequest, AccessRequestUpdateDTO>().ReverseMap();
        CreateMap<News, NewsDTO>().ReverseMap();
        CreateMap<News, NewsAddDTO>().ReverseMap();
        CreateMap<News, NewsUpdateDTO>().ReverseMap();
        CreateMap<Fund, FundDTO>().ReverseMap();
        CreateMap<Fund, FundAddDTO>().ReverseMap();
        CreateMap<Fund, FundUpdateDTO>().ReverseMap();
        CreateMap<Events, EventsAddDTO>().ReverseMap();
        CreateMap<Events, EventsDTO>().ReverseMap();
        CreateMap<Events, EventUpdateDTO>().ReverseMap();
        CreateMap<TagsNew, TagAddDTO>().ReverseMap();
        CreateMap<TagsNew, TagDTO>().ReverseMap();
        CreateMap<TagsNew, TagUpdateDTO>().ReverseMap();
        CreateMap<Post, PostAddDTO>().ReverseMap();
        CreateMap<Post, PostDTO>().ReverseMap();
        CreateMap<Post, PostUpdateDTO>().ReverseMap();
        CreateMap<CardItemDTO, CardInSchool>().ReverseMap();
        CreateMap<AttributeDTO, Attributes>().ReverseMap();

    }

}
