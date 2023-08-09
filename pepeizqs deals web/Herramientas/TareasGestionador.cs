#nullable disable

namespace Herramientas
{
	public class TareasGestionador : IHostedService, IDisposable
	{
		private Timer _timerNotification;
		public IConfiguration _iconfiguration;
		private readonly IServiceScopeFactory _serviceScopeFactory;
		private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _env;

		public TareasGestionador(IServiceScopeFactory serviceScopeFactory, Microsoft.AspNetCore.Hosting.IHostingEnvironment env, IConfiguration iconfiguration)
		{
			_serviceScopeFactory = serviceScopeFactory;
			_env = env;
			_iconfiguration = iconfiguration;
		}

		public Task StartAsync(CancellationToken stoppingToken)
		{
			_timerNotification = new Timer(RunJob, null, TimeSpan.Zero, TimeSpan.FromMinutes(50)); 

			return Task.CompletedTask;
		}

		private void RunJob(object state)
		{
			using (var scrope = _serviceScopeFactory.CreateScope())
			{
				try
				{
					APIs.GamersGate.Tienda.BuscarOfertas();

				}
				catch (Exception ex)
				{

				}
			}
		}

		public Task StopAsync(CancellationToken stoppingToken)
		{
			_timerNotification?.Change(Timeout.Infinite, 0);

			return Task.CompletedTask;
		}

		public void Dispose()
		{
			_timerNotification?.Dispose();
		}
	}
}
