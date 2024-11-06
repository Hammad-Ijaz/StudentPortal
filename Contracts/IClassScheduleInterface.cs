using static WebApiValidation.DTOs.ServiceResponse;
using WebApiValidation.ViewModels;

namespace WebApiValidation.Contracts
{
    public interface IClassScheduleInterface
    {
        Task<GetTimeTableResponse> GetTimeTable(int Id , string UserId); 
        Task<SearchTimeTableResponse> GetSearchTimeTable(string Course); 
        Task<TimeTableResponse> AddTimeTable(AddScheduleClassViewModel model);
    }
}
