﻿#nullable disable

using BaseDatos.Tiendas;
using Herramientas;
using Microsoft.Data.SqlClient;

namespace Tareas.Suscripciones
{
	public class EAPlay : BackgroundService
	{
		private string id = "eaplay";

		private readonly ILogger<EAPlay> _logger;
		private readonly IServiceScopeFactory _factoria;
		private readonly IDecompiladores _decompilador;

		public EAPlay(ILogger<EAPlay> logger, IServiceScopeFactory factory, IDecompiladores decompilador)
		{
			_logger = logger;
			_factoria = factory;
			_decompilador = decompilador;
		}

		protected override async Task ExecuteAsync(CancellationToken tokenParar)
		{
			using PeriodicTimer timer = new PeriodicTimer(TimeSpan.FromSeconds(20));

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
						TimeSpan siguienteComprobacion = TimeSpan.FromHours(4);

						bool sePuedeUsar = Admin.ComprobarTiendaUso(conexion, siguienteComprobacion, id);

						if (sePuedeUsar == true && Admin.ComprobarTiendasUso(conexion, TimeSpan.FromSeconds(60)) == null)
						{
							try
							{
								await APIs.EA.Suscripcion.Buscar();

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
	}
}
