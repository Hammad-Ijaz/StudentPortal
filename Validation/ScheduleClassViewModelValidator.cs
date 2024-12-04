using FluentValidation;
using WebApiValidation.ViewModels;

namespace WebApiValidation.Validation
{
	public class ScheduleClassViewModelValidator : AbstractValidator<ScheduleClassViewModel>
	{
		public ScheduleClassViewModelValidator() { 
		 RuleFor(x => x.ClassId)
				.NotNull().NotEmpty();
         RuleFor(x => x.Course_Id)
					.NotNull().NotEmpty();
		 RuleFor(x => x.TeacherId)
					.NotEmpty().NotNull();
         RuleFor(x => x.DurationTime)
					.NotNull().NotEmpty();
			RuleFor(x => x.Days)
					.NotNull().NotEmpty();
         RuleFor(x => x.StartDate)
					.NotNull().NotEmpty();
         RuleFor(x => x.EndDate)
					.NotNull().NotEmpty();
         RuleFor(x => x.Room)
					.NotNull().NotEmpty();
		}
	}
}
