using WebApiValidation.Models;

namespace WebApiValidation.ViewModels
{
    public class FinanceDetailViewModel
    {
        public int FinanceId { get; set; }
        public string? SessionName { get; set; }
        // user self input according to his/her on choice
        public int Installments { get; set; }
        public decimal RemainingAmount { get; set; }
        public string? ChallanVoucher { get; set; }
        public DateTime PaymentDate { get; set; }
      //  public List<Installment>? Installmentss { get; set; }
    }
    public class InstallmentViewModel
    {
        public int InstallmentId { get; set; }
        public int StudentId { get; set; }
        public decimal Paid { get; set; }
        public decimal Unpaid { get; set; }
        public string? Status { get; set; } 
        public DateTime PaymentDate { get; set; }
    }
}
