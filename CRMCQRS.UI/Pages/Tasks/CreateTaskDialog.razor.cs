using CRMCQRS.Domain;
using CRMCQRS.Infrastructure.Repository;
using CRMCQRS.Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using MissionPriority = MedOk.Extensions.Enums.MissionPriority;

namespace CRMCQRS.UI.Pages.Tasks;

public partial class CreateTaskDialog
{
    [Inject]
    private NavigationManager _navigationManager { get; set; }

    [Inject]
    private ISnackbar _snackbar { get; set; }

    [CascadingParameter]
    private MudDialogInstance _mudDialog { get; set; }

    private MudForm _form;
    private IList<Tag> _tags { get; set; } = new List<Tag>();
    private IList<SelectListItem> _users { get; set; } = new List<SelectListItem>();
    private IList<SelectListItem> _projects { get; set; } = new List<SelectListItem>();

    private List<MissionPriority> _missionPriority { get; set; } =
        typeof(MissionPriority).GetEnumValues().Cast<MissionPriority>().ToList();

    private IEnumerable<string> _selectedTags { get; set; } = new HashSet<string>();

    [Parameter]
    public MissionModel _model { get; set; } = new();

    private MissionModelFluentValidator _missionModelValidator = new();
    private IRepository<Project> _projectRepository { get; set; }
    private IRepository<User> _userRepository { get; set; }
    private IRepository<Mission> _missionRepository { get; set; }
    private IRepository<Tag> _tagRepository { get; set; }

    [Inject]
    private IUnitOfWork _unitOfWork { get; set; }

    protected override async Task OnInitializedAsync()
    {
        _projectRepository = _unitOfWork.GetRepository<Project>();
        _userRepository = _unitOfWork.GetRepository<User>();
        _missionRepository = _unitOfWork.GetRepository<Mission>();
        _tagRepository = _unitOfWork.GetRepository<Tag>();

        _users = await _userRepository.GetAllAsync(selector: item =>
            new SelectListItem($"{item.Lastname} {item.Firstname}", item.Id.ToString()), disableTracking: false);
        _projects = await _projectRepository.GetAllAsync(selector: item =>
            new SelectListItem(item.Title, item.Id.ToString()), disableTracking: false);
        
        _tags = await _tagRepository.GetAllAsync(disableTracking: false);
        
        if (_model.Tags != null && _model.Tags.Count > 0)
        {
            _selectedTags =
                new HashSet<string>(_model.TagNames.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList());
        }
    }

    void Cancel() => _mudDialog.Cancel();

    private async Task Submit()
    {
        await _form.Validate();

        if (_form.IsValid)
        {
            if (_model.Id == Guid.Empty)
                await _missionRepository.InsertAsync(MissionModel.ToEntity(_model));
            else
                _missionRepository.Update(MissionModel.ToEntity(_model));
            await _unitOfWork.SaveChangesAsync();
            
            _snackbar.SendAfterSave(_unitOfWork.LastSaveChangesResult.IsOk);

            _navigationManager.NavigateTo("tasks", true);
        }
    }
}