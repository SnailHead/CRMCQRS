using AutoMapper;
using CRMCQRS.Application.Common.Mappings;
using CRMCQRS.Application.Users.Commands.CreateUser;

namespace CRMCQRS.API.Models.Users;

public class CreateUserDto : IMapWith<CreateUserCommand>
{
    public string Title { get; set; }
    public string Color { get; set; }
    public bool IsFilled { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateUserDto, CreateUserCommand>();
    }
}