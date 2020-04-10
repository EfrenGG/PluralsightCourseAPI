using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

using CourseLibrary.API.Services;
using CourseLibrary.API.Entities;

namespace PluralsightCourseAPI.Controllers
{
    [ApiController]
    [Route("api/authors")]
    public class AuthorsController : ControllerBase
    {

        private readonly ICourseLibraryRepository _repo;

        public AuthorsController(ICourseLibraryRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        [HttpGet]
        public IActionResult GetAuthors()
        {
            IEnumerable<Author> authors = _repo.GetAuthors();
            return Ok(authors);
        }

        [HttpGet("{authorId}")]
        public IActionResult GetAuthor(Guid authorId)
        {
            Author author = _repo.GetAuthor(authorId);

            if (author == null)
            {
                return NotFound();
            }

            return Ok(author);
        }
    }
}