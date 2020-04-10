using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

using CourseLibrary.API.Services;
using CourseLibrary.API.Entities;
using PluralsightCourseAPI.Models;
using PluralsightCourseAPI.Helpers;

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
            IEnumerable<Author> authorsEntity = _repo.GetAuthors();
            IList<AuthorDto> authors = new List<AuthorDto>();

            foreach (Author author in authorsEntity)
            {
                authors.Add(new AuthorDto()
                {
                    Id = author.Id,
                    Name = $"${author.FirstName} ${author.LastName}",
                    Age = author.DateOfBirth.GetCurrentAge(),
                    MainCategory = author.MainCategory

                });

            }
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