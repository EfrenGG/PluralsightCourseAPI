using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

using PluralsightCourseAPI.Services;
using PluralsightCourseAPI.Entities;
using PluralsightCourseAPI.Models;

namespace PluralsightCourseAPI.Controllers
{
    [ApiController]
    [Route("api/authors/{authorId}/courses")]
    public class CoursesController : ControllerBase
    {

        private readonly ICourseLibraryRepository _repo;
        private readonly IMapper _mapper;

        public CoursesController(ICourseLibraryRepository repo, IMapper mapper)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        [HttpHead]
        public ActionResult<IEnumerable<CourseDto>> GetCoursesForAuthor(Guid authorId)
        {
            if (!_repo.AuthorExists(authorId))
            {
                return NotFound();
            }
            IEnumerable<Course> courseEntities = _repo.GetCourses(authorId);
            return Ok(_mapper.Map<IEnumerable<CourseDto>>(courseEntities));
        }

        [HttpGet("{courseId}")]
        [HttpHead("{courseId}")]
        public ActionResult<CourseDto> GetCourseForAuthor(Guid authorId, Guid courseId)
        {
            if (!_repo.AuthorExists(authorId))
            {
                return NotFound();
            }

            Course courseEntity = _repo.GetCourse(authorId, courseId);

            if (courseEntity == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<CourseDto>(courseEntity));
        }
    }
}