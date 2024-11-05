using WebApiValidation.Models;

namespace WebApiValidation.ViewModels
{
    public class ScheduleClassViewModel
    {
        public int Id { get; set; }
        public int Course_Id { get; set; }
        public int ClassId { get; set; }
        public int TeacherId {  get; set; }
        public string? Classess { get; set; }
        public  string? Coursess { get; set; }
        public List<string>? DurationTime { get; set; }
        // for day show in table
        public string? Days { get; set; }
        public DateTime StartDate {  get; set; }
        public DateTime EndDate {  get; set; }
        public string? Room { get; set; }
    }
    public class AddScheduleClassViewModel
    {
        public int Id { get; set; }
        public int Course_Id { get; set; }
        public int ClassId { get; set; }
        public int TeacherId { get; set; }
        public List<int>? Classes { get; set; }
        public List<int>? Courses { get; set; }
        public List<string>? DurationTime { get; set; }
        // for api adds 
        public int Day { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Room { get; set; }
    }

}
