using Herramientas;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using pepeizqs_deals_web.Areas.Identity.Data;
using pepeizqs_deals_web.Data;
using System.IO.Compression;
using Toolbelt.Blazor.Extensions.DependencyInjection;
using System.Threading.RateLimiting;
using System.Globalization;
using Microsoft.AspNetCore.Http.Connections;
using ApexCharts;

var builder = WebApplication.CreateBuilder(args);

#region Compresion (Primero)

builder.Services.AddResponseCompression(options =>
{
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
    options.EnableForHttps = true;
});

builder.Services.Configure<BrotliCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.Optimal;
});

builder.Services.Configure<GzipCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.Optimal;
});

#endregion

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
	opciones.UseSqlServer(conexionTexto); 
	opciones.EnableSensitiveDataLogging();
});

//builder.Services.AddPooledDbContextFactory<pepeizqs_deals_webContext>(opciones => { 
//	opciones.UseSqlite(conexionTexto);
//    opciones.EnableSensitiveDataLogging();
//});

builder.Services.AddDataProtection().PersistKeysToDbContext<pepeizqs_deals_webContext>().SetDefaultKeyLifetime(TimeSpan.FromDays(30));

//builder.Services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(Directory.GetCurrentDirectory())).SetDefaultKeyLifetime(TimeSpan.FromDays(30));

builder.Services.AddRazorPages();

//----------------------------------------------------------------------------------

#region Detallado en Componentes Razor

builder.Services.AddServerSideBlazor(opciones =>
{
	opciones.DisconnectedCircuitRetentionPeriod = TimeSpan.FromMinutes(90);
})
.AddCircuitOptions(opciones => 
{
	opciones.DetailedErrors = true;
})
.AddHubOptions(opciones =>
{
	opciones.ClientTimeoutInterval = TimeSpan.FromSeconds(20);
	opciones.KeepAliveInterval = TimeSpan.FromSeconds(20);
});

#endregion

#region Redireccionador

builder.Services.AddControllersWithViews();

#endregion

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromSeconds(10);
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
});

#region Seo

builder.Services.AddHeadElementHelper();

#endregion

#region Tareas

builder.Services.Configure<HostOptions>(opciones =>
{
	opciones.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore;
});

builder.Services.AddSingleton<Tareas.Minimos>();
builder.Services.AddSingleton<Tareas.Pings>();
builder.Services.AddSingleton<Tareas.CorreosEnviar>();
builder.Services.AddSingleton<Tareas.Divisas>();
builder.Services.AddSingleton<Tareas.CorreosDeals>();
builder.Services.AddSingleton<Tareas.CorreosApps>();
builder.Services.AddSingleton<Tareas.Pendientes>();
builder.Services.AddSingleton<Tareas.Errores>();
builder.Services.AddSingleton<Tareas.LimpiarMinimos>();
builder.Services.AddSingleton<Tareas.Patreon>();

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
builder.Services.AddSingleton<Tareas.Tiendas.Allyouplay>();

builder.Services.AddSingleton<Tareas.Suscripciones.EAPlay>();
builder.Services.AddSingleton<Tareas.Suscripciones.XboxGamePass>();
builder.Services.AddSingleton<Tareas.Suscripciones.UbisoftPlusClassics>();
builder.Services.AddSingleton<Tareas.Suscripciones.UbisoftPlusPremium>();

builder.Services.AddSingleton<Tareas.Streaming.GeforceNOW>();

builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Minimos>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Pings>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.CorreosEnviar>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Divisas>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.CorreosDeals>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.CorreosApps>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Pendientes>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Errores>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.LimpiarMinimos>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Patreon>());

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
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Tiendas.Allyouplay>());

builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Suscripciones.EAPlay>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Suscripciones.XboxGamePass>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Suscripciones.UbisoftPlusClassics>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Suscripciones.UbisoftPlusPremium>());

builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Streaming.GeforceNOW>());

#endregion

#region Acceder Usuario en Codigo y RSS

builder.Services.AddControllers().AddNewtonsoftJson();
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

#region Blazor

builder.Services.AddRazorComponents().AddInteractiveServerComponents();

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
	opciones.AccessDeniedPath = "/Identity/Account/AccessDenied";
	opciones.Cookie.Name = "cookiePepeizq";
	opciones.ExpireTimeSpan = TimeSpan.FromDays(30);
	opciones.LoginPath = "/Identity/Account/Login";
	opciones.SlidingExpiration = true;
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
		if (contexto.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
		{
			contexto.HttpContext.Response.Headers.RetryAfter =
				((int)retryAfter.TotalSeconds).ToString(NumberFormatInfo.InvariantInfo);
		}

		contexto.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
		contexto.HttpContext.Response.WriteAsync("Too many requests. Please try again later. If you are a bot, go fuck yourself somewhere else.");

		return new ValueTask();
	};

	opciones.GlobalLimiter = PartitionedRateLimiter.CreateChained(
		PartitionedRateLimiter.Create<HttpContext, string>(contexto =>
		{
			string? usuario = contexto.User.Identity?.Name;

			if (string.IsNullOrEmpty(usuario) == true)
			{
				return RateLimitPartition.GetFixedWindowLimiter(
					partitionKey: contexto.Request.Headers.Host.ToString(),
					factory: partition => new FixedWindowRateLimiterOptions
					{
						AutoReplenishment = true,
						PermitLimit = 200,
						QueueLimit = 0,
						Window = TimeSpan.FromMinutes(1)
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

var app = builder.Build();

//if (!app.Environment.IsDevelopment())
//{
//app.UseExceptionHandler("/Error");
app.UseDeveloperExceptionPage();

    app.UseHsts();
//}

#region Compresion (Primero)

app.UseResponseCompression();

#endregion

app.UseHttpsRedirection();
app.MapStaticAssets();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.MapRazorPages();
app.MapBlazorHub(opciones =>
{
	opciones.WebSockets.CloseTimeout = new TimeSpan(1, 1, 1);
	opciones.Transports = HttpTransportType.WebSockets | HttpTransportType.LongPolling;
});

#region Seo

app.UseHeadElementServerPrerendering();

#endregion

#region Redireccionador

app.MapControllers();

#endregion

#region Antibots

app.UseRateLimiter();

#endregion

app.Run();
