using AutoMapper;
using CRMCQRS.Application.Common.Mappings;
using CRMCQRS.Application.Users.Commands.CreateUser;

namespace CRMCQRS.API.Models.Users;

public class CreateUserDto : IMapWith<CreateUserCommand>
{
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string? Middlename { get; set; }
    public DateTime? BirthDate { get; set; }
    public int DepartmentId { get; set; }
    public long TelegramChatId { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateUserDto, CreateUserCommand>();
    }
}