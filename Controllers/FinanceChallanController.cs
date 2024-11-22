using iText.Commons.Actions.Contexts;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApiValidation.Contracts;
using WebApiValidation.DTOs;
using WebApiValidation.Models;
using WebApiValidation.ViewModels;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace WebApiValidation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FinanceChallanController : ControllerBase
    {
        private readonly IFinanceChallanInterface _financeChallanInterface;
		private readonly ApplicationDbcontext _dbcontext; 
		private readonly Microsoft.AspNetCore.Identity.UserManager<User> _userManager;

		public FinanceChallanController(IFinanceChallanInterface financeChallanInterface, ApplicationDbcontext dbcontext,
				Microsoft.AspNetCore.Identity.UserManager<User> userManager)
        {
            _financeChallanInterface = financeChallanInterface;
			_dbcontext = dbcontext;
			_userManager = userManager;
        }
		[HttpGet("GenerateChallan")]
        public async Task<IActionResult> FinanceChallan()
        {
            var userid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByIdAsync(userid);
            if (user == null) { return NotFound("user does not exist."); }
            try
            {
                var userGetStd = await _dbcontext.Studentslist.FirstOrDefaultAsync(x => x.Email == user.Email);
                if (userGetStd == null)
                {     return NotFound("Student data not found for the user.");      }
                var stdCourse =  _dbcontext.StudentCourses.Where(p => p.StudentId == userGetStd.StudentId)
                  .Select(student => new CourseViewModel
                  {
                      Course_Id = student.Course_Id,
                      Courses = student.Course.Courses
                  }).ToList();
                var GeneratechallanNumber = _financeChallanInterface.GenerateChallanNumber();
				ChallanViewModel model = new ChallanViewModel
                {
                    Std_Registration = userGetStd.RegistrationNumber,
                    StudentName = userGetStd.Name,
                    Semester = userGetStd.ClassId.ToString(),
                    CreateDate = DateTime.Now,
					DueDate = DateTime.Now.AddDays(1),
                    TotalFees = stdCourse.Count * 10000,
                  ChallanViewId = $"{userGetStd.RegistrationNumber+GeneratechallanNumber.Result}"
                };
                var check = await _dbcontext.Challans.FirstOrDefaultAsync(x => x.ChallanVoucher == model.ChallanViewId);
				if (model != null && check == null)
				{
					Challan challanDb = new Challan()
					{
						ChallanVoucher = model.ChallanViewId,
						CreatedDate = model.CreateDate,
						DueDate = model.DueDate,
						ToatalFees = model.TotalFees
					};
					_dbcontext.Challans.Add(challanDb);
					await _dbcontext.SaveChangesAsync();
				}
				// generate pdf of Challan
				using (MemoryStream ms = new MemoryStream())
                {
                    PdfWriter pdfWriter = new PdfWriter(ms);
                    PdfDocument pdf = new PdfDocument(pdfWriter);
                    Document document = new Document(pdf);
                    document.Add(new Paragraph("Challan Details").SetFontSize(18).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
                    Table table = new Table(2);
                    table.AddCell("Challan Voucher:");
                    table.AddCell(model.ChallanViewId);
                    table.AddCell("Session:");
                    table.AddCell("FALL-2024");
                    table.AddCell("Student Name:");
                    table.AddCell(model.StudentName);
                    table.AddCell("Registration Number:");
                    table.AddCell(model.Std_Registration);
                    table.AddCell("Semester:");
                    table.AddCell(model.Semester);
                    table.AddCell("Create Date:");
                    table.AddCell(model.CreateDate.ToString("yyyy-MM-dd"));
                    table.AddCell("Due Date:");
                    table.AddCell(model.DueDate.ToString("yyyy-MM-dd"));
                    table.AddCell("Total Fees:");
                    table.AddCell($"PKR {model.TotalFees}");
                    document.Add(table);
                    document.Close();
                    return File(ms.ToArray(), "application/pdf", "Challan.pdf");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating challan: {ex.Message}");
                return StatusCode(500, "An error occurred while generating the challan.");
            }
        }
        [HttpGet("ShowFinanceDetails")]
        public async Task<IActionResult> FinanceDetails()
        {
            var userid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByIdAsync(userid);
            if (user == null){ return NotFound("user does not exist."); }
            try
            {
                var userGetStd = await _dbcontext.Studentslist.FirstOrDefaultAsync(x => x.Email == user.Email);
                if (userGetStd == null)
                             { return NotFound("Student data not found for the user."); }
                InstallmentViewModel model = new InstallmentViewModel()
                {
                    Unpaid = 20000,
                    Status = "Unpaid"
                };
                return Ok(model);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet("AddFinanceDetails")]
        public async Task<IActionResult> PostFinanceDetails(FinanceDetailViewModel model)
        {
            var userid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByIdAsync(userid);
            if (user == null) { return NotFound("user does not exist."); }
            try
            {
                var userGet = await _dbcontext.Admin.FirstOrDefaultAsync(x => x.Email == user.Email);
                if (userGet == null)
                { return Unauthorized(new { message = "You do not have access to this resource." }); }
                model.Installments = 0;
                model.SessionName = "FALL-2024";
                model.PaymentDate = DateTime.Now;
                model.ChallanVoucher = "24000324";
               // model.RemainingAmount = UnPaid - paid;
                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }
	}
}
