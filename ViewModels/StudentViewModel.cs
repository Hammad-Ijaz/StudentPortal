using WebApiValidation.Models;

namespace WebApiValidation.ViewModels
{
    public class StudentViewModel
	{
		public int Id { get; set; }
		public string? Name { get; set; }
		public string? Contactno { get; set; }
		public string? Email { get; set; }
		public string? Password { get; set; }
		public  int ClassIds { get; set; }
		// get classes into string
		public string? ClassN {  get; set; }
	}
}
