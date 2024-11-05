using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApiValidation.Models
{
    public class TeacherCourse
    {
        [Key]
        public int Id { get; set; }
        public int TeacherId { get; set; }
        public int Course_Id { get; set; }

        [ForeignKey(nameof(Course_Id))]
        public virtual Course? Course { get; set; }

        [ForeignKey(nameof(TeacherId))]
        public virtual TeacherRegister? Teacher { get; set; }
    }
}
