using AutoMapper;
using CRMCQRS.Application.Common.Mappings;
using CRMCQRS.Application.Users.Queries.GetPageUser;

namespace CRMCQRS.Application.Dto.Users;

public class GetPageUserDto : IMapWith<GetPageUserQuery>
{
    public GetPageUserDto(string name, int page)
    {
        Name = name;
        Page = page;
    }

    public string Name { get; set; }
    public int Page { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<GetPageUserDto, GetPageUserQuery>();
    }
}