#nullable disable

using Herramientas;
using Microsoft.Data.SqlClient;

namespace Tareas
{
    public class Gestionador : BackgroundService
    {
        private readonly ILogger<Gestionador> _logger;
        private readonly IServiceScopeFactory _factoria;
        private readonly IDecompiladores _decompilador;

        public Gestionador(ILogger<Gestionador> logger, IServiceScopeFactory factory, IDecompiladores decompilador)
        {
            _logger = logger;
            _factoria = factory;
            _decompilador = decompilador;
        }

        protected override async Task ExecuteAsync(CancellationToken tokenParar)
        {
            using PeriodicTimer timer = new(TimeSpan.FromSeconds(60));

            while (await timer.WaitForNextTickAsync(tokenParar))
            {
                using (AsyncServiceScope scope = _factoria.CreateAsyncScope())
                {
                    SqlConnection conexion = Herramientas.BaseDatos.Conectar();

					try
					{
						await Tiendas.Ejecutar(conexion, _decompilador);
					}
					catch { }

					try
					{
						await Noticias.Ejecutar(conexion);
					}
					catch (Exception ex)
					{
						BaseDatos.Errores.Insertar.Ejecutar("Tarea - Noticias", ex, conexion);
					}

					try
                    {
                        await Minimos.Ejecutar(conexion);
                    }
                    catch (Exception ex)
                    {
                        BaseDatos.Errores.Insertar.Ejecutar("Tarea - Minimos", ex, conexion);
                    }

                    try
                    {
                        await Divisas.Ejecutar(conexion);
                    }
					catch (Exception ex)
					{
						BaseDatos.Errores.Insertar.Ejecutar("Tarea - Divisas", ex, conexion);
					}

					try
					{
						await Sorteos.Ejecutar(conexion);
					}
					catch (Exception ex)
					{
						BaseDatos.Errores.Insertar.Ejecutar("Tarea - Sorteos", ex, conexion);
					}
				}
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            await base.StopAsync(stoppingToken);
        }
    }

	public class GestionadorNoticias : BackgroundService
	{
		private readonly ILogger<GestionadorNoticias> _logger;
		private readonly IServiceScopeFactory _factoria;
		private readonly IDecompiladores _decompilador;

		public GestionadorNoticias(ILogger<GestionadorNoticias> logger, IServiceScopeFactory factory, IDecompiladores decompilador)
		{
			_logger = logger;
			_factoria = factory;
			_decompilador = decompilador;
		}

		protected override async Task ExecuteAsync(CancellationToken tokenParar)
		{
			using PeriodicTimer timer = new(TimeSpan.FromSeconds(60));

			while (await timer.WaitForNextTickAsync(tokenParar))
			{
				using (AsyncServiceScope scope = _factoria.CreateAsyncScope())
				{
					SqlConnection conexion = Herramientas.BaseDatos.Conectar();

					try
					{
						await Noticias.Ejecutar(conexion);
					}
					catch (Exception ex)
					{
						BaseDatos.Errores.Insertar.Ejecutar("Tarea - Noticias", ex, conexion);
					}
				}
			}
		}

		public override async Task StopAsync(CancellationToken stoppingToken)
		{
			await base.StopAsync(stoppingToken);
		}
	}

	public class GestionadorTiendas : BackgroundService
	{
		private readonly ILogger<GestionadorTiendas> _logger;
		private readonly IServiceScopeFactory _factoria;
		private readonly IDecompiladores _decompilador;

		public GestionadorTiendas(ILogger<GestionadorTiendas> logger, IServiceScopeFactory factory, IDecompiladores decompilador)
		{
			_logger = logger;
			_factoria = factory;
			_decompilador = decompilador;
		}

		protected override async Task ExecuteAsync(CancellationToken tokenParar)
		{
			using PeriodicTimer timer = new(TimeSpan.FromSeconds(60));

			while (await timer.WaitForNextTickAsync(tokenParar))
			{
				using (AsyncServiceScope scope = _factoria.CreateAsyncScope())
				{
					SqlConnection conexion = Herramientas.BaseDatos.Conectar();

					try
					{
						await Tiendas.Ejecutar(conexion, _decompilador);
					}
					catch { }
				}
			}
		}

		public override async Task StopAsync(CancellationToken stoppingToken)
		{
			await base.StopAsync(stoppingToken);
		}
	}

	public class GestionadorMinimos : BackgroundService
	{
		private readonly ILogger<GestionadorMinimos> _logger;
		private readonly IServiceScopeFactory _factoria;
		private readonly IDecompiladores _decompilador;

		public GestionadorMinimos(ILogger<GestionadorMinimos> logger, IServiceScopeFactory factory, IDecompiladores decompilador)
		{
			_logger = logger;
			_factoria = factory;
			_decompilador = decompilador;
		}

		protected override async Task ExecuteAsync(CancellationToken tokenParar)
		{
			using PeriodicTimer timer = new(TimeSpan.FromSeconds(60));

			while (await timer.WaitForNextTickAsync(tokenParar))
			{
				using (AsyncServiceScope scope = _factoria.CreateAsyncScope())
				{
					SqlConnection conexion = Herramientas.BaseDatos.Conectar();

					try
					{
						await Minimos.Ejecutar(conexion);
					}
					catch (Exception ex)
					{
						BaseDatos.Errores.Insertar.Ejecutar("Tarea - Minimos", ex, conexion);
					}
				}
			}
		}

		public override async Task StopAsync(CancellationToken stoppingToken)
		{
			await base.StopAsync(stoppingToken);
		}
	}

	public class GestionadorDivisas : BackgroundService
	{
		private readonly ILogger<GestionadorMinimos> _logger;
		private readonly IServiceScopeFactory _factoria;
		private readonly IDecompiladores _decompilador;

		public GestionadorMinimos(ILogger<GestionadorMinimos> logger, IServiceScopeFactory factory, IDecompiladores decompilador)
		{
			_logger = logger;
			_factoria = factory;
			_decompilador = decompilador;
		}

		protected override async Task ExecuteAsync(CancellationToken tokenParar)
		{
			using PeriodicTimer timer = new(TimeSpan.FromSeconds(60));

			while (await timer.WaitForNextTickAsync(tokenParar))
			{
				using (AsyncServiceScope scope = _factoria.CreateAsyncScope())
				{
					SqlConnection conexion = Herramientas.BaseDatos.Conectar();

					try
					{
						await Minimos.Ejecutar(conexion);
					}
					catch (Exception ex)
					{
						BaseDatos.Errores.Insertar.Ejecutar("Tarea - Minimos", ex, conexion);
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
