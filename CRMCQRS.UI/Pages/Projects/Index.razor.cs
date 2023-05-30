using System.Net.Http.Json;
using CRMCQRS.Application.Dto.Projects;
using CRMCQRS.Application.Notification;
using CRMCQRS.Application.Projects.Queries;
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
    [Inject]
    private HttpClient _httpClient { get; set; }

    private IPagedList<ProjectViewModel> _pagedList { get; set; }
    private MudForm _form;

    protected override async Task OnInitializedAsync()
    {
        await GetProjects();
    }

    private async Task SelectedPage(int page)
    {
        _pagedList.PageIndex = page;
        await GetProjects();
    }

    private async Task GetProjects()
    {
        var response = await _httpClient.PostAsJsonAsync("Projects/GetPage", new GetPageProjectDto("", _pagedList.PageIndex));
        if (!response.IsSuccessStatusCode)
        {
            _snackbar.Add(NotificationMessages.ErrorFromGet, Severity.Error);
            return;
        }
        _pagedList = await response.Content.ReadFromJsonAsync<IPagedList<ProjectViewModel>>();
    }

    private void OpenDialog()
    {
        DialogOptions closeOnEscapeKey = new DialogOptions() { CloseOnEscapeKey = true };
        _dialogService.Show<CreateProjectDialog>("Создание проекта", closeOnEscapeKey);
    }

    private async Task ClearFilter()
    {
        await GetProjects();
    }
}