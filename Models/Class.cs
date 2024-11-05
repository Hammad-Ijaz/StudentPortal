using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiValidation.Models
{
    public class Class
    {
        public Class()
        {
            Students = new HashSet<Studentrec>();
        }
        public int ClassId { get; set; }
        [DisplayName("Class Name")]
        public string ClassName { get; set; }
        public virtual ICollection<Studentrec>? Students { get; set; }
        public virtual ICollection<ScheduleClass>? ScheduleClass { get; set; }
    }
}
