using CRMCQRS.Application.Dto.Tags;
using CRMCQRS.Application.Notification;
using CRMCQRS.Application.Tags.Queries;
using CRMCQRS.Infrastructure.Pages;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CRMCQRS.UI.Pages.Tags;

public partial class Index
{
    [Inject]
    private NavigationManager _navigationManager { get; set; }

    [Inject]
    private ISnackbar _snackbar { get; set; }

    [Inject]
    private HttpClient _httpClient { get; set; }

    [Inject]
    private IDialogService _dialogService { get; set; }

    private IPagedList<TagViewModel> _pagedList { get; set; }
    private MudForm _form;

    protected override async Task OnInitializedAsync()
    {
        await GetTags();
    }

    private async Task SelectedPage(int page)
    {
        _pagedList.PageIndex = page;
        await GetTags();
    }

    private async Task GetTags()
    {
        var response =
            await _httpClient.PostAsJsonAsync<GetPageTagDto>("Tags/GetPage", 
                new GetPageTagDto("", _pagedList.PageIndex));
        
        if (!response.IsSuccessStatusCode)
        {
            _snackbar.Add(NotificationMessages.ErrorFromGet, Severity.Error);
            return;
        }
        _pagedList = await response.Content.ReadFromJsonAsync<IPagedList<TagViewModel>>();
    }

    private async Task DeleteTag(Guid id)
    {
        var response = await _httpClient.GetAsync($"Tags/Delete/{id}");
        if (!response.IsSuccessStatusCode)
        {
            _snackbar.Add(NotificationMessages.ErrorFromGet, Severity.Error);
            return;
        }
        bool result = await response.Content.ReadFromJsonAsync<bool>();
        if (result)
        {
            _snackbar.Add(NotificationMessages.SuccessDelete, Severity.Success);
            _navigationManager.NavigateTo("tags", true);
            return;
        }
        else
        {
            _snackbar.Add(NotificationMessages.ErrorFromDelete, Severity.Error);
            return;
        }
    }

    private void OpenDialog()
    {
        DialogOptions closeOnEscapeKey = new DialogOptions() { CloseOnEscapeKey = true };

        _dialogService.Show<CreateTagDialog>("Создание тега", closeOnEscapeKey);
    }

    private async Task StartedEditingItem(TagViewModel item)
    {
        DialogOptions closeOnEscapeKey = new DialogOptions() { CloseOnEscapeKey = true };
        var parameters = new DialogParameters { ["_model"] = item };
        _dialogService.Show<CreateTagDialog>("Редактирование тега", parameters, closeOnEscapeKey);
    }

    void CanceledEditingItem(TagViewModel item)
    {
    }

    void CommittedItemChanges(TagViewModel item)
    {
    }
}