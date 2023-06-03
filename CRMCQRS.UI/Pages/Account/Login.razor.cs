using Blazored.LocalStorage;
using CRMCQRS.Application.Dto.Account;
using CRMCQRS.Application.Dto.Users;
using CRMCQRS.Application.Notification;
using CRMCQRS.Application.Validators.Users;
using CRMCQRS.Domain;
using CRMCQRS.UI.Application;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.JSInterop;
using MudBlazor;

namespace CRMCQRS.UI.Pages.Account;

public partial class Login
{
    [Inject]
    private IJSRuntime _js { get; set; }
    [Inject]
    private NavigationManager _navigationManager { get; set; }
    [Inject]
    private HttpClient _httpClient { get; set; }
    [Inject]
    private ILocalStorageService _localStorage { get; set; }
    [Inject]
    private AuthenticationStateProvider _authState { get; set; }
    [Inject]
    private ISnackbar _snackbar { get; set; }
    [Inject]
    private IConfiguration _configuration { get; set; }

    private LoginDtoValidator _validator = new();

    private MudForm Form;
    private LoginDto _model { get; set; } = new();

    private async Task Submit()
    {
        await Form.Validate();
        var data = new[]
        {
            new KeyValuePair<string, string>("client_id", "CRMCQRS-Identity-ID"),
            new KeyValuePair<string, string>("client_secret", "CRMCQRS-Identity-SECRET"),
            new KeyValuePair<string, string>("scope", "offline_access openid"),
            new KeyValuePair<string, string>("grant_type", "password"),
            new KeyValuePair<string, string>("username", _model.Email),
            new KeyValuePair<string, string>("password", _model.Password),
        };
        string domain = _configuration.GetValue<string>("IdentityUrl") ??
                        throw new ArgumentNullException("IdentityUrl", "Parameter IdentityUrl is null");
        var response = await new HttpClient().PostAsync(domain+"connect/token", new FormUrlEncodedContent(data));
        if (!response.IsSuccessStatusCode)
        {
            _snackbar.Add(NotificationMessages.FormValidationFail, Severity.Error);
            return;
        }
        var tokenDto = await response.Content.ReadFromJsonAsync<TokenDto>();
        await _localStorage.SetItemAsync("access_token", tokenDto.AccessToken);
        await _authState.GetAuthenticationStateAsync();
        
        if (tokenDto != null)
        {
            _navigationManager.NavigateTo("/", forceLoad: true);
        }
    }
}