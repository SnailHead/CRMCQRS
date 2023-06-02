using CRMCQRS.Application.Dto.Users;
using CRMCQRS.Application.Validators.Users;
using CRMCQRS.Domain.Common.Enums;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CRMCQRS.UI.Pages.Users;

public partial class Update
{
    [Inject]
    private HttpClient _httpClient { get; set; }
    [Inject]
    private NavigationManager _navigationManager { get; set; }

    [Inject]
    private ISnackbar _snackbar { get; set; }

    [Inject]
    private IDialogService _dialogService { get; set; }

    private MudForm _form;
    private UpdateUserDto _model { get; set; } = new();
    private UpdateUserDtoValidator _validator = new();

    [Parameter]
    public Guid? Id { get; set; }

    protected override async Task OnInitializedAsync()
    {
    }
    private async Task Submit()
    {
        await _form.Validate();

        if (_form.IsValid)
        {
            var response = await _httpClient.PostAsJsonAsync("users/update", _model);
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