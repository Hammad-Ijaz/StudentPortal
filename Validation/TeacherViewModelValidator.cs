using FluentValidation;
using WebApiValidation.ViewModels;

namespace WebApiValidation.Validation
{
    public class TeacherViewModelValidator : AbstractValidator<TeacherViewModel>
    {
        public TeacherViewModelValidator()
        { 
          RuleFor(n => n.Name)
                .NotEmpty().NotNull().WithMessage("Name field must be fill!");
          RuleFor(c => c.Contactno).NotNull().NotEmpty().MaximumLength(12)
                .WithMessage("Contact number must be valid!!!");
		 RuleFor(e => e.Email)
                .NotEmpty().NotNull().WithMessage("Email field must be fill!")
                .EmailAddress().WithMessage("Invalid Email!!");
         RuleFor(p => p.Password)
                .NotEmpty().NotNull().WithMessage("Password field must be fill!")
               .Length(6, 12).WithMessage("Password must be between 6 and 12 Characters!!")
               .Matches("(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^\\da-zA-Z]).{6,12}$")
               .WithMessage("Password must be contain alphanumeric & special characters");
        }
    }
}
