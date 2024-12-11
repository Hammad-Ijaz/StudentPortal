namespace WebApiValidation.ViewModels
{
	public class InternalMarksViewModel
	{
		public int Course_Id { get; set; }
		public int ClassId { get; set; }
		public string? MarkStatus {  get; set; }
		public DateTime TakingDate { get; set; }
		public float TotalMarks { get; set; }
		public List<BulkMarksViewModel>? StudentMarkRecord { get; set; }
	}
	public class BulkMarksViewModel()
	{
		public float ObtainedMarks { get; set; }
		public int StudentId { get; set; }
	}
}
