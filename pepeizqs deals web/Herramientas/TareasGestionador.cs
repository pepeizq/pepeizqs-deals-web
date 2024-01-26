﻿#nullable disable

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

		protected override async Task ExecuteAsync(CancellationToken tokenParar)
		{
			using PeriodicTimer timer = new(TimeSpan.FromSeconds(60));
			
			try
			{
				while (await timer.WaitForNextTickAsync(tokenParar))
				{
					using (AsyncServiceScope scope = _factoria.CreateAsyncScope())
					{
						Tareas tareas = scope.ServiceProvider.GetService<Tareas>();

						SqlConnection conexion = BaseDatos.Conectar();

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

						if (DateTime.UtcNow.Hour == 14 && DateTime.UtcNow.Minute == 0)
						{
							Tareas.Divisas(conexion);
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
            await base.StopAsync(stoppingToken);
        }
    }
}
