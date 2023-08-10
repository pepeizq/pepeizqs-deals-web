#nullable disable

using Quartz;

namespace Herramientas
{
	public class TareasGestionador : BackgroundService
	{
		private readonly ILogger<TareasGestionador> _logger;

		public TareasGestionador(ILogger<TareasGestionador> logger)
		{
			_logger = logger;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			_logger.LogInformation("Timed Hosted Service running.");

			//HacerTarea();

			using PeriodicTimer cronometro = new(TimeSpan.FromMinutes(30));

			try
			{
				while (await cronometro.WaitForNextTickAsync(stoppingToken))
				{
					HacerTarea();
				}
			}
			catch (OperationCanceledException)
			{
				_logger.LogInformation("Timed Hosted Service is stopping.");
			}
		}

		private void HacerTarea()
		{
			int orden = global::BaseDatos.Tiendas.Admin.CronLeerOrden();

			try
			{
				if (orden == 0)
				{
					APIs.Steam.Tienda.BuscarOfertas(true);
				}
				else if (orden == 1)
				{
					APIs.GamersGate.Tienda.BuscarOfertas();
				}
				else if (orden == 2)
				{
					APIs.Gamesplanet.Tienda.BuscarOfertasUk();
				}
				else if (orden == 3)
				{
					APIs.Gamesplanet.Tienda.BuscarOfertasFr();
				}
				else if (orden == 4)
				{
					APIs.Gamesplanet.Tienda.BuscarOfertasDe();
				}
				else if (orden == 5)
				{
					APIs.Gamesplanet.Tienda.BuscarOfertasUs();
				}
				else if (orden == 6)
				{
					APIs.Fanatical.Tienda.BuscarOfertas();
				}
				else if (orden == 7)
				{
					APIs.GreenManGaming.Tienda.BuscarOfertas();
				}
				//else if (orden == 8)
				//            {
				//                APIs.Steam.Tienda.BuscarOfertas(false);
				//            }
				else if (orden == 8)
				{
					Divisas.Ejecutar();
				}
			}
			catch
			{

			}

			orden += 1;

			if (orden == 9)
			{
				orden = 0;
			}

			global::BaseDatos.Tiendas.Admin.CronAumentarOrden(orden);

			//_logger.LogInformation("Timed Hosted Service is working. Count: {Count}", count);
		}
	}





	//public class TareasGestionador : IHostedService, IDisposable
	//{
	//	private int executionCount = 0;
	//	private readonly ILogger<TareasGestionador> _logger;
	//	private Timer _timer = null;

	//	public TareasGestionador(ILogger<TareasGestionador> logger)
	//	{
	//		_logger = logger;
	//	}

	//	public Task StartAsync(CancellationToken stoppingToken)
	//	{
	//		_logger.LogInformation("Timed Hosted Service running.");

	//		_timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(5));

	//		return Task.CompletedTask;
	//	}

	//	private void DoWork(object state)
	//	{
	//		var count = Interlocked.Increment(ref executionCount);

	//		int orden = global::BaseDatos.Tiendas.Admin.CronLeerOrden();

	//		orden += 1;

	//		if (orden == 9)
	//		{
	//			orden = -1;
	//		}

	//		global::BaseDatos.Tiendas.Admin.CronAumentarOrden(orden);

	//		try
	//		{
	//			if (orden == 0)
	//			{
	//				APIs.Steam.Tienda.BuscarOfertas(true);
	//			}
	//			else if (orden == 1)
	//			{
	//				APIs.GamersGate.Tienda.BuscarOfertas();
	//			}
	//			else if (orden == 2)
	//			{
	//				APIs.Gamesplanet.Tienda.BuscarOfertasUk();
	//			}
	//			else if (orden == 3)
	//			{
	//				APIs.Gamesplanet.Tienda.BuscarOfertasFr();
	//			}
	//			else if (orden == 4)
	//			{
	//				APIs.Gamesplanet.Tienda.BuscarOfertasDe();
	//			}
	//			else if (orden == 5)
	//			{
	//				APIs.Gamesplanet.Tienda.BuscarOfertasUs();
	//			}
	//			else if (orden == 6)
	//			{
	//				APIs.Fanatical.Tienda.BuscarOfertas();
	//			}
	//			else if (orden == 7)
	//			{
	//				APIs.GreenManGaming.Tienda.BuscarOfertas();
	//			}
	//			//else if (orden == 8)
	//			//            {
	//			//                APIs.Steam.Tienda.BuscarOfertas(false);
	//			//            }
	//			else if (orden == 8)
	//			{
	//				Divisas.Ejecutar();
	//			}
	//		}
	//		catch
	//		{

	//		}

	//		_logger.LogInformation("Timed Hosted Service is working. Count: {Count}", count);
	//	}

	//	public Task StopAsync(CancellationToken stoppingToken)
	//	{
	//		_logger.LogInformation("Timed Hosted Service is stopping.");

	//		//_timer?.Change(Timeout.Infinite, 0);
	//		_timer.Change(5000, Timeout.Infinite);

	//		return Task.CompletedTask;
	//	}

	//	public void Dispose()
	//	{
	//		_timer?.Dispose();
	//	}
	//}
}
