using AutoMapper;
using CRMCQRS.Application.Common.Mappings;
using CRMCQRS.Application.Tags.Commands.CreateTag;
using CRMCQRS.Application.Tags.Commands.UpdateTag;
using CRMCQRS.Domain;

namespace CRMCQRS.API.Models;

public class UpdateTagDto : IMapWith<UpdateTagCommand>
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Color { get; set; }
    public bool IsFilled { get; set; }
    
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<UpdateTagDto, UpdateTagCommand>();
    }
}