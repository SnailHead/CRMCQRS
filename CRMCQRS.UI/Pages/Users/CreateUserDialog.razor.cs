using CRMCQRS.Application.Dto.Users;
using CRMCQRS.Application.Validators.Users;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CRMCQRS.UI.Pages.Users;

public partial class CreateUserDialog
{
    [Inject]
    private NavigationManager _navigationManager { get; set; }

    [Inject]
    private ISnackbar _snackbar { get; set; }

    [Inject]
    private HttpClient _httpClient { get; set; }

    private MudForm _form;
    [CascadingParameter]
    private MudDialogInstance _mudDialog { get; set; }

    [Parameter]
    public CreateUserDto Model { get; set; } = new();

    private CreateUserDtoValidator _validator = new();

    protected override async Task OnInitializedAsync()
    {
    }
    void Cancel() => _mudDialog.Cancel();

    private async Task Submit()
    {
        await _form.Validate();

        if (_form.IsValid)
        {
            var response = await _httpClient.PostAsJsonAsync("users/create", Model);
            if (response.IsSuccessStatusCode)
            {
                _snackbar.Add("Данные сохранены", Severity.Success, options => options.CloseAfterNavigation = false);
            }
            else
            {
                _snackbar.Add("При сохранение возникла ошибка", Severity.Error, options => options.CloseAfterNavigation = false);
            }

            _navigationManager.NavigateTo("users", true);
        }
    }
}