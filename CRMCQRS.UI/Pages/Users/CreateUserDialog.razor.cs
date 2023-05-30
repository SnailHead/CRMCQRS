using System.Net.Http.Json;
using CRMCQRS.Domain;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CRMCQRS.UI.Pages.Users;

public partial class CreateUserDialog
{
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private ISnackbar Snackbar { get; set; }
    [Inject] private HttpClient Client { get; set; }
    private MudForm Form;
    private List<string> Roles { get; set; } = new();
    private IEnumerable<string> SelectedRoles { get; set; } = new HashSet<string>() { UserRoles.User };
    [Parameter] public UserModel Model { get; set; } = new();
    private UserModelFluentValidator UserModelValidator = new();
    private bool Exists;

    protected override async Task OnInitializedAsync()
    {
        Roles = JsonConvert.DeserializeObject<List<Role>>(await Client.GetStringAsync("roles/get")).Select(item => item.Name)!.ToList();

        if (Model.Roles != null)
        {
            SelectedRoles =
                new HashSet<string>(Model.Roles.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList());
        }
        else
        {
            Model.Roles = UserRoles.User;
        }

        Exists = Model?.Id != Guid.Empty;
    }

    private async Task Submit()
    {
        await Form.Validate();

        if (Form.IsValid)
        {
            var response = await Client.PostAsJsonAsync("users/update", Model);
            if (response.IsSuccessStatusCode)
            {
                Snackbar.Add("Данные сохранены", Severity.Success);
            }
            else
            {
                Snackbar.Add("При сохранение возникла ошибка", Severity.Error);
            }

            NavigationManager.NavigateTo("users", true);
        }
    }
}