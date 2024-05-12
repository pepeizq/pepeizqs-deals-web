#nullable disable

using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.IO.Compression;
using System.Net;

namespace Herramientas
{

	public interface IDecompiladores
    {
        Task<string> Estandar(string enlace);
    }

	public class Decompiladores2 : IDecompiladores
	{
        private readonly IHttpClientFactory fabrica;

        public Decompiladores2(IHttpClientFactory _fabrica)
		{
			fabrica = _fabrica;
		}

		[HttpGet("Decompilador")]
		public async Task<string> Estandar(string enlace)
		{
            HttpClient cliente = fabrica.CreateClient("Decompilador");

            string contenido = string.Empty;

            cliente.DefaultRequestHeaders.Clear();
            cliente.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:109.0) Gecko/20100101 Firefox/114.0");

            try
            {
                HttpResponseMessage respuesta = await cliente.GetAsync(enlace, HttpCompletionOption.ResponseHeadersRead);
                contenido = await respuesta.Content.ReadAsStringAsync();
                respuesta.Dispose();
            }
            catch { }

            return contenido;
        }
	}

    public static class Decompiladores
    {
		//private static readonly HttpClient cliente = new HttpClient(new SocketsHttpHandler
		//{
		//	AutomaticDecompression = DecompressionMethods.GZip,
		//	PooledConnectionLifetime = TimeSpan.FromMinutes(15),
		//	PooledConnectionIdleTimeout = TimeSpan.FromMinutes(10),
		//	MaxConnectionsPerServer = 2
		//}, false);

		public static async Task<string> Estandar(string enlace, SqlConnection conexion = null)
        {
			ServiceProvider servicio = new ServiceCollection().AddHttpClient().BuildServiceProvider();
			IHttpClientFactory factoria = servicio.GetService<IHttpClientFactory>() ?? throw new InvalidOperationException();
			HttpClient cliente = factoria.CreateClient("Decompilador");

			string contenido = string.Empty;

			cliente.DefaultRequestHeaders.Clear();
			cliente.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:109.0) Gecko/20100101 Firefox/114.0");

			try
			{
				using (HttpResponseMessage respuesta = await cliente.GetAsync(enlace, HttpCompletionOption.ResponseContentRead))
				{
                    contenido = respuesta.Content.ReadAsStringAsync().Result;
                    //respuesta.EnsureSuccessStatusCode();

                    //Task tarea = respuesta.Content.ReadAsStreamAsync().ContinueWith(t =>
                    //{
                    //    Stream stream = t.Result;
                    //    using (StreamReader lector = new StreamReader(stream))
                    //    {
                    //        contenido = lector.ReadToEnd();
                    //    }
                    //});

                    //tarea.Wait();
                };
			}
			catch (Exception ex)
			{ 
				if (conexion != null)
				{
					global::BaseDatos.Errores.Insertar.Ejecutar("Decompilador", ex);
				}
			}

			return contenido;
        }

		public static async Task<string> GZipFormato(string enlace) 
        {
            await Task.Delay(1000);

            string html = string.Empty;

			HttpRequestMessage mensaje = new HttpRequestMessage();
            mensaje.RequestUri = new Uri(enlace);
			mensaje.Headers.Accept.ParseAdd("text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,*/*;q=0.8");
			mensaje.Headers.AcceptEncoding.ParseAdd("gzip, deflate, br");
			mensaje.Headers.AcceptLanguage.ParseAdd("es,en-US;q=0.7,en;q=0.3");
			mensaje.Headers.Connection.ParseAdd("keep-alive");
			mensaje.Headers.UserAgent.ParseAdd("Mozilla/5.0 (Linux; Android 10; Generic Android-x86_64 Build/QD1A.190821.014.C2; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/79.0.3945.36 Safari/537.36");

			var cookieContainer = new CookieContainer();

			using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer, UseCookies = true })
			{
				using (HttpClient cliente = new HttpClient(handler) { BaseAddress = new Uri(enlace) })
				{
					Task<HttpResponseMessage> tarea = cliente.SendAsync(mensaje);
					tarea.Wait();

					Task<Stream> respuesta = tarea.Result.Content.ReadAsStreamAsync();
					respuesta.Wait();

					Stream stream = respuesta.Result;

					using (GZipStream descompresion = new GZipStream(stream, CompressionMode.Decompress, false))
					{
						using (StreamReader lector = new StreamReader(descompresion))
						{
							html = lector.ReadToEnd();
						}
					}
				}
			}

            return html;
        }
    }
}
