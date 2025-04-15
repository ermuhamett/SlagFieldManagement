using FluentValidation;

namespace SlagFieldManagement.Application.Commands.CreateBucket;

public sealed class CreateBucketCommandValidator:AbstractValidator<CreateBucketCommand>
{
    public CreateBucketCommandValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Описание ковша обязательно.")
            .MaximumLength(250);
    }
}