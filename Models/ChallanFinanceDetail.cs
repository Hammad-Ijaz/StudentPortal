namespace WebApiValidation.Models
{
    public class ChallanFinanceDetail
    {
            public int  ChallanFinanceId{ get; set; }
            public int ChallanId { get; set; }
            public Challan? Challan { get; set; }
            public int FinanceId { get; set; }
            public FinanceDetails? FinanceDetails { get; set; }
    }
}
