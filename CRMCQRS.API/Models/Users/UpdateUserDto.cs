using AutoMapper;
using CRMCQRS.Application.Common.Mappings;
using CRMCQRS.Application.Users.Commands.UpdateUser;

namespace CRMCQRS.API.Models.Users;

public class UpdateUserDto : IMapWith<UpdateUserCommand>
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Color { get; set; }
    public bool IsFilled { get; set; }
    
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<UpdateUserDto, UpdateUserCommand>();
    }
}