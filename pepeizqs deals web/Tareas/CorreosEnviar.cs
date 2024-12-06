#nullable disable

using Herramientas;
using Microsoft.Data.SqlClient;

namespace Tareas
{
	public class CorreosEnviar : BackgroundService
	{
		private readonly ILogger<CorreosEnviar> _logger;
		private readonly IServiceScopeFactory _factoria;
		private readonly IDecompiladores _decompilador;

		public CorreosEnviar(ILogger<CorreosEnviar> logger, IServiceScopeFactory factory, IDecompiladores decompilador)
		{
			_logger = logger;
			_factoria = factory;
			_decompilador = decompilador;
		}

		protected override async Task ExecuteAsync(CancellationToken tokenParar)
		{
			using PeriodicTimer timer = new PeriodicTimer(TimeSpan.FromSeconds(20));

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
							TimeSpan siguienteComprobacion = TimeSpan.FromMinutes(1);

							if (BaseDatos.Admin.Buscar.TareaPosibleUsar("correosEnviar", siguienteComprobacion, conexion) == true)
							{
								BaseDatos.Admin.Actualizar.TareaUso("correosEnviar", DateTime.Now, conexion);

								List<BaseDatos.CorreosEnviar.CorreoPendienteEnviar> pendientes = BaseDatos.CorreosEnviar.Buscar.PendientesEnviar(conexion);

								if (pendientes.Count > 0)
								{
									foreach (var pendiente in pendientes)
									{
										bool enviado = Herramientas.Correos.EnviarCorreo(pendiente.Html, pendiente.Titulo, pendiente.CorreoDesde, pendiente.CorreoHacia);

										if (enviado == true)
										{
											BaseDatos.CorreosEnviar.Borrar.Ejecutar(pendiente.Id, conexion);
										}
										else
										{
											DateTime nuevaFecha = DateTime.Now;
											nuevaFecha = nuevaFecha.AddMinutes(10);

											BaseDatos.Admin.Actualizar.TareaUso("correosEnviar", nuevaFecha, conexion);
											break;
										}
									}
								}
							}
						}
						catch (Exception ex)
						{
							BaseDatos.Errores.Insertar.Mensaje("Tarea - Correos Enviar", ex, conexion);
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
