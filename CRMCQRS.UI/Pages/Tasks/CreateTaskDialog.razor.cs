using Blazored.LocalStorage;
using CRMCQRS.Application.Dto.Missions;
using CRMCQRS.Application.Notification;
using CRMCQRS.Application.Validators.Missions;
using CRMCQRS.Domain;
using CRMCQRS.Domain.Common.Enums;
using CRMCQRS.UI.Application;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.Rendering;
using MudBlazor;

namespace CRMCQRS.UI.Pages.Tasks;

public partial class CreateTaskDialog
{
    [Inject]
    private ILocalStorageService _localStorage { get; set; }
    [Inject]
    private HttpClient _httpClient { get; set; }
    [Inject]
    private NavigationManager _navigationManager { get; set; }

    [Inject]
    private ISnackbar _snackbar { get; set; }

    [CascadingParameter]
    private MudDialogInstance _mudDialog { get; set; }

    private MudForm _form;
    private IList<Tag> _tags { get; set; } = new List<Tag>();
    private IList<SelectListItem> _users { get; set; } = new List<SelectListItem>();
    private IList<SelectListItem> _projects { get; set; } = new List<SelectListItem>();

    private List<MissionPriority> _missionPriority { get; set; } =
        typeof(MissionPriority).GetEnumValues().Cast<MissionPriority>().ToList();

    private IEnumerable<string> _selectedTags { get; set; } = new HashSet<string>();

    [Parameter]
    public CreateMissionDto _model { get; set; } = new();

    private CreateMissionDtoValidator _validator = new();

    protected override async Task OnInitializedAsync()
    {
        await _httpClient.SetBearerAuth(_localStorage);
        var response = await _httpClient.GetAsync("");

    }

    void Cancel() => _mudDialog.Cancel();

    private async Task Submit()
    {
        await _form.Validate();
        if (!_form.IsValid)
        {
            _snackbar.Add(NotificationMessages.FormValidationFail, Severity.Warning);
            return;
        }

        if (_form.IsValid)
        {
            _navigationManager.NavigateTo("tasks", true);
        }
    }
}