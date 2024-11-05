using WebApiValidation.Models;
using WebApiValidation.ViewModels;
using static WebApiValidation.DTOs.ServiceResponse;
namespace WebApiValidation.Contracts
{
    public interface IAccountInterface
    {
        Task<GeneralResponse> CreateAccount(TeacherViewModel model);
        Task<StudentCreateAccountResponse> StudentCreateAccount(StudentViewModel studentrec);
        Task<DeleteResponse> DeleteTeacherAccount(int Id);
        Task<LoginResponse>  LoginAccount(UserLogin userLogin);
        Task<LoginResponse> RefreshToken(RefreshTokenModel model);
    }
}
