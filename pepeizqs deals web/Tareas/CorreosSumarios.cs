#nullable disable

using Herramientas;
using Microsoft.Data.SqlClient;

namespace Tareas
{
	public class CorreosSumarios : BackgroundService
	{
		private readonly ILogger<CorreosSumarios> _logger;
		private readonly IServiceScopeFactory _factoria;
		private readonly IDecompiladores _decompilador;

		public CorreosSumarios(ILogger<CorreosSumarios> logger, IServiceScopeFactory factory, IDecompiladores decompilador)
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
							TimeSpan siguienteComprobacion = TimeSpan.FromHours(12);

							if (DateTime.Now.Hour == 21)
							{
								siguienteComprobacion = TimeSpan.FromMinutes(30);
							}

							if (BaseDatos.Admin.Buscar.TareaPosibleUsar("correosSumarios", siguienteComprobacion, conexion) == true)
							{
								BaseDatos.Admin.Actualizar.TareaUso("correosSumarios", DateTime.Now, conexion);

								List<string> usuariosId = BaseDatos.Usuarios.Buscar.UsuariosCorreoSumario(conexion);

								if (usuariosId.Count > 0)
								{

								}
							}
						}
						catch (Exception ex)
						{
							BaseDatos.Errores.Insertar.Mensaje("Tarea - Correos Sumarios", ex, conexion);
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
