using Blazored.LocalStorage;
using CRMCQRS.Application.Dto.Projects;
using CRMCQRS.Application.Notification;
using CRMCQRS.Application.Tags.Queries;
using CRMCQRS.Application.Validators.Projects;
using CRMCQRS.UI.Application;
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
    [Inject]
    private ILocalStorageService _localStorage { get; set; }

    [CascadingParameter]
    private MudDialogInstance _mudDialog { get; set; }

    private MudForm _form;
    private List<TagViewModel> _tags { get; set; } = new();
    private List<Guid> _selectedTags { get; set; } = new List<Guid>();

    [Parameter]
    public CreateProjectDto _model { get; set; } = new();

    private CreateProjectDtoValidator _validator = new();

    protected override async Task OnInitializedAsync()
    {
        await _httpClient.SetBearerAuth(_localStorage);

        var response = await _httpClient.GetAsync("tags/get-for-select");
        if (response.IsSuccessStatusCode)
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
        _navigationManager.NavigateTo("projects", true);
    }

    private void SelectItem(IEnumerable<string> list)
    {
        _model.TagIds = _tags.Where(item => list.Contains(item.Title)).Select(item => item.Id).ToList();
    }
}