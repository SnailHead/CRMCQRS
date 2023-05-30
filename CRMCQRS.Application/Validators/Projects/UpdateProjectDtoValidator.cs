using CRMCQRS.Application.Dto.Projects;
using CRMCQRS.Application.Dto.Tags;
using FluentValidation;

namespace CRMCQRS.Application.Validators.Projects;

public class UpdateProjectDtoValidator: AbstractValidator<UpdateProjectDto>
{
    public UpdateProjectDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotNull()
            .NotEmpty()
            .Length(1, 100);

        RuleFor(x => x.Info);

        RuleFor(x => x.TagIds);
    }

    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result =
            await ValidateAsync(ValidationContext<UpdateProjectDto>.CreateWithOptions((UpdateProjectDto)model,
                x => x.IncludeProperties(propertyName)));
        if (result.IsValid)
            return Array.Empty<string>();
        return result.Errors.Select(e => e.ErrorMessage);
    };
}