#nullable disable

using BaseDatos.Tiendas;
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

                            if (Admin.ComprobarTareaUso(conexion, "correos2", tiempoSiguiente) == true)
                            {
                                Admin.ActualizarTareaUso(conexion, "correos2", DateTime.Now);

                                List<CorreoConId> correosApps = Correos.ComprobarNuevosCorreos(1);

                                if (correosApps.Count > 0)
                                {
                                    Admin.ActualizarDato(conexion, "correos2", correosApps.Count.ToString());
                                }
                                else
                                {
                                    Admin.ActualizarDato(conexion, "correos2", "0");
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
