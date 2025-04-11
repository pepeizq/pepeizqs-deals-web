using ApexCharts;
using Herramientas;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using pepeizqs_deals_web.Areas.Identity.Data;
using pepeizqs_deals_web.Data;
using System.Globalization;
using System.IO.Compression;
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

builder.Services.AddWebOptimizer(opciones => {
	opciones.AddCssBundle("/css/bundle.css", new NUglify.Css.CssSettings
	{
		CommentMode = NUglify.Css.CssComment.None,

	}, "lib/bootstrap/dist/css/bootstrap.min.css", "css/maestro.css", "css/cabecera_cuerpo_pie.css", "css/resto.css", "css/site.css", "lib/font-awesome/css/all.css");

	opciones.AddJavaScriptBundle("/superjs.js", "pushNotifications.js", "lib/jquery/dist/jquery.min.js", "lib/bootstrap/dist/js/bootstrap.bundle.min.js", "js/site.js");
});

var conexionTexto = builder.Configuration.GetConnectionString(Herramientas.BaseDatos.cadenaConexion) ?? throw new InvalidOperationException("Connection string 'pepeizqs_deals_webContextConnection' not found.");

builder.Services.AddDefaultIdentity<Usuario>(opciones =>
{
	opciones.SignIn.RequireConfirmedAccount = false;
	opciones.Lockout.MaxFailedAccessAttempts = 15;
	opciones.Lockout.AllowedForNewUsers = true;
    opciones.User.RequireUniqueEmail = true;
}
).AddEntityFrameworkStores<pepeizqs_deals_webContext>();

builder.Services.AddDbContextPool<pepeizqs_deals_webContext>(opciones => { 
	opciones.UseSqlServer(conexionTexto, opciones2 =>
	{
		opciones2.CommandTimeout(30);
	}); 
	opciones.EnableSensitiveDataLogging();
});

//builder.Services.AddPooledDbContextFactory<pepeizqs_deals_webContext>(opciones => { 
//	opciones.UseSqlite(conexionTexto);
//    opciones.EnableSensitiveDataLogging();
//});

builder.Services.AddDataProtection().PersistKeysToDbContext<pepeizqs_deals_webContext>().SetDefaultKeyLifetime(TimeSpan.FromDays(30));

//builder.Services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(Directory.GetCurrentDirectory())).SetDefaultKeyLifetime(TimeSpan.FromDays(30));

builder.Services.AddRazorPages();

#region Quitar Logs en Compilacion

builder.Logging.AddFilter("Microsoft.AspNetCore.Authorization.*", LogLevel.None);

#endregion

//----------------------------------------------------------------------------------

#region Redireccionador

//builder.Services.AddControllersWithViews();

builder.Services.AddControllersWithViews().AddMvcOptions(opciones =>
	opciones.Filters.Add(
		new ResponseCacheAttribute
		{
			NoStore = true,
			Location = ResponseCacheLocation.None
		}));

#endregion

//builder.Services.AddDistributedMemoryCache();

//builder.Services.AddSession(options =>
//{
//	options.IdleTimeout = TimeSpan.FromSeconds(10);
//	options.Cookie.HttpOnly = true;
//	options.Cookie.IsEssential = true;
//});

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
builder.Services.AddSingleton<Tareas.Errores>();
builder.Services.AddSingleton<Tareas.Patreon>();
builder.Services.AddSingleton<Tareas.RedesSociales>();

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
//builder.Services.AddSingleton<Tareas.Tiendas._2Game>();
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

builder.Services.AddSingleton<Tareas.Suscripciones.EAPlay>();
builder.Services.AddSingleton<Tareas.Suscripciones.XboxGamePass>();
builder.Services.AddSingleton<Tareas.Suscripciones.UbisoftPlusClassics>();
builder.Services.AddSingleton<Tareas.Suscripciones.UbisoftPlusPremium>();
builder.Services.AddSingleton<Tareas.Suscripciones.AmazonLunaPlus>();

builder.Services.AddSingleton<Tareas.Streaming.GeforceNOW>();
builder.Services.AddSingleton<Tareas.Streaming.AmazonLuna>();

builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Mantenimiento>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Minimos>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Pings>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.CorreosEnviar>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Divisas>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.CorreosDeals>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.CorreosApps>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Pendientes>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Errores>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Patreon>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.RedesSociales>());

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
//builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Tiendas._2Game>());
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

builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Suscripciones.EAPlay>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Suscripciones.XboxGamePass>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Suscripciones.UbisoftPlusClassics>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Suscripciones.UbisoftPlusPremium>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Suscripciones.AmazonLunaPlus>());

builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Streaming.GeforceNOW>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Streaming.AmazonLuna>());

#endregion

#region Acceder Usuario en Codigo y RSS

builder.Services.AddControllers(opciones =>
{
	opciones.ModelMetadataDetailsProviders.Add(new SystemTextJsonValidationMetadataProvider());
}).AddNewtonsoftJson();
builder.Services.AddHttpContextAccessor();

