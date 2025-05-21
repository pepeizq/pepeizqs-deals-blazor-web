using ApexCharts;
using Autofac.Core;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using pepeizqs_deals_blazor_web.Componentes;
using pepeizqs_deals_blazor_web.Componentes.Account;
using pepeizqs_deals_blazor_web.Componentes.Cuenta;
using pepeizqs_deals_web.Data;
using System.IO.Compression;

var builder = WebApplication.CreateBuilder(args);

#region Compresion (Primero)

builder.Services.AddResponseCompression(opciones =>
{
	opciones.Providers.Add<GzipCompressionProvider>();
	opciones.EnableForHttps = true;
	opciones.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
				new[] { "application/octet-stream", "application/rss+xml", "text/html", "text/css", "image/png", "image/x-icon", "text/javascript" });
});

builder.Services.Configure<GzipCompressionProviderOptions>(opciones =>
{
	opciones.Level = CompressionLevel.Optimal;
});

#endregion

#region Optimizador

builder.Services.AddWebOptimizer(opciones => {
	opciones.AddCssBundle("/css/bundle.css", new NUglify.Css.CssSettings
	{
		CommentMode = NUglify.Css.CssComment.None,

	}, "lib/bootstrap/dist/css/bootstrap.min.css", "css/maestro.css", "css/cabecera_cuerpo_pie.css", "css/resto.css", "css/site.css", "lib/font-awesome/css/all.css");

	opciones.AddJavaScriptBundle("/superjs.js", "pushNotifications.js", "lib/jquery/dist/jquery.min.js", "lib/bootstrap/dist/js/bootstrap.bundle.min.js", "js/site.js");
});

#endregion

builder.Services.AddRazorComponents().AddInteractiveServerComponents(opciones =>
{
	opciones.DetailedErrors = true;
});

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<UsuarioAcceso>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(opciones =>
{
	opciones.DefaultScheme = IdentityConstants.ApplicationScheme;
	opciones.DefaultSignInScheme = IdentityConstants.ExternalScheme;
})
	.AddIdentityCookies();

var conexionTexto = builder.Configuration.GetConnectionString("pepeizqs_deals_webContextConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<pepeizqs_deals_webContext>(opciones => {
	opciones.UseSqlServer(conexionTexto, opciones2 =>
	{
		opciones2.CommandTimeout(30);
	});
	opciones.EnableSensitiveDataLogging();
	opciones.EnableDetailedErrors();
});

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

builder.Services.ConfigureApplicationCookie(opciones =>
{
	opciones.AccessDeniedPath = "/";
	opciones.Cookie.Name = "cookiePepeizq";
	opciones.ExpireTimeSpan = TimeSpan.FromDays(30);
	opciones.LoginPath = "/account/login";
});

builder.Services.AddSingleton<IEmailSender<Usuario>, IdentityNoOpEmailSender>();

#region Redireccionador

builder.Services.AddControllersWithViews().AddMvcOptions(opciones =>
	opciones.Filters.Add(
		new ResponseCacheAttribute
		{
			NoStore = true,
			Location = ResponseCacheLocation.None
		}));

#endregion

#region Linea Grafico

builder.Services.AddApexCharts(e =>
{
	e.GlobalOptions = new ApexChartBaseOptions
	{
		Debug = false,
		Theme = new Theme
		{
			Palette = PaletteType.Palette2,
			Mode = Mode.Dark
		}
	};
});

#endregion

#region Mejora velocidad carga

builder.Services.AddHsts(opciones =>
{
	opciones.Preload = true;
	opciones.IncludeSubDomains = true;
	opciones.MaxAge = TimeSpan.FromDays(730);
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
	app.UseDeveloperExceptionPage();
	//app.UseExceptionHandler("/Error", createScopeForErrors: true);
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode(opciones =>
{
	opciones.DisableWebSocketCompression = true;
});

#region Compresion (Primero)

app.UseResponseCompression();

#endregion

#region Optimizador (Despues Compresion)

app.UseWebOptimizer();

#endregion

#region Redireccionador

app.MapControllers();

#endregion

app.UseAuthentication();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();
