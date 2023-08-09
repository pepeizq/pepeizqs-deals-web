using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using pepeizqs_deals_web.Areas.Identity.Data;
using pepeizqs_deals_web.Data;
using Quartz;
using static Quartz.Logging.OperationName;
using Herramientas;

var builder = WebApplication.CreateBuilder(args);
var conexionTexto = builder.Configuration.GetConnectionString("pepeizqs_deals_webContextConnection") ?? throw new InvalidOperationException("Connection string 'pepeizqs_deals_webContextConnection' not found.");

builder.Services.AddDataProtection().PersistKeysToDbContext<pepeizqs_deals_webContext>().SetDefaultKeyLifetime(TimeSpan.FromDays(900));

builder.Services.AddDbContext<pepeizqs_deals_webContext>(options => options.UseSqlServer(conexionTexto));

builder.Services.AddDefaultIdentity<Usuario>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
}
).AddEntityFrameworkStores<pepeizqs_deals_webContext>();

//----------------------------------------------------------------------------------
//Error Detallado en Componentes Razor

builder.Services.AddServerSideBlazor().AddCircuitOptions(x => x.DetailedErrors = true);

//----------------------------------------------------------------------------------
//Tareas Cron

//builder.Services.AddQuartz(q =>
//{
//    q.UseMicrosoftDependencyInjectionScopedJobFactory();
//    var jobKey = new JobKey("CronGestionador");
//    q.AddJob<Herramientas.CronGestionador>(opciones => opciones.WithIdentity(jobKey));

//    q.AddTrigger(opciones => opciones
//        .ForJob(jobKey)
//        .WithIdentity("CronGestionador-trigger")
//		.WithSimpleSchedule(x => x.WithIntervalInMinutes(50).RepeatForever())
//        //.WithSchedule(CronScheduleBuilder.CronSchedule("0 0/45 * * * ?"))
//        //.WithCronSchedule("0 0/45 * * * ?")
//    );
//});
//builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

builder.Services.AddHostedService<TareasGestionador>();

//----------------------------------------------------------------------------------
//Acceder Usuario en Codigo

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();

//----------------------------------------------------------------------------------

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

//builder.Services.AddDataProtection().UseCryptographicAlgorithms(
//	new AuthenticatedEncryptorConfiguration
//	{
//		EncryptionAlgorithm = EncryptionAlgorithm.AES_256_CBC,
//		ValidationAlgorithm = ValidationAlgorithm.HMACSHA256
//	});

builder.Services.AddServerSideBlazor()
    .AddHubOptions(options =>
    {
        options.ClientTimeoutInterval = TimeSpan.FromSeconds(30);
        options.EnableDetailedErrors = false;
        options.HandshakeTimeout = TimeSpan.FromSeconds(15);
        options.KeepAliveInterval = TimeSpan.FromSeconds(15);
        options.MaximumParallelInvocationsPerClient = 1;
        options.MaximumReceiveMessageSize = 32 * 1024;
        options.StreamBufferCapacity = 10;
    });

builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    //app.UseExceptionHandler("/Error");
    app.UseDeveloperExceptionPage();

	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(name: "game",
				pattern: "game/{*id}");

app.MapRazorPages();
app.MapBlazorHub();

app.Run();
