using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;

namespace pepeizqs_deals_web.Pages
{
    public class SitemapModel : PageModel
    {
		public IActionResult OnGet()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\"\r\n        xmlns:news=\"http://www.google.com/schemas/sitemap-news/0.9\">\r\n");

			string textoIndex = "<url>" + Environment.NewLine +
					"<loc>https://pepeizqdeals.com/</loc>" + Environment.NewLine +
					"<changefreq>hourly</changefreq>" + Environment.NewLine +
					"<priority>0.9</priority> " + Environment.NewLine +
					"</url>";

			sb.Append(textoIndex);

			string textoBundles = "<url>" + Environment.NewLine +
					"<loc>https://pepeizqdeals.com/Bundles</loc>" + Environment.NewLine +
					"<changefreq>daily</changefreq>" + Environment.NewLine +
					"<priority>0.7</priority> " + Environment.NewLine +
					"</url>";

			sb.Append(textoBundles);

			string textoGratis = "<url>" + Environment.NewLine +
					"<loc>https://pepeizqdeals.com/Free</loc>" + Environment.NewLine +
					"<changefreq>daily</changefreq>" + Environment.NewLine +
					"<priority>0.7</priority> " + Environment.NewLine +
					"</url>";

			sb.Append(textoGratis);

			string textoSuscripciones = "<url>" + Environment.NewLine +
					"<loc>https://pepeizqdeals.com/Subscriptions</loc>" + Environment.NewLine +
					"<changefreq>daily</changefreq>" + Environment.NewLine +
					"<priority>0.7</priority> " + Environment.NewLine +
					"</url>";

			sb.Append(textoSuscripciones);

			string textoMinimos = "<url>" + Environment.NewLine +
					"<loc>https://pepeizqdeals.com/HistoricalLow</loc>" + Environment.NewLine +
					"<changefreq>hourly</changefreq>" + Environment.NewLine +
					"<priority>0.9</priority> " + Environment.NewLine +
					"</url>";

			sb.Append(textoMinimos);

			string textoNoticias = "<url>" + Environment.NewLine +
					"<loc>https://pepeizqdeals.com/LastNews</loc>" + Environment.NewLine +
					"<changefreq>hourly</changefreq>" + Environment.NewLine +
					"<priority>0.9</priority> " + Environment.NewLine +
					"</url>";

			sb.Append(textoNoticias);

			string textoAñadidos = "<url>" + Environment.NewLine +
					"<loc>https://pepeizqdeals.com/LastAdded</loc>" + Environment.NewLine +
					"<changefreq>hourly</changefreq>" + Environment.NewLine +
					"<priority>0.9</priority> " + Environment.NewLine +
					"</url>";

			sb.Append(textoAñadidos);

			#region Noticias

			//List<Noticias.Noticia> noticias = BaseDatos.Noticias.Buscar.Todas();

			//foreach (Noticias.Noticia noticia in noticias)
			//{
   //             DateTime fechaTemporal = noticia.FechaEmpieza;
   //             fechaTemporal = fechaTemporal.AddDays(3);

			//	if (fechaTemporal > DateTime.Now)
			//	{
   //                 string texto = "<url>" + Environment.NewLine +
   //                 "<loc>https://pepeizqdeals.com/news/" + noticia.Id.ToString() + "</loc>" + Environment.NewLine +
   //                 "<news:news>" + Environment.NewLine +
   //                 "<news:publication>" + Environment.NewLine +
   //                 "<news:name>pepeizq's deals</news:name>" + Environment.NewLine +
   //                 "<news:language>en</news:language>" + Environment.NewLine +
   //                 "</news:publication>" + Environment.NewLine +
   //                 "<news:publication_date>" + noticia.FechaEmpieza.ToString("yyyy-MM-dd") + "</news:publication_date>" + Environment.NewLine +
   //                 "<news:title>" + noticia.TituloEn + "</news:title>" + Environment.NewLine +
   //                 "</news:news>" + Environment.NewLine +
   //                 "</url>";

   //                 sb.Append(texto);
   //             }
			//}

			#endregion

			sb.Append("</urlset>");

			return new ContentResult
			{
				ContentType = "application/xml",
				Content = sb.ToString(),
				StatusCode = 200
			};
		}
	}
}
