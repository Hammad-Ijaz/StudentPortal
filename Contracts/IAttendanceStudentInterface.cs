using WebApiValidation.ViewModels;
using static WebApiValidation.DTOs.AttendanceResponse;

namespace WebApiValidation.Contracts
{
	public interface IAttendanceStudentInterface
	{
		Task<SearchClassAttendanceResponse> GetSheetforAttendancebySearch(int Teacherid, int Class, int Course);
		Task<PostClassAttendanceResponse> PostAttendanceStudents(AttendanceViewModel model);
		Task<GetGeneralAttendanceResponse> ShowStudentAttendance(string userid);
		Task<GetStudentAttendanceResponse> StudentAttendanceGetbyStudent(int Studentid , string Class, string Course);
		  // Enter numbers internal
		Task<PostClassInternalNumbersResponse> PostInternalMarksStudents(InternalMarksViewModel model);

    }
}
