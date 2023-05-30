using CRMCQRS.Application.Validators.Users;
using CRMCQRS.Domain;
using CRMCQRS.Domain.Common.Enums;
using CRMCQRS.Infrastructure.Repository;
using CRMCQRS.Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CRMCQRS.UI.Pages.Users;

public partial class Update
{
    [Inject]
    private NavigationManager _navigationManager { get; set; }

    [Inject]
    private ISnackbar _snackbar { get; set; }
    [Inject]
    private IDialogService _dialogService { get; set; }

    private MudForm _form;
    private UserStatisticModel _model { get; set; } = new();
    private UpdateUserDtoValidator _userModelValidator = new();

    [Parameter]
    public Guid? Id { get; set; }

    private int _totalTasks { get; set; }
    private int _tasksCompleted { get; set; }
    private int _tasksInProgress { get; set; }
    private List<ChartSeries> _series = new();

    private string[] _xAxisLabels =
        { "Понедельник", "Вторник", "Среда", "Четверг", "Пятница", "Суббота", "Воскресенье" };

    protected override async Task OnInitializedAsync()
    {
        if (Id.HasValue)
        {
            var entity = await _userRepository.GetFirstOrDefaultAsync(predicate: item => item.Id == Id,
                include: item => item.Include(item => item.UserRoles)
                    .ThenInclude(item => item.Role)
                    .Include(item => item.OfficeTimers)
                    .Include(item => item.Missions));
            _model = UserStatisticModel.FromEntity(entity);
            _totalTasks = _model.Missions.Count;
            _tasksInProgress = _model.Missions.Count(item => item.Status != MissionStatus.Complete);
            _tasksCompleted = _model.Missions.Count(item => item.Status == MissionStatus.Complete);
            var groupData = _model.OfficeTimers.GroupBy(item => item.StartTime.Date);
            var chartData = new List<double>();
            foreach (var item in groupData)
            {
                var timeSpan =
                    new TimeSpan(item.Sum((time => ((time.EndTime ?? DateTime.Now) - time.StartTime).Ticks)));

                chartData.Add(timeSpan.Hours + (timeSpan.Microseconds / 10));
            }

            _series = new() { new() { Name = "Время работы", Data = chartData.ToArray() } };
        }
    }

    private async void OpenDialog()
    {
        var entity = await _userRepository.GetFirstOrDefaultAsync(predicate: item => item.Id == Id);
        var model = UserModel.FromEntity(entity); 
        DialogOptions closeOnEscapeKey = new DialogOptions() { CloseOnEscapeKey = true };
        var parameters = new DialogParameters { ["Model"] = model };
        _dialogService.Show<CreateUserDialog>("Редактирование пользователя", parameters, closeOnEscapeKey);
    }
}