using FluentValidation;
using MarketAPI.Communication.Requests;
using MarketAPI.Exception;

namespace MarketAPI.Application.UseCases.Category.Update;

public class UpdateCategoryValidator : AbstractValidator<RequestCategoryJson>
{
    public UpdateCategoryValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.VALIDATION_NAME_REQUIRED);
    }
}