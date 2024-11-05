using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiValidation.Models
{
    public class Course 
    {
        public Course()
        {
            StudentCourses = new HashSet<StudentCor>();
            ScheduleClass = new HashSet<ScheduleClass>();
        }
        [Key]
        public int Course_Id { get; set; }
        [Required]
		public   string? Courses { get; set; }
        [InverseProperty("Course")]
        public virtual ICollection<StudentCor> StudentCourses { get; set; }
        public virtual ICollection<TeacherCourse> TeacherCourses { get; set; }
        public virtual ICollection<ScheduleClass>? ScheduleClass { get; set; }


    }
}
