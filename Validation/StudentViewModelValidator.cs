using FluentValidation;
using WebApiValidation.ViewModels;
namespace WebApiValidation.Validation
{
	public class StudentViewModelValidator : AbstractValidator<StudentViewModel>
	{
		public StudentViewModelValidator()
		{
			RuleFor(s => s.Name)
				.NotNull().NotEmpty();
			RuleFor(s => s.Contactno)
				.NotNull()
				.NotEmpty()
				.MaximumLength(12).WithMessage("Contact number must be valid means 11 digits!!!");
			RuleFor(s => s.ClassIds)
				.NotEmpty().NotNull();
			RuleFor(s => s.Email)
	            .EmailAddress().WithMessage("Invalid Email!!")
            	.NotNull().NotEmpty();
			RuleFor(d => d.Password)
				.NotNull().WithMessage("Password is Required")
				.NotEmpty().WithMessage("Password cannot be Empty")
				.Length(6,12).WithMessage("Password must be between 6 and 12 Characters!!")
			   .Matches("(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^\\da-zA-Z]).{6,12}$")
			   .WithMessage("Password must be contain alphanumeric & special characters");								 
		}
	}
}
