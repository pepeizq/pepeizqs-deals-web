#nullable disable

using Herramientas;
using Microsoft.Data.SqlClient;

namespace Tareas.Tiendas
{
	public class JoyBuggy : BackgroundService
	{
		private string id = APIs.JoyBuggy.Tienda.Generar().Id;

		private readonly ILogger<JoyBuggy> _logger;
		private readonly IServiceScopeFactory _factoria;
		private readonly IDecompiladores _decompilador;

		public JoyBuggy(ILogger<JoyBuggy> logger, IServiceScopeFactory factory, IDecompiladores decompilador)
		{
			_logger = logger;
			_factoria = factory;
			_decompilador = decompilador;
		}

		protected override async Task ExecuteAsync(CancellationToken tokenParar)
		{
			Random azar = new Random();
			using PeriodicTimer timer = new PeriodicTimer(TimeSpan.FromSeconds(azar.Next(10, 60)));

			while (await timer.WaitForNextTickAsync(tokenParar))
			{
				WebApplicationBuilder builder = WebApplication.CreateBuilder();
				string piscinaTiendas = builder.Configuration.GetValue<string>("PoolTiendas:Contenido");
				string piscinaUsada = Environment.GetEnvironmentVariable("APP_POOL_ID", EnvironmentVariableTarget.Process);

				if (piscinaTiendas == piscinaUsada)
				{
					SqlConnection conexion = new SqlConnection();

					try
					{
						conexion = Herramientas.BaseDatos.Conectar();
					}
					catch { }

					if (conexion.State == System.Data.ConnectionState.Open)
					{
						TimeSpan siguienteComprobacion = TimeSpan.FromHours(4);

						if (DateTime.Now.Hour == 19)
						{
							siguienteComprobacion = TimeSpan.FromHours(5);
						}

						bool sePuedeUsar = BaseDatos.Admin.Buscar.TiendasPosibleUsar(siguienteComprobacion, id, conexion);

						if (sePuedeUsar == true && BaseDatos.Admin.Buscar.TiendasEnUso(TimeSpan.FromSeconds(60), conexion) == null)
						{
							try
							{
								await Tiendas2.TiendasCargar.TareasGestionador(conexion, id);

								Environment.Exit(1);
							}
							catch (Exception ex)
							{
								BaseDatos.Errores.Insertar.Mensaje(id, ex, conexion);
							}
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