#nullable disable

using BaseDatos.Tiendas;
using Herramientas;
using Microsoft.Data.SqlClient;

namespace Tareas
{
    public class Tiendas : BackgroundService
    {
        private readonly ILogger<Tiendas> _logger;
        private readonly IServiceScopeFactory _factoria;
        private readonly IDecompiladores _decompilador;

        public Tiendas(ILogger<Tiendas> logger, IServiceScopeFactory factory, IDecompiladores decompilador)
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
                using PeriodicTimer timer = new PeriodicTimer(TimeSpan.FromSeconds(1));

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
                        TimeSpan siguienteComprobacion = TimeSpan.FromMinutes(120);
                        List<string> ids = new List<string>();

                        foreach (var tienda in Tiendas2.TiendasCargar.GenerarListado())
                        {
                            if (tienda.AdminInteractuar == true)
                            {
                                ids.Add(tienda.Id);
                            }
                        }

                        if (Admin.ComprobarTiendasUso(conexion, TimeSpan.FromSeconds(60)) == false)
                        {
                            AdminTarea tiendaComprobar = Admin.TiendaSiguiente(conexion);

                            if (DateTime.Now - tiendaComprobar.Fecha > siguienteComprobacion)
                            {
                                try
                                {
                                    await Tiendas2.TiendasCargar.TareasGestionador(conexion, tiendaComprobar.Id, _decompilador);
                                }
                                catch (Exception ex)
                                {
                                    BaseDatos.Errores.Insertar.Ejecutar(tiendaComprobar.Id, ex, conexion);
                                }
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
