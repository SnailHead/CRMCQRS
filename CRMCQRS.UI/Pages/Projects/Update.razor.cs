using System.Net.Http.Json;
using CRMCQRS.Application.Notification;
using CRMCQRS.Application.Projects.Queries;
using CRMCQRS.Application.Validators.Projects;
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
    private HttpClient _httpClient { get; set; }

    [Inject]
    private IDialogService _dialogService { get; set; }

    private MudForm _form;
    private ProjectViewModel _model { get; set; } = new();

    [Parameter]
    public Guid? Id { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var response = await _httpClient.GetAsync($"Projects/{Id}");
        if (!response.IsSuccessStatusCode)
        {
            _snackbar.Add(NotificationMessages.ErrorFromGet, Severity.Error);
            return;
        }

        _model = await response.Content.ReadFromJsonAsync<ProjectViewModel>();
    }

    private void OpenDialog()
    {
        DialogOptions closeOnEscapeKey = new DialogOptions() { CloseOnEscapeKey = true };
        var parameters = new DialogParameters { ["_model"] = _model };
        _dialogService.Show<CreateProjectDialog>("Редактирование проекта", parameters, closeOnEscapeKey);
    }
}