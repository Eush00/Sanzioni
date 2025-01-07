using Microsoft.AspNetCore.Authentication.Negotiate;
using Sanzioni.Interfaces;
using Sanzioni.Services;

var builder = WebApplication.CreateBuilder(args);

// Configura il logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.AddFile("Logs/sanzioni-{Date}.txt");

// Configura l'autenticazione Windows in modo più semplice
builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
    .AddNegotiate();

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = options.DefaultPolicy;
});

// Aggiungi i servizi per AD
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IAdGroupService, AdGroupService>();

// Add services to the container
builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=VediSanzioni}/{action=Index}/{id?}");

app.Run();