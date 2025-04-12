using FluentValidation;

namespace SlagFieldManagement.Application.Commands.OutOfUse;

public class OutOfUseCommandValidator:AbstractValidator<OutOfUseCommand>
{
    public OutOfUseCommandValidator()
    {
        RuleFor(x => x.PlaceId)
            .NotEmpty().WithMessage("Идентификатор места обязателен.");
    }
}