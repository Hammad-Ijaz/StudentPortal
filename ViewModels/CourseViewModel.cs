using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;

namespace WebApiValidation.ViewModels
{
    public class CourseViewModel
    {
        public int Course_Id { get; set; }
        public string? Courses { get; set; }
    }
}
