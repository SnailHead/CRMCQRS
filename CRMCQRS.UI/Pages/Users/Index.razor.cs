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
    private ISnackbar _snackbar { get; set; }

    [Inject]
    private IDialogService _dialogService { get; set; }

    private IPagedList<UserViewModel> _pagedList { get; set; }

    private MudForm _form;
    private IRepository<User> _userRepository { get; set; }

    [Inject]
    private IUnitOfWork _unitOfWork { get; set; }
    

    protected override async Task OnInitializedAsync()
    {
        _userRepository = _unitOfWork.GetRepository<User>();
        await GetUsers();
    }

    private async Task SelectedPage(int page)
    {
        _pagedList.PageIndex = page;
        await GetUsers();
    }

    private async Task GetUsers()
    {
        
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