#endregion

#region Tiempo Token Enlaces Correos 

builder.Services.Configure<DataProtectionTokenProviderOptions>(opciones => opciones.TokenLifespan = TimeSpan.FromHours(3));

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

#region Blazor Servidor

builder.Services.AddServerSideBlazor();

//builder.Services.AddRazorComponents().AddInteractiveServerComponents(opciones =>
//{
//	opciones.DetailedErrors = true;
//}).AddHubOptions(opciones =>
//{
//	opciones.ClientTimeoutInterval = TimeSpan.FromSeconds(60);
//	opciones.HandshakeTimeout = TimeSpan.FromSeconds(30);
//	opciones.EnableDetailedErrors = true;
//	opciones.MaximumReceiveMessageSize = null;
//}).AddCircuitOptions(opciones =>
//{
//	opciones.DetailedErrors = true;
//});

#endregion

//builder.Services.AddSignalR(opciones =>
//{
//	opciones.EnableDetailedErrors = true;
//	opciones.KeepAliveInterval = TimeSpan.FromSeconds(15);
//	opciones.ClientTimeoutInterval = TimeSpan.FromSeconds(30);
//	opciones.MaximumReceiveMessageSize = 102400000;
//});

#region Necesario para Juegos

builder.Services.Configure<HubOptions>(opciones =>
{
	opciones.MaximumReceiveMessageSize = null;
	opciones.EnableDetailedErrors = true;
});

#endregion

//builder.Services.Configure<IdentityOptions>(opciones =>
//{
//    //opciones.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
//    opciones.Lockout.MaxFailedAccessAttempts = 15;
//    opciones.Lockout.AllowedForNewUsers = true;
//    opciones.User.RequireUniqueEmail = true;
//});

builder.Services.ConfigureApplicationCookie(opciones =>
{
	opciones.AccessDeniedPath = "/";
	opciones.Cookie.Name = "cookiePepeizq";
	opciones.ExpireTimeSpan = TimeSpan.FromDays(30);
	opciones.LoginPath = "/account/login";
});

//builder.Services.AddDataProtection().UseCryptographicAlgorithms(new AuthenticatedEncryptorConfiguration
//{
//    EncryptionAlgorithm = EncryptionAlgorithm.AES_256_CBC,
//    ValidationAlgorithm = ValidationAlgorithm.HMACSHA256
//});

//builder.WebHost.ConfigureKestrel(opciones =>
//{
//	opciones.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(30);
//	opciones.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(60);
//	//serverOptions.Limits.MaxRequestBodySize = 100_000_000;
//	opciones.AllowSynchronousIO = true;
//	opciones.ConfigureHttpsDefaults(opciones2 => {
//		opciones2.SslProtocols = System.Security.Authentication.SslProtocols.Tls13;
//	});
//});

#region Linea Grafico

builder.Services.AddApexCharts(e =>
{
	e.GlobalOptions = new ApexChartBaseOptions
	{
		Debug = false,
		Theme = new Theme { 
			Palette = PaletteType.Palette2,
			Mode = Mode.Dark
		}
	};
}); 

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
			if (Herramientas.BloqueosIps.EstaBloqueada(contexto.Connection?.RemoteIpAddress?.ToString()) == true)
			{
				return RateLimitPartition.GetFixedWindowLimiter(
					partitionKey: contexto.Request.Headers.Host.ToString(),
					factory: partition => new FixedWindowRateLimiterOptions
					{
						AutoReplenishment = false,
						PermitLimit = 1,
						QueueLimit = 1,
						Window = TimeSpan.FromSeconds(1)
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

#region CORS necesario para extension

builder.Services.AddCors(policy => {
	policy.AddPolicy("Extension", builder =>
		builder.WithOrigins("https://*:5001/").SetIsOriginAllowedToAllowWildcardSubdomains().AllowAnyOrigin()
	);
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

//if (!app.Environment.IsDevelopment())
//{
	//app.UseExceptionHandler("/Error");
	app.UseDeveloperExceptionPage();
    app.UseHsts();
//}

app.UseHttpsRedirection();

app.MapStaticAssets();

#region Optimizador (Despues Compresion)

app.UseWebOptimizer();

#endregion

#region Compresion (Primero)

app.UseResponseCompression();

#endregion

app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub(opciones =>
{
	opciones.WebSockets.CloseTimeout = new TimeSpan(1, 1, 1);
	opciones.Transports = HttpTransportType.WebSockets | HttpTransportType.LongPolling | HttpTransportType.ServerSentEvents;
	opciones.AllowStatefulReconnects = true;
});

#region CORS necesario para extension

app.UseCors("Extension");

#endregion

#region Redireccionador

app.MapControllers();

#endregion

#region Antibots

app.UseRateLimiter();

#endregion

#region Login

app.MapRazorPages().WithStaticAssets();

#endregion

app.Run();
