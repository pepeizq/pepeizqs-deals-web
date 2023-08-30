//https://github.com/damienbod/AspNetCoreHangfire

#nullable disable

namespace Herramientas
{
	public interface ITareasGestionador
	{
		public void HacerTarea();
	}

	public class TareasGestionador : ITareasGestionador
	{
		public void HacerTarea()
		{
			int orden = global::BaseDatos.Tiendas.Admin.TareaLeerOrden();

			Tiendas2.TiendasCargar.TareasGestionador(orden, TimeSpan.FromMinutes(50));

			orden += 1;

			if (orden + 1 > Tiendas2.TiendasCargar.GenerarListado().Count)
			{
				orden = 0;
			}

			global::BaseDatos.Tiendas.Admin.TareaCambiarOrden(orden);
		}
	}


























	public class TimedHostedService : IHostedService, IDisposable
	{
		private int executionCount = 0;
		private readonly ILogger<TimedHostedService> _logger;
		private Timer _timer = null;

		public TimedHostedService(ILogger<TimedHostedService> logger)
		{
			_logger = logger;
		}

		public Task StartAsync(CancellationToken stoppingToken)
		{
			_logger.LogInformation("Timed Hosted Service running.");

			_timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));

			return Task.CompletedTask;
		}

		private void DoWork(object state)
		{
			var count = Interlocked.Increment(ref executionCount);

			_logger.LogInformation("Timed Hosted Service is working. Count: {Count}", count);
		}

		public Task StopAsync(CancellationToken stoppingToken)
		{
			_logger.LogInformation("Timed Hosted Service is stopping.");

			_timer?.Change(Timeout.Infinite, 0);

			return Task.CompletedTask;
		}

		public void Dispose()
		{
			_timer?.Dispose();
		}
	}

	internal interface IServicioHacerTarea
	{
		Task DoWork(CancellationToken stoppingToken);
	}

	internal class ServicioHacerTarea : IServicioHacerTarea
	{
		private TimeSpan tiempoEntreTareas = TimeSpan.FromMinutes(90);
		private readonly ILogger _logger;

		public ServicioHacerTarea(ILogger<ServicioHacerTarea> logger)
		{
			_logger = logger;
		}

		private void HacerTarea()
		{		
			int orden = global::BaseDatos.Tiendas.Admin.TareaLeerOrden();

			Tiendas2.TiendasCargar.TareasGestionador(orden, tiempoEntreTareas);
			
			orden += 1;

			if (orden + 1 > Tiendas2.TiendasCargar.GenerarListado().Count)
			{
				orden = 0;
			}

			global::BaseDatos.Tiendas.Admin.TareaCambiarOrden(orden);

			//_logger.LogInformation("Timed Hosted Service is working. Count: {Count}", count);
		}

		public async Task DoWork(CancellationToken pararToken)
		{
			while (!pararToken.IsCancellationRequested)
			{
				HacerTarea();

				await Task.Delay(tiempoEntreTareas, pararToken);
			}
		}
	}

	public class ConsumeScopedServiceHostedService : BackgroundService
	{
		private readonly ILogger<ConsumeScopedServiceHostedService> _logger;

		public ConsumeScopedServiceHostedService(IServiceProvider services,
			ILogger<ConsumeScopedServiceHostedService> logger)
		{
			Services = services;
			_logger = logger;
		}

		public IServiceProvider Services { get; }

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			_logger.LogInformation("Consume Scoped Service Hosted Service running.");

			await DoWork(stoppingToken);
		}

		private async Task DoWork(CancellationToken pararToken)
		{
			_logger.LogInformation("Consume Scoped Service Hosted Service is working.");

			using (var scope = Services.CreateScope())
			{
				var scopedProcessingService = scope.ServiceProvider.GetRequiredService<IServicioHacerTarea>();

				await scopedProcessingService.DoWork(pararToken);
			}
		}

		public override async Task StopAsync(CancellationToken pararToken)
		{
			_logger.LogInformation("Consume Scoped Service Hosted Service is stopping.");

			await base.StopAsync(pararToken);
		}
	}















	//public class TareasGestionador : BackgroundService
	//{
	//	private TimeSpan tiempo = TimeSpan.FromMinutes(50);
 //       private readonly ILogger<TareasGestionador> _logger;

	//	public TareasGestionador(ILogger<TareasGestionador> logger)
	//	{
	//		_logger = logger;
	//	}

	//	protected override async Task ExecuteAsync(CancellationToken tokenParar)
	//	{
	//		_logger.LogInformation("Timed Hosted Service running.");

	//		//HacerTarea();

	//		//using PeriodicTimer cronometro = new (tiempo);

	//		try
	//		{
	//			while (tokenParar.IsCancellationRequested == false)
	//			{
	//				HacerTarea();

	//				await Task.Delay(tiempo, tokenParar);
	//			}
	//		}
	//		catch (OperationCanceledException)
	//		{
	//			_logger.LogInformation("Timed Hosted Service is stopping.");
	//		}
	//	}

	//	private void HacerTarea()
	//	{
	//		int orden = global::BaseDatos.Tiendas.Admin.TareaLeerOrden();

	//		try
	//		{
	//			if (orden == 0)
	//			{
	//				DateTime ultimaComprobacion = global::BaseDatos.Tiendas.Admin.TareaLeerTienda(APIs.Steam.Tienda.Generar().Id);

	//				if ((DateTime.Now - ultimaComprobacion) > tiempo)
	//				{
 //                       APIs.Steam.Tienda.BuscarOfertas(true);
 //                   }				
	//			}
	//			else if (orden == 1)
	//			{
 //                   DateTime ultimaComprobacion = global::BaseDatos.Tiendas.Admin.TareaLeerTienda(APIs.GamersGate.Tienda.Generar().Id);

 //                   if ((DateTime.Now - ultimaComprobacion) > tiempo)
 //                   {
 //                       APIs.GamersGate.Tienda.BuscarOfertas();
 //                   }                   
	//			}
	//			else if (orden == 2)
	//			{
 //                   DateTime ultimaComprobacion = global::BaseDatos.Tiendas.Admin.TareaLeerTienda(APIs.Gamesplanet.Tienda.GenerarUk().Id);

 //                   if ((DateTime.Now - ultimaComprobacion) > tiempo)
 //                   {
 //                       APIs.Gamesplanet.Tienda.BuscarOfertasUk();
 //                   }
 //               }
	//			else if (orden == 3)
	//			{
 //                   DateTime ultimaComprobacion = global::BaseDatos.Tiendas.Admin.TareaLeerTienda(APIs.Gamesplanet.Tienda.GenerarFr().Id);

 //                   if ((DateTime.Now - ultimaComprobacion) > tiempo)
 //                   {
 //                       APIs.Gamesplanet.Tienda.BuscarOfertasFr();
 //                   }
 //               }
	//			else if (orden == 4)
	//			{
 //                   DateTime ultimaComprobacion = global::BaseDatos.Tiendas.Admin.TareaLeerTienda(APIs.Gamesplanet.Tienda.GenerarDe().Id);

 //                   if ((DateTime.Now - ultimaComprobacion) > tiempo)
 //                   {
 //                       APIs.Gamesplanet.Tienda.BuscarOfertasDe();
 //                   }
 //               }
	//			else if (orden == 5)
	//			{
 //                   DateTime ultimaComprobacion = global::BaseDatos.Tiendas.Admin.TareaLeerTienda(APIs.Gamesplanet.Tienda.GenerarUs().Id);

 //                   if ((DateTime.Now - ultimaComprobacion) > tiempo)
 //                   {
 //                       APIs.Gamesplanet.Tienda.BuscarOfertasUs();
 //                   }
 //               }
	//			else if (orden == 6)
	//			{
 //                   DateTime ultimaComprobacion = global::BaseDatos.Tiendas.Admin.TareaLeerTienda(APIs.Fanatical.Tienda.Generar().Id);

 //                   if ((DateTime.Now - ultimaComprobacion) > tiempo)
 //                   {
 //                       APIs.Fanatical.Tienda.BuscarOfertas();
 //                   }
 //               }
	//			else if (orden == 7)
	//			{
 //                   DateTime ultimaComprobacion = global::BaseDatos.Tiendas.Admin.TareaLeerTienda(APIs.GreenManGaming.Tienda.Generar().Id);

 //                   if ((DateTime.Now - ultimaComprobacion) > tiempo)
 //                   {
 //                       APIs.GreenManGaming.Tienda.BuscarOfertas();
 //                   }
	//			}
	//			else if (orden == 8)
	//			{
	//				Divisas.Ejecutar();
	//			}
	//		}
	//		catch
	//		{

	//		}

	//		orden += 1;

	//		if (orden == 9)
	//		{
	//			orden = 0;
	//		}

	//		global::BaseDatos.Tiendas.Admin.TareaCambiarOrden(orden);

	//		//_logger.LogInformation("Timed Hosted Service is working. Count: {Count}", count);
	//	}
	//}
}
