namespace WebApiValidation.Models
{
    public class InternalMarks
    {
        public int MarksId { get; set; }
        public int StudentId { get; set; }
        public int Course_Id { get; set; }
        public int ClassId { get; set; }
        public string? MarkStatus { get; set; }
        public float TotalMarks { get; set; }
        public float ObtainedMarks { get; set; }
        public float TotalResult { get; set; }
        public DateTime TakingDate { get; set; }
        public Course? Course { get; set; }
        public Class? Class { get; set; }
        public Studentrec? Student { get; set; }

    }
}
