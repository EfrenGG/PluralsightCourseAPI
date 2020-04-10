using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PluralsightCourseAPI.ValidationAttributes;

namespace PluralsightCourseAPI.Models
{
    [CourseTitleMustBeDifferentFromDescriptionAttribute(
        ErrorMessage = "The title and description should not be the same."
    )]
    public class CourseForCreationDto
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(1500)]
        public string Description { get; set; }
    }
}