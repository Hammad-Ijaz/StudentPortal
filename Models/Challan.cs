namespace WebApiValidation.Models
{
    public class Challan
    {
        public Challan()
        {
            ChallanFinanceDetails = new HashSet<ChallanFinanceDetail>();
        }
        public int ChallanId { get; set; }
        public int StudentId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime DueDate { get; set; }
        public decimal Amount { get; set; }
        public string? Status { get; set; }
        public Studentrec? Student { get; set; }
        public ICollection<ChallanFinanceDetail>? ChallanFinanceDetails { get; set; }
    }
}
