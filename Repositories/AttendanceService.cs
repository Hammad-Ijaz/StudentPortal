using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApiValidation.Contracts;
using WebApiValidation.DTOs;
using static WebApiValidation.DTOs.AttendanceResponse;

namespace WebApiValidation.Repositories
{
	public class AttendanceService(
		 UserManager<User> userManager,
		 RoleManager<IdentityRole> roleManager,
		 ApplicationDbcontext dbcontext) : IAttendanceStudentInterface
	{
		public async Task<GetGeneralResponse> ShowStudentAttendance(string userId)
		{
			var user = await userManager.FindByIdAsync(userId);
			if (user == null) { return new GetGeneralResponse("Role not exited", "User not existed."); }
			var role = await userManager.GetRolesAsync(user);
			try
			{
				if(role.Contains("Teacher") || role.Contains("Admin")) {
				//	var TeacherclassCourse = await dbcontext.ScheduleClass.w
				 var listStdAttendance = await dbcontext.Attendances.ToListAsync();
				}
				return new GetGeneralResponse("Teacher", "Successfully, Student Attendance Record Show.");
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
