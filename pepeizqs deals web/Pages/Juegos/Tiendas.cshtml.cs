using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO.Compression;
using System.Net;

namespace pepeizqs_deals_web.Pages.Juegos
{
    public class TiendasModel : PageModel
    {
        public void OnGet()
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

			Console.WriteLine(cliente.DefaultRequestHeaders.UserAgent.ToString());

			var task = cliente.GetAsync("https://www.humblebundle.com/store/api/search?filter=onsale&sort=discount&request=2&page_size=20&page=0");
			task.Wait(); 

			var taskResponse = task.Result.Content.ReadAsStreamAsync();
			taskResponse.Wait();

			var html = taskResponse.Result;

			string res;
			using (var decompress = new GZipStream(html, CompressionMode.Decompress))
			using (var sr = new StreamReader(decompress))
			{
				res = sr.ReadToEnd();
			}

			Console.WriteLine($"Response: {res}");

		}
    }
}
