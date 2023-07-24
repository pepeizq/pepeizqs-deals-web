using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO.Compression;
using System.Net;

namespace pepeizqs_deals_web.Pages.Admin.Tiendas
{
    public class TiendasModel : PageModel
    {
        public string html2 = string.Empty;

        public void OnGet()
        {
            HttpClient cliente = new HttpClient();
            cliente.DefaultRequestHeaders.Accept.ParseAdd("text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,*/*;q=0.8");
            cliente.DefaultRequestHeaders.AcceptEncoding.ParseAdd("gzip, deflate, br");
            cliente.DefaultRequestHeaders.AcceptLanguage.ParseAdd("it,en-US;q=0.7,en;q=0.3");
            cliente.DefaultRequestHeaders.Connection.ParseAdd("keep-alive");
            cliente.DefaultRequestHeaders.Host = "www.humblebundle.com";
            cliente.DefaultRequestHeaders.Referrer = new Uri("https://www.humblebundle.com/");
            cliente.DefaultRequestHeaders.Upgrade.ParseAdd("1");
            cliente.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Linux; Android 11; TX6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.116 Safari/537.36");

            cliente.DefaultRequestHeaders.Add("Cookie", "_simpleauth_sess=eyJpZCI6IlA2Zzk4ZW9Md0kifQ==|1690107705|990dc3ae5f2e06b2ceab54ffbdd2d8ed9bb961ae; __cf_bm=hU4VpHmh1ImQoNqA_7nPls.AJb7tRR.JzSJPclp.Rkw-1690107574-0-AStg0XLhW6ii9rTCTeEtSm1WCYFEKLJvuuKRB/jQ3Wi6VMFkqhxnxPlUj1LVKb7XFp4DUt4y7FT9pYagCpGv6C4=");

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
            html2 = res;
        }
    }
}
