namespace WebApiValidation.Models
{
    public class Challan
    {
        public int ChallanId { get; set; }
        public string? ChallanVoucher {  get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime DueDate { get; set; }
        public decimal ToatalFees { get; set; }
    }
}
