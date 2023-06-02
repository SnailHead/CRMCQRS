using CRMCQRS.Identity.BuilderExtensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.ConfigureBaseServices();
builder.Services.ConfigureMapperServicesAsync();
builder.Services.ConfigureSwaggerServices();
builder.Services.ConfigureDatabaseServices(builder);
builder.Services.ConfigureUnitOfWorkServices();
builder.Services.ConfigureMediatorServices();
builder.Services.ConfigureOpenIddictServices();
// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}
app.ConfigureBaseApplication();
app.ConfigureMapperApplication();
app.ConfigureSwaggerApplication();
await app.ConfigureDatabaseApplication();
app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.Run();
