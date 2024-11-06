using WebApiValidation.Contracts;
using WebApiValidation.DTOs;
using WebApiValidation.Models;
using WebApiValidation.ViewModels;
using static WebApiValidation.DTOs.FinanceResponse;

namespace WebApiValidation.Repositories
{
    public class FinanceService( 
                    ApplicationDbcontext dbcontext) : IFinanceChallanInterface
    {
        public async Task<AddFinanceResponse> AddFinance(FinanceDetailViewModel financModel)
        {
            if(financModel == null)
            {
                return new AddFinanceResponse(false ,"Model is an empty");
            }
            try
            {
                var addFinanace = new FinanceDetails()
                {
                    Session = financModel.Session,
                    Installments = financModel.Installments,
                    TotalAmount = financModel.TotalAmount,
                    PaidAmount = financModel.PaidAmount,
                    PaymentDate = financModel.PaymentDate,
                    Status = financModel.Status
                };
                
                await dbcontext.FinanceDetailss.AddAsync(addFinanace);
                await dbcontext.SaveChangesAsync();
                return new AddFinanceResponse(true, "Generate Challan.");
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }
            //  Save  CHALLAN Content 
        public async Task<AddFinanceResponse> AddFinanceChallan(ChallanViewModel challanModel)
        {
            if (challanModel == null)
            {
                return new AddFinanceResponse(false, "Model is an empty");
            }
            try
            {
                ChallanFinanceDetail financechallan = new ChallanFinanceDetail();
                var Challan = new Challan()
                {
                    StudentId = challanModel.StudentId,
                    CreatedDate = DateTime.Now,
                    DueDate = DateTime.Now.AddDays(10),
                    Amount = challanModel.Amount,
                    Status = challanModel.Status
                };
                financechallan.Challan = Challan;
                await dbcontext.Challans.AddAsync(Challan);
                await dbcontext.ChallanFinanceDetails.AddAsync(financechallan);
                await dbcontext.SaveChangesAsync();
                return new AddFinanceResponse(true, "Save Challan Content.");
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }
    }
}
