#nullable disable

using BaseDatos.Tiendas;
using Herramientas;
using Microsoft.Data.SqlClient;

namespace Tareas
{
    public class CorreosDeals : BackgroundService
    {
        private readonly ILogger<CorreosDeals> _logger;
        private readonly IServiceScopeFactory _factoria;
        private readonly IDecompiladores _decompilador;

        public CorreosDeals(ILogger<CorreosDeals> logger, IServiceScopeFactory factory, IDecompiladores decompilador)
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

                        if (Admin.ComprobarTareaUso(conexion, "correos", tiempoSiguiente) == true)
                        {
                            Admin.ActualizarTareaUso(conexion, "correos", DateTime.Now);

                            List<CorreoConId> correosDeals = Correos.ComprobarNuevosCorreos(0);

                            if (correosDeals.Count > 0)
                            {
                                Admin.ActualizarDato(conexion, "correos", correosDeals.Count.ToString());
                            }
                            else
                            {
                                Admin.ActualizarDato(conexion, "correos", "0");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        BaseDatos.Errores.Insertar.Ejecutar("Tarea - Correos Deals", ex, conexion);
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
