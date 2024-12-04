namespace WebApiValidation.Models
{
	public class Attendance
	{
        public int AttendanceId { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public string? AttendanceStatus{ get; set; }
        public DateTime AttendanceDate { get; set; }
        public Studentrec? Students { get; set; }
        public Course? Course { get; set; }
    }
}
