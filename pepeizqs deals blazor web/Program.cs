using ApexCharts;
using BlazorNotification;
using Herramientas;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using pepeizqs_deals_blazor_web.Componentes;
using pepeizqs_deals_blazor_web.Componentes.Account;
using pepeizqs_deals_web.Data;
using System.Globalization;
using System.IO.Compression;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;

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

	}, "lib/bootstrap/dist/css/bootstrap.css", "lib/bootstrap/dist/css/bootstrap-grid.css", "lib/bootstrap/dist/css/bootstrap-reboot.css", "lib/bootstrap/dist/css/bootstrap-utilities.css", "css/maestro.css", "css/cabecera_cuerpo_pie.css", "css/resto.css", "css/site.css");

	opciones.AddJavaScriptBundle("/superjs.js", "lib/jquery/dist/jquery.min.js", "lib/bootstrap/dist/js/bootstrap.js");
});

#endregion

builder.Services.AddRazorComponents().AddInteractiveServerComponents(opciones =>
{
	opciones.DetailedErrors = true;
});

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<UsuarioAcceso>();
builder.Services.AddScoped<IdentityRedirectManager>();
//builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(opciones =>
{
	opciones.DefaultScheme = IdentityConstants.ApplicationScheme;
	opciones.DefaultSignInScheme = IdentityConstants.ExternalScheme;
}).AddIdentityCookies();

var conexionTexto = builder.Configuration.GetConnectionString("pepeizqs_deals_webContextConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContextPool<pepeizqs_deals_webContext>(opciones => {
	opciones.UseSqlServer(conexionTexto, opciones2 =>
	{
		opciones2.CommandTimeout(30);
	});
	opciones.EnableSensitiveDataLogging();
	opciones.EnableDetailedErrors();
});

builder.Services.ConfigureApplicationCookie(opciones =>
{
	opciones.AccessDeniedPath = "/";
	opciones.Cookie.Name = "cookiePepeizq";
	opciones.ExpireTimeSpan = TimeSpan.FromDays(30);
	opciones.LoginPath = "/account/login";
	opciones.LogoutPath = "/account/logout";
	opciones.SlidingExpiration = true;
	opciones.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
});

builder.Services.AddDataProtection().PersistKeysToDbContext<pepeizqs_deals_webContext>().SetDefaultKeyLifetime(TimeSpan.FromDays(30));

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

#region Tareas

builder.Services.Configure<HostOptions>(opciones =>
{
	opciones.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore;
});

builder.Services.AddSingleton<Tareas.Mantenimiento>();
builder.Services.AddSingleton<Tareas.Minimos>();
builder.Services.AddSingleton<Tareas.Pings>();
builder.Services.AddSingleton<Tareas.CorreosEnviar>();
builder.Services.AddSingleton<Tareas.Divisas>();
builder.Services.AddSingleton<Tareas.CorreosDeals>();
builder.Services.AddSingleton<Tareas.CorreosApps>();
builder.Services.AddSingleton<Tareas.Pendientes>();
builder.Services.AddSingleton<Tareas.Patreon>();
builder.Services.AddSingleton<Tareas.FichasActualizar>();
builder.Services.AddSingleton<Tareas.Duplicados>();

builder.Services.AddSingleton<Tareas.Tiendas.Steam>();
builder.Services.AddSingleton<Tareas.Tiendas.HumbleStore>();
builder.Services.AddSingleton<Tareas.Tiendas.GOG>();
builder.Services.AddSingleton<Tareas.Tiendas.Fanatical>();
builder.Services.AddSingleton<Tareas.Tiendas.GreenManGaming>();
builder.Services.AddSingleton<Tareas.Tiendas.GreenManGamingGold>();
builder.Services.AddSingleton<Tareas.Tiendas.Gamersgate>();
builder.Services.AddSingleton<Tareas.Tiendas.GamesplanetUk>();
builder.Services.AddSingleton<Tareas.Tiendas.GamesplanetFr>();
builder.Services.AddSingleton<Tareas.Tiendas.GamesplanetDe>();
builder.Services.AddSingleton<Tareas.Tiendas.GamesplanetUs>();
builder.Services.AddSingleton<Tareas.Tiendas.WinGameStore>();
builder.Services.AddSingleton<Tareas.Tiendas.IndieGala>();
builder.Services.AddSingleton<Tareas.Tiendas.GameBillet>();
builder.Services.AddSingleton<Tareas.Tiendas._2Game>();
builder.Services.AddSingleton<Tareas.Tiendas.DLGamer>();
builder.Services.AddSingleton<Tareas.Tiendas.Voidu>();
builder.Services.AddSingleton<Tareas.Tiendas.JoyBuggy>();
builder.Services.AddSingleton<Tareas.Tiendas.Battlenet>();
builder.Services.AddSingleton<Tareas.Tiendas.EA>();
builder.Services.AddSingleton<Tareas.Tiendas.EpicGames>();
builder.Services.AddSingleton<Tareas.Tiendas.Ubisoft>();
builder.Services.AddSingleton<Tareas.Tiendas.Playsum>();
//builder.Services.AddSingleton<Tareas.Tiendas.Allyouplay>();
builder.Services.AddSingleton<Tareas.Tiendas.PlanetPlay>();
builder.Services.AddSingleton<Tareas.Tiendas.Nexus>();

builder.Services.AddSingleton<Tareas.Suscripciones.XboxGamePass>();
builder.Services.AddSingleton<Tareas.Suscripciones.UbisoftPlusClassics>();
builder.Services.AddSingleton<Tareas.Suscripciones.UbisoftPlusPremium>();
builder.Services.AddSingleton<Tareas.Suscripciones.AmazonLunaPlus>();

