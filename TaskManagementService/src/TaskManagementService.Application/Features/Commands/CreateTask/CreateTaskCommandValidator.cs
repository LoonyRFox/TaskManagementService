using FluentValidation;
using TaskManagementService.Application.Interfaces;

namespace TaskManagementService.Application.Features.Commands.CreateTask;

public class CreateTaskCommandValidator : AbstractValidator<CreateTaskCommand>
{
    public CreateTaskCommandValidator(ITranslator translator)
    {

        RuleFor(p => p.Title)
            .NotNull()
            .NotEmpty()
            .MaximumLength(100)
            .WithName(p => translator[nameof(p.Title)]);

        RuleFor(x => x.Description)
            .NotNull()
            .NotEmpty()
            .MaximumLength(250)
            .WithName(p => translator[nameof(p.Description)]);
    }
}