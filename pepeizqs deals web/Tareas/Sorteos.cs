#nullable disable

using Herramientas;
using Microsoft.Data.SqlClient;
using Sorteos2;

namespace Tareas
{
    public class Sorteos : BackgroundService
    {
        private readonly ILogger<Sorteos> _logger;
        private readonly IServiceScopeFactory _factoria;
        private readonly IDecompiladores _decompilador;

        public Sorteos(ILogger<Sorteos> logger, IServiceScopeFactory factory, IDecompiladores decompilador)
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
                            TimeSpan tiempoSiguiente = TimeSpan.FromMinutes(20);

                            if (BaseDatos.Admin.Buscar.TareaPosibleUsar("sorteos", tiempoSiguiente, conexion) == true)
                            {
								BaseDatos.Admin.Actualizar.TareaUso("sorteos", DateTime.Now, conexion);

                                List<Sorteo> listaSorteos = BaseDatos.Sorteos.Buscar.Todos(conexion);

                                if (listaSorteos != null)
                                {
                                    if (listaSorteos.Count > 0)
                                    {
                                        foreach (var sorteo in listaSorteos)
                                        {
                                            if (DateTime.Now > sorteo.FechaTermina && string.IsNullOrEmpty(sorteo.GanadorId) == true)
                                            {
                                                if (sorteo.Participantes != null)
                                                {
                                                    if (sorteo.Participantes.Count > 0)
                                                    {
                                                        Random rnd = new Random();
                                                        int ganador = rnd.Next(0, sorteo.Participantes.Count);
                                                        string usuarioId = sorteo.Participantes[ganador];

                                                        string correo = BaseDatos.Usuarios.Buscar.UnUsuarioCorreo(conexion, usuarioId);

                                                        if (string.IsNullOrEmpty(correo) == false)
                                                        {
                                                            Juegos.Juego juego = BaseDatos.Juegos.Buscar.UnJuego(sorteo.JuegoId.ToString());

                                                            BaseDatos.Usuarios.Clave nuevaClave = new BaseDatos.Usuarios.Clave
                                                            {
                                                                Nombre = juego.Nombre,
                                                                JuegoId = juego.Id.ToString(),
                                                                Codigo = sorteo.Clave
                                                            };

                                                            BaseDatos.Usuarios.Actualizar.Claves(conexion, usuarioId, nuevaClave);
                                                            BaseDatos.Sorteos.Actualizar.Ganador(sorteo, conexion, usuarioId);
                                                            Correos.EnviarGanadorSorteo(juego, sorteo, correo);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            BaseDatos.Errores.Insertar.Mensaje("Tarea - Sorteos", ex, conexion);
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
