using WebApiValidation.Models;
using WebApiValidation.ViewModels;

namespace WebApiValidation.DTOs
{
    public class ServiceResponse 
    {
        public record class GeneralResponse(bool Flag, string Message);
        public record class StudentCreateAccountResponse(bool Flag, string Message);
        public record class GetDataResponse(bool Flag, string Role, User user  ,List<StudentViewModel> student);
        public record class SearchStdResponse(string Name, string Class , List<StudentViewModel> student);
        public record class SearchReportStudentResponse(List<StudentViewModel> student);
        public record class GetDataTeacherResponse(bool Flag, string Role,List<TeacherViewModel> teacher);
        public  record class UpdateUserResponse(bool Flag, string Message);
        public record class DeleteResponse(bool Flag, string Message);
        public record class LoginResponse(bool IsLoggedin, string Token, string Refreshtoken, string Message);
        public record class TimeTableResponse(bool flag, string message);
        public record class GetTimeTableResponse(bool flag, string message, User user, List<ScheduleClassViewModel> classes);
        public record class SearchTimeTableResponse(bool flag, string message , List<ScheduleClassViewModel> classes);
        public record class EditTimeTableResponse(bool flag, string message , ScheduleClassViewModel model);
        public record class SearchTeacher(string name,List<TeacherViewModel> Teacher);
    }
}
