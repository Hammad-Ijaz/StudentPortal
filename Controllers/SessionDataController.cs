using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiValidation.Models;
using WebApiValidation.ViewModels;

namespace WebApiValidation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class SessionDataController : ControllerBase
    {
        private readonly ApplicationDbcontext _db;
        public SessionDataController(ApplicationDbcontext dbcontext)
        {
            _db = dbcontext;   
        }
        [HttpGet("ShowSession")]
        public async Task<IActionResult> Get()
        {
            List<SessionViewModel> model = new List<SessionViewModel>();
            var getSessions = await _db.Sessions.ToListAsync();
            if(getSessions != null && getSessions.Any()) { 
              foreach(var data in getSessions)
                {
                    SessionViewModel sessionList = new SessionViewModel()
                    {
                        SessionId = data.SessionId,
                        SessionName = data.SessionName,
                        SessionStart = data.SessionStart,
                        SessionEnd = data.SessionEnd
                    };
                    model.Add(sessionList);
                }
				return Ok(new { message = "Availabled  Session Show Successfully." , model = model });
			}
			return Ok(new { message = "No record may be session available." , model = model });
		}
        [HttpPost("AddSession")]
        public async Task<IActionResult> Post(SessionViewModel model)
        {
            if (ModelState.IsValid)
            {
                Session session = new Session()
                {
                    SessionName = model.SessionName,
                    SessionStart = model.SessionStart,
                    SessionEnd = model.SessionEnd
                };
                await _db.Sessions.AddAsync(session);
                await _db.SaveChangesAsync();
                return Ok(new {message = "Session Add Successfully."});
            }
			return Ok(new { message = "Something wrong or an empty model , Session Add failed!" });
		}
		[HttpGet("GetSession")]
		public async Task<IActionResult> GetSession(int Sessionid)
		{
			var getSessions = await _db.Sessions.FirstOrDefaultAsync(x => x.SessionId == Sessionid);
			if (getSessions != null)
			{
					SessionViewModel model= new SessionViewModel()
					{
						SessionId = getSessions.SessionId,
						SessionName = getSessions.SessionName,
						SessionStart = getSessions.SessionStart,
						SessionEnd = getSessions.SessionEnd
					};
				return Ok(new { message = "Get  Session for updation.", model = model });
			}
			return Ok(new { message = "No record may be session available aginst this Id."});
		}
        [HttpPut("UpdateSession")]
        public async Task<IActionResult> Edit(SessionViewModel model)
        {
            if (ModelState.IsValid)
            {
                Session session = new Session()
                {
                    SessionId = model.SessionId,
                    SessionName = model.SessionName,
                    SessionStart = model.SessionStart,
                    SessionEnd = model.SessionEnd
                };
                _db.Sessions.Update(session);
                await _db.SaveChangesAsync();
                return Ok(new { message = "Session Updated Successfully." });
            }
            return Ok(new { message = "Something wrong or an empty model , Session Updation failed!" });
        }
        [HttpDelete("DeleteSession")]
        public async Task<IActionResult> Delete(int Sessionid)
        {
            var getfromDb = await _db.Sessions.FirstOrDefaultAsync(x => x.SessionId == Sessionid);
            if (getfromDb != null) {
                _db.Sessions.Remove(getfromDb);
                await _db.SaveChangesAsync();
                return Ok(new {message = "Deleted Session Successfully!"});
            }
            return Ok(new { message = "Errorr! No deleted this Session." });
        }
    }
}
