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
			using PeriodicTimer contador = new PeriodicTimer(tiempo);
			{
				while (!tokenParar.IsCancellationRequested && await contador.WaitForNextTickAsync(tokenParar))
				{
					if (DateTime.UtcNow.Second == 0)
					{
						using (AsyncServiceScope scope = _factoria.CreateAsyncScope())
						{
							Tareas tareas = scope.ServiceProvider.GetService<Tareas>();
						
							SqlConnection conexion = BaseDatos.Conectar();

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

							conexion.Dispose();
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
