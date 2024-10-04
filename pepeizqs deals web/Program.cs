using Herramientas;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using pepeizqs_deals_web.Areas.Identity.Data;
using pepeizqs_deals_web.Data;
using System.IO.Compression;
using Toolbelt.Blazor.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

#region Compresion (Primero)

builder.Services.AddResponseCompression(options =>
{
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "image/svg+xml", "image/jpeg" });
    options.EnableForHttps = true;
});

builder.Services.Configure<BrotliCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.Fastest;
});

builder.Services.Configure<GzipCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.SmallestSize;
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

builder.Services.AddDbContextPool<pepeizqs_deals_webContext>(opciones => opciones.UseSqlServer(conexionTexto));
builder.Services.AddPooledDbContextFactory<pepeizqs_deals_webContext>(opciones => opciones.UseSqlite(conexionTexto));

builder.Services.AddDataProtection().PersistKeysToDbContext<pepeizqs_deals_webContext>().SetDefaultKeyLifetime(TimeSpan.FromDays(30));

//builder.Services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(Directory.GetCurrentDirectory())).SetDefaultKeyLifetime(TimeSpan.FromDays(30));

builder.Services.AddRazorPages();

//----------------------------------------------------------------------------------

#region Detallado en Componentes Razor

builder.Services.AddServerSideBlazor().AddCircuitOptions(x => x.DetailedErrors = true);

#endregion

#region Redireccionador

builder.Services.AddControllersWithViews();

#endregion

#region Seo

builder.Services.AddHeadElementHelper();

#endregion

#region Tareas

builder.Services.Configure<HostOptions>(hostOptions =>
{
	hostOptions.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore;
});

builder.Services.AddSingleton<Tareas.Pings>();
builder.Services.AddSingleton<Tareas.Divisas>();
builder.Services.AddSingleton<Tareas.Sorteos>();
builder.Services.AddSingleton<Tareas.CorreosDeals>();
builder.Services.AddSingleton<Tareas.CorreosApps>();
builder.Services.AddSingleton<Tareas.Pendientes>();
builder.Services.AddSingleton<Tareas.Errores>();
builder.Services.AddSingleton<Tareas.Solicitudes>();

builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Pings>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Divisas>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Sorteos>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.CorreosDeals>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.CorreosApps>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Pendientes>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Errores>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Solicitudes>());

#endregion

#region Acceder Usuario en Codigo y RSS

builder.Services.AddControllers();
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddHttpContextAccessor();

#endregion

#region Tiempo Token Enlaces Correos 

builder.Services.Configure<DataProtectionTokenProviderOptions>(opciones => opciones.TokenLifespan = TimeSpan.FromHours(3));

#endregion

#region Estado Middlewares

builder.Services.AddHealthChecks();

#endregion

//#region Cache

//builder.Services.AddResponseCaching();

//#endregion

#region Decompilador

builder.Services.AddHttpClient<IDecompiladores, Decompiladores2>()
    .ConfigurePrimaryHttpMessageHandler(() =>
        new HttpClientHandler
        {
            AutomaticDecompression = System.Net.DecompressionMethods.GZip,
			MaxConnectionsPerServer = 2
		});

builder.Services.AddSingleton<IDecompiladores, Decompiladores2>();

#endregion

#region Blazor

builder.Services.AddRazorComponents().AddInteractiveServerComponents();

#endregion

//builder.Services.AddSignalR(opciones =>
//{
//	opciones.EnableDetailedErrors = true;
//	//opciones.ClientTimeoutInterval = TimeSpan.FromMinutes(30);
//	//opciones.KeepAliveInterval = TimeSpan.FromMinutes(20);
//	//opciones.MaximumReceiveMessageSize = 102400000;
//});

builder.Services.Configure<HubOptions>(opciones =>
{
	opciones.MaximumReceiveMessageSize = null;
	opciones.EnableDetailedErrors = true;
});

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

//builder.Services.AddMemoryCache();
//builder.Services.AddSession(opciones =>
//{
//	opciones.IdleTimeout = TimeSpan.FromSeconds(10);
//	opciones.Cookie.HttpOnly = true;
//	opciones.Cookie.IsEssential = true;
//});

//builder.Services.AddDataProtection().UseCryptographicAlgorithms(new AuthenticatedEncryptorConfiguration
//{
//    EncryptionAlgorithm = EncryptionAlgorithm.AES_256_CBC,
//    ValidationAlgorithm = ValidationAlgorithm.HMACSHA256
//});





//builder.Services.AddRateLimiter(_ => _
//    .AddFixedWindowLimiter(policyName: "fixed", options =>
//    {
//        options.PermitLimit = 2;
//        options.Window = TimeSpan.FromSeconds(10);
//        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
//        options.QueueLimit = 5;
//    }));

//builder.WebHost.ConfigureKestrel(opciones =>
//{
//	opciones.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(20);
//	opciones.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(60);
//	//serverOptions.Limits.MaxRequestBodySize = 100_000_000;
//	opciones.AllowSynchronousIO = true;
//});

var app = builder.Build();

//if (!app.Environment.IsDevelopment())
//{
//app.UseExceptionHandler("/Error");
app.UseDeveloperExceptionPage();

    app.UseHsts();
//}

//app.Use(async (context, next) =>
//{
//	await next();
//	if (context.Response.StatusCode == 404)
//	{
//		context.Request.Path = "/";
//		await next();
//	}
//});

#region Compresion (Primero)

app.UseResponseCompression();

#endregion

#region Seo

app.UseHeadElementServerPrerendering();

#endregion

app.UseHttpsRedirection();
app.UseStaticFiles();

//#region Cache

//app.UseResponseCaching();

//#endregion

#region Redireccionador

app.MapControllers();

#endregion

//app.MapHealthChecks("/vida");

//app.UseAuthorization();

//app.UseRateLimiter();

app.UseRouting();

app.MapRazorPages();

app.MapBlazorHub(opciones =>
{
	opciones.WebSockets.CloseTimeout = new TimeSpan(1, 1, 1);
	opciones.Transports = Microsoft.AspNetCore.Http.Connections.HttpTransportType.WebSockets | Microsoft.AspNetCore.Http.Connections.HttpTransportType.LongPolling;
});

//app.UseSession();

app.Run();
