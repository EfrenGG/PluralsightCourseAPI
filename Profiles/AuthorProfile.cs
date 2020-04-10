using AutoMapper;
using PluralsightCourseAPI.Entities;
using PluralsightCourseAPI.Helpers;
using PluralsightCourseAPI.Models;

namespace PluralsightCourseAPI.Profiles
{
    public class AuthorProfile : Profile
    {
        public AuthorProfile()
        {
            CreateMap<Author, AuthorDto>()
                .ForMember(
                    des => des.Name,
                    opt => opt.MapFrom(src => $"${src.FirstName} ${src.LastName}"))
                .ForMember(
                    des => des.Age,
                    opt => opt.MapFrom(src => src.DateOfBirth.GetCurrentAge()));
        }
    }
}