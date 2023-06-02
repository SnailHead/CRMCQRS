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

    [Parameter]
    public CreateUserDto Model { get; set; } = new();

    private CreateUserDtoValidator _validator = new();

    protected override async Task OnInitializedAsync()
    {
    }

    private async Task Submit()
    {
        await _form.Validate();

        if (_form.IsValid)
        {
            var response = await _httpClient.PostAsJsonAsync("users/update", Model);
            if (response.IsSuccessStatusCode)
            {
                _snackbar.Add("Данные сохранены", Severity.Success);
            }
            else
            {
                _snackbar.Add("При сохранение возникла ошибка", Severity.Error);
            }

            _navigationManager.NavigateTo("users", true);
        }
    }
}