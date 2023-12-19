using System.Net.Mime;
using System.ComponentModel.DataAnnotations;

namespace CourseManagement.ViewModels;

public class LessonRequest
{
    [StringLength(60, MinimumLength = 3)]
    [Required]
    public string? Title { get; set; }
    public IFormFile? Image { get; set; }
    [Required]
    public string? Introduction { get; set; }
    public string? Content { get; set; }
    [Required]
    [Display(Name = "Course")]
    public int CourseId { get; set; }
}

public class LessonViewModel
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? ImagePath { get; set; }
    public IFormFile? Image { get; set; }
    public string? Introduction { get; set; }
    public string? Content { get; set; }
    [Display(Name = "Date Created")]
    [DataType(DataType.Date)]
    public DateTime DateCreated { get; set; }
    [Display(Name = "Course")]
    public int CourseId { get; set; }
    [Display(Name = "Course")]
    public int CourseName { get; set; }
}