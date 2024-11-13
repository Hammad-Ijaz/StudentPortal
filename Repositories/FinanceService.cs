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
        public async Task<AddFinanceResponse> GetFinanceChallan()
        {
            try
            {
                ChallanViewModel model = new ChallanViewModel();
                
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
