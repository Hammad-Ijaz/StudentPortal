using iText.Commons.Actions.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApiValidation.Contracts;
using WebApiValidation.DTOs;
using WebApiValidation.Models;
using WebApiValidation.ViewModels;
using static WebApiValidation.DTOs.AttendanceResponse;

namespace WebApiValidation.Repositories
{
	public class AttendanceService(
		 UserManager<User> userManager,
		 RoleManager<IdentityRole> roleManager,
		 ApplicationDbcontext dbcontext) : IAttendanceStudentInterface
	{
		public async Task<SearchClassAttendanceResponse> GetSheetforAttendancebySearch(int Teacherid, int Class, int Course)
		{
			List<StudentViewModel> listStudents = new List<StudentViewModel>();
			if (Teacherid == 0 || Class == 0 || Course == 0)
			{
				return new SearchClassAttendanceResponse("Invalid Model, Plzz Check it.", listStudents);
			}
			try
			{
				var teacherClass = await dbcontext.ScheduleClass.Where(x => x.TeacherId == Teacherid && x.Class.ClassId == Class && x.Course.Course_Id == Course).Select(x => x.Class).FirstOrDefaultAsync();
				if (teacherClass == null)
				{ return new SearchClassAttendanceResponse("Class is not existed." , listStudents); }
				var teacherCourse = await dbcontext.TeacherCourse.Where(c => c.TeacherId == Teacherid && c.Course.Course_Id == Course).Select(c => c.Course).FirstOrDefaultAsync();
				if (teacherCourse == null)
				{ return new SearchClassAttendanceResponse("Course is not existed.", listStudents); }
				var getStudentsFromDb = await dbcontext.Studentslist
							.Where(x => x.Class.ClassName == teacherClass.ClassName && x.StudentCourses.Any(c => c.Course.Courses == teacherCourse.Courses))
						   .ToListAsync();
				if (getStudentsFromDb != null)
				{
					foreach (var dd in getStudentsFromDb)
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
				{ return new SearchClassAttendanceResponse("List of registered students.", listStudents ); }
				return new SearchClassAttendanceResponse("Class or Course  is not teaching by this teacher." , listStudents);
			}
			catch (Exception)
			{
				throw;
			}
		}
		public async Task<PostClassAttendanceResponse> PostAttendanceStudents(AttendanceViewModel model)
		{
			List<Attendance> attendaceAddList = new List<Attendance>();
			if (model == null || model.AttendanceRecord == null || !model.AttendanceRecord.Any())
			{
				return new PostClassAttendanceResponse("Plzz fill all the fields", attendaceAddList);
			}
			try
			{
				foreach (var data in model.AttendanceRecord)
				{
					Attendance attendance = new Attendance()
					{
						StudentId = data.StudentId,
						CourseId = model.CourseId,
						ClassId = model.ClassId,
						AttendanceStatus = data.AttendanceStatus,
						AttendanceDate = DateTime.Now
					};
					if (attendance.AttendanceStatus == null || attendance.AttendanceStatus == "")
					{ attendance.AttendanceStatus = "A"; }
					attendaceAddList.Add(attendance);
				}
				if (attendaceAddList != null)
				{
					dbcontext.Attendances.AddRange(attendaceAddList);
					await dbcontext.SaveChangesAsync();
					return new PostClassAttendanceResponse("Successfully, Marked Attendance.", attendaceAddList );
				}
				return new PostClassAttendanceResponse("Failed, No Marked Attendance.", attendaceAddList );
			}
			catch (Exception)
			{
				throw;
			}
		}
        public async Task<GetGeneralAttendanceResponse> ShowStudentAttendance(string userid)
		{
			List<AttendanceViewModel> attendanceRecord = new List<AttendanceViewModel>();
			var user = await userManager.FindByIdAsync(userid);
			if (user == null) { return new GetGeneralAttendanceResponse("Role not existed", "User not existed.", attendanceRecord); }
			var role = await userManager.GetRolesAsync(user);
			var teacherExist = await dbcontext.Teachers.FirstOrDefaultAsync(x => x.Email == user.Email);
			try
			{
				if(role.Contains("Teacher") || role.Contains("Admin") && teacherExist != null) {
					var listStdAttendance = await dbcontext.Attendances.ToListAsync();
					foreach(var data in listStdAttendance)
					{
					var student = await dbcontext.Studentslist.FirstOrDefaultAsync(x => x.StudentId == data.StudentId);
					var teacherClass = await dbcontext.ScheduleClass
						.FirstOrDefaultAsync(x => x.ClassId == data.ClassId && x.Course_Id == data.CourseId
						                       && x.TeacherId == teacherExist.TeacherId);
			    		if (teacherClass != null && student != null) {
						AttendanceViewModel model = new AttendanceViewModel() 
						{
							CourseId = data.CourseId,
							ClassId = data.ClassId,
							StudentId = data.StudentId,
							RegistrationNumber = student.RegistrationNumber,
							Name = student.Name,
							AttendanceStatus = data.AttendanceStatus,
							GetDate = data.AttendanceDate.Date
						};
							attendanceRecord.Add(model);
						}
					}
				return new GetGeneralAttendanceResponse("Teacher", "Successfully, Student Attendance Record Show.", attendanceRecord);
				}
				return new GetGeneralAttendanceResponse("Teacher",
					"Failed, Student Attendance Record not Show you may be invalid user or some other issues.Check it."
					, attendanceRecord);
			}
			catch (Exception)
			{
				throw;
			}
		}
		public async Task<GetStudentAttendanceResponse> StudentAttendanceGetbyStudent(int Studentid , string Class, string Course)
		{
			List<AttendanceViewModel> attendanceRecord = new List<AttendanceViewModel>();
			try
			{
				if(Studentid != 0 && Class != null && Course != null)
				{
					var listStdAttendance = await dbcontext.Attendances.Where(x => x.StudentId == Studentid && x.Course.Courses == Course).ToListAsync();
					foreach (var data in listStdAttendance)
					{
						var student = await dbcontext.Studentslist.FirstOrDefaultAsync(x => x.StudentId == Studentid && x.Class.ClassName == Class);
						if (student != null)
						{
							AttendanceViewModel model = new AttendanceViewModel()
							{
								CourseId = data.CourseId,
								ClassId = data.ClassId,
								StudentId = data.StudentId,
								RegistrationNumber = student.RegistrationNumber,
								Name = student.Name,
								AttendanceStatus = data.AttendanceStatus,
								GetDate = data.AttendanceDate.Date
							};
							attendanceRecord.Add(model);
						}
					}
					return new GetStudentAttendanceResponse("Successfully, Student Attendance Record Show.", attendanceRecord);
				}
				return new GetStudentAttendanceResponse(
		"Failed, Student Attendance Record not Show you may be invalid user or some other issues.Check it."
		, attendanceRecord);
			}
			catch (Exception)
			{
				throw;
			}
		}
        // Enter Number internal Student
        public async Task<PostClassInternalNumbersResponse> PostInternalMarksStudents(InternalMarksViewModel model)
        {
			List<InternalMarks> listmarks = new List<InternalMarks>();
			if(model == null || model.StudentMarkRecord == null) 
			{ return new PostClassInternalNumbersResponse("Model is an empty may be." , listmarks); }
			try
			{
				if(model != null && model.StudentMarkRecord != null)
				{
					foreach(var data in model.StudentMarkRecord) {
                        InternalMarks stdmarks = new InternalMarks()
                        {
                            Course_Id  =   model.Course_Id,
                            ClassId    =   model.ClassId,
                            MarkStatus =   model.MarkStatus,
                            TotalMarks =   model.TotalMarks,
                            TakingDate =   model.TakingDate,
                            StudentId  =   data.StudentId,
                           ObtainedMarks = data.ObtainedMarks,
                           TotalResult =   data.ObtainedMarks / 6
                        };
                    if (stdmarks.TakingDate == null)
                        { stdmarks.TakingDate = DateTime.Now; }
						listmarks.Add(stdmarks);
                    }
					if(listmarks != null)
					{
						dbcontext.InternalMarks.AddRange(listmarks);
						await dbcontext.SaveChangesAsync();
                        return new PostClassInternalNumbersResponse("Successfully , Inserted Internal Numbers of Students", listmarks);
                    }
                    return new PostClassInternalNumbersResponse("UnSuccessfull, Some issue to save data.", listmarks);				
				}
                return new PostClassInternalNumbersResponse("Error, Failed to  Inserted Internal Numbers of Students", listmarks);
            }
            catch (Exception)
			{
				throw;
			}
        }
    }
}
