using CRMCQRS.Application.Dto.Users;
using CRMCQRS.Application.Validators.Users;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CRMCQRS.UI.Pages.Users;

public partial class Login
{
    [Inject]
    private NavigationManager _navigationManager { get; set; }

    [Inject]
    private IConfiguration _configuration { get; set; }

    [Inject]
    private ILocalStorageService localStorage { get; set; }

    [Inject]
    private ISnackbar _snackbar { get; set; }

    [Inject]
    private AuthenticationStateProvider authState { get; set; }

    [Inject]
    private HttpClient Client { get; set; }

    private LoginDtoValidator _validator = new();

    private MudForm Form;
    private LoginDto _model { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
    }

    private async Task Submit()
    {
        await Form.Validate();
        if (Form.IsValid)
        {
            var result = 
                await Client.PostAsJsonAsync(_configuration.GetValue<string>("Authority") + "/Identity/Login",
                _model);
            if (!result.IsSuccessStatusCode)
            {
                _snackbar.Add("Ошибка авторизации", Severity.Error);
                return;
            }

            var token = await result.Content.ReadFromJsonAsync<TokenResponse>();
            await localStorage.SetItemAsync("token", token.AccessToken);
            await authState.GetAuthenticationStateAsync();
            if (token != null)
            {
                _navigationManager.NavigateTo("/", forceLoad: true);
            }
        }
    }
}