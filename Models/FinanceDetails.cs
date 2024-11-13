namespace WebApiValidation.Models
{
    public class FinanceDetails
    {
        public int FinanceId { get; set; }  
        public int  SessionId { get; set; }
        // user self input according to his/her on choice
        public int Installments { get; set; }
        public decimal RemainingAmount { get; set; }
        public DateTime PaymentDate { get; set; }
        public Session? Session { get; set; }
    }
}
