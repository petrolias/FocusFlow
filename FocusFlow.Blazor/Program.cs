using FocusFlow.Blazor.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<ProtectedSessionStorage>();
builder.Services.AddAuthenticationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddAuthorization();
builder.Services.AddScoped<AuthorizationMessageHandler>();
builder.Services.AddHttpClient("LocalApi", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["LocalApi"]);
});
builder.Services.AddHttpClient("ExternalApi", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ExternalApi"]);
    //client.DefaultRequestHeaders.Add("Accept", "application/json");
});//.AddHttpMessageHandler<AuthorizationMessageHandler>();

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("MyApi"));
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();
app.MapControllers();
app.Run();
