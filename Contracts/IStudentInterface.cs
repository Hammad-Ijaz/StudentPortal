using WebApiValidation.ViewModels;
using static WebApiValidation.DTOs.ServiceResponse;

namespace WebApiValidation.Contracts
{
    public interface IStudentInterface
    {
        Task<GetDataResponse> ShowDataUser(int Id); 
        Task<SearchStdResponse> SearchUserData(string Name, string Class); 
        Task<SearchReportStudentResponse> SearchQuizAssignmentStudent(int Id,string Name); 
        Task<GetDataTeacherResponse> ShowDataTeacher();
        Task<UpdateUserResponse> UpdateAccount(StudentViewModel model);
    }
}
