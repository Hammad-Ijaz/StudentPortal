namespace WebApiValidation.Models
{
    public class Installment
    {
        public int InstallmentId { get; set; }
        public int StudentId { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal Paid {  get; set; }
        public decimal Unpaid {  get; set; }
        public string? Status {  get; set; } 
        public Studentrec? Student { get; set; }
    }
}
