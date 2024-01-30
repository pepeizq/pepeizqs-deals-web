#nullable disable

using Microsoft.Data.SqlClient;

namespace Herramientas
{
	public class TareasGestionador : BackgroundService
	{
		private readonly ILogger<TareasGestionador> _logger;
		private readonly IServiceScopeFactory _factoria;
		public bool funcionando { get; set; }

		public TareasGestionador(ILogger<TareasGestionador> logger, IServiceScopeFactory factory)	
		{
			_logger = logger;
			_factoria = factory;
		}

		protected override async Task ExecuteAsync(CancellationToken tokenParar)
		{
			using PeriodicTimer timer = new(TimeSpan.FromSeconds(60));
			
			try
			{
				while (await timer.WaitForNextTickAsync(tokenParar))
				{
					using (AsyncServiceScope scope = _factoria.CreateAsyncScope())
					{
						funcionando = true;

						Tareas tareas = scope.ServiceProvider.GetService<Tareas>();

						SqlConnection conexion = BaseDatos.Conectar();

						if (conexion.State == System.Data.ConnectionState.Open)
						{
							try
							{
								await Tareas.Portada(conexion);
							}
							catch { }

							try
							{
								await Tareas.Tiendas(conexion);
							}
							catch { }

							try
							{
								await Tareas.Divisas(conexion);
							}
							catch { }
						}

						conexion.Dispose();
					}
				}
			}
			catch (OperationCanceledException)
			{
				
			}
		}

		public override async Task StopAsync(CancellationToken stoppingToken)
        {
			funcionando = false;

            await base.StopAsync(stoppingToken);
        }
    }
}
