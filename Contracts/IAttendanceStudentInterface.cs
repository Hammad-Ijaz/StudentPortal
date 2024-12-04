using static WebApiValidation.DTOs.AttendanceResponse;

namespace WebApiValidation.Contracts
{
	public interface IAttendanceStudentInterface
	{
		Task<GetGeneralResponse> ShowStudentAttendance(string studentId);
	}
}
