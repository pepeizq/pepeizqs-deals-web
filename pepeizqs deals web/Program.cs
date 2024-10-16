using Autofac.Core;
using Herramientas;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Pqc.Crypto.Lms;
using pepeizqs_deals_web.Areas.Identity.Data;
using pepeizqs_deals_web.Data;
using Radzen;
using System.IO.Compression;
using Toolbelt.Blazor.Extensions.DependencyInjection;
using System.Net.WebSockets;
using System.Text;

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

builder.Services.AddDbContextPool<pepeizqs_deals_webContext>(opciones => { 
	opciones.UseSqlServer(conexionTexto); 
	opciones.EnableSensitiveDataLogging();
});
builder.Services.AddPooledDbContextFactory<pepeizqs_deals_webContext>(opciones => { 
	opciones.UseSqlite(conexionTexto); 
});

builder.Services.AddDataProtection().PersistKeysToDbContext<pepeizqs_deals_webContext>().SetDefaultKeyLifetime(TimeSpan.FromDays(30));

//builder.Services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(Directory.GetCurrentDirectory())).SetDefaultKeyLifetime(TimeSpan.FromDays(30));

builder.Services.AddRazorPages();

//----------------------------------------------------------------------------------

#region Detallado en Componentes Razor

builder.Services.AddServerSideBlazor(options =>
{
	options.DisconnectedCircuitRetentionPeriod = TimeSpan.FromMinutes(90);
}).AddCircuitOptions(x => x.DetailedErrors = true).AddHubOptions(options =>
{
	options.ClientTimeoutInterval = TimeSpan.FromMinutes(60);
	options.KeepAliveInterval = TimeSpan.FromMinutes(90);
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

builder.Services.AddSingleton<Tareas.Pings>();
builder.Services.AddSingleton<Tareas.Divisas>();
builder.Services.AddSingleton<Tareas.Sorteos>();
builder.Services.AddSingleton<Tareas.CorreosDeals>();
builder.Services.AddSingleton<Tareas.CorreosApps>();
builder.Services.AddSingleton<Tareas.Pendientes>();
builder.Services.AddSingleton<Tareas.Errores>();
builder.Services.AddSingleton<Tareas.Solicitudes>();
builder.Services.AddSingleton<Tareas.LimpiarMinimos>();

builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Pings>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Divisas>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Sorteos>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.CorreosDeals>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.CorreosApps>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Pendientes>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Errores>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Solicitudes>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.LimpiarMinimos>());

#endregion

#region Acceder Usuario en Codigo y RSS

builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddHttpContextAccessor();

#endregion

#region Tiempo Token Enlaces Correos 

builder.Services.Configure<DataProtectionTokenProviderOptions>(opciones => opciones.TokenLifespan = TimeSpan.FromHours(3));

#endregion

#region Estado Middlewares

builder.Services.AddHealthChecks();

#endregion

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

builder.Services.AddSignalR(opciones =>
{
	opciones.EnableDetailedErrors = true;
	opciones.KeepAliveInterval = TimeSpan.FromSeconds(15);
	//opciones.ClientTimeoutInterval = TimeSpan.FromSeconds(30);
	//opciones.MaximumReceiveMessageSize = 102400000;
});

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

#region Radzen Graficos

builder.Services.AddRadzenComponents();

#endregion

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

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.UseSession();

//app.MapHealthChecks("/vida");

app.MapRazorPages();
app.MapBlazorHub(opciones =>
{
	opciones.WebSockets.CloseTimeout = new TimeSpan(1, 1, 1);
	//opciones.Transports = Microsoft.AspNetCore.Http.Connections.HttpTransportType.WebSockets || Microsoft.AspNetCore.Http.Connections.HttpTransportType.LongPolling;
});

#region Seo

app.UseHeadElementServerPrerendering();

#endregion

#region Redireccionador

app.MapControllers();

#endregion

//var webSocketOptions = new WebSocketOptions
//{
//	KeepAliveInterval = TimeSpan.FromMinutes(2)
//};

//app.UseWebSockets(webSocketOptions);

app.Run();
