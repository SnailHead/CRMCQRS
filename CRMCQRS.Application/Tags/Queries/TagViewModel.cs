using AutoMapper;
using CRMCQRS.Application.Common.Mappings;
using CRMCQRS.Domain;

namespace CRMCQRS.Application.Tags.Queries;

public class TagViewModel : IMapWith<Tag>
{
    public TagViewModel(Guid id, string title, string color, bool isFilled)
    {
        Id = id;
        Title = title;
        Color = color;
        IsFilled = isFilled;
    }

    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Color { get; set; }
    public bool IsFilled { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Tag, TagViewModel>();
    }
}