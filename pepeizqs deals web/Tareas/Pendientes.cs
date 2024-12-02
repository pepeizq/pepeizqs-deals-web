#nullable disable

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

                            if (BaseDatos.Admin.Buscar.TareaPosibleUsar("pendientes", tiempoSiguiente, conexion) == true)
                            {
								BaseDatos.Admin.Actualizar.TareaUso("pendientes", DateTime.Now, conexion);

                                int cantidadTiendas = BaseDatos.Pendientes.Buscar.TiendasCantidad(conexion);

                                int cantidadSuscripcion = BaseDatos.Pendientes.Buscar.SuscripcionCantidad(conexion);

                                int cantidadStreaming = BaseDatos.Pendientes.Buscar.StreamingCantidad(conexion);

                                if (cantidadTiendas + cantidadSuscripcion + cantidadStreaming > 0)
                                {
                                    BaseDatos.Admin.Actualizar.Dato("pendientes", (cantidadTiendas + cantidadSuscripcion + cantidadStreaming).ToString(), conexion);
                                }
                                else
                                {
									BaseDatos.Admin.Actualizar.Dato("pendientes", "0", conexion);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            BaseDatos.Errores.Insertar.Mensaje("Tarea - Pendientes", ex, conexion);
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
