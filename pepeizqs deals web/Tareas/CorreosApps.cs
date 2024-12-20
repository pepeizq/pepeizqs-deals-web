﻿#nullable disable

using Herramientas;
using Microsoft.Data.SqlClient;

namespace Tareas
{
    public class CorreosApps : BackgroundService
    {
        private readonly ILogger<CorreosApps> _logger;
        private readonly IServiceScopeFactory _factoria;
        private readonly IDecompiladores _decompilador;

        public CorreosApps(ILogger<CorreosApps> logger, IServiceScopeFactory factory, IDecompiladores decompilador)
        {
            _logger = logger;
            _factoria = factory;
            _decompilador = decompilador;
        }

        protected override async Task ExecuteAsync(CancellationToken tokenParar)
        {
            using PeriodicTimer timer = new PeriodicTimer(TimeSpan.FromSeconds(60));

            while (await timer.WaitForNextTickAsync(tokenParar))
            {
                WebApplicationBuilder builder = WebApplication.CreateBuilder();
                string piscinaApp = builder.Configuration.GetValue<string>("PoolWeb:Contenido");
                string piscinaUsada = Environment.GetEnvironmentVariable("APP_POOL_ID", EnvironmentVariableTarget.Process);

                if (piscinaApp == piscinaUsada)
                {
                    SqlConnection conexion = new SqlConnection();

                    try
                    {
                        conexion = Herramientas.BaseDatos.Conectar();
                    }
                    catch { }

                    if (conexion.State == System.Data.ConnectionState.Open)
                    {
                        try
                        {
                            TimeSpan tiempoSiguiente = TimeSpan.FromMinutes(30);

                            if (BaseDatos.Admin.Buscar.TareaPosibleUsar("correos2", tiempoSiguiente, conexion) == true)
                            {
								BaseDatos.Admin.Actualizar.TareaUso("correos2", DateTime.Now, conexion);

                                List<CorreoConId> correosApps = Correos.ComprobarNuevosCorreos(1);

                                if (correosApps.Count > 0)
                                {
									BaseDatos.Admin.Actualizar.Dato("correos2", correosApps.Count.ToString(), conexion);
                                }
                                else
                                {
									BaseDatos.Admin.Actualizar.Dato("correos2", "0", conexion);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            BaseDatos.Errores.Insertar.Mensaje("Tarea - Correos Apps", ex, conexion);
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
