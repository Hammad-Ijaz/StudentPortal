using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiValidation.ViewModels;

namespace WebApiValidation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        private readonly ApplicationDbcontext _db;
        public SessionController(ApplicationDbcontext dbcontext)
        {
            _db = dbcontext;   
        }
        [HttpPost("SessionPost")]
        public async Task<IActionResult> Post(SessionViewModel model)
        {
            if (ModelState.IsValid)
            {

            }
            return null;
        }
    }
}
