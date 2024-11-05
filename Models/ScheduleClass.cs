using System.ComponentModel;

namespace WebApiValidation.Models
{
    public class ScheduleClass
    {
        public int Id { get; set; }
        public int TeacherId { get; set; }
        public int ClassId { get; set; }
        public int Course_Id { get; set; }
        public DayOfWeek Days { get; set; }
        [DisplayName("Duration Time")]
        public List<string>? DurationTime {  get; set; }
        [DisplayName("Start Date")]
        public DateTime StartDate { get; set; }
        [DisplayName("End Date")]
        public DateTime EndDate { get; set; }
        public string? Room { get; set; }
        public virtual Course? Course { get; set; }
        public virtual Class? Class{ get; set; }
        public virtual TeacherRegister? Teacher {  get; set; }
    }
}
