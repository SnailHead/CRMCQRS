using System.Reflection;
using CRMCQRS.Application.Common.Mappings;
using CRMCQRS.Infrastructure.Database;
using CRMCQRS.Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
string connectionString = builder.Configuration.GetConnectionString("DefaultConnectionString");

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddAutoMapper(config =>
{
    config.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));
    config.AddProfile(new AssemblyMappingProfile(typeof(CRMCQRS.Application.Common.Filtration.ExpressionBuilder).Assembly));
});
builder.Services.AddDbContext<DefaultDbContext>(option => option.UseSqlServer(connectionString));
builder.Services.AddUnitOfWork<DefaultDbContext>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<Program>();
    cfg.RegisterServicesFromAssemblyContaining<CRMCQRS.Application.Users.Queries.GetPageUser.GetPageUserQuery>();
});
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    //options.Audience = builder.Configuration.GetValue<string>("IdentityServerUrl:Audience");
    options.Authority = builder.Configuration.GetValue<string>("IdentityServerUrl:Authority");
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters.ClockSkew = TimeSpan.FromHours(4);
    options.TokenValidationParameters.ValidateAudience = false;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();
app.UseHsts();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();