using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGeneration.Design;
using WebApiValidation.Contracts;
using WebApiValidation.DTOs;
using WebApiValidation.Models;
using WebApiValidation.ViewModels;
using static WebApiValidation.DTOs.ServiceResponse;

namespace WebApiValidation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class TeacherController : ControllerBase
    {
        private readonly ApplicationDbcontext _db;
        private readonly IStudentInterface _userService;
        private readonly IAccountInterface _userAccount;
        private readonly UserManager<User> _userManager;
        public TeacherController(ApplicationDbcontext db , IStudentInterface userService , IAccountInterface userAccount)
        {
            _db = db;
            _userService = userService;
            _userAccount = userAccount;
        }
        [HttpGet("TeacherData")]
        public async Task<IActionResult> Get()
        {
            var response = await _userService.ShowDataTeacher();
            return Ok(response);
        }
        [HttpGet("SearchTeacher")]
        public async Task<SearchTeacher> SearchTeacher(string? name)
        {
            if(name  == null) throw new ArgumentNullException("name");
                List<TeacherViewModel> teachermodel = new List<TeacherViewModel>();
                if (name != null )
                {
                    var users = await _db.Teachers.Where(s => s.Name == name).ToListAsync();
                    foreach (var user in users)
                    {
                        TeacherViewModel model = new TeacherViewModel
                        {
                            Id = user.TeacherId,
                            Name = user.Name,
                            Contactno = user.Contactno,
                            Email = user.Email,
                            Password = user.Password
                        };
                        teachermodel.Add(model);
                    }
                }
               return new SearchTeacher(name , teachermodel);
        }
        [HttpGet("TeacherGet")]
        public async Task<IActionResult> EditGet(int? Teacherid)
        {
            TeacherViewModel model = new TeacherViewModel();
            var getFromDb = await _db.Teachers.FirstOrDefaultAsync(c => c.TeacherId == Teacherid);
            if (getFromDb != null && Teacherid != null)
            {
                model.Id = getFromDb.TeacherId;
                model.Name = getFromDb.Name;
                model.Contactno = getFromDb.Contactno;
                model.Email = getFromDb.Email;
                model.Password = getFromDb.Password;
                return Ok(new { message = "Successfully , get teacher record.!! ", data = model });
			}
			return Ok(new { message = "May be no teacher exist or something wrong for edit!! ", data = model });
		}
        [HttpPut("TeacherUpdated")]
        public async Task<IActionResult> EditPost(TeacherViewModel model)
        {
            if (model == null) { return Ok(new { message = "Model is null or invalid!!" ,data = model}); }
            try
            {
                var user = await _db.Teachers.FirstOrDefaultAsync(d => d.TeacherId == model.Id);
                user.Name = model.Name;
                user.Contactno = model.Contactno;
                user.Email = model.Email;
                user.Password = model.Password;
                if (user != null) {
					_db.Teachers.Update(user);
					await _db.SaveChangesAsync();
					return Ok(new { message = "Successfully , updated teacher record.!! ", data = model });
				}
				return Ok(new { message = "Error!! No update Teacher.", data = model });
			}
            catch (Exception)
            {
                throw;
            }
        }
        [HttpDelete("TeacherDelete")]
        public async Task<IActionResult> TeacherDelete(int TeacherId)
        {
            var response = await _userAccount.DeleteTeacherAccount(TeacherId);
            return Ok(response);
        }
    }
}
