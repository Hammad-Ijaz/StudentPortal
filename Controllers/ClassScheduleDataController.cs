using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Security.Claims;
using System.Text.RegularExpressions;
using WebApiValidation.Contracts;
using WebApiValidation.ViewModels;

namespace WebApiValidation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Teacher")]
    public class ClassScheduleDataController : ControllerBase
    {
        private readonly IClassScheduleInterface _classSchedule;
        private readonly ApplicationDbcontext _db;
        public ClassScheduleDataController(IClassScheduleInterface classSchedule , ApplicationDbcontext dbcontext)
        {
            _classSchedule = classSchedule;
            _db = dbcontext;
        }
        [HttpGet("ShowScheduleClasses")]
        public async Task<IActionResult> TimeTableGet(int Id) 
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var response = await _classSchedule.GetTimeTable(Id, userId!);
            if(response.classes == null || response.user.Id !=  userId)
            {
                return Forbid("UnAuthorized!!");
            }
            return Ok(response);
        }
        [HttpPost("AddScheduleClass")]
        public async Task<IActionResult> TimeTablePost(AddScheduleClassViewModel model)
        {
            var response = await _classSchedule.AddTimeTable(model);
            return Ok(response);
        }
        [HttpDelete("DeleteScheduleClass")]
        public async Task<IActionResult> DeleteSch(int Id)
        {
            if (Id != 0)
            {
                var userDb = await _db.ScheduleClass.FirstOrDefaultAsync(x => x.Id == Id);
                if (userDb != null)
                {
                    _db.ScheduleClass.Remove(userDb);
                    _db.SaveChanges();
                    return Ok("Successfully, Schedule deleted.");
                }
                return Ok("Schedule is not available , your provided Id.");
            }
            return Ok("Id must not be null for specific Schedule delete.");
        }
    }
}
