using WebApiValidation.Models;

namespace WebApiValidation.ViewModels
{
    public class FinanceDetailViewModel
    {
        public int FinanceId { get; set; }
        public int? SessionId { get; set; }
        // user self input according to his/her on choice
        public int Installments { get; set; }
        public decimal RemainingAmount { get; set; }
       // public int ChallanVoucher { get; set; }
        public DateTime PaymentDate { get; set; }
        public List<Installment>? Installmentss { get; set; }
    }
}
