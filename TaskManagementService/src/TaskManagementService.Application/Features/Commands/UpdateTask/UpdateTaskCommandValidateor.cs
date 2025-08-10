using FluentValidation;
using TaskManagementService.Application.Interfaces;

namespace TaskManagementService.Application.Features.Commands.UpdateTask;

public class UpdateTaskCommandValidateor : AbstractValidator<UpdateTaskCommand>
{
    public UpdateTaskCommandValidateor(ITranslator translator)
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

        RuleFor(x => x.Status)
            .NotNull()
            .NotEmpty()
            .Must(status => Enum.TryParse<TaskStatus>(status, ignoreCase: true, out _))
            .WithMessage(p => $"{translator[nameof(p.Status)]} имеет недопустимое значение. Возможные значения: {string.Join(", ", Enum.GetNames(typeof(TaskStatus)))}")
            .WithName(p => translator[nameof(p.Status)]);
    }
}