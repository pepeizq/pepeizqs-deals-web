#nullable disable

using Herramientas;
using Microsoft.Data.SqlClient;
using Noticias;

namespace Tareas
{
    public class Noticias : BackgroundService
    {
        private readonly ILogger<Noticias> _logger;
        private readonly IServiceScopeFactory _factoria;
        private readonly IDecompiladores _decompilador;

        public Noticias(ILogger<Noticias> logger, IServiceScopeFactory factory, IDecompiladores decompilador)
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
                            List<Noticia> noticiasMostrar = new List<Noticia>();
                            List<Noticia> noticiaEvento = new List<Noticia>();

                            List<Noticia> noticias = BaseDatos.Noticias.Buscar.Todas().OrderBy(x => x.FechaEmpieza).Reverse().ToList();

                            if (noticias.Count > 0)
                            {
                                int i = 0;
                                foreach (var noticia in noticias)
                                {
                                    if (DateTime.Now >= noticia.FechaEmpieza && DateTime.Now <= noticia.FechaTermina)
                                    {
                                        if (noticia.Tipo == NoticiaTipo.Eventos && noticiaEvento.Count == 0)
                                        {
                                            DateTime fechaEncabezado = noticia.FechaEmpieza;
                                            fechaEncabezado = fechaEncabezado.AddDays(3);

                                            if (DateTime.Now < fechaEncabezado)
                                            {
                                                noticiaEvento.Add(noticia);
                                            }
                                        }

                                        if (i < 6)
                                        {
                                            noticiasMostrar.Add(noticia);
                                            i += 1;
                                        }
                                    }
                                }
                            }

                            if (noticiasMostrar.Count > 0)
                            {
                                BaseDatos.Portada.Limpiar.Ejecutar("portadaNoticias", conexion);

                                foreach (var noticia in noticiasMostrar)
                                {
                                    BaseDatos.Portada.Insertar.Noticia(noticia, "portadaNoticias", conexion);
                                }
                            }

                            if (noticiaEvento.Count > 0)
                            {
                                BaseDatos.Portada.Limpiar.Ejecutar("portadaNoticiasEvento", conexion);

                                foreach (var noticia in noticiaEvento)
                                {
                                    BaseDatos.Portada.Insertar.Noticia(noticia, "portadaNoticiasEvento", conexion);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            BaseDatos.Errores.Insertar.Ejecutar("Tarea - Noticias", ex, conexion);
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
