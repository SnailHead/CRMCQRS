using Microsoft.AspNetCore.Components;

namespace CRMCQRS.UI.Pages.Tags;

public partial class CreateTagDialog
{
    [Inject]
    private NavigationManager _navigationManager { get; set; }

    [Inject]
    private ISnackbar _snackbar { get; set; }

    [Inject]
    private IUnitOfWork _unitOfWork { get; set; }

    [CascadingParameter]
    private MudDialogInstance _mudDialog { get; set; }

    private MudForm _form;

    [Parameter]
    public TagModel _model { get; set; } = new TagModel();

    private List<int> _colors { get; set; } = typeof(Color).GetEnumValues().Cast<int>().ToList();
    private TagModelFluentValidator _tagModelValidator = new();
    
    private IRepository<Tag> _tagRepository { get; set; }

    private void Cancel() => _mudDialog.Cancel();
    protected override async Task OnInitializedAsync()
    {
        _tagRepository = _unitOfWork.GetRepository<Tag>();
    }
    private async Task Submit()
    {
        await _form.Validate();

        if (_form.IsValid)
        {
            if (_model.Id == Guid.Empty)
                await _tagRepository.InsertAsync(TagModel.ToEntity(_model));
            else
                _tagRepository.Update(TagModel.ToEntity(_model));
            
            await _unitOfWork.SaveChangesAsync();
            
            _snackbar.SendAfterSave(_unitOfWork.LastSaveChangesResult.IsOk);
            _navigationManager.NavigateTo("tags", true);
        }
    }

    private void OnChangeColorPicker(MudColor mudColor)
    {
        _model.Color = mudColor.Value;
    }
}