using CRMCQRS.Domain;
using CRMCQRS.Domain.Common.Enums;
using CRMCQRS.Infrastructure.Repository;
using CRMCQRS.Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CRMCQRS.UI.Pages.Projects;

public partial class Update
{
    [Inject]
    private NavigationManager _navigationManager { get; set; }

    [Inject]
    private ISnackbar _snackbar { get; set; }

    [Inject]
    private IDialogService _dialogService { get; set; }

    private MudForm _form;
    private ProjectModel _model { get; set; } = new();
    private ProjectModelFluentValidator _projectModelValidator = new();

    [Parameter]
    public Guid? Id { get; set; }

    private int _totalTasks { get; set; }
    private int _tasksCompleted { get; set; }
    private int _tasksInProgress { get; set; }
    private IRepository<Project> _projectRepository { get; set; }

    [Inject]
    private IUnitOfWork _unitOfWork { get; set; }

    protected override async Task OnInitializedAsync()
    {
        _projectRepository = _unitOfWork.GetRepository<Project>();
        if (Id.HasValue)
        {
            var entity = await _projectRepository.GetFirstOrDefaultAsync(disableTracking:true,
                predicate: item => item.Id == Id, 
                include: proj => proj.Include(i => i.Missions)
                    .Include(i => i.Tags)
                    .ThenInclude(i=> i.Tag));
            _model = ProjectModel.FromEntity(entity);
            _totalTasks = _model.Missions.Count;
            _tasksInProgress = _model.Missions.Count(item => item.Status != MissionStatus.Complete);
            _tasksCompleted = _model.Missions.Count(item => item.Status == MissionStatus.Complete);
        }
    }

    private void OpenDialog()
    {
        DialogOptions closeOnEscapeKey = new DialogOptions() { CloseOnEscapeKey = true };
        var parameters = new DialogParameters { ["_model"] = _model };
        _dialogService.Show<CreateProjectDialog>("Редактирование проекта", parameters, closeOnEscapeKey);
    }
}