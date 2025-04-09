using FluentValidation;

namespace SlagFieldManagement.Application.Commands.CreatePlaceCommand;

public class CreateSlagFieldPlaceCommandValidator:AbstractValidator<CreateSlagFieldPlaceCommand>
{
    public CreateSlagFieldPlaceCommandValidator()
    {
        RuleFor(x => x.Row)
            .NotEmpty().WithMessage("Ряд обязателен.")
            .Matches("^[a-zA-Z0-9]+$").WithMessage("Некорректный формат ряда.");

        RuleFor(x => x.Number)
            .GreaterThan(0).WithMessage("Номер должен быть больше 0.");
    }
}