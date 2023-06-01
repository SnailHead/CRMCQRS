using System.Security.Claims;
using AutoMapper;
using CRMCQRS.Application.Common.Mappings;
using OpenIddict.Abstractions;

namespace CRMCQRS.Identity.Endpoints.Account.ViewModel;

public class UserAccountViewModel : IMapWith<ClaimsIdentity>
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? MiddleName { get; set; }
    public string? Email { get; set; }
    public List<string>? Roles { get; set; }
    public string? PhoneNumber { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<ClaimsIdentity, UserAccountViewModel>()
            .ForMember(prop => prop.Id,
                m => m.MapFrom(item => Guid.Parse(item.Claims.FirstOrDefault(c => c.Type == OpenIddictConstants.Claims.Subject).Value)))
            .ForMember(prop => prop.UserName,
                m => m.MapFrom(item => item.Claims.FirstOrDefault(c => c.Type == OpenIddictConstants.Claims.Name).Value))
            .ForMember(prop => prop.FirstName,
                m => m.MapFrom(item => item.Claims.FirstOrDefault(c => c.Type == OpenIddictConstants.Claims.GivenName).Value))
            .ForMember(prop => prop.LastName,
                m => m.MapFrom(item => item.Claims.FirstOrDefault(c => c.Type == OpenIddictConstants.Claims.FamilyName).Value))
            .ForMember(prop => prop.MiddleName,
                m => m.Ignore())
            .ForMember(prop => prop.Email,
                m => m.MapFrom(item =>
                    item.Claims.FirstOrDefault(c => c.Type == OpenIddictConstants.Claims.Email).Value))
            .ForMember(prop => prop.Roles,
                m => m.MapFrom(item => item.Claims.FirstOrDefault(c => c.Type == OpenIddictConstants.Claims.Role).Value.Split(',', StringSplitOptions.RemoveEmptyEntries)))
            .ForMember(prop => prop.PhoneNumber,
                m => m.MapFrom(item => item.Claims.FirstOrDefault(c => c.Type == OpenIddictConstants.Claims.PhoneNumber).Value))
            .ForMember(prop => prop.PhoneNumber,
                m => m.Ignore())
            ;
    }
}