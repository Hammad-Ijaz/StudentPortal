using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiValidation.Contracts;
using WebApiValidation.ViewModels;

namespace WebApiValidation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FinanceChallanController : ControllerBase
    {
        private readonly IFinanceChallanInterface _financeChallanInterface;
        public FinanceChallanController(IFinanceChallanInterface financeChallanInterface)
        {
            _financeChallanInterface = financeChallanInterface;
        }
        [HttpPost("PostFinance")]
        public async Task<IActionResult> PostFinance(FinanceDetailViewModel model)
        {
            var response = await  _financeChallanInterface.AddFinance(model);
            return Ok(response);
        }
        [HttpPost("PostFinanceChallan")]
        public async Task<IActionResult> PostFinanceChallan(ChallanViewModel model)
        {
            var response = await _financeChallanInterface.AddFinanceChallan(model);
            return Ok(model);
        }
    }
}
