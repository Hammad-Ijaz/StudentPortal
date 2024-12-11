using iText.Commons.Actions.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApiValidation.Contracts;
using WebApiValidation.Models;
using WebApiValidation.ViewModels;

namespace WebApiValidation.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class StudentAttendanceController : ControllerBase
	{
        private readonly IAttendanceStudentInterface _attendanceStudent;
        public StudentAttendanceController( IAttendanceStudentInterface attendanceStudent)
        {
           _attendanceStudent = attendanceStudent;
        }
        [HttpPost("SearchClassAttendance")]
        public async Task<IActionResult> SearchClassAttendance(int Teacherid,int Class ,int Course)
        {
            var response = await _attendanceStudent.GetSheetforAttendancebySearch(Teacherid,Class,Course);
            return Ok(response);
        }
        [HttpPost("MarkAttendanceStatus")]
        public async Task<IActionResult> MarkAttendance(AttendanceViewModel model)
        {
           var response = await _attendanceStudent.PostAttendanceStudents(model);
            return Ok(response);
        }
        [HttpGet("ShowAttendaceStudent")]
        public async Task<IActionResult> GetAttendance()
        {
			var userid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
		     var response = await _attendanceStudent.ShowStudentAttendance(userid);
            return Ok(response);
        }
		[HttpPost("SearchCourseAttendance")]
		public async Task<IActionResult> SearchCourseAttendance(int Studentid, string Class, string Course)
		{
			var response = await _attendanceStudent.StudentAttendanceGetbyStudent(Studentid, Class, Course);
			return Ok(response);
		}
        // Enter Internal Marks by Student
        [HttpPost("PostStdIntrenalMarks")]
        public async Task<IActionResult> PostStdNumbers(InternalMarksViewModel model)
        {
            var response = await _attendanceStudent.PostInternalMarksStudents(model);
            return Ok(response);    
        }
    }
}
