﻿using CRMCQRS.Application.Dto.Tags;
using CRMCQRS.Application.Dto.Users;
using CRMCQRS.Application.Notification;
using CRMCQRS.Application.Tags.Queries;
using CRMCQRS.Application.Users.Queries;
using CRMCQRS.Domain;
using CRMCQRS.Infrastructure.Pages;
using CRMCQRS.Infrastructure.Repository;
using CRMCQRS.Infrastructure.UnitOfWork;
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
    private IDialogService _dialogService { get; set; }

    private PagedList<UserViewModel> _pagedList { get; set; } = new();

    private MudForm _form;


    protected override async Task OnInitializedAsync()
    {
        
        await GetUsers();
    }

    private async Task SelectedPage(int page)
    {
        _pagedList.PageIndex = page;
        await GetUsers();
    }

    private async Task GetUsers()
    {
        var response = await _httpClient.PostAsJsonAsync("users/GetPage", 
                new GetPageUserDto("", _pagedList.PageIndex));
        
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
        await GetUsers();
    }
}