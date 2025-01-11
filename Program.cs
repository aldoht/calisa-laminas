using System.Text;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages(options => 
{
    options.Conventions.AddPageRoute("/IniciarSesion/Index", "/");
    options.Conventions.AuthorizeFolder("/");
    options.Conventions.AllowAnonymousToFolder("/IniciarSesion");
});

builder.Services.AddScoped<Supabase.Client>(_ => 
    new Supabase.Client(
        builder.Configuration["SUPABASE_URL"]!,
        builder.Configuration["SUPABASE_KEY"],
        new Supabase.SupabaseOptions
        {
            AutoConnectRealtime = true,
            AutoRefreshToken = true
        }
    )
);

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(
    options => 
    {
        options.Cookie.SameSite = SameSiteMode.None;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.IsEssential = true;
        options.LoginPath = "/IniciarSesion/Index";
        options.LogoutPath = "/Configuracion/CerrarSesion/Index";
    }
    );

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
