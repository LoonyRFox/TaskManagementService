using FluentValidation;

namespace TaskManagementServiceLoging.Application.Features.Commands.CreateLog;

public class CreateLogCommandValidator : AbstractValidator<CreateLogCommand>
{
    public CreateLogCommandValidator()
    {

        RuleFor(p => p.Payload)
            .NotNull()
            .NotEmpty()
            .WithName(p => nameof(p.Payload));

        RuleFor(x => x.Type)
            .NotNull()
            .NotEmpty()
            .MaximumLength(20)
            .WithName(p => nameof(p.Type));
    }
}