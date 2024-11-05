using System.ComponentModel;
namespace WebApiValidation.Models
{
    public class TeacherRegister
    {
        public TeacherRegister()
        {
            ScheduleClass = new HashSet<ScheduleClass>();
        }
        public int TeacherId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Contactno { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty ;
        public string Password { get; set; } = string.Empty;
        public virtual ICollection<ScheduleClass>? ScheduleClass { get; set; }
    }
}
