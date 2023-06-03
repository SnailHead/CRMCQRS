using Blazored.LocalStorage;
using CRMCQRS.Application.Dto.Users;
using CRMCQRS.Application.Notification;
using CRMCQRS.Application.Users.Queries;
using CRMCQRS.Infrastructure.Pages;
using CRMCQRS.UI.Application;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CRMCQRS.UI.Pages.Users;

public partial class Index
{
    [Inject]
    private NavigationManager _navigationManager { get; set; }
    [Inject]
    private HttpClient _httpClient { get; set; }

    [Inject]
    private ISnackbar _snackbar { get; set; }

    [Inject]
    private ILocalStorageService _localStorage { get; set; }
    [Inject]
    private IDialogService _dialogService { get; set; }

    private PagedList<UserViewModel> _pagedList { get; set; } = new();
    private GetPageUserDto _getPageUserDto { get; set; } = new();

    private MudForm _form;


    protected override async Task OnInitializedAsync()
    {
        await _httpClient.SetBearerAuth(_localStorage);
        await GetUsers();
    }

    private async Task SelectedPage(int page)
    {
        _pagedList.PageIndex = page;
        _getPageUserDto.Page = page;
        await GetUsers();
    }

    private async Task GetUsers()
    {
        var response = await _httpClient.PostAsJsonAsync("users/GetPage", 
            _getPageUserDto);
        
        if (!response.IsSuccessStatusCode)
        {
            _snackbar.Add(NotificationMessages.ErrorFromGet, Severity.Error);
            return;
        }
        _pagedList = await response.Content.ReadFromJsonAsync<PagedList<UserViewModel>>();
    }

    private void OpenDialog()
    {
        DialogOptions closeOnEscapeKey = new DialogOptions() { CloseOnEscapeKey = true };

        _dialogService.Show<CreateUserDialog>("Создание пользователя", closeOnEscapeKey);
    }

    private async Task DeleteUser(Guid id)
    {
    }

    private async Task ClearFilter()
    {
        _getPageUserDto = new GetPageUserDto();
        await GetUsers();
    }
}