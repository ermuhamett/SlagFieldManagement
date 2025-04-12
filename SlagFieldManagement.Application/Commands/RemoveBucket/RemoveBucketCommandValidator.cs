using FluentValidation;

namespace SlagFieldManagement.Application.Commands.RemoveBucket;

public sealed class RemoveBucketCommandValidator:AbstractValidator<RemoveBucketCommand>
{
    public RemoveBucketCommandValidator()
    {
        RuleFor(x => x.PlaceId)
            .NotEmpty().WithMessage("Идентификатор места обязателен.");
    }
    
}