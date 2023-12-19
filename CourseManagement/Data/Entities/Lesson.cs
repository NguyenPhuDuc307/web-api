using System.Net.Mime;
using System.ComponentModel.DataAnnotations;

namespace CourseManagement.Data.Entities;

public class Lesson
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? ImagePath { get; set; }
    public string? Introduction { get; set; }
    public string? Content { get; set; }
    [DataType(DataType.Date)]
    public DateTime DateCreated { get; set; }

    public int CourseId { get; set; }
    public Course? Course { get; set; }
}