using AutoMapper;
using CRMCQRS.Application.Common.Mappings;
using CRMCQRS.Application.Missions.Queries;
using CRMCQRS.Domain;

namespace CRMCQRS.Application.Users.Queries;

public class UserDetailViewModel : IMapWith<User>
{
    public Guid Id { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string? Middlename { get; set; }
    public string Email { get; set; }
    public DateTime? BirthDate { get; set; }
    public DateTime RegistrationDate { get; set; }
    public string Password { get; set; }
    public int DepartmentId { get; set; }
    public long TelegramChatId { get; set; }
    public int TotalTasks { get; set; }
    public int TasksInProgress { get; set; }
    public int TasksCompleted { get; set; }
    public List<MissionViewModel> Missions { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<User, UserViewModel>();
    }
}