using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiValidation.Models
{
    public class StudentCor
    {
        [Key]
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int Course_Id { get; set; }

        [ForeignKey(nameof(Course_Id))]
		public virtual Course? Course { get; set; }

		[ForeignKey(nameof(StudentId))]
		[InverseProperty("StudentCourses")]
		public virtual Studentrec? Student { get; set; }
	}
}
