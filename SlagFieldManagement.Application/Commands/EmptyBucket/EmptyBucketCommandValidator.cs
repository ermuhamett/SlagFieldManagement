using FluentValidation;

namespace SlagFieldManagement.Application.Commands.EmptyBucket;

public sealed class EmptyBucketCommandValidator:AbstractValidator<EmptyBucketCommand>
{
    public EmptyBucketCommandValidator()
    {
        RuleFor(x => x.PlaceId)
            .NotEmpty()
            .WithMessage("Место не найдено или отключено.");
        
        RuleFor(x => x.EndDate)
            .NotEmpty()
            .WithMessage("Дата и время опорожнения обязательны.")
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Дата опорожнения не может быть в будущем.");
    }
}