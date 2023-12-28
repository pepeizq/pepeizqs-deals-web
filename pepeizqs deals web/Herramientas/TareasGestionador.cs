#nullable disable

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

		private readonly TimeSpan tiempo = TimeSpan.FromSeconds(120);

		protected override async Task ExecuteAsync(CancellationToken tokenParar)
		{
			using PeriodicTimer contador = new PeriodicTimer(tiempo);
			{
				while (!tokenParar.IsCancellationRequested && await contador.WaitForNextTickAsync(tokenParar))
				{
					await using AsyncServiceScope scope = _factoria.CreateAsyncScope();
					{
						Tareas tareas = scope.ServiceProvider.GetRequiredService<Tareas>();

						await tareas.PortadaTarea();
						await tareas.TiendasTarea(tiempo);
					}
				}
			}
		}
	}
}
