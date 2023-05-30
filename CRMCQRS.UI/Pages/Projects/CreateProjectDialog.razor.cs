using CRMCQRS.Domain;
using CRMCQRS.Infrastructure.Repository;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CRMCQRS.UI.Pages.Projects;

public partial class CreateProjectDialog
{
    [Inject]
    private NavigationManager _navigationManager { get; set; }
    [Inject]
    private ISnackbar _snackbar { get; set; }

    [CascadingParameter]
    private MudDialogInstance _mudDialog { get; set; }

    private MudForm _form;
    private IList<Tag> _tags { get; set; } = new List<Tag>();
    private IEnumerable<string> _selectedTags { get; set; } = new HashSet<string>();

    [Parameter]
    public ProjectModel _model { get; set; } = new();

    private ProjectModelFluentValidator _projectModelValidator = new();
    private IRepository<Tag> _tagRepository { get; set; }
    private IRepository<ProjectTag> _projectTagRepository { get; set; }
    private IRepository<Project> _projectRepository { get; set; }

    protected override async Task OnInitializedAsync()
    {
        _tagRepository = _unitOfWork.GetRepository<Tag>();
        _projectTagRepository = _unitOfWork.GetRepository<ProjectTag>();
        _projectRepository = _unitOfWork.GetRepository<Project>();
        _tags = await _tagRepository.GetAllAsync(disableTracking: false);
        if (_model.TagNames != null)
        {
            _selectedTags =
                new HashSet<string>(_model.TagNames.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList());
        }
    }

    private void Cancel() => _mudDialog.Cancel();

    private async Task Submit()
    {
        await _form.Validate();
        
        if (_form.IsValid)
        {
            if (_model.Id == Guid.Empty)
            {
                var entity = ProjectModel.ToEntity(_model);
                entity.Tags = (await _tagRepository.GetAllAsync(predicate: item => _model.TagNames.Contains(item.Title), 
                    selector:item => new ProjectTag(){ProjectsId = entity.Id, TagsId = item.Id})).ToList();
                await _projectRepository.InsertAsync(entity);
            }
               
            else
            {
                var entity = ProjectModel.ToEntity(_model);
                
                entity.Tags = (await _tagRepository.GetAllAsync(predicate: item => _model.TagNames.Contains(item.Title), 
                    selector:item => new ProjectTag(){ProjectsId = entity.Id, TagsId = item.Id})).ToList();
                var projectTags =
                    await _projectTagRepository.GetAllAsync(predicate: item => item.ProjectsId == _model.Id, disableTracking: true);
                _projectTagRepository.UpdateRelated(projectTags.ToList(),entity.Tags, 
                    (oldItem, newItem) =>  oldItem.ProjectsId == newItem.ProjectsId && oldItem.TagsId == newItem.TagsId);
                _projectRepository.Update(entity);
            }
            
            await _unitOfWork.SaveChangesAsync();
            _snackbar.SendAfterSave(_unitOfWork.LastSaveChangesResult.IsOk);
            _navigationManager.NavigateTo("projects", true);
        }
    }
}