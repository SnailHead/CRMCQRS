using Microsoft.AspNetCore.Components;

namespace CRMCQRS.UI.Pages.Tags;

public partial class Index
{
    [Inject]
    private NavigationManager _navigationManager { get; set; }

    [Inject]
    private ISnackbar _snackbar { get; set; }

    [Inject]
    private IDialogService _dialogService { get; set; }

    private IList<TagModel> _tagList = new List<TagModel>();
    private MudForm _form;
    public MetaData _metaData { get; set; } = new MetaData();
    private PageParameters _paginationParameters = new PageParameters();
    private IRepository<Tag> _tagRepository { get; set; }

    [Inject]
    private IUnitOfWork _unitOfWork { get; set; }


    protected override async Task OnInitializedAsync()
    {
        _tagRepository = _unitOfWork.GetRepository<Tag>();
        await GetTags();
    }

    private async Task SelectedPage(int page)
    {
        _paginationParameters.PageNumber = page;
        await GetTags();
    }

    private async Task GetTags()
    {
        var tagPagedList = await _tagRepository.GetPagedListAsync(pageSize: _paginationParameters.PageSize,
            pageIndex: _paginationParameters.PageNumber - 1, disableTracking: true);
        var modelList = TagModel.FromEntitiesList(tagPagedList.Items);
        var pagedList = new PagedList<TagModel>(modelList, tagPagedList.TotalCount,
            _paginationParameters.PageNumber,
            _paginationParameters.PageSize);
        _tagList = modelList;
        _metaData = pagedList.MetaData;
    }

    private async Task DeleteTag(Guid id)
    {
        _tagRepository.Delete(id);
        await _unitOfWork.SaveChangesAsync();
        if (_unitOfWork.LastSaveChangesResult.IsOk)
        {
            _snackbar.Add("Данные удалены", Severity.Success);
            _navigationManager.NavigateTo("tags", true);
        }
        else
        {
            _snackbar.Add("Ошибка при удаление", Severity.Error);
        }
    }

    private void OpenDialog()
    {
        DialogOptions closeOnEscapeKey = new DialogOptions() { CloseOnEscapeKey = true };

        _dialogService.Show<CreateTagDialog>("Создание тега", closeOnEscapeKey);
    }

    private async Task StartedEditingItem(TagModel item)
    {
        DialogOptions closeOnEscapeKey = new DialogOptions() { CloseOnEscapeKey = true };
        var parameters = new DialogParameters { ["_model"] = item };
        _dialogService.Show<CreateTagDialog>("Редактирование тега", parameters, closeOnEscapeKey);
    }

    void CanceledEditingItem(TagModel item)
    {
    }

    void CommittedItemChanges(TagModel item)
    {
    }
}