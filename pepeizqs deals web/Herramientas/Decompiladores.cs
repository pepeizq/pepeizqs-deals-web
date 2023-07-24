#nullable disable

using System.IO.Compression;
using System.Net;

namespace Herramientas
{
    public static class Decompiladores
    {
        public static async Task<string> Estandar(string enlace)
        {
            HttpClient cliente = new HttpClient();
            cliente.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:109.0) Gecko/20100101 Firefox/114.0");

            HttpResponseMessage respuesta = await cliente.GetAsync(enlace);
            string contenido = await respuesta.Content.ReadAsStringAsync();
            respuesta.Dispose();

            cliente.Dispose();

            return contenido;
        }

		public static string GZipFormato(string enlace) 
        {
			string html = string.Empty;

			HttpRequestMessage mensaje = new HttpRequestMessage();
            mensaje.RequestUri = new Uri(enlace);
			mensaje.Headers.Accept.ParseAdd("text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,*/*;q=0.8");
			mensaje.Headers.AcceptEncoding.ParseAdd("gzip, deflate, br");
			mensaje.Headers.AcceptLanguage.ParseAdd("es,en-US;q=0.7,en;q=0.3");
			mensaje.Headers.Connection.ParseAdd("keep-alive");
			mensaje.Headers.Host = "www.humblebundle.com";
			mensaje.Headers.Referrer = new Uri("https://www.humblebundle.com/");
			mensaje.Headers.Upgrade.ParseAdd("1");
			mensaje.Headers.UserAgent.ParseAdd("Mozilla/5.0 (Linux; Android 10; Generic Android-x86_64 Build/QD1A.190821.014.C2; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/79.0.3945.36 Safari/537.36");

			var cookieContainer = new CookieContainer();

			using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer, UseCookies = true })
			{
				using (HttpClient cliente = new HttpClient(handler) { BaseAddress = new Uri(enlace) })
				{
					
					cookieContainer.Add(new Uri(enlace), new Cookie("_simpleauth_sess", "eyJpZCI6IlA2Zzk4ZW9Md0kifQ==|1690107712|8a4c5b9ae42d246113e05ccc7ff18c2a13392b25"));
					
					Task<HttpResponseMessage> tarea = cliente.SendAsync(mensaje);
					tarea.Wait();

					Task<Stream> respuesta = tarea.Result.Content.ReadAsStreamAsync();
					respuesta.Wait();

					Stream stream = respuesta.Result;

					using (GZipStream descompresion = new GZipStream(stream, CompressionMode.Decompress))
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
