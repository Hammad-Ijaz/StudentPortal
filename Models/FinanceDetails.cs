namespace WebApiValidation.Models
{
    public class FinanceDetails
    {
        public FinanceDetails()
        {
            ChallanFinanceDetails = new HashSet<ChallanFinanceDetail>();
        }
        public int FinanceId { get; set; }  
        public string? Session { get; set; }
        public int Installments { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal RemainingAmount => TotalAmount - PaidAmount;
        public int ChallanVoucher { get; set; }
        public DateTime PaymentDate { get; set; }
        public string? Status { get; set;}
        public ICollection<ChallanFinanceDetail>? ChallanFinanceDetails { get; set; }
    }
}
