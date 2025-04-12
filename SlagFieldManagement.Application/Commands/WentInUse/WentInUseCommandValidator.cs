using FluentValidation;

namespace SlagFieldManagement.Application.Commands.WentInUse;

public sealed class WentInUseCommandValidator:AbstractValidator<WentInUseCommand>
{
    public WentInUseCommandValidator()
    {
        RuleFor(x => x.PlaceId)
            .NotEmpty().WithMessage("Идентификатор места обязателен.");
    }
}