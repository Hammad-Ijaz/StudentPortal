using Microsoft.AspNetCore.Mvc.Rendering;
using WebApiValidation.Models;

namespace WebApiValidation.ViewModels
{
	public class StudentCourseViewModel
	{
		public int Std_id { get; set; }
		public int Teacher_id { get; set; }
		public int Course_Id { get; set; }
		public string? Course { get; set; }
		public List<int>? courseids { get; set; } 
	}
}
