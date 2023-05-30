using CRMCQRS.UI;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddScoped(sp =>
    new HttpClient
    {
        BaseAddress = new Uri(builder.Configuration.GetValue<string>("ApiUrl") 
                              ?? throw new ArgumentNullException("ApiUrl", "Configuration parameter ApiUrl is null"))
    });


await builder.Build().RunAsync();