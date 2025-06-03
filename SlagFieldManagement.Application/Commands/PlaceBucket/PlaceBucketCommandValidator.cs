using FluentValidation;

namespace SlagFieldManagement.Application.Commands.PlaceBucket;

public sealed class PlaceBucketCommandValidator
    : AbstractValidator<PlaceBucketCommand>
{
    public PlaceBucketCommandValidator()
    {
        RuleFor(x => x.PlaceId)
            .NotEmpty().WithMessage("Идентификатор места обязателен.");

        RuleFor(x => x.BucketId)
            .NotEmpty().WithMessage("Идентификатор ковша обязателен.");

        RuleFor(x => x.MaterialId)
            .NotEmpty().WithMessage("Идентификатор материала обязателен.");

        RuleFor(x => x.SlagWeight)
            .GreaterThan(0).WithMessage("Вес должен быть больше 0.");

        RuleFor(x => x.StartDate)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Дата начала не может быть в будущем.");
    }
}