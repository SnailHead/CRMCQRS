using Microsoft.AspNetCore.Components;

namespace CRMCQRS.UI.Pages.Tasks;

public partial class Index
{
    [Inject]
    private NavigationManager _navigationManager { get; set; }

    [Inject]
    private ISnackbar _snackbar { get; set; }

    [Inject]
    private IDialogService _dialogService { get; set; }

    private List<MissionModel> _taskList = new();
    private MissionFilterModel _filterModel = new();
    private MudForm _form;
    public MetaData _metaData { get; set; } = new MetaData();
    private PageParameters _paginationParameters = new PageParameters();
    private IRepository<Mission> _missionRepository { get; set; }

    [Inject]
    private IUnitOfWork _unitOfWork { get; set; }

    protected override async Task OnInitializedAsync()
    {
        _missionRepository = _unitOfWork.GetRepository<Mission>();
        await GetTasks();
    }

    private async Task SelectedPage(int page)
    {
        _paginationParameters.PageNumber = page;
        await GetTasks();
    }

    private async Task GetTasks()
    {
        var taskPagedList = await _missionRepository.GetPagedListAsync(pageSize: _paginationParameters.PageSize,
            pageIndex: _paginationParameters.PageNumber - 1, disableTracking: true);
        _taskList = MissionModel.FromEntitiesList(taskPagedList.Items);

        var pagedList = new PagedList<MissionModel>(_taskList, taskPagedList.TotalCount,
            _paginationParameters.PageNumber,
            _paginationParameters.PageSize);
        _metaData = pagedList.MetaData;
    }

    private void OpenDialog()
    {
        DialogOptions closeOnEscapeKey = new DialogOptions() { CloseOnEscapeKey = true };
        _dialogService.Show<CreateTaskDialog>("Создание задачи", closeOnEscapeKey);
    }

    private async Task ClearFilter()
    {
        _filterModel = new MissionFilterModel();
        await GetTasks();
    }
}