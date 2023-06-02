using CRMCQRS.Application.Missions.Queries;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CRMCQRS.UI.Pages.Tasks;

public partial class View
{
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private HttpClient Client { get; set; }
    [Inject] private ISnackbar Snackbar { get; set; }
    [Inject] private IDialogService DialogService { get; set; }
    [Parameter] public Guid Id { get; set; }
    private MissionViewModel Model { get; set; } = new();
}