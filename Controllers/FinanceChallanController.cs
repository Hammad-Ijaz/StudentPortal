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
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        public async Task<IActionResult> FinanceChallan(int Id)
        {
            var userGetStd = await _dbcontext.Studentslist.FirstOrDefaultAsync(x => x.StudentId == Id);
            var userGetStdInstall = await _dbcontext.Installments.FirstOrDefaultAsync(x => x.StudentId == Id);
           if (userGetStd == null || userGetStdInstall == null) { return NotFound(" User does not exist."); }
            try
            {
                var GeneratechallanNumber = _financeChallanInterface.GenerateChallanNumber();
                ChallanViewModel model = new ChallanViewModel
                {
                     StudentId    = userGetStd.StudentId,
                    Std_Registration = userGetStd.RegistrationNumber,
                    StudentName = userGetStd.Name,
                    Semester = userGetStd.ClassId.ToString(),
                    Installment = userGetStdInstall.InstallmentId.ToString(),
                    CreateDate = DateTime.Now,
                    DueDate = DateTime.Now.AddDays(1),
                    TotalFees = userGetStdInstall.Unpaid,
                    ChallanViewId = $"{userGetStd.RegistrationNumber + GeneratechallanNumber.Result}"
                };
                var check = await _dbcontext.Challans.FirstOrDefaultAsync(x => x.ChallanVoucher == model.ChallanViewId);
                if (model != null && check == null)
                {
                    Challan challanDb = new Challan()
                    {
                        StudentId = model.StudentId,
                        ChallanVoucher = model.ChallanViewId,
                        CreatedDate = model.CreateDate,
                        DueDate = model.DueDate,
                        ToatalFees = model.TotalFees
                    };
                    _dbcontext.Challans.Add(challanDb);
                    await _dbcontext.SaveChangesAsync();
                }
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
                    table.AddCell("Installment");
                    table.AddCell(model.Installment);
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
                    var data = model;
                    var file = File(ms.ToArray(), "application/pdf", "Challan.pdf");
                    return Ok(new { data = model , file =file});
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating challan: {ex.Message}");
                return StatusCode(500, "An error occurred while generating the challan.");
            }
        }
        // Download Challan
        [HttpGet("SaveChallan")]
        public async Task<IActionResult> SavePdfChallan(int Id)
        {
            var userGetStd = await _dbcontext.Studentslist.FirstOrDefaultAsync(x => x.StudentId == Id);
            var VerifyStudent = await _dbcontext.Challans.FirstOrDefaultAsync(x => x.StudentId == Id);
            if (userGetStd == null || VerifyStudent == null) { return NotFound(" User does not exist."); }
            try
            {
                ChallanViewModel model = new ChallanViewModel
                {
                    Std_Registration = userGetStd.RegistrationNumber,
                    StudentName = userGetStd.Name,
                    Semester = userGetStd.ClassId.ToString(),
                    CreateDate = VerifyStudent.CreatedDate,
                    DueDate = VerifyStudent.DueDate,
                    TotalFees = VerifyStudent.ToatalFees,
                    ChallanViewId = VerifyStudent.ChallanVoucher
                };
                using (MemoryStream ms = new MemoryStream())
                {
                    PdfWriter pdfWriter = new PdfWriter(ms);
                    PdfDocument pdf = new PdfDocument(pdfWriter);
                    Document document = new Document(pdf);
                    document.Add(new Paragraph("BIMS").SetFontSize(25).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
                    document.Add(new Paragraph("HBL.pvt").SetFontSize(20).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
                    document.Add(new Paragraph("Account-name: New Student / Regular").SetFontSize(20).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
                    document.Add(new Paragraph("Account_no : 12345678906").SetFontSize(20).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
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
        // Pay Challan
        [HttpPut("PayChallan")]
        public async Task<IActionResult> PayChallan(int Id)
        {
            var user = await _dbcontext.Challans.FirstOrDefaultAsync(x => x.StudentId == Id);
            var InstallmentUser =  _dbcontext.Installments.Where(x => x.Status == "UnPaid").FirstOrDefault(s => s.StudentId == user.StudentId);
            if (user == null || InstallmentUser == null) { return NotFound("user does not exist."); }
            try
            {
                InstallmentUser.Status = "Paid";
                if (InstallmentUser != null)
                {
                    _dbcontext.Installments.Update(InstallmentUser);
                    await _dbcontext.SaveChangesAsync();
                    return Ok(new { message = "Successfully, fees paid & status updates as well.", data = InstallmentUser });
                }
                return Ok(new { message = "Failed, fees no paid or may be unpaid not exist.", data = InstallmentUser });
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet("ShowFinanceDetails")]
        public async Task<IActionResult> FinanceDetails(int Id)
        {
            var user = await _dbcontext.Studentslist.FirstOrDefaultAsync(x => x.StudentId == Id);
            if (user == null){ return NotFound("user does not exist."); }
            try
            {
                List<InstallmentViewModel> listfees = new List<InstallmentViewModel>();
                var userGetFeesDetail = await _dbcontext.Installments.Where(x => x.StudentId == user.StudentId)
                                               .ToListAsync();
                if (userGetFeesDetail == null)
                             { return NotFound("Student's fees  data  not found."); }
                foreach(var dd in userGetFeesDetail)
                {
                    InstallmentViewModel model = new InstallmentViewModel()
                    {
                        InstallmentId = dd.InstallmentId,
                        PaymentDate = dd.PaymentDate,
                        Paid = dd.Paid,
                        Unpaid = dd.Unpaid,
                        Status = dd.Status
                    };
                    listfees.Add(model);
                }
                return Ok(new { message = "Successfully list show of fees",  listfees = listfees });
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost("AddFinanceDetailsInatallment")]
        public async Task<IActionResult> AddFinanceDetailsInstallment(InstallmentViewModel model)
        {
            var user = await _dbcontext.Studentslist.FirstOrDefaultAsync(x => x.StudentId == model.StudentId);
            if (user == null) { return NotFound("user does not exist."); }
            try
            {
                Installment installment = new Installment()
                {
                    StudentId = model.StudentId,
                    Paid = model.Paid,
                    Unpaid = model.Unpaid,
                };
                if(installment.Paid != null && installment.Unpaid == null)
                {
                    installment.PaymentDate = DateTime.Now;
                    installment.Status = "Paid";
                }else if(installment.Paid != null && installment.Paid != 0 && installment.Unpaid != null)
                {
                    installment.Status = "PartiallyPaid";
					installment.PaymentDate = DateTime.Now;
				}
                else
                {
                    installment.PaymentDate = model.PaymentDate;
                    installment.Status = "UnPaid";
                }
                if(installment != null)
                {
					_dbcontext.Installments.Add(installment);
					await _dbcontext.SaveChangesAsync();
					return Ok(new { message = "Successfully Add Fees.", model = model });
				}
				return Ok(new { message = "Error!! Not Add Fees.", model = model });
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
