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
					HttpClient httpClient = new HttpClient();

					using var request = new HttpRequestMessage(HttpMethod.Get, new Uri("http://tiendas.pepeizqdeals.com/"));
					request.Headers.TryAddWithoutValidation("Accept", "text/html,application/xhtml+xml,application/xml");
					request.Headers.TryAddWithoutValidation("Accept-Encoding", "gzip, deflate");
					request.Headers.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.2; WOW64; rv:19.0) Gecko/20100101 Firefox/19.0");
					request.Headers.TryAddWithoutValidation("Accept-Charset", "ISO-8859-1");

					using var response = await httpClient.SendAsync(request, default).ConfigureAwait(false);
					response.EnsureSuccessStatusCode();

					//HttpRequestMessage peticion2 = new HttpRequestMessage(HttpMethod.Post, "http://tiendas.pepeizqdeals.com/");
     //               HttpResponseMessage respuesta2 = await httpClient.SendAsync(peticion2);

     //               respuesta2.EnsureSuccessStatusCode();

                    //---------------------------------------------------------------------------------------

                    string bingApiClave = builder.Configuration.GetValue<string>("BingAPI:Contenido");

					string bingEnlace = "https://ssl.bing.com/webmaster/api.svc/json/SubmitUrl?apiKey=" + bingApiClave;

                    Juegos.Juego aleatorio = BaseDatos.Juegos.Buscar.Aleatorio();

                    if (aleatorio != null)
                    {
                        Juegos.Juego aleatorio2 = BaseDatos.Juegos.Buscar.UnJuego(aleatorio.Id);

						BingApi nuevoAleatorio = new BingApi("https://pepeizqdeals.com/game/" + aleatorio2.Id.ToString() + "/" + EnlaceAdaptador.Nombre(aleatorio2.Nombre) + "/");

						HttpRequestMessage peticion = new HttpRequestMessage(HttpMethod.Post, bingEnlace)
						{
							Content = JsonContent.Create(nuevoAleatorio)
						};

						HttpResponseMessage respuesta = await httpClient.SendAsync(peticion);

						respuesta.EnsureSuccessStatusCode();
					}
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
