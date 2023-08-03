using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using pepeizqs_deals_web.Areas.Identity.Data;
using pepeizqs_deals_web.Data;
using Quartz;

var builder = WebApplication.CreateBuilder(args);
var conexionTexto = builder.Configuration.GetConnectionString("pepeizqs_deals_webContextConnection") ?? throw new InvalidOperationException("Connection string 'pepeizqs_deals_webContextConnection' not found.");

builder.Services.AddDbContext<pepeizqs_deals_webContext>(options => options.UseSqlServer(conexionTexto));

builder.Services.AddDefaultIdentity<Usuario>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
}
).AddEntityFrameworkStores<pepeizqs_deals_webContext>();

//----------------------------------------------------------------------------------
//Tareas Cron

builder.Services.AddQuartz(q =>
{
    q.UseMicrosoftDependencyInjectionScopedJobFactory();
    var jobKey = new JobKey("CronGestionador");
    q.AddJob<Herramientas.CronGestionador>(opciones => opciones.WithIdentity(jobKey));

    q.AddTrigger(opciones => opciones
        .ForJob(jobKey)
        .WithIdentity("CronGestionador-trigger")
		.WithSimpleSchedule(x => x.WithIntervalInMinutes(30).RepeatForever())
	);
});
builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

//----------------------------------------------------------------------------------
//Acceder Usuario en Codigo

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();

//----------------------------------------------------------------------------------

//builder.Services.Configure<IdentityOptions>(options =>
//{
//    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
//    options.Lockout.MaxFailedAccessAttempts = 15;
//    options.Lockout.AllowedForNewUsers = true;
//    options.User.RequireUniqueEmail = true;
//});

builder.Services.ConfigureApplicationCookie(opciones =>
{
    opciones.AccessDeniedPath = "/Identity/Account/AccessDenied";
    opciones.Cookie.Name = "cookiePepeizq";
    opciones.ExpireTimeSpan = TimeSpan.FromDays(30);
    opciones.LoginPath = "/Identity/Account/Login";
    opciones.SlidingExpiration = false;
});

//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
//    options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
//});

//builder.Services.ConfigureApplicationCookie(options => options.Cookie.Name = "pepeCookie");

builder.Services.AddDataProtection().UseCryptographicAlgorithms(
	new AuthenticatedEncryptorConfiguration
	{
		EncryptionAlgorithm = EncryptionAlgorithm.AES_256_CBC,
		ValidationAlgorithm = ValidationAlgorithm.HMACSHA256
	});

builder.Services.AddServerSideBlazor();
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
