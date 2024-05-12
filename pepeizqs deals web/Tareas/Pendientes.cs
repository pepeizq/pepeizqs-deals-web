#nullable disable

using BaseDatos.Tiendas;
using Herramientas;
using Microsoft.Data.SqlClient;

namespace Tareas
{
    public class Pendientes : BackgroundService
    {
        private readonly ILogger<Pendientes> _logger;
        private readonly IServiceScopeFactory _factoria;
        private readonly IDecompiladores _decompilador;

        public Pendientes(ILogger<Pendientes> logger, IServiceScopeFactory factory, IDecompiladores decompilador)
        {
            _logger = logger;
            _factoria = factory;
            _decompilador = decompilador;
        }

        protected override async Task ExecuteAsync(CancellationToken tokenParar)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder();
            string piscinaApp = builder.Configuration.GetValue<string>("PoolWeb:Contenido");
            string piscinaUsada = Environment.GetEnvironmentVariable("APP_POOL_ID", EnvironmentVariableTarget.Process);

            if (piscinaApp != piscinaUsada)
            {
                using PeriodicTimer timer = new PeriodicTimer(TimeSpan.FromSeconds(60));

                while (await timer.WaitForNextTickAsync(tokenParar))
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

                            if (Admin.ComprobarTareaUso(conexion, "pendientes", tiempoSiguiente) == true)
                            {
                                Admin.ActualizarTareaUso(conexion, "pendientes", DateTime.Now);

                                List<BaseDatos.Pendientes.Pendiente> pendientes = BaseDatos.Pendientes.Buscar.Todos(conexion);

                                if (pendientes.Count > 0)
                                {
                                    Admin.ActualizarDato(conexion, "pendientes", pendientes.Count.ToString());
                                }
                                else
                                {
                                    Admin.ActualizarDato(conexion, "pendientes", "0");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            BaseDatos.Errores.Insertar.Ejecutar("Tarea - Pendientes", ex, conexion);
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
