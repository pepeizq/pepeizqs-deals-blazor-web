using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using pepeizqs_deals_blazor_web.Componentes;
using pepeizqs_deals_blazor_web.Componentes.Account;
using pepeizqs_deals_blazor_web.Componentes.Cuenta;
using pepeizqs_deals_web.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents().AddInteractiveServerComponents(opciones =>
{
	opciones.DetailedErrors = true;
});

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<UsuarioAcceso>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options =>
{
	options.DefaultScheme = IdentityConstants.ApplicationScheme;
	options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
})
	.AddIdentityCookies();

var conexionTexto = builder.Configuration.GetConnectionString("pepeizqs_deals_webContextConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContextFactory<pepeizqs_deals_webContext>(opciones => {
	opciones.UseSqlServer(conexionTexto, opciones2 =>
	{
		opciones2.CommandTimeout(30);
	});
	opciones.EnableSensitiveDataLogging();
}, ServiceLifetime.Transient);

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<Usuario>(opciones =>
{
	opciones.SignIn.RequireConfirmedAccount = false;
	opciones.Lockout.MaxFailedAccessAttempts = 15;
	opciones.Lockout.AllowedForNewUsers = true;
	opciones.User.RequireUniqueEmail = true;
})
	.AddEntityFrameworkStores<pepeizqs_deals_webContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<Usuario>, IdentityNoOpEmailSender>();

#region Optimizador

builder.Services.AddWebOptimizer(opciones => {
	opciones.AddCssBundle("/css/bundle.css", new NUglify.Css.CssSettings
	{
		CommentMode = NUglify.Css.CssComment.None,

	}, "lib/bootstrap/dist/css/bootstrap.min.css", "css/maestro.css", "css/cabecera_cuerpo_pie.css", "css/resto.css", "css/site.css", "lib/font-awesome/css/all.css");

	opciones.AddJavaScriptBundle("/superjs.js", "pushNotifications.js", "lib/jquery/dist/jquery.min.js", "lib/bootstrap/dist/js/bootstrap.bundle.min.js", "js/site.js");
});

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode(opciones =>
{
	//opciones.DisableWebSocketCompression = true;
});

#region Optimizador (Despues Compresion)

app.UseWebOptimizer();

#endregion



// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();
