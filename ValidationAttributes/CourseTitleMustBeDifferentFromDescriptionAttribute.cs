using System.ComponentModel.DataAnnotations;
using PluralsightCourseAPI.Models;

namespace PluralsightCourseAPI.ValidationAttributes
{
    public class CourseTitleMustBeDifferentFromDescriptionAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            CourseForCreationDto course = (CourseForCreationDto)validationContext.ObjectInstance;

            if (course.Title == course.Description)
            {
                return new ValidationResult(
                    "The provided description should be different from title.",
                    new[] { "Global" }
                );
            }

            return ValidationResult.Success;
        }
    }
}