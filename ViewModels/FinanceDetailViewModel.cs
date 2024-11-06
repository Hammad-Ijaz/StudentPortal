namespace WebApiValidation.ViewModels
{
    public class FinanceDetailViewModel
    {
        public string? Session { get; set; }
        public int Installments { get; set; }
        public int ChallanVoucher {  get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string? Status { get; set; }
    }
}
