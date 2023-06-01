using CRMCQRS.Application.Dto.Users;
using CRMCQRS.Application.Validators.Users;
using CRMCQRS.Domain;
using CRMCQRS.Identity.Application.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using MudBlazor;

namespace CRMCQRS.Identity.Pages.Account;

public partial class Login
{
    [Inject]
    private IHttpContextAccessor _httpContextAccessor { get; set; }
    [Inject]
    private NavigationManager _navigationManager { get; set; }
    [Inject]
    private SignInManager<User> _signInManager { get; set; }
    [Inject]
    private IAccountService _accountService { get; set; }

    [Inject]
    private UserManager<User> _userManager { get; set; }
    [Inject]
    private ISnackbar _snackbar { get; set; }

    private LoginDtoValidator _validator = new();

    private MudForm Form;
    private LoginDto _model { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
    }

    private async Task Submit()
    {
        await Form.Validate();

        var httpClient = new HttpClient();
        var a = await httpClient.PostAsJsonAsync("http://localhost:5272/api/account/singin", _model);
        if (!a.IsSuccessStatusCode)
        {
            _snackbar.Add("Не верный логин или пароль", Severity.Warning);
        }
    }
}