using WebApiValidation.Models;

namespace WebApiValidation.ViewModels
{
    public class ChallanViewModel
    {
        public int ChallanId { get; set; }
        public int StudentId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime DueDate { get; set; }
        public decimal Amount { get; set; }
        public string? Status { get; set; }
    }
}
