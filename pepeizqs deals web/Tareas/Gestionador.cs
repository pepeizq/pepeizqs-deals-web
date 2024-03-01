#nullable disable

using Herramientas;
using Microsoft.Data.SqlClient;

namespace Tareas
{
    public class Gestionador : BackgroundService
    {
        private readonly ILogger<Gestionador> _logger;
        private readonly IServiceScopeFactory _factoria;
        private readonly IDecompiladores _decompilador;

        public Gestionador(ILogger<Gestionador> logger, IServiceScopeFactory factory, IDecompiladores decompilador)
        {
            _logger = logger;
            _factoria = factory;
            _decompilador = decompilador;
        }

        protected override async Task ExecuteAsync(CancellationToken tokenParar)
        {
            using PeriodicTimer timer = new(TimeSpan.FromSeconds(60));

            while (await timer.WaitForNextTickAsync(tokenParar))
            {
                using (AsyncServiceScope scope = _factoria.CreateAsyncScope())
                {
                    SqlConnection conexion = Herramientas.BaseDatos.Conectar();

                    try
                    {
                        await Sorteos.Ejecutar(conexion);
                    }
                    catch (Exception ex)
                    {
                        BaseDatos.Errores.Insertar.Ejecutar("Tarea - Sorteos", ex, conexion);
                    }

                    try
                    {
                        Minimos minimos = scope.ServiceProvider.GetService<Minimos>();
                        await minimos.Ejecutar(conexion);
                    }
                    catch (Exception ex)
                    {
                        BaseDatos.Errores.Insertar.Ejecutar("Tarea - Minimos", ex, conexion);
                    }

                    try
                    {
                        Noticias noticias = scope.ServiceProvider.GetService<Noticias>();
                        await noticias.Ejecutar(conexion);
                    }
                    catch (Exception ex)
                    {
                        BaseDatos.Errores.Insertar.Ejecutar("Tarea - Noticias", ex, conexion);
                    }

                    try
                    {
                        Tiendas tiendas = scope.ServiceProvider.GetService<Tiendas>();
                        await tiendas.Ejecutar(conexion, _decompilador);
                    }
                    catch { }

                    try
                    {
                        Divisas divisas = scope.ServiceProvider.GetService<Divisas>();
                        await divisas.Ejecutar(conexion);
                    }
					catch (Exception ex)
					{
						BaseDatos.Errores.Insertar.Ejecutar("Tarea - Divisas", ex, conexion);
					}

     //               BaseDatos.Errores.Insertar.Mensaje("sorteotest", "test3", conexion);

     //               try
     //               {
     //                   await Sorteos.Ejecutar(conexion);
     //               }
					//catch (Exception ex)
					//{
					//	BaseDatos.Errores.Insertar.Ejecutar("Tarea - Sorteos", ex, conexion);
					//}
				}
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            await base.StopAsync(stoppingToken);
        }
    }
}
