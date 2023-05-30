using System.Net.Http.Json;
using CRMCQRS.Application.Dto.Projects;
using CRMCQRS.Application.Notification;
using CRMCQRS.Application.Tags.Queries;
using CRMCQRS.Application.Validators.Projects;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CRMCQRS.UI.Pages.Projects;

public partial class CreateProjectDialog
{
    [Inject]
    private NavigationManager _navigationManager { get; set; }
    [Inject]
    private ISnackbar _snackbar { get; set; }
    [Inject]
    private HttpClient _httpClient { get; set; }

    [CascadingParameter]
    private MudDialogInstance _mudDialog { get; set; }

    private MudForm _form;
    private List<TagViewModel> _tags { get; set; } = new();
    private IEnumerable<string> _selectedTags { get; set; } = new HashSet<string>();

    [Parameter]
    public CreateProjectDto _model { get; set; } = new();

    private CreateProjectDtoValidator _validator = new();

    protected override async Task OnInitializedAsync()
    {
        var response = await _httpClient.GetAsync("tags/get-select");
        if (!response.IsSuccessStatusCode)
        {
            _tags = await response.Content.ReadFromJsonAsync<List<TagViewModel>>();
        }
    }

    private void Cancel() => _mudDialog.Cancel();

    private async Task Submit()
    {
        await _form.Validate();
        if (!_form.IsValid)
        {
            _snackbar.Add(NotificationMessages.FormValidationFail, Severity.Warning);
            return;
        }

        var response = await _httpClient.PostAsJsonAsync("Projects/Create", _model);
        if (!response.IsSuccessStatusCode)
        {
            _snackbar.Add(NotificationMessages.ErrorFromCreate, Severity.Error);
            return;
        }
        _snackbar.Add(NotificationMessages.SuccessCreate, Severity.Success);
        _navigationManager.NavigateTo("tags", true);
    }
}