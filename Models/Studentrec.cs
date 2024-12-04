namespace WebApiValidation.Models
{
    public class Studentrec
    {
        public Studentrec()
        {
            StudentCourses = new HashSet<StudentCor>();
            StudentInstallments = new HashSet<Installment>();
            StudentChallans = new HashSet<Challan>();
            StudentAttendances = new HashSet<Attendance>();
        }
        public int StudentId { get; set; }
        public string? Name { get; set; }
        public string? RegistrationNumber {  get; set; }
        public string? Contactno { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public int ClassId { get; set; }
        public virtual Class Class { get; set; }
        public virtual ICollection<StudentCor> StudentCourses { get; set; }
        public virtual ICollection<Installment> StudentInstallments { get; set; }
        public virtual ICollection<Challan> StudentChallans { get; set; }
        public virtual ICollection<Attendance> StudentAttendances { get; set; }
    }
}
