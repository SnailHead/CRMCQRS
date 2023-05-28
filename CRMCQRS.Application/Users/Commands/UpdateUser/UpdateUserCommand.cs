using AutoMapper;
using CRMCQRS.Application.Common.Mappings;
using CRMCQRS.Application.Users.Commands.CreateUser;
using CRMCQRS.Domain;
using MediatR;

namespace CRMCQRS.Application.Users.Commands.UpdateUser;

public class UpdateUserCommand : IRequest<bool>, IMapWith<User>
{
    public Guid Id { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string? Middlename { get; set; }
    public DateTime? BirthDate { get; set; }
    public int DepartmentId { get; set; }
    public long TelegramChatId { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<UpdateUserCommand, User>();
    }
}