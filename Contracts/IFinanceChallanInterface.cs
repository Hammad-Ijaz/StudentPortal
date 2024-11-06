using WebApiValidation.Models;
using WebApiValidation.ViewModels;
using static WebApiValidation.DTOs.FinanceResponse;

namespace WebApiValidation.Contracts
{
    public interface IFinanceChallanInterface
    {
        Task<AddFinanceResponse> AddFinance(FinanceDetailViewModel financModel);
        Task<AddFinanceResponse> AddFinanceChallan(ChallanViewModel challanModel);
    }
}
