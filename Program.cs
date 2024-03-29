using MudBlazor.Services;
using Microsoft.EntityFrameworkCore;
using QuizEdu.Src.Data;
using QuizEdu.Src;

/// Initializes the application and configures the HTTP request pipeline.
var builder = WebApplication.CreateBuilder(args);

/// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMudServices();

/// Add the database context to the container.
builder.Services.AddDbContext<ApplicationDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpClient();

/// Add the repositories for data access
builder.Services.AddScoped<QuizRepository>();
builder.Services.AddScoped<QuestionRepository>();
builder.Services.AddScoped<QuizCombinationRepository>();
builder.Services.AddScoped<GameResultRepository>();

/// Builds the application with the configured services and the request pipeline.
var app = builder.Build();

/// Configure the HTTP request pipeline to use the following middleware.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

/// Run the application with the configured services and middlewares
app.Run();