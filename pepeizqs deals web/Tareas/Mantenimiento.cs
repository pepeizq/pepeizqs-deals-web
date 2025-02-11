#nullable disable

using Herramientas;
using Microsoft.Data.SqlClient;

namespace Tareas
{
	public class Mantenimiento : BackgroundService
	{
		private readonly ILogger<Mantenimiento> _logger;
		private readonly IServiceScopeFactory _factoria;
		private readonly IDecompiladores _decompilador;

		public Mantenimiento(ILogger<Mantenimiento> logger, IServiceScopeFactory factory, IDecompiladores decompilador)
		{
			_logger = logger;
			_factoria = factory;
			_decompilador = decompilador;
		}

		protected override async Task ExecuteAsync(CancellationToken tokenParar)
		{
			using PeriodicTimer timer = new PeriodicTimer(TimeSpan.FromSeconds(60));

			while (await timer.WaitForNextTickAsync(tokenParar))
			{
				WebApplicationBuilder builder = WebApplication.CreateBuilder();
				string piscinaApp = builder.Configuration.GetValue<string>("PoolWeb:Contenido");
				string piscinaUsada = Environment.GetEnvironmentVariable("APP_POOL_ID", EnvironmentVariableTarget.Process);

				if (piscinaApp == piscinaUsada)
				{
					SqlConnection conexion = new SqlConnection();

					try
					{
						conexion = Herramientas.BaseDatos.Conectar();
					}
					catch { }

					if (conexion.State == System.Data.ConnectionState.Open)
					{
						try
						{
							TimeSpan tiempoSiguiente = TimeSpan.FromHours(48);

							if (DateTime.Now.Hour == 4)
							{
								tiempoSiguiente = TimeSpan.FromMinutes(30);
							}

							if (BaseDatos.Admin.Buscar.TareaPosibleUsar("mantenimiento", tiempoSiguiente, conexion) == true)
							{
								BaseDatos.Admin.Actualizar.TareaUso("mantenimiento", DateTime.Now, conexion);

								BaseDatos.Analisis.Limpiar.Ejecutar();
								BaseDatos.Juegos.Limpiar.Minimos();

								Array.ForEach(Directory.GetFiles(@"./wwwroot/imagenes/webps/"), File.Delete);

								HttpClientHandler manjeador = new HttpClientHandler();
								manjeador.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

								HttpClient cliente = new HttpClient(manjeador);
								HttpRequestMessage mensaje = new HttpRequestMessage();
								mensaje.RequestUri = new Uri("https://pepeizqdeals.com");
								mensaje.Headers.Accept.ParseAdd("text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,*/*;q=0.8");
								mensaje.Headers.AcceptEncoding.ParseAdd("gzip, deflate, br");
								mensaje.Headers.AcceptLanguage.ParseAdd("es,en-US;q=0.7,en;q=0.3");
								mensaje.Headers.Connection.ParseAdd("keep-alive");
								mensaje.Headers.UserAgent.ParseAdd("Mozilla/5.0 (Linux; Android 10; Generic Android-x86_64 Build/QD1A.190821.014.C2; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/79.0.3945.36 Safari/537.36");
								HttpResponseMessage respuesta2 = await cliente.SendAsync(mensaje);
							}
						}
						catch (Exception ex)
						{
							BaseDatos.Errores.Insertar.Mensaje("Tarea - Mantenimiento", ex, conexion);
						}
					}
				}
			}
		}

		public override async Task StopAsync(CancellationToken stoppingToken)
		{
			await base.StopAsync(stoppingToken);
		}
	}
}