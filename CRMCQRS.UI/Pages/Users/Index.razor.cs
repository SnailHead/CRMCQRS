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

    private List<UserModel> _userList = new();
    private UsersFilterModel _filterModel = new();
    private MudForm _form;
    public MetaData _metaData { get; set; } = new MetaData();
    private PageParameters _paginationParameters = new PageParameters();
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
        _paginationParameters.PageNumber = page;
        await GetUsers();
    }

    private async Task GetUsers()
    {
        var userPagedList = await _userRepository.GetPagedListAsync(pageSize: _paginationParameters.PageSize,
            pageIndex: _paginationParameters.PageNumber - 1, disableTracking: true);
        _userList = UserModel.FromEntitiesList(userPagedList.Items);
        var pagedList = new PagedList<UserModel>(_userList, userPagedList.TotalCount,
            _paginationParameters.PageNumber,
            _paginationParameters.PageSize);
        _metaData = pagedList.MetaData;
    }

    private void OpenDialog()
    {
        DialogOptions closeOnEscapeKey = new DialogOptions() { CloseOnEscapeKey = true };

        _dialogService.Show<CreateUserDialog>("Создание пользователя", closeOnEscapeKey);
    }

    private async Task DeleteUser(Guid id)
    {
        _userRepository.Delete(id);
        await _unitOfWork.SaveChangesAsync();
        _snackbar.SendAfterSave(_unitOfWork.LastSaveChangesResult.IsOk);
    }

    private async Task ClearFilter()
    {
        _filterModel = new UsersFilterModel();
        await GetUsers();
    }
}