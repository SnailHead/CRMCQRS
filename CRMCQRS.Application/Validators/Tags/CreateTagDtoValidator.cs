using CRMCQRS.Application.Dto.Tags;
using FluentValidation;

namespace CRMCQRS.Application.Validators.Tags;

public class CreateTagDtoValidator : AbstractValidator<CreateTagDto>
{
    public CreateTagDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotNull()
            .NotEmpty()
            .Length(1, 100);

        RuleFor(x => x.Color)
            .NotEmpty();

        RuleFor(x => x.IsFilled)
            .NotNull();
    }

    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result =
            await ValidateAsync(ValidationContext<CreateTagDto>.CreateWithOptions((CreateTagDto)model,
                x => x.IncludeProperties(propertyName)));
        if (result.IsValid)
            return Array.Empty<string>();
        return result.Errors.Select(e => e.ErrorMessage);
    };
}