﻿#nullable disable

using BaseDatos.Tiendas;
using Herramientas;
using Microsoft.Data.SqlClient;

namespace Tareas.Tiendas
{
	public class Steam : BackgroundService
	{
		private string id = APIs.Steam.Tienda.Generar().Id;

		private readonly ILogger<Steam> _logger;
		private readonly IServiceScopeFactory _factoria;
		private readonly IDecompiladores _decompilador;

		public Steam(ILogger<Steam> logger, IServiceScopeFactory factory, IDecompiladores decompilador)
		{
			_logger = logger;
			_factoria = factory;
			_decompilador = decompilador;
		}

		protected override async Task ExecuteAsync(CancellationToken tokenParar)
		{
			Random azar = new Random();
			using PeriodicTimer timer = new PeriodicTimer(TimeSpan.FromSeconds(azar.Next(5, 30)));

			while (await timer.WaitForNextTickAsync(tokenParar))
			{
				WebApplicationBuilder builder = WebApplication.CreateBuilder();
				string piscinaTiendas = builder.Configuration.GetValue<string>("PoolTiendas:Contenido");
				string piscinaUsada = Environment.GetEnvironmentVariable("APP_POOL_ID", EnvironmentVariableTarget.Process);

				if (piscinaTiendas == piscinaUsada)
				{
					SqlConnection conexion = new SqlConnection();

					try
					{
						conexion = Herramientas.BaseDatos.Conectar();
					}
					catch { }

					if (conexion.State == System.Data.ConnectionState.Open)
					{
						TimeSpan siguienteComprobacion = TimeSpan.FromHours(3);

						if (DateTime.Now.Hour == 17 || DateTime.Now.Hour == 18)
						{
							siguienteComprobacion = TimeSpan.FromHours(6);
						}

						if (DateTime.Now.Hour == 19 || DateTime.Now.Hour == 20)
						{
							siguienteComprobacion = TimeSpan.FromMinutes(30);
						}

						bool sePuedeUsar = Admin.ComprobarTiendaUso(conexion, siguienteComprobacion, id);

						if (sePuedeUsar == true && Admin.ComprobarTiendasUso(conexion, TimeSpan.FromSeconds(60)) == null)
						{
							try
							{
								await Tiendas2.TiendasCargar.TareasGestionador(conexion, id);

								Environment.Exit(1);
							}
							catch (Exception ex)
							{
								BaseDatos.Errores.Insertar.Mensaje(id, ex, conexion);
							}
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