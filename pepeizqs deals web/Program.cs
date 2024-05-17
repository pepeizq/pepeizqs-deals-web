using Herramientas;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Owl.reCAPTCHA;
using pepeizqs_deals_web.Areas.Identity.Data;
using pepeizqs_deals_web.Data;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);
var conexionTexto = builder.Configuration.GetConnectionString(Herramientas.BaseDatos.cadenaConexion) ?? throw new InvalidOperationException("Connection string 'pepeizqs_deals_webContextConnection' not found.");

builder.Services.AddDataProtection().PersistKeysToDbContext<pepeizqs_deals_webContext>().SetDefaultKeyLifetime(TimeSpan.FromDays(900));

builder.Services.AddDbContext<pepeizqs_deals_webContext>(options => options.UseSqlServer(conexionTexto));
builder.Services.AddDbContextFactory<pepeizqs_deals_webContext>(opt =>
    opt.UseSqlite(Herramientas.BaseDatos.cadenaConexion));

builder.Services.AddDefaultIdentity<Usuario>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
}
).AddEntityFrameworkStores<pepeizqs_deals_webContext>();

builder.Services.AddRazorPages(opciones =>
{
    opciones.Conventions.AddPageRoute("/Sitemap", "Sitemap.xml");
});

builder.Services.AddServerSideBlazor(opciones =>
{
    opciones.DetailedErrors = true;
});

//----------------------------------------------------------------------------------

#region Detallado en Componentes Razor

builder.Services.AddServerSideBlazor().AddCircuitOptions(x => x.DetailedErrors = true);

#endregion

#region Tareas

builder.Services.Configure<HostOptions>(hostOptions =>
{
	hostOptions.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore;
});

builder.Services.AddSingleton<Tareas.Pings>();
builder.Services.AddSingleton<Tareas.Noticias>();
builder.Services.AddSingleton<Tareas.Minimos>();
builder.Services.AddSingleton<Tareas.Divisas>();
builder.Services.AddSingleton<Tareas.Sorteos>();
builder.Services.AddSingleton<Tareas.CorreosDeals>();
builder.Services.AddSingleton<Tareas.CorreosApps>();
builder.Services.AddSingleton<Tareas.Pendientes>();
builder.Services.AddSingleton<Tareas.Errores>();
builder.Services.AddSingleton<Tareas.DLCs>();
builder.Services.AddSingleton<Tareas.Solicitudes>();

builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Pings>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Noticias>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Minimos>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Divisas>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Sorteos>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.CorreosDeals>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.CorreosApps>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Pendientes>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Errores>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.DLCs>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Tareas.Solicitudes>());

#endregion

#region Acceder Usuario en Codigo

builder.Services.AddControllers();
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddHttpContextAccessor();

#endregion

#region Tiempo Token Enlaces Correos 

builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
       options.TokenLifespan = TimeSpan.FromHours(3));

#endregion

#region Estado Middlewares

builder.Services.AddHealthChecks();

#endregion

//----------------------------------------------------------------------------------

builder.Services.AddHttpClient<IDecompiladores, Decompiladores2>()
    .ConfigurePrimaryHttpMessageHandler(() =>
        new HttpClientHandler
        {
            AutomaticDecompression = System.Net.DecompressionMethods.GZip,
			MaxConnectionsPerServer = 2,
		});

builder.Services.AddSingleton<IDecompiladores, Decompiladores2>();

//builder.Services.AddSignalR(opciones =>
//{
//    opciones.EnableDetailedErrors = true;
//    opciones.ClientTimeoutInterval = TimeSpan.FromMinutes(30);
//    opciones.KeepAliveInterval = TimeSpan.FromMinutes(15);
//    opciones.MaximumReceiveMessageSize = 1000;
//});

builder.Services.Configure<IdentityOptions>(opciones =>
{
    opciones.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    opciones.Lockout.MaxFailedAccessAttempts = 15;
    opciones.Lockout.AllowedForNewUsers = true;
    opciones.User.RequireUniqueEmail = true;
});

builder.Services.ConfigureApplicationCookie(opciones =>
{
    opciones.AccessDeniedPath = "/Identity/Account/AccessDenied";
    opciones.Cookie.Name = "cookiePepeizq";
    opciones.ExpireTimeSpan = TimeSpan.FromDays(90);
    opciones.LoginPath = "/Identity/Account/Login";
    opciones.SlidingExpiration = true;
});

builder.Services.AddDataProtection().UseCryptographicAlgorithms(new AuthenticatedEncryptorConfiguration
{
    EncryptionAlgorithm = EncryptionAlgorithm.AES_256_CBC,
    ValidationAlgorithm = ValidationAlgorithm.HMACSHA256
});

builder.Services.Configure<HubOptions>(options =>
{
	options.MaximumReceiveMessageSize = null;
});

builder.Services.AddreCAPTCHAV3(x =>
{
    x.SiteKey = "6Lfxf4AUAAAAAKK-pxZOeWCZOeyx9OVrEvn1Fu2-";
    x.SiteSecret = "6Lfxf4AUAAAAACUB7u6vbTqOQVuLOAIV4f1xKIdq";
});

builder.Services.AddControllersWithViews();

//builder.Services.AddRateLimiter(_ => _
//    .AddFixedWindowLimiter(policyName: "fixed", options =>
//    {
//        options.PermitLimit = 2;
//        options.Window = TimeSpan.FromSeconds(10);
//        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
//        options.QueueLimit = 5;
//    }));

//builder.WebHost.ConfigureKestrel(serverOptions =>
//{
//	serverOptions.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(10);
//	//serverOptions.Limits.MaxRequestBodySize = 100_000_000;
//	serverOptions.AllowSynchronousIO = true;
//});

var app = builder.Build();

//if (!app.Environment.IsDevelopment())
//{
    //app.UseExceptionHandler("/Error");
    app.UseDeveloperExceptionPage();

    app.UseHsts();
//}

app.MapControllers();

app.UseRouting();

app.UseAuthorization();

//app.UseRateLimiter();

//app.UseResponseCaching();

//app.Use(async (context, next) =>
//{
//	await next();
//	if (context.Response.StatusCode == 404)
//	{
//		context.Request.Path = "/Index";
//		await next();
//	}
//});

app.UseHttpsRedirection();

app.UseStaticFiles();

//app.UseStaticFiles(new StaticFileOptions
//{
//	FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "imagenes")),
//	RequestPath = "/imagenes"
//});

//app.MapHealthChecks("/estado");

//app.UseRequestLocalization();

app.MapRazorPages();

app.MapBlazorHub(/*options => options.WebSockets.CloseTimeout = new TimeSpan(1, 1, 1)*/);

app.Run();
