﻿#nullable disable

using Herramientas;
using Microsoft.Data.SqlClient;

namespace Tareas.Tiendas
{
	public class EpicGames : BackgroundService
	{
		private string id = APIs.EpicGames.Tienda.Generar().Id;

		private readonly ILogger<EpicGames> _logger;
		private readonly IServiceScopeFactory _factoria;
		private readonly IDecompiladores _decompilador;

		public EpicGames(ILogger<EpicGames> logger, IServiceScopeFactory factory, IDecompiladores decompilador)
		{
			_logger = logger;
			_factoria = factory;
			_decompilador = decompilador;
		}

		protected override async Task ExecuteAsync(CancellationToken tokenParar)
		{
			Random azar = new Random();
			using PeriodicTimer timer = new PeriodicTimer(TimeSpan.FromSeconds(azar.Next(20, 60)));

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
						TimeSpan siguienteComprobacion = TimeSpan.FromHours(6);

						if (DateTime.Now.Hour == 19)
						{
							siguienteComprobacion = TimeSpan.FromHours(7);
						}

						if (DateTime.Now.Hour == 17)
                        {
                            siguienteComprobacion = TimeSpan.FromMinutes(30);
                        }

                        bool sePuedeUsar = BaseDatos.Admin.Buscar.TiendasPosibleUsar(siguienteComprobacion, id, conexion);

						if (sePuedeUsar == true && BaseDatos.Admin.Buscar.TiendasEnUso(TimeSpan.FromSeconds(60), conexion) == null)
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