#nullable disable

using Microsoft.Data.SqlClient;

namespace Herramientas
{
	public class TareasGestionador : BackgroundService
	{
		private readonly ILogger<TareasGestionador> _logger;
		private readonly IServiceScopeFactory _factoria;
		private readonly IDecompiladores _decompilador;
        public bool Funcionando { get; set; }

		public TareasGestionador(ILogger<TareasGestionador> logger, IServiceScopeFactory factory, IDecompiladores decompilador)	
		{
			_logger = logger;
			_factoria = factory;
			_decompilador = decompilador;
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
						Funcionando = true;

						Tareas tareas = scope.ServiceProvider.GetService<Tareas>();

						SqlConnection conexion = BaseDatos.Conectar();

						try
						{
							await Tareas.Portada(conexion);
						}
						catch { }

						try
						{
							await Tareas.Tiendas(conexion, _decompilador);
						}
						catch { }

						try
						{
							await Tareas.Divisas(conexion);
						}
						catch { }

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
			Funcionando = false;

            await base.StopAsync(stoppingToken);
        }
    }
}
