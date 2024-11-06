namespace WebApiValidation.Models
{
    public class Studentrec
    {
        public Studentrec()
        {
            StudentCourses = new HashSet<StudentCor>();
        }
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Contactno { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public int ClassId { get; set; }
        public virtual Class Class { get; set; }
        public virtual ICollection<StudentCor> StudentCourses { get; set; }
    }
}
