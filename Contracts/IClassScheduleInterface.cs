using static WebApiValidation.DTOs.ServiceResponse;
using WebApiValidation.ViewModels;

namespace WebApiValidation.Contracts
{
    public interface IClassScheduleInterface
    {
        Task<GetTimeTableResponse> GetTimeTable(int Id); 
        Task<GetTimeTableResponse> GetSearchTimeTable(string Course); 
        Task<TimeTableResponse> AddTimeTable(AddScheduleClassViewModel model);
    }
}
