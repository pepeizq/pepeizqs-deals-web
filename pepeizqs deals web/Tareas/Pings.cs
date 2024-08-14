﻿#nullable disable

using Herramientas;
using Microsoft.Data.SqlClient;
using System.Net.Http;
using System.Text;

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
                    string html = await Decompiladores.Estandar("https://minimos.pepeizqdeals.com/");
					string html2 = await Decompiladores.Estandar("https://tiendas.pepeizqdeals.com/");

					//---------------------------------------------------------------------------------------

					string bingApiClave = builder.Configuration.GetValue<string>("BingAPI:Contenido");

					HttpClient httpClient = new HttpClient();
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
