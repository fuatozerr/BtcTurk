using BtcTurk.Dto;
using FluentValidation;

namespace BtcTurk.Validators
{
    public class InstructionValidator : AbstractValidator<InstructionDto>
    {
        public InstructionValidator()
        {
            RuleFor(c => c.UserId).NotEmpty().WithMessage("UserId is required")
                .GreaterThan(0).WithMessage("UserId must not be less than or equal to 0");
            RuleFor(c => c.DayOfMonth).NotEmpty().WithMessage("DayOfMonth is required").GreaterThan(0).LessThanOrEqualTo(28).WithMessage("DayOfMonth must be in the range of 1 to 29 "); ;
            RuleFor(c => c.Amount).NotEmpty().WithMessage("Amount is required")
                .InclusiveBetween(100, 20000).WithMessage("Amount must be between 100 and 20000.");
            RuleFor(prefs => prefs.SendMailNotification || prefs.SendMailNotification || prefs.SendPushNotification)
            .Must(sel => sel == true)
            .WithMessage("At least one channel must be selected\r\n.");
        }
    }
}
