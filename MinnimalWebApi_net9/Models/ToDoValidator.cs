using FluentValidation;

namespace MinnimalWebApi_net9.Models;

public class ToDoValidator:AbstractValidator<AssignmentDto>
{
    public ToDoValidator()
    {
        RuleFor(x=>x.Description).NotEmpty()
            .MinimumLength(3)
            .WithMessage("Description is empty You read Task");
        RuleFor(x => x.EndDate)
            .NotEmpty()
            .WithMessage("EndDate is required.")
            .GreaterThanOrEqualTo(DateTime.Today)
            .WithMessage("EndDate must not be in the past.");
    }
}