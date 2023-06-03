using AutoMapper;
using CRMCQRS.Application.Common.Mappings;
using CRMCQRS.Application.Users.Queries.GetPageUser;

namespace CRMCQRS.Application.Dto.Users;

public class GetPageUserDto : IMapWith<GetPageUserQuery>
{
    public GetPageUserDto()
    {
        Page = 1;
    }
    public GetPageUserDto(string query, int page)
    {
        Query = query;
        Page = page;
    }

    public string? Query { get; set; }
    public int Page { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<GetPageUserDto, GetPageUserQuery>();
    }
}