using CRMCQRS.Domain;
using CRMCQRS.Infrastructure.Pages;
using CRMCQRS.Infrastructure.Repository;
using CRMCQRS.Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CRMCQRS.UI.Pages.Projects;

public partial class Index
{
    [Inject]
    private NavigationManager _navigationManager { get; set; }

    [Inject]
    private ISnackbar _snackbar { get; set; }

    [Inject]
    private IDialogService _dialogService { get; set; }

    private List<ProjectStatisticModel> _projectList = new();
    private ProjectFilterModel _filterModel = new();
    private MudForm _form;
    public MetaData _metaData { get; set; } = new MetaData();
    private PageParameters _paginationParameters = new PageParameters();
    private IRepository<Project> _projectRepository { get; set; }

    [Inject]
    private IUnitOfWork _unitOfWork { get; set; }

    protected override async Task OnInitializedAsync()
    {
        _projectRepository = _unitOfWork.GetRepository<Project>();
        await GetProjects();
    }

    private async Task SelectedPage(int page)
    {
        _paginationParameters.PageNumber = page;
        await GetProjects();
    }

    private async Task GetProjects()
    {
        var projectPagedList = await _projectRepository.GetPagedListAsync(pageSize: _paginationParameters.PageSize,
            pageIndex: _paginationParameters.PageNumber - 1, disableTracking: true,
            predicate: ProjectFilterModel.GetExpression(_filterModel));
        _projectList =  ProjectStatisticModel.FromEntitiesList(projectPagedList.Items.ToList());
        
        var pagedList = new PagedList<ProjectStatisticModel>(_projectList, projectPagedList.TotalCount,
            _paginationParameters.PageNumber,
            _paginationParameters.PageSize);
        _metaData = pagedList.MetaData;
    }

    private void OpenDialog()
    {
        DialogOptions closeOnEscapeKey = new DialogOptions() { CloseOnEscapeKey = true };
        _dialogService.Show<CreateProjectDialog>("Создание проекта", closeOnEscapeKey);
    }

    private async Task ClearFilter()
    {
        _filterModel = new ProjectFilterModel();
        await GetProjects();
    }
}