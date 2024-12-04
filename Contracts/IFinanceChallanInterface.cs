using WebApiValidation.Models;
using WebApiValidation.ViewModels;
using static WebApiValidation.DTOs.FinanceResponse;

namespace WebApiValidation.Contracts
{
    public interface IFinanceChallanInterface
    {
        Task<string> GenerateChallanNumber();
		Task<AddFinanceResponse> GetFinanceChallan(string userId);
    }
}
