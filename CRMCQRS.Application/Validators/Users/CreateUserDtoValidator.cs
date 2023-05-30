using CRMCQRS.Application.Dto.Tags;
using CRMCQRS.Application.Dto.Users;
using FluentValidation;

namespace CRMCQRS.Application.Validators.Users;

public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
{
    public CreateUserDtoValidator()
    {
        RuleFor(x => x.Firstname)
            .NotNull()
            .NotEmpty()
            .Length(1, 100);
        RuleFor(x => x.Lastname)
            .NotNull()
            .NotEmpty()
            .Length(1, 100);
        RuleFor(x => x.Middlename);
        RuleFor(x => x.Email)
            .NotNull()
            .NotEmpty()
            .EmailAddress()
            .Length(1, 150);
        RuleFor(x => x.BirthDate);
        RuleFor(x => x.TelegramChatId).NotNull();
        RuleFor(x => x.Password).NotNull().NotEmpty();
        RuleFor(x => x.DepartmentId)
            .NotNull();
    }

    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result =
            await ValidateAsync(ValidationContext<CreateUserDto>.CreateWithOptions((CreateUserDto)model,
                x => x.IncludeProperties(propertyName)));
        if (result.IsValid)
            return Array.Empty<string>();
        return result.Errors.Select(e => e.ErrorMessage);
    };
}