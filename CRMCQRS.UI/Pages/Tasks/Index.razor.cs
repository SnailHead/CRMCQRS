using CRMCQRS.Infrastructure.Pages;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CRMCQRS.UI.Pages.Tasks;

public partial class Index
{
    [Inject]
    private NavigationManager _navigationManager { get; set; }

    [Inject]
    private ISnackbar _snackbar { get; set; }

    [Inject]
    private IDialogService _dialogService { get; set; }

    private IPagedList<MissionModel> _pagedList { get; set; }
    private MudForm _form;

    protected override async Task OnInitializedAsync()
    {
        await GetTasks();
    }

    private async Task SelectedPage(int page)
    {
        _pagedList.PageIndex = page;
        await GetTasks();
    }

    private async Task GetTasks()
    {
    }

    private void OpenDialog()
    {
        DialogOptions closeOnEscapeKey = new DialogOptions() { CloseOnEscapeKey = true };
        _dialogService.Show<CreateTaskDialog>("Создание задачи", closeOnEscapeKey);
    }

    private async Task ClearFilter()
    {
        await GetTasks();
    }
}