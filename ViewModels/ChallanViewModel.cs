using WebApiValidation.Models;

namespace WebApiValidation.ViewModels
{
    public class ChallanViewModel
    {
        public string? ChallanViewId { get; set; }
        public string? Std_Registration { get; set; }
        public string? StudentName { get; set; }
        public string? Semester { get; set; }
        public string?  SessionName { get; set; }
        public int Installment { get; set; }
        public decimal TotalFees { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime DueDate { get; set; }

    }
}
