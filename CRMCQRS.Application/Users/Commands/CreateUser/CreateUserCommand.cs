using AutoMapper;
using CRMCQRS.Application.Common.Mappings;
using CRMCQRS.Domain;
using MediatR;

namespace CRMCQRS.Application.Users.Commands.CreateUser;

public class CreateUserCommand : IRequest<Guid>, IMapWith<User>
{
    public string Firstname { get; set; }
    public string Lastname { get; set; } 
    public string? Middlename { get; set; }
    public DateTime? BirthDate { get; set; }
    public int DepartmentId { get; set; }
    public long TelegramChatId { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateUserCommand, User>();
    }
}