using AutoMapper;
using PluralsightCourseAPI.Entities;
using PluralsightCourseAPI.Models;

namespace PluralsightCourseAPI.Profiles
{
    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            CreateMap<Course, CourseDto>();
        }
    }
}