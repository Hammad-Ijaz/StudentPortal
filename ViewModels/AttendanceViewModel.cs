namespace WebApiValidation.ViewModels
{
	public class AttendanceViewModel
	{
		public int CourseId { get; set; }
		public List<BulkAttendanceViewModel>? AttendanceRecord { get; set; }
		public int ClassId { get; set; }
	      // For Get Means Display GetDate , AttendanceStatus and StudentId
		public DateTime GetDate { get; set; }
		public string? AttendanceStatus { get; set; }
		public int StudentId { get; set; }
		public string? RegistrationNumber { get; set; }
		public string? Name { get; set; }
	}
	public class BulkAttendanceViewModel()
	{
		public string? AttendanceStatus { get; set; }
		public int StudentId { get; set; }
	}
}
