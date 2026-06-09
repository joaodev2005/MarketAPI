using FluentValidation;
using MarketAPI.Communication.Requests;
using MarketAPI.Exception;

namespace MarketAPI.Application.UseCases.Category.Create;

public class CreateCategoryValidator : AbstractValidator<RequestCategoryJson>
{
    public CreateCategoryValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
    .        WithMessage(ResourceMessagesException.VALIDATION_NAME_REQUIRED);
    }
}