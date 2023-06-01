using AutoMapper;
using CRMCQRS.Identity.Endpoints.Permission.ViewModel;

namespace CRMCQRS.Identity.Endpoints.Permission;

public class PermissionMappingProfile : Profile
{
    public PermissionMappingProfile()
    {
        CreateMap<PermissionViewModel, CRMCQRS.Domain.Permission>()
            .ForMember(x => x.ApplicationUserProfiles, o => o.Ignore())
            .ForMember(x => x.Id, o => o.Ignore());
    }
}