builder.Services.AddSingleton<Tareas.Streaming.GeforceNOW>();
builder.Services.AddSingleton<Tareas.Streaming.AmazonLuna>();
builder.Services.AddSingleton<Tareas.Streaming.Boosteroid>();

builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Mantenimiento>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Minimos>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Pings>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.CorreosEnviar>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Divisas>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.CorreosDeals>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.CorreosApps>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Pendientes>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Patreon>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.FichasActualizar>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Duplicados>());

builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Tiendas.Steam>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Tiendas.HumbleStore>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Tiendas.GOG>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Tiendas.Fanatical>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Tiendas.GreenManGaming>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Tiendas.GreenManGamingGold>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Tiendas.Gamersgate>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Tiendas.GamesplanetUk>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Tiendas.GamesplanetFr>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Tiendas.GamesplanetDe>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Tiendas.GamesplanetUs>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Tiendas.WinGameStore>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Tiendas.IndieGala>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Tiendas.GameBillet>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Tiendas._2Game>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Tiendas.DLGamer>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Tiendas.Voidu>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Tiendas.JoyBuggy>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Tiendas.Battlenet>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Tiendas.EA>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Tiendas.EpicGames>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Tiendas.Ubisoft>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Tiendas.Playsum>());
//builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Tiendas.Allyouplay>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Tiendas.PlanetPlay>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Tiendas.Nexus>());

builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Suscripciones.XboxGamePass>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Suscripciones.UbisoftPlusClassics>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Suscripciones.UbisoftPlusPremium>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Suscripciones.AmazonLunaPlus>());

builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Streaming.GeforceNOW>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Streaming.AmazonLuna>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Streaming.Boosteroid>());

#endregion

#region Decompilador

builder.Services.AddHttpClient<IDecompiladores, Decompiladores2>()
	.ConfigurePrimaryHttpMessageHandler(() =>
		new HttpClientHandler
		{
			AutomaticDecompression = System.Net.DecompressionMethods.GZip,
			MaxConnectionsPerServer = 50
		});

builder.Services.AddSingleton<IDecompiladores, Decompiladores2>();

#endregion

#region CORS necesario para extension navegador

builder.Services.AddCors(policy => {
	policy.AddPolicy("Extension", builder =>
		builder.WithOrigins("https://*:5001/").SetIsOriginAllowedToAllowWildcardSubdomains().AllowAnyOrigin()
	);
});

#endregion

#region Redireccionador

builder.Services.AddControllersWithViews().AddMvcOptions(opciones =>
	opciones.Filters.Add(
		new ResponseCacheAttribute
		{
			NoStore = true,
			Location = ResponseCacheLocation.None
		}));

#endregion

#region Antibots

builder.Services.AddRateLimiter(opciones =>
{
	opciones.OnRejected = (contexto, _) =>
	{
		if (contexto.Lease.TryGetMetadata(MetadataName.RetryAfter, out var reintento))
		{
			contexto.HttpContext.Response.Headers.RetryAfter = ((int)reintento.TotalSeconds).ToString(NumberFormatInfo.InvariantInfo);
		}

		contexto.HttpContext.Response.WriteAsync(@"Your IP is blocked.

If you are a human with blood running through your veins, contact admin@pepeizqdeals.com to remove your block.

If you're a bot, sorry but I'm in the resistance with John Connor.");

		return new ValueTask();
	};

	opciones.GlobalLimiter = PartitionedRateLimiter.CreateChained(
		PartitionedRateLimiter.Create<HttpContext, string>(contexto =>
		{
			var ipAddress = contexto.Connection.RemoteIpAddress;

			if (Bots.botsIps.Contains(ipAddress?.ToString()) == true)
			{
				return RateLimitPartition.GetFixedWindowLimiter(
					partitionKey: contexto.Request.Headers.Host.ToString(),
					factory: partition => new FixedWindowRateLimiterOptions
					{
						AutoReplenishment = false,
						PermitLimit = 1,
						QueueLimit = 1,
						Window = TimeSpan.FromHours(24)
					});
			}
			else
			{
				return RateLimitPartition.GetNoLimiter("");
			}
		})
	);
});

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

#region Tiempo Token Enlaces Correos 

builder.Services.Configure<DataProtectionTokenProviderOptions>(opciones => opciones.TokenLifespan = TimeSpan.FromHours(3));

#endregion

#region Acceder Usuario en Codigo y RSS

builder.Services.AddControllers(opciones =>
{
	opciones.ModelMetadataDetailsProviders.Add(new SystemTextJsonValidationMetadataProvider());
}).AddJsonOptions(opciones2 =>
{
	opciones2.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
	opciones2.JsonSerializerOptions.PropertyNameCaseInsensitive = true;

});
builder.Services.AddHttpContextAccessor();

#endregion

builder.Services.AddBlazorNotification();

var app = builder.Build();

app.Use(async (contexto, next) =>
{
	var ipAddress = contexto.Connection.RemoteIpAddress;

	if (Bots.botsIps.Contains(ipAddress?.ToString()))
	{
		contexto.Response.StatusCode = StatusCodes.Status403Forbidden;
		await contexto.Response.WriteAsync("Your IP is blocked: " + ipAddress?.ToString() + Environment.NewLine + Environment.NewLine +

@"If you are a human with blood running through your veins, contact admin@pepeizqdeals.com to remove your block.

If you're a bot, sorry but I'm in the resistance with John Connor.");

		return;
	}

	await next.Invoke();
});

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

#region CORS necesario para extension navegador web

app.UseCors("Extension");

#endregion

#region Redireccionador

app.MapControllers();

#endregion

#region Antibots

app.UseRateLimiter();

#endregion

app.UseStatusCodePagesWithRedirects("/error");

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();
