#nullable disable

using Microsoft.Data.SqlClient;

namespace Herramientas
{
	public class TareasGestionador : BackgroundService
	{
		private readonly ILogger<TareasGestionador> _logger;
		private readonly IServiceScopeFactory _factoria;

		public TareasGestionador(ILogger<TareasGestionador> logger, IServiceScopeFactory factory)	
		{
			_logger = logger;
			_factoria = factory;
		}

		private readonly TimeSpan tiempo = TimeSpan.FromSeconds(1);

		protected override async Task ExecuteAsync(CancellationToken tokenParar)
		{
			SqlConnection conexion = BaseDatos.Conectar();

			using PeriodicTimer contador = new PeriodicTimer(tiempo);
			{
				while (!tokenParar.IsCancellationRequested && await contador.WaitForNextTickAsync(tokenParar))
				{
					if (DateTime.UtcNow.Second == 0)
					{
						global::BaseDatos.Tiendas.Admin.TareaCambiarUltimaComprobacion(DateTime.Now.ToString());
						
						try
						{
							await Tareas.Portada();
						}
						catch { }

						try
						{
							await Tareas.Tiendas(conexion);
						}
						catch { }					
					}
				}
			}
		}
	}
}
