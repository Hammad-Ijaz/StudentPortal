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
        private readonly ApplicationDbcontext _context;
        public StudentAttendanceController(ApplicationDbcontext dbcontext , IAttendanceStudentInterface attendanceStudent)
        {
            _context = dbcontext;
           _attendanceStudent = attendanceStudent;
        }
        [HttpPost("SearchClassAttendance")]
        public async Task<IActionResult> SearchClassAttendance(int Teacherid,int Class ,int Course)
        {
            List<StudentViewModel> listStudents = new List<StudentViewModel>();
            if(Teacherid == 0 ||  Class == 0 || Course == 0) {
                return BadRequest(new { message = "Invalid Model, Plzz Check it." });   }
            try
            {
                var teacherClass = await _context.ScheduleClass.Where(x => x.TeacherId == Teacherid && x.Class.ClassId == Class && x.Course.Course_Id == Course).Select(x => x.Class).FirstOrDefaultAsync();
                if(teacherClass == null)
                { return Ok(new { message = "Class is not existed." }); }
                var teacherCourse = await _context.TeacherCourse.Where(c => c.TeacherId == Teacherid && c.Course.Course_Id == Course).Select(c => c.Course).FirstOrDefaultAsync();
				if (teacherCourse == null)
				{ return Ok(new { message = "Course is not existed." }); }
                var getStudentsFromDb = await _context.Studentslist
							.Where(x => x.Class.ClassName == teacherClass.ClassName && x.StudentCourses.Any(c => c.Course.Courses == teacherCourse.Courses))
						   .ToListAsync();
                if (getStudentsFromDb != null)
                {
                    foreach(var dd in getStudentsFromDb)
                    {
                        StudentViewModel model = new StudentViewModel()
                        {
                            Id = dd.StudentId,
                            RegistrationNumber = dd.RegistrationNumber,
                            Name = dd.Name
                        };
                        listStudents.Add(model);
                    }
                }
				if (getStudentsFromDb.Any())
				{	return Ok(new{	message = "List of registered students." , data = listStudents});}
				return Ok(new { message = "Class or Course  is not teaching by this teacher." });
            }catch(Exception ex)
            {
				return StatusCode(500, new { message = "An error occurred while processing your request.", details = ex.Message });
			}
        }
        [HttpPost("MarkAttendanceStatus")]
        public async Task<IActionResult> MarkAttendance(AttendanceViewModel model)
        {
            if(model == null || model.AttendanceRecord == null || !model.AttendanceRecord.Any()) {
                return BadRequest("Plzz fill all the fields");   }
            try
            {
                List<Attendance> attendaceAddList = new List<Attendance>();
                foreach(var data in model.AttendanceRecord)
                {
					Attendance attendance = new Attendance()
					{
						StudentId = data.StudentId,
						CourseId = model.CourseId,
						AttendanceStatus = data.AttendanceStatus,
						AttendanceDate = DateTime.Now
					};
					if (attendance.AttendanceStatus == null || attendance.AttendanceStatus == "")
					{ attendance.AttendanceStatus = "A"; }
					attendaceAddList.Add(attendance);
				}
                if(attendaceAddList != null)
                {
					_context.Attendances.AddRange(attendaceAddList);
					await _context.SaveChangesAsync();
					return Ok(new { message = "Successfully, Marked Attendance.", model = attendaceAddList });
				}
				return Ok(new { message = "Failed, No Marked Attendance.", model = attendaceAddList });
			}
			catch (Exception ex) {
				return StatusCode(500, new { message = "An error occurred while processing your attendance request.", details = ex.Message });
			}

        }
        [HttpGet("ShowAttendaceStudent")]
        public async Task<IActionResult> GetAttendance()
        {
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
		     var response = await _attendanceStudent.ShowStudentAttendance(userId);
            return Ok(response);
        }
    }
}
