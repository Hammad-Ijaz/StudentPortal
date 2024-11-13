using WebApiValidation.Models;

namespace WebApiValidation.ViewModels
{
    public class ChallanViewModel
    {
        public int ChallanViewId { get; set; }
        public string? StudentName { get; set; }
        public string?  SessionName { get; set; }
        public int Installment { get; set; }
        public decimal TotalFees { get; set; }
        public DateTime CrateDate { get; set; }
        public DateTime DueDate { get; set; }

    }
}
