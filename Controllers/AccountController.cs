using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApiValidation.Contracts;
using WebApiValidation.DTOs;
using WebApiValidation.Models;
using WebApiValidation.ViewModels;
namespace WebApiValidation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigins")]
	public class AccountController  : ControllerBase
    {
        private readonly IAccountInterface userAccount;
        private readonly ApplicationDbcontext dbcontext;
        public AccountController(IAccountInterface useraccount , ApplicationDbcontext applicationDbcontext)
        {
            dbcontext = applicationDbcontext;
            userAccount = useraccount;
        }
        [HttpPost("TeacherRegister")]
        public async Task<IActionResult> Register(TeacherViewModel model)
        {
            var response = await userAccount.CreateAccount(model);
			return new JsonResult(response);
        }
        [HttpPost("Login")]
		public async Task<IActionResult> Login(UserLogin userLogin)
        {
            var login = await userAccount.LoginAccount(userLogin);
            return Ok(login);
        }
        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken(RefreshTokenModel model)
        {
            var loginresult = await userAccount.RefreshToken(model);
            if(loginresult.IsLoggedin)
            {
                return Ok(loginresult);
            }
            return Unauthorized();
        }
    }
}
