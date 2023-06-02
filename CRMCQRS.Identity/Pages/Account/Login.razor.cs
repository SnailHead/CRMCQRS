using CRMCQRS.Application.Dto.Users;
using CRMCQRS.Application.Validators.Users;
using CRMCQRS.Domain;
using CRMCQRS.Identity.Application.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.JSInterop;
using MudBlazor;

namespace CRMCQRS.Identity.Pages.Account;

public partial class Login
{
    [Inject]
    private IJSRuntime _js { get; set; }
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
        var uri = _navigationManager.ToAbsoluteUri(_navigationManager.Uri);
        if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("ReturnUrl", out var returnUrl))
        {
            _model.RedirectUrl = returnUrl.ToString();
        }
    }

    private async Task Submit()
    {
        await Form.Validate();
        await RedirectTo($"https://localhost:20001/api/account/singin?email={_model.Email}&" +
                   $"password={System.Net.WebUtility.UrlEncode(_model.Password.ToString())}&" +
                   $"redirectUrl={System.Net.WebUtility.UrlEncode(_model.RedirectUrl)}");
    }
    
    private async Task<string> RedirectTo(string path)
    {
        return await _js.InvokeAsync<string>(
            "clientJsfunctions.RedirectTo", path);
    }   
}