using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CRMCQRS.UI.Pages.Users;

public partial class Login
{
    [Inject]
    private NavigationManager NavigationManager { get; set; }

    [Inject]
    private IConfiguration Configuration { get; set; }

    [Inject]
    private ILocalStorageService localStorage { get; set; }

    [Inject]
    private ISnackbar Snackbar { get; set; }

    [Inject]
    private AuthenticationStateProvider authState { get; set; }

    [Inject]
    private HttpClient Client { get; set; }

    private MudForm Form;
    private LoginModel Model { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
    }

    private async Task Submit()
    {
        await Form.Validate();
        if (Form.IsValid)
        {
            var result = 
                await Client.PostAsJsonAsync(Configuration.GetValue<string>("Authority") + "/Identity/Login",
                Model);
            if (!result.IsSuccessStatusCode)
            {
                Snackbar.Add("Ошибка авторизации", Severity.Error);
                return;
            }

            var token = await result.Content.ReadFromJsonAsync<TokenResponse>();
            await localStorage.SetItemAsync("token", token.AccessToken);
            await authState.GetAuthenticationStateAsync();
            if (token != null)
            {
                NavigationManager.NavigateTo("/", forceLoad: true);
            }
        }
    }
}