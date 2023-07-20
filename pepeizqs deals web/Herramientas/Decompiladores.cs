using System.IO.Compression;

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

		public static void Humble(string enlace) 
        {
            HttpClient cliente = new HttpClient();
            cliente.DefaultRequestHeaders.Accept.ParseAdd("text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,*/*;q=0.8");
            cliente.DefaultRequestHeaders.AcceptEncoding.ParseAdd("gzip, deflate, br");
            cliente.DefaultRequestHeaders.AcceptLanguage.ParseAdd("es,en-US;q=0.7,en;q=0.3");
            cliente.DefaultRequestHeaders.Connection.ParseAdd("keep-alive");
            cliente.DefaultRequestHeaders.Host = "www.humblebundle.com";
            cliente.DefaultRequestHeaders.Referrer = new Uri("https://www.humblebundle.com/");
            cliente.DefaultRequestHeaders.Upgrade.ParseAdd("1");
            cliente.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:109.0) Gecko/20100101 Firefox/114.0");

            cliente.DefaultRequestHeaders.Add("Cookie", "_simpleauth_sess=eyJpZCI6Ijc4TTl5NHk5VkQifQ==|1688323090|955afe02de34caea04d4982ffbd0e1747df9d377; __cf_bm=IbitFjme5MDB089Vht_yykr.2IulViRq9CTq7It910Q-1688323076-0-AcqOcvonM+I9cTpzMmS1kJ8Jne7Gho2h83rHICnemmaU4YVOeGRWqxnHK9Oo0HztaSQO8CQqp/mmrE0K0tbj1hI=");

            Task<HttpResponseMessage> tarea = cliente.GetAsync(enlace);
            tarea.Wait();

            Task<Stream> respuesta = tarea.Result.Content.ReadAsStreamAsync();
            respuesta.Wait();

            Stream stream = respuesta.Result;
            string html = string.Empty;

            using (GZipStream descompresion = new GZipStream(stream, CompressionMode.Decompress))
            {
                using (StreamReader lector = new StreamReader(descompresion))
                {
                    html = lector.ReadToEnd();
                }
            }

            Console.WriteLine($"Response: {html}");
        }
    }
}
