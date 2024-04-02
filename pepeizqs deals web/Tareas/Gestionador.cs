﻿#nullable disable

using Herramientas;
using Microsoft.Data.SqlClient;

namespace Tareas
{
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
			using PeriodicTimer timer = new(TimeSpan.FromSeconds(10));

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
		private readonly ILogger<GestionadorDivisas> _logger;
		private readonly IServiceScopeFactory _factoria;
		private readonly IDecompiladores _decompilador;

		public GestionadorDivisas(ILogger<GestionadorDivisas> logger, IServiceScopeFactory factory, IDecompiladores decompilador)
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
						await Divisas.Ejecutar(conexion);
					}
					catch (Exception ex)
					{
						BaseDatos.Errores.Insertar.Ejecutar("Tarea - Divisas", ex, conexion);
					}
				}
			}
		}

		public override async Task StopAsync(CancellationToken stoppingToken)
		{
			await base.StopAsync(stoppingToken);
		}
	}

	public class GestionadorSorteos : BackgroundService
	{
		private readonly ILogger<GestionadorSorteos> _logger;
		private readonly IServiceScopeFactory _factoria;
		private readonly IDecompiladores _decompilador;

		public GestionadorSorteos(ILogger<GestionadorSorteos> logger, IServiceScopeFactory factory, IDecompiladores decompilador)
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

	public class GestionadorAdminCorreos : BackgroundService
	{
		private readonly ILogger<GestionadorAdminCorreos> _logger;
		private readonly IServiceScopeFactory _factoria;
		private readonly IDecompiladores _decompilador;

		public GestionadorAdminCorreos(ILogger<GestionadorAdminCorreos> logger, IServiceScopeFactory factory, IDecompiladores decompilador)
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

                    await DatosAdmin.Correos(conexion);
                }
			}
		}

		public override async Task StopAsync(CancellationToken stoppingToken)
		{
			await base.StopAsync(stoppingToken);
		}
	}

    public class GestionadorAdminPendientes : BackgroundService
    {
        private readonly ILogger<GestionadorAdminPendientes> _logger;
        private readonly IServiceScopeFactory _factoria;
        private readonly IDecompiladores _decompilador;

        public GestionadorAdminPendientes(ILogger<GestionadorAdminPendientes> logger, IServiceScopeFactory factory, IDecompiladores decompilador)
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

                    await DatosAdmin.Pendientes(conexion);
                }
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            await base.StopAsync(stoppingToken);
        }
    }

    public class GestionadorAdminErrores : BackgroundService
    {
        private readonly ILogger<GestionadorAdminErrores> _logger;
        private readonly IServiceScopeFactory _factoria;
        private readonly IDecompiladores _decompilador;

        public GestionadorAdminErrores(ILogger<GestionadorAdminErrores> logger, IServiceScopeFactory factory, IDecompiladores decompilador)
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

                    await DatosAdmin.Errores(conexion);
                }
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            await base.StopAsync(stoppingToken);
        }
    }

    public class GestionadorAdminDLCs : BackgroundService
    {
        private readonly ILogger<GestionadorAdminDLCs> _logger;
        private readonly IServiceScopeFactory _factoria;
        private readonly IDecompiladores _decompilador;

        public GestionadorAdminDLCs(ILogger<GestionadorAdminDLCs> logger, IServiceScopeFactory factory, IDecompiladores decompilador)
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

                    await DatosAdmin.DLCs(conexion);
                }
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            await base.StopAsync(stoppingToken);
        }
    }

    public class GestionadorAdminSolicitudes : BackgroundService
    {
        private readonly ILogger<GestionadorAdminSolicitudes> _logger;
        private readonly IServiceScopeFactory _factoria;
        private readonly IDecompiladores _decompilador;

        public GestionadorAdminSolicitudes(ILogger<GestionadorAdminSolicitudes> logger, IServiceScopeFactory factory, IDecompiladores decompilador)
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

                    await DatosAdmin.Solicitudes(conexion);
                }
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            await base.StopAsync(stoppingToken);
        }
    }
}
