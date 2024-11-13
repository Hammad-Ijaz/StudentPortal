namespace WebApiValidation.Models
{
    public class Session
    {
        public Session()
        {
            FinanceDetails = new HashSet<FinanceDetails>(); 
        }
        public int SessionId { get; set; }
        public DateTime SessionStart { get; set; }
        public DateTime SessionEnd { get; set; }
        public string? SessionName { get; set; }
        public ICollection<FinanceDetails>? FinanceDetails { get; set; }
    }
}
