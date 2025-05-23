﻿#nullable disable

using Herramientas;
using Microsoft.Data.SqlClient;

namespace Tareas.Suscripciones
{
    public class UbisoftPlusClassics : BackgroundService
    {
        private string id = "ubisoftplusclassics";

        private readonly ILogger<UbisoftPlusClassics> _logger;
        private readonly IServiceScopeFactory _factoria;
        private readonly IDecompiladores _decompilador;

        public UbisoftPlusClassics(ILogger<UbisoftPlusClassics> logger, IServiceScopeFactory factory, IDecompiladores decompilador)
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

                        bool sePuedeUsar = BaseDatos.Admin.Buscar.TiendasPosibleUsar(siguienteComprobacion, id, conexion);

                        if (sePuedeUsar == true && BaseDatos.Admin.Buscar.TiendasEnUso(TimeSpan.FromSeconds(60), conexion) == null)
                        {
                            try
                            {
                                await APIs.Ubisoft.Suscripcion.Buscar(conexion);

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