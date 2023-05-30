using AutoMapper;
using CRMCQRS.Application.Common.Mappings;
using CRMCQRS.Domain;

namespace CRMCQRS.Application.Missions.Queries;

public class MissionViewModel : IMapWith<Mission>
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Color { get; set; }
    public bool IsFilled { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Mission, MissionViewModel>();
    }
}