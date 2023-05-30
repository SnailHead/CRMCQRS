using System.Net.Http.Json;
using CRMCQRS.Application.Dto.Tags;
using CRMCQRS.Application.Notification;
using CRMCQRS.Application.Validators.Tags;
using CRMCQRS.Domain;
using CRMCQRS.Infrastructure.Repository;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using MudBlazor.Utilities;
using Color = System.Drawing.Color;

namespace CRMCQRS.UI.Pages.Tags;

public partial class CreateTagDialog
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

    [Parameter]
    public CreateTagDto _model { get; set; } = new CreateTagDto();

    private List<int> _colors { get; set; } = typeof(Color).GetEnumValues().Cast<int>().ToList();
    private CreateTagDtoValidator _validator = new ();

    private IRepository<Tag> _tagRepository { get; set; }

    private void Cancel() => _mudDialog.Cancel();

    protected override async Task OnInitializedAsync()
    {
    }

    private async Task Submit()
    {
        await _form.Validate();
        if (!_form.IsValid)
        {
            _snackbar.Add(NotificationMessages.FormValidationFail, Severity.Warning);
            return;
        }

        var response = await _httpClient.PostAsJsonAsync("Tags/Create", _model);
        if (!response.IsSuccessStatusCode)
        {
            _snackbar.Add(NotificationMessages.ErrorFromCreate, Severity.Error);
            return;
        }
        
        _navigationManager.NavigateTo("tags", true);
    }

    private void OnChangeColorPicker(MudColor mudColor)
    {
        _model.Color = mudColor.Value;
    }
}