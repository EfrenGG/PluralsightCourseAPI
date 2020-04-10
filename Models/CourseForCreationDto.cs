using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PluralsightCourseAPI.ValidationAttributes;

namespace PluralsightCourseAPI.Models
{
    [CourseTitleMustBeDifferentFromDescriptionAttribute]
    public class CourseForCreationDto
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(1500)]
        public string Description { get; set; }
    }
}