namespace WebApiValidation.ViewModels
{
	public class AttendanceViewModel
	{
		public int CourseId { get; set; }
		public List<BulkAttendanceViewModel>? AttendanceRecord { get; set; }
		public int ClassId { get; set; }
    }
	public class BulkAttendanceViewModel()
	{
		public string? AttendanceStatus { get; set; }
		public int StudentId { get; set; }
	}
}
