using AutoMapper;
using CRMCQRS.Application.Common.Mappings;
using CRMCQRS.Application.Tags.Queries;
using CRMCQRS.Domain;
using CRMCQRS.Domain.Common.Enums;

namespace CRMCQRS.Application.Projects.Queries;

public class ProjectViewModel : IMapWith<Project>
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string? Info { get; set; }
    public DateTime CreationDate { get; set; }
    public int TotalTasks { get; set; }
    public int TasksCompleted { get; set; }
    public int TasksInProgress { get; set; }
    public int HoursSpent { get; set; }
    public List<TagViewModel> Tags { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Project, ProjectViewModel>()
            .ForMember(m => m.Tags,
                c => c.MapFrom(x => x.Tags
                    .Select(item => item.Tag)
                    .Select(item => new TagViewModel(item.Id, item.Title, item.Color, item.IsFilled))))
            .ForMember(m => m.TotalTasks,
                c => c.MapFrom(x => x.Missions.Count(item => item.IsVisible)))
            .ForMember(m => m.TasksCompleted,
                c => c.MapFrom(x => x.Missions.Count(item => item.Status == MissionStatus.Complete)))
            .ForMember(m => m.TasksCompleted,
                c => c.MapFrom(x => x.Missions.Count(item => item.Status != MissionStatus.Complete)))
            .ForMember(m => m.HoursSpent,
                c => c.MapFrom(x => 100 ))//todo добавить в сущность данных о многих таймерах
            ;
    }
}