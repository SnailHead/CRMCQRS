using CRMCQRS.Application.Dto.Missions;
using CRMCQRS.Application.Dto.Projects;
using FluentValidation;

namespace CRMCQRS.Application.Validators.Missions;

public class UpdateMissionDtoValidator: AbstractValidator<UpdateMissionDto>
{
    public UpdateMissionDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotNull()
            .NotEmpty()
            .Length(1, 100);
    }

    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result =
            await ValidateAsync(ValidationContext<UpdateMissionDto>.CreateWithOptions((UpdateMissionDto)model,
                x => x.IncludeProperties(propertyName)));
        if (result.IsValid)
            return Array.Empty<string>();
        return result.Errors.Select(e => e.ErrorMessage);
    };
}