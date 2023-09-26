using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using pepeizqs_deals_web.Areas.Identity.Data;
using pepeizqs_deals_web.Data;
using Herramientas;
using Microsoft.AspNetCore.SignalR;
using Hangfire;
using Hangfire.SqlServer;
using Owl.reCAPTCHA;

var builder = WebApplication.CreateBuilder(args);
var conexionTexto = builder.Configuration.GetConnectionString(Herramientas.BaseDatos.cadenaConexion) ?? throw new InvalidOperationException("Connection string 'pepeizqs_deals_webContextConnection' not found.");

builder.Services.AddDataProtection().PersistKeysToDbContext<pepeizqs_deals_webContext>().SetDefaultKeyLifetime(TimeSpan.FromDays(900));

builder.Services.AddDbContext<pepeizqs_deals_webContext>(options => options.UseSqlServer(conexionTexto));

builder.Services.AddDefaultIdentity<Usuario>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
}
).AddEntityFrameworkStores<pepeizqs_deals_webContext>();

//----------------------------------------------------------------------------------

#region Detallado en Componentes Razor

builder.Services.AddServerSideBlazor().AddCircuitOptions(x => x.DetailedErrors = true);

#endregion

#region Tareas

builder.Services.AddScoped<ITareasGestionador, TareasGestionador>();

builder.Services.AddHangfire(hangfire =>
{
	hangfire.SetDataCompatibilityLevel(CompatibilityLevel.Version_170);
	hangfire.UseSimpleAssemblyNameTypeSerializer();
	hangfire.UseRecommendedSerializerSettings();
	hangfire.UseColouredConsoleLogProvider();
	hangfire.UseSqlServerStorage(conexionTexto,
		new SqlServerStorageOptions
		{
			CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
			SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
			QueuePollInterval = TimeSpan.Zero,
			UseRecommendedIsolationLevel = true,
			DisableGlobalLocks = true
		});

	//BackgroundJobServer servidor = new BackgroundJobServer(new BackgroundJobServerOptions
	//{
	//	ServerName = "servidor"
 //       //Queues = new[] { "portada", "tiendas" }
	//});
});

//builder.Services.AddHangfireServer(action =>
//{
//	action.ServerName = "portada";
//	action.Queues = new[] { "portada" };
//	//action.WorkerCount = 1;
//});

//builder.Services.AddHangfireServer(action =>
//{
//	action.ServerName = "tiendas";
//	action.Queues = new[] { "tiendas" };
//	//action.WorkerCount = 1 /*Environment.ProcessorCount * 5*/;
//});

#endregion

#region Acceder Usuario en Codigo

builder.Services.AddControllers();
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddHttpContextAccessor();

#endregion

//----------------------------------------------------------------------------------

builder.Services.AddResponseCaching();

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

builder.Services.AddServerSideBlazor(options =>
{
    options.DetailedErrors = true;
    options.DisconnectedCircuitMaxRetained = 100;
    options.DisconnectedCircuitRetentionPeriod = TimeSpan.FromMinutes(3);
    options.JSInteropDefaultCallTimeout = TimeSpan.FromMinutes(1);
    options.MaxBufferedUnacknowledgedRenderBatches = 10;
}).AddHubOptions(options =>
{
    options.ClientTimeoutInterval = TimeSpan.FromSeconds(30);
	options.EnableDetailedErrors = true;
    options.HandshakeTimeout = TimeSpan.FromSeconds(15);
    options.KeepAliveInterval = TimeSpan.FromSeconds(15);
}).AddCircuitOptions(options =>
{
    options.DetailedErrors = true;
    options.DisconnectedCircuitRetentionPeriod = TimeSpan.FromMinutes(3);
    options.DisconnectedCircuitMaxRetained = 0;
}); ;

builder.Services.Configure<HubOptions>(options =>
{
	options.MaximumReceiveMessageSize = null;
});

builder.Services.AddreCAPTCHAV3(x =>
{
    x.SiteKey = "6Lfxf4AUAAAAAKK-pxZOeWCZOeyx9OVrEvn1Fu2-";
    x.SiteSecret = "6Lfxf4AUAAAAACUB7u6vbTqOQVuLOAIV4f1xKIdq";
});

builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

//builder.Services.AddRateLimiter(_ => _
//    .AddFixedWindowLimiter(policyName: "fixed", options =>
//    {
//        options.PermitLimit = 4;
//        options.Window = TimeSpan.FromSeconds(12);
//        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
//        options.QueueLimit = 2;
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

//app.Use(async (contexto, siguiente) => {
//	if (contexto.Request.Path.StartsWithSegments("/robots.txt"))
//	{
//		string robotsFichero = Path.Combine(app.Environment.ContentRootPath, $"robots.{app.Environment.EnvironmentName}.txt");
//		string contenido = "User-agent: *  \nDisallow: /";
		
//		if (File.Exists(robotsFichero))
//		{
//			contenido = await File.ReadAllTextAsync(robotsFichero);
//		}

//		contexto.Response.ContentType = "text/plain";
//		await contexto.Response.WriteAsync(contenido);
//	}
//	else await siguiente();
//});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

//app.UseResponseCaching();

app.UseAuthorization();

app.MapControllerRoute(name: "game",
				pattern: "{controller=Game}/{action=CogerJuegoId}/{id?}");

app.MapRazorPages();

app.MapControllers();
app.MapBlazorHub(options => options.WebSockets.CloseTimeout = new TimeSpan(1, 1, 1));

app.UseRequestLocalization();

app.UseHangfireDashboard();
app.MapHangfireDashboard();

app.UseResponseCaching();

app.Run();
