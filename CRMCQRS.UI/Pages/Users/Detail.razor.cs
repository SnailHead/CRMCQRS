using Blazored.LocalStorage;
using CRMCQRS.Application.Notification;
using CRMCQRS.Application.Users.Queries;
using CRMCQRS.UI.Application;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CRMCQRS.UI.Pages.Users;

public partial class Detail
{
    [Inject]
    private HttpClient _httpClient { get; set; }
    [Inject]
    private ILocalStorageService _localStorage { get; set; }
    [Inject]
    private ISnackbar _snackbar { get; set; }
    [Inject]
    private NavigationManager _navigationManager { get; set; }

    private MudForm _form;
    private UserDetailViewModel _model { get; set; } = new();

    [Parameter]
    public Guid Id { get; set; }
    private List<ChartSeries> _series = new();

    private string[] _xAxisLabels =
        { "Понедельник", "Вторник", "Среда", "Четверг", "Пятница", "Суббота", "Воскресенье" };

    protected override async Task OnInitializedAsync()
    {
        await _httpClient.SetBearerAuth(_localStorage);

        var response = await _httpClient.GetAsync("users/"+Id);
        if (!response.IsSuccessStatusCode)
        {
            _snackbar.Add(NotificationMessages.ErrorFromGet, Severity.Error);
            return;
        }
        _model = await response.Content.ReadFromJsonAsync<UserDetailViewModel>();
    }
}