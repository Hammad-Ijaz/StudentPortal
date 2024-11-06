using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiValidation.ViewModels;
using WebApiValidation.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using WebApiValidation.Contracts;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.VisualStudio.Web.CodeGeneration.Design;

namespace WebApiValidation.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	[Authorize(Roles = "Admin,Teacher")]
	public class StudentDataController : ControllerBase
	{
        private readonly ApplicationDbcontext _db;
		private readonly IValidator<StudentViewModel> _validator;
		private readonly IConfiguration _configuration;
		private readonly IStudentInterface _userService;
		private readonly IAccountInterface _userAccount;
		public StudentDataController(IAccountInterface userAccount,IStudentInterface userService,ApplicationDbcontext db , IValidator<StudentViewModel> validator )
		{
			_validator = validator;
			_db = db;
			_userService = userService;
			_userAccount = userAccount;
		}
        [HttpGet("ShowData")]
        public async Task<IActionResult> Show(int Id)
		{
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var response = await _userService.ShowDataUser(Id,userId!);
            if (response.student == null || response.user.Id!= userId) {
				return Forbid("Un-Authorized!!");
			}
			return Ok(response);
		}
		[HttpGet("SearchStudent")]
		public async Task<IActionResult> Search(string Name , string Class )
		{
			var searchResp = await _userService.SearchUserData(Name,Class);
			return Ok(searchResp);
		}
		[HttpGet("SearchQuizAssignmentStd")]
		public async Task<IActionResult> GetStudentQuizAsss(int id, string name)
		{
			var searchResp = await _userService.SearchQuizAssignmentStudent(id, name);
			return Ok(searchResp);
		}
		[HttpPost("AddStd")]
		public async Task<IActionResult> Add(StudentViewModel model)
		{
			var response = await _userAccount.StudentCreateAccount(model);
            return Ok(response);
            //Studentrec student = new Studentrec();
            //List<StudentCor> studentCourses = new List<StudentCor>();
            //if (!ModelState.IsValid)
            //{
            //	return new JsonResult("Error to saving data  from API!!");
            //}
            //else
            //{
            //	student.Id = model.Id;
            //	student.Name = model.Name;
            //	student.Contactno = model.Contactno;
            //	student.Email = model.Email;
            //	student.Semester = model.Semester;
            //	student.Programe = model.Programe;
            //	student.Password = model.Password;
            //if (model.CourseIds != null)
            //{
            //	foreach (var courseId in model.CourseIds)
            //	{
            //		studentCourses.Add(
            //	   new StudentCor
            //	   {
            //		   Course_Id = courseId,
            //		   StudentId = model.Id
            //	   });
            //	}
            //	student.StudentCourses = studentCourses;
            //	//}
            //	_db.Studentslist.Add(student);
            //	_db.SaveChanges();
            //}	
        }
		[HttpGet("EditGet")]
		public JsonResult Edit(int? id)
		{ 
			StudentViewModel model = new StudentViewModel();			
			if(id == null) { return new JsonResult("User not found may be id is not valid!!"); }
			if (id != null)
			{
				var student = _db.Studentslist.FirstOrDefault(x => x.StudentId == id);
				model.Id = student.StudentId;
				model.Name = student.Name;
				model.Contactno = student.Contactno;
				model.Email = student.Email;
				model.Password = student.Password;
                var stds = _db.Studentslist.Where(z => z.StudentId == student.StudentId).Select(z => new
                { z.ClassId, z.Class.ClassName }).FirstOrDefault();
                if (stds != null) {
                    model.ClassIds = stds.ClassId; model.ClassN = stds.ClassName;  }
            }
			return new JsonResult(model);
		}
		[HttpPut("UpdateStudent")]
		public async Task<IActionResult> Edit(StudentViewModel model)
		{
	    	var update = await _userService.UpdateAccount(model);
			return Ok(update);
		}

        [HttpDelete("DropStudent")]
        public JsonResult DropStudent(int? id)
        {
            Studentrec std = new Studentrec();
            List<StudentCor> stdcor = new List<StudentCor>();
            if (id != null)
            {
                std = _db.Studentslist.Include("StudentCourses").FirstOrDefault(x => x.StudentId == id);
				std.StudentCourses.ToList().ForEach(s => stdcor.Add(s));
                _db.StudentCourses.RemoveRange(stdcor);
                _db.SaveChanges();
            }
            return new JsonResult("Drop Student from api end!!");
        }
        [HttpDelete("Delete")]
		public JsonResult Delete(int? id)
		{
			Studentrec std = new Studentrec();
			List<StudentCor> stdcor = new List<StudentCor>();
			if (id != null)
			{
				std = _db.Studentslist.Include("StudentCourses").FirstOrDefault(x => x.StudentId == id)!;
				std.StudentCourses.ToList().ForEach(s => stdcor.Add(s));
				_db.StudentCourses.RemoveRange(stdcor);
			 var  delStd =	 _db.Studentslist.Remove(std);
				if (delStd != null)
				{
					var userStd =_db.Users.FirstOrDefault(x => x.Email == std.Email);
					_db.Users.Remove(userStd);
				}
				_db.SaveChanges();
                return new JsonResult("Deleted from api end!!");
            }
			return new JsonResult("Error !! Delted for git updation.");
		}
		[HttpDelete("Alldel")]
		public JsonResult Alldel()
		{
			List<StudentCor> stdcor = new List<StudentCor>();
			List<Studentrec> stdd = new List<Studentrec>();
			stdd = _db.Studentslist.Include("StudentCourses").ToList();
			if (stdd != null)
			{
				foreach (var data in stdd)
				{
					data.StudentCourses.ToList().ForEach(s => stdcor.Add(s));
					_db.StudentCourses.RemoveRange(stdcor);
					_db.Studentslist.Remove(data);
				}
				_db.SaveChanges();
			}
            return new JsonResult("Deleted from api end , ALL Student Records!!");
        }
		[AllowAnonymous]
        [HttpGet("DisplayUserName")]
        public IActionResult DisplayUserName()
        {
            var tokenid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (tokenid == null)
            {
                return new JsonResult("Token not found");
            }
            var userid = tokenid.Value;
            var user = _db.Users.Find(userid);
            if (user == null) { return new JsonResult("User not found" ); }
            // Teacher login
            var checkTeacher = _db.Teachers.FirstOrDefault(x => x.Email == user.Email);
            if (checkTeacher != null) {
				var email = checkTeacher.Email; var teacherid = checkTeacher.TeacherId;
                return Ok(new { Email = email, TeacherId = teacherid });
            }
			// Student Login
			var checkStudent = _db.Studentslist.FirstOrDefault(x => x.Email == user.Email);
            if (checkStudent != null)
            {
                var email = checkStudent.Email; var id = checkStudent.StudentId;
                return Ok(new { Email = email, StudentId = id });
            }
            return new JsonResult(new {Email = user.Email });
        }
    }
}
