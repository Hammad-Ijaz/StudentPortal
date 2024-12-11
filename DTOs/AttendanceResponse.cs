using WebApiValidation.Models;
using WebApiValidation.ViewModels;

namespace WebApiValidation.DTOs
{
	public class AttendanceResponse
	{
		public record class SearchClassAttendanceResponse(string Message , List<StudentViewModel> listStudents);
		public record class PostClassAttendanceResponse(string Message , List<Attendance> attendaceAddList);
		public record class GetGeneralAttendanceResponse(string Role, string Message , List<AttendanceViewModel> model);
		public record class GetStudentAttendanceResponse(string Message , List<AttendanceViewModel> model);
        // Enter Numbers internal Student Response
        public record class PostClassInternalNumbersResponse(string Message, List<InternalMarks> markslistStd);


    }
}
