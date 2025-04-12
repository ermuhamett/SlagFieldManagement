using FluentValidation;

namespace SlagFieldManagement.Application.Commands.Invalid;

public sealed class InvalidCommandValidator:AbstractValidator<InvalidCommand>
{
    public InvalidCommandValidator()
    {
        RuleFor(x => x.PlaceId)
            .NotEmpty().WithMessage("Идентификатор места обязателен.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Причина очистки обязательна.");
    }
}