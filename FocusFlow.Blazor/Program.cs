using FocusFlow.Blazor.Auth;
using FocusFlow.Blazor.Components;
using FocusFlow.Blazor.Constants;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<ProtectedSessionStorage>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthorizationMessageHandler>();
builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthStateProvider>();
builder.Services.AddHttpClient(Constants.HttpClients.LocalApi, c => { c.BaseAddress = new Uri(builder.Configuration[Constants.HttpClients.LocalApi]); });
builder.Services
    .AddHttpClient(Constants.HttpClients.ExternalApi, c => { c.BaseAddress = new Uri(builder.Configuration[Constants.HttpClients.ExternalApi]); })
    .AddHttpMessageHandler<AuthorizationMessageHandler>();

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("MyApi"));
builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("https://localhost:7039") // Your frontend port
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials(); // Required to allow cookies
    });
});
var app = builder.Build();
app.UseCors("AllowFrontend");
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();
app.MapControllers();
app.Run();