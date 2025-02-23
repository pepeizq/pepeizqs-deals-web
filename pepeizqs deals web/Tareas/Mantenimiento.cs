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

								List<Noticias.Noticia> noticias = BaseDatos.Noticias.Buscar.Actuales(conexion);

								if (noticias.Count > 0)
								{
									foreach (Noticias.Noticia noticia in noticias)
									{
										await Herramientas.Ficheros.Imagenes.DescargarYGuardar(noticia.Imagen, noticia.Id.ToString() + "-noticia");
									}
								}
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