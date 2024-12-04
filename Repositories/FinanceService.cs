using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.AspNet.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApiValidation.Contracts;
using WebApiValidation.DTOs;
using System.IO;
using WebApiValidation.Models;
using WebApiValidation.ViewModels;
using static WebApiValidation.DTOs.FinanceResponse;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http.HttpResults;

namespace WebApiValidation.Repositories
{
    public class FinanceService(
                    ApplicationDbcontext dbcontext,
                Microsoft.AspNetCore.Identity.UserManager<User> userManager) : IFinanceChallanInterface
    {
        Task<AddFinanceResponse> IFinanceChallanInterface.GetFinanceChallan(string userId)
        {
            throw new NotImplementedException();
        }
        // Registration Number Sequence 
        public async Task<string> GenerateChallanNumber()
        {
            var lastChallan = await dbcontext.Challans
                .OrderByDescending(s => s.ChallanVoucher)
                .FirstOrDefaultAsync();
            int nextRegNumber = 1;
            DateTime currentYear = DateTime.Now;
            string year = currentYear.ToString("yy");
            if (lastChallan != null && int.TryParse(lastChallan.ChallanVoucher, out int lastRegNumber))
            {
                nextRegNumber = lastRegNumber + 1;
                return nextRegNumber.ToString();
            }
            var regNumber = nextRegNumber.ToString("D2");
            return year + regNumber;
        }

        public string File()
        {
            throw new NotImplementedException();
        }
    }
}
