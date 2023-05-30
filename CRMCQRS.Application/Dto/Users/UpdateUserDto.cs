using AutoMapper;
using CRMCQRS.Application.Common.Mappings;
using CRMCQRS.Application.Users.Commands.UpdateUser;

namespace CRMCQRS.Application.Dto.Users;

public class UpdateUserDto : IMapWith<UpdateUserCommand>
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
        profile.CreateMap<UpdateUserDto, UpdateUserCommand>();
    }
}