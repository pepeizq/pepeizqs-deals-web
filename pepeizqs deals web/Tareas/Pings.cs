#nullable disable

using AngleSharp.Dom;
using Herramientas;
using System.Net.Http;

namespace Tareas
{
    public class Pings : BackgroundService
    {
        private readonly ILogger<Pings> _logger;
        private readonly IServiceScopeFactory _factoria;
        private readonly IDecompiladores _decompilador;

        public Pings(ILogger<Pings> logger, IServiceScopeFactory factory, IDecompiladores decompilador)
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
                    try
                    {
						HttpClientHandler clientHandler = new HttpClientHandler();
						clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

						HttpClient httpClient = new HttpClient(clientHandler);
						HttpRequestMessage mensaje = new HttpRequestMessage();
						mensaje.RequestUri = new Uri("http://tiendas.pepeizqdeals.com/");
						mensaje.Headers.Accept.ParseAdd("text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,*/*;q=0.8");
						mensaje.Headers.AcceptEncoding.ParseAdd("gzip, deflate, br");
						mensaje.Headers.AcceptLanguage.ParseAdd("es,en-US;q=0.7,en;q=0.3");
						mensaje.Headers.Connection.ParseAdd("keep-alive");
						mensaje.Headers.UserAgent.ParseAdd("Mozilla/5.0 (Linux; Android 10; Generic Android-x86_64 Build/QD1A.190821.014.C2; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/79.0.3945.36 Safari/537.36");
						HttpResponseMessage respuesta2 = await httpClient.SendAsync(mensaje);
						respuesta2.EnsureSuccessStatusCode();
					}
                    catch (Exception ex)
                    {
						BaseDatos.Errores.Insertar.Mensaje("Ping Tiendas", ex);
					}

					//---------------------------------------------------------------------------------------

					//               string bingApiClave = builder.Configuration.GetValue<string>("BingAPI:Contenido");

					//string bingEnlace = "https://ssl.bing.com/webmaster/api.svc/json/SubmitUrl?apiKey=" + bingApiClave;

					//               Juegos.Juego aleatorio = BaseDatos.Juegos.Buscar.Aleatorio();

					//               if (aleatorio != null)
					//               {
					//                   Juegos.Juego aleatorio2 = BaseDatos.Juegos.Buscar.UnJuego(aleatorio.Id);

					//	BingApi nuevoAleatorio = new BingApi("https://pepeizqdeals.com/game/" + aleatorio2.Id.ToString() + "/" + EnlaceAdaptador.Nombre(aleatorio2.Nombre) + "/");

					//	HttpRequestMessage peticion = new HttpRequestMessage(HttpMethod.Post, bingEnlace)
					//	{
					//		Content = JsonContent.Create(nuevoAleatorio)
					//	};

					//	HttpResponseMessage respuesta = await httpClient.SendAsync(peticion);

					//	respuesta.EnsureSuccessStatusCode();
					//}
				}
			}
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            await base.StopAsync(stoppingToken);
        }
    }

	public class BingApi
	{
		public string siteUrl { get; set; }
		public string url { get; set; }

		public BingApi(string newurl)
		{
			siteUrl = "https://pepeizqdeals.com";
			url = newurl;
		}
	}
}
