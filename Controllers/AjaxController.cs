using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiValidation.ViewModels;
using WebApiValidation.Models;
using Microsoft.AspNetCore.Authorization;
using NuGet.Protocol;
using System.Security.Claims;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Collections;
using WebApiValidation.Contracts;
using Microsoft.AspNetCore.Identity;
using WebApiValidation.DTOs;
namespace WebApiValidation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
	[Authorize]
	public class AjaxController : ControllerBase
    {
        private readonly ApplicationDbcontext _db;
        private readonly UserManager<User> userManager;
        private readonly IClassScheduleInterface _classSchedule;
        public AjaxController(ApplicationDbcontext db ,UserManager<User> _user,IClassScheduleInterface classSchedule)
        {
            _db = db;
           _classSchedule = classSchedule;
            userManager = _user;
        }
        [HttpGet("ShowCourses")]
		public async Task<IEnumerable<CourseViewModel>> Show(int Id)
		{
            List<CourseViewModel> model = new List<CourseViewModel>();
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await userManager.FindByIdAsync(userId);
            var role = await userManager.GetRolesAsync(user);
            if (Id == 0)
            {
                var courses = _db.Courserecord.ToList();
                if (courses != null && courses.Any())
                {
                    foreach (var item in courses)
                    {
                        CourseViewModel vm = new CourseViewModel();
                        vm.Course_Id = item.Course_Id;
                        vm.Courses = item.Courses;
                        model.Add(vm);
                    }
                }
            }
            else if (role.Contains("Teacher") || role.Contains("Admin"))
            {  // Student Course
                model = _db.TeacherCourse.Where(p => p.TeacherId == Id)
                    .Select(student => new CourseViewModel
                    {
                        Course_Id = student.Course_Id,
                        Courses = student.Course.Courses
                    }).ToList();
            }
            else {  // Teacher Course
                model = _db.StudentCourses.Where(p => p.StudentId == Id)
                  .Select(student => new CourseViewModel
                  {
                      Course_Id = student.Course_Id,
                      Courses = student.Course.Courses
                  }).ToList();
            }
            if (model == null || user.Id != userId)
            {
                return (IEnumerable<CourseViewModel>)Forbid("UnAuthorized");
            }
            return model;
		}
        [HttpGet("SearchCourse")]
        public IActionResult SearchCourse(string CourseName)
        {
            try
            {
				CourseViewModel model = new CourseViewModel();
				var courses = _db.Courserecord.Where(x => x.Courses == CourseName).FirstOrDefault();
				if (courses != null )
				{
                    model.Course_Id = courses.Course_Id;
                    model.Courses = courses.Courses;
					return Ok(new { message = "Successfully , Search Course here!! ", data = model });
				}
                return Ok(new { message = "May be no course exist or something wrong!! " ,  data = model });
			}
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet("CheckCourseTime")]
        public async Task<IActionResult> CourseTime(string Course)
        {
            if (Course != null)
            {
                var response = await _classSchedule.GetSearchTimeTable(Course);
                return Ok(response);
            }
            return null;
        }
        [HttpPost("Insert")]
        public JsonResult AddCourse(StudentCourseViewModel model)
        {
			if (model.Course_Id == 0 && model.Std_id == 0 && model.Teacher_id == 0)
            {
                var checkExist = _db.Courserecord.FirstOrDefault(x => x.Courses == model.Course);
                if(checkExist != null) { return new JsonResult("Already Exist this Class."); }
				var coursesbj = new Course()
				{
					Courses = model.Course,
					Course_Id = model.Course_Id
				};
				if (coursesbj != null)
				{
					_db.Courserecord.Add(coursesbj);
					_db.SaveChanges();
				}
			}
			else if(model.Teacher_id == 0)
			{
				StudentCor studentCourses = new StudentCor();
				if (model.courseids != null)
				{
					foreach (var courseId in model.courseids)
					{
						studentCourses = new StudentCor()
						{
							Course_Id = courseId,
							StudentId = model.Std_id
						};
						_db.StudentCourses.Add(studentCourses);
						_db.SaveChanges();
					}
				}
			}
            else if (model.Std_id == 0)
            {
                TeacherCourse teacherCourses = new TeacherCourse();
                if (model.courseids != null)
                {
                    foreach (var courseId in model.courseids)
                    {
                        teacherCourses = new TeacherCourse()
                        {
                            Course_Id = courseId,
                            TeacherId = model.Teacher_id
                        };
                        _db.TeacherCourse.Add(teacherCourses);
                        _db.SaveChanges();
                    }
                }
            }
            return new JsonResult("Saved Course successfully!! from API");
        }
		[HttpGet("Update")]
        public JsonResult Edit(int course_Id)
        {
            var getfromdb = _db.Courserecord.FirstOrDefault(x => x.Course_Id == course_Id);

            if (getfromdb == null)
            {
                return null;
            }
            CourseViewModel model = new CourseViewModel
            {
                Course_Id = getfromdb.Course_Id,
                Courses = getfromdb.Courses,
            };
           //var authHeader = this.HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            //if (authHeader == null || !authHeader.StartsWith("Bearer "))
            //{
            //    return Unauthorized("Authorization header is missing or invalid");
            //}
            // Extract the JWT token by removing the 'Bearer ' prefix
            //string jwtToken = authHeader.Replace("Bearer ","");
              //  var jwt = new JwtSecurityToken(jwtToken);
            //var result = $"{Environment.NewLine} Authorized!! {Environment.NewLine} " +
             //$"Exp Time: {jwt.ValidTo.ToLongTimeString()} , Time: {DateTime.Now.ToLongTimeString()}";
            return new JsonResult(model);   
            //}
            //catch (SecurityTokenException ex)
            //{
            //    // Handle token format errors
            //    return BadRequest($"Token parsing failed: {ex.Message}");
            //}
            // return model;
        }
        [HttpPut("UpdateCourse")]
        public JsonResult Update(CourseViewModel model)
        {
            var checkExist = _db.Courserecord.FirstOrDefault(x => x.Courses == model.Courses);
            if (checkExist != null) { return new JsonResult("Already Exist this Class."); }
            Course coursesbj = new Course()
            {
                Course_Id = model.Course_Id,
                Courses = model.Courses
            };

            if (model.Course_Id != null)
            {
                _db.Courserecord.Update(coursesbj);
                _db.SaveChanges();
            }
            return new JsonResult("Updated from Api controller.");
        }
        [HttpDelete("DeleteCourse")]
        public JsonResult Delete(int? course_Id)
		{
			Course? coursesbj = new Course();
            List<StudentCor> stdcor = new List<StudentCor>();
            List<ScheduleClass> schedules = new List<ScheduleClass>();
			if (course_Id != null)
            {
				coursesbj = _db.Courserecord.Include("StudentCourses").FirstOrDefault(z => z.Course_Id == course_Id);
				coursesbj = _db.Courserecord.Include("ScheduleClass").FirstOrDefault(z => z.Course_Id == course_Id);
				coursesbj!.StudentCourses.ToList().ForEach(s => stdcor.Add(s));
				coursesbj!.ScheduleClass.ToList().ForEach(s => schedules.Add(s));
                _db.StudentCourses.RemoveRange(stdcor);
                _db.ScheduleClass.RemoveRange(schedules);
				_db.Courserecord.Remove(coursesbj);
			    _db.SaveChanges();
            }
            return new JsonResult("Deleted Successfully!!"); 
        }
		[HttpDelete("Drop")]
		public JsonResult Drop(int course_Id)
		{
			Course coursesbj = new Course();
			List<StudentCor> stdcor = new List<StudentCor>();
			if (course_Id != null)
			{
				coursesbj = _db.Courserecord.Include("StudentCourses").FirstOrDefault(z => z.Course_Id == course_Id)!;
				coursesbj.StudentCourses.ToList().ForEach(s => stdcor.Add(s));
				_db.StudentCourses.RemoveRange(stdcor);
				_db.SaveChanges();
			}
			return new JsonResult("Drop Successfully from API!!");
		}

    }
}
