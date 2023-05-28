using AutoMapper;
using CRMCQRS.Application.Common.Mappings;
using CRMCQRS.Application.Users.Queries.GetPageUser;

namespace CRMCQRS.API.Models.Users;

public class GetPageUserDto : IMapWith<GetPageUserQuery>
{
    public string Title { get; set; }
    public int Page { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<GetPageUserDto, GetPageUserQuery>();
    }
}