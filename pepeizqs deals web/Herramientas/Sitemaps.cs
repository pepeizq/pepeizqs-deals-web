#nullable disable

using BaseDatos.Publishers;
using Bundles2;
using Juegos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Noticias;
using System.Text;

namespace Herramientas
{
	public class Sitemaps : Controller
	{
		[HttpGet("sitemap.xml")]
		public IActionResult Sitemap()
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
					"<loc>https://pepeizqdeals.com/bundles/</loc>" + Environment.NewLine +
					"<changefreq>daily</changefreq>" + Environment.NewLine +
					"<priority>0.7</priority> " + Environment.NewLine +
					"</url>";

			sb.Append(textoBundles);

			string textoGratis = "<url>" + Environment.NewLine +
					"<loc>https://pepeizqdeals.com/free/</loc>" + Environment.NewLine +
					"<changefreq>daily</changefreq>" + Environment.NewLine +
					"<priority>0.7</priority> " + Environment.NewLine +
					"</url>";

			sb.Append(textoGratis);

			string textoSuscripciones = "<url>" + Environment.NewLine +
					"<loc>https://pepeizqdeals.com/subscriptions/</loc>" + Environment.NewLine +
					"<changefreq>daily</changefreq>" + Environment.NewLine +
					"<priority>0.7</priority> " + Environment.NewLine +
					"</url>";

			sb.Append(textoSuscripciones);

			string textoMinimos = "<url>" + Environment.NewLine +
					"<loc>https://pepeizqdeals.com/historical-lows/</loc>" + Environment.NewLine +
					"<changefreq>hourly</changefreq>" + Environment.NewLine +
					"<priority>0.9</priority> " + Environment.NewLine +
					"</url>";

			sb.Append(textoMinimos);

			string textoNoticias = "<url>" + Environment.NewLine +
					"<loc>https://pepeizqdeals.com/last-news/</loc>" + Environment.NewLine +
					"<changefreq>hourly</changefreq>" + Environment.NewLine +
					"<priority>0.9</priority> " + Environment.NewLine +
					"</url>";

			sb.Append(textoNoticias);

			string textoAñadidos = "<url>" + Environment.NewLine +
					"<loc>https://pepeizqdeals.com/last-added/</loc>" + Environment.NewLine +
					"<changefreq>hourly</changefreq>" + Environment.NewLine +
					"<priority>0.9</priority> " + Environment.NewLine +
					"</url>";

			sb.Append(textoAñadidos);

			string textoPatreon = "<url>" + Environment.NewLine +
					"<loc>https://pepeizqdeals.com/patreon/</loc>" + Environment.NewLine +
					"<changefreq>hourly</changefreq>" + Environment.NewLine +
					"<priority>0.9</priority> " + Environment.NewLine +
					"</url>";

			sb.Append(textoPatreon);

			List<Noticia> noticias = global::BaseDatos.Noticias.Buscar.Ultimas("20");

			if (noticias.Count > 0)
			{
				foreach (Noticia noticia in noticias)
				{
					DateTime fechaTemporal = noticia.FechaEmpieza;
					fechaTemporal = fechaTemporal.AddDays(7);

					if (fechaTemporal > DateTime.Now)
					{
						string titulo = noticia.TituloEn;
						titulo = titulo.Replace("&", "&amp;");

						string texto = "<url>" + Environment.NewLine +
						"<loc>https://pepeizqdeals.com/news/" + noticia.Id.ToString() + "/" + EnlaceAdaptador.Nombre(noticia.TituloEn) + "/</loc>" + Environment.NewLine +
						"<news:news>" + Environment.NewLine +
						"<news:publication>" + Environment.NewLine +
						"<news:name>pepeizq's deals</news:name>" + Environment.NewLine +
						"<news:language>en</news:language>" + Environment.NewLine +
						"</news:publication>" + Environment.NewLine +
						"<news:publication_date>" + noticia.FechaEmpieza.ToString("yyyy-MM-dd") + "</news:publication_date>" + Environment.NewLine +
						"<news:title>" + titulo + "</news:title>" + Environment.NewLine +
						"</news:news>" + Environment.NewLine +
						"</url>";

						sb.Append(texto);
					}
				}
			}

			sb.Append("</urlset>");

			return new ContentResult
			{
				ContentType = "application/xml",
				Content = sb.ToString(),
				StatusCode = 200
			};
		}

		[HttpGet("sitemap-games.xml")]
		public IActionResult SitemapMinimos()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\"\r\n        xmlns:news=\"http://www.google.com/schemas/sitemap-news/0.9\">\r\n");

			string textoIndex = "<url>" + Environment.NewLine +
					"<loc>https://pepeizqdeals.com/</loc>" + Environment.NewLine +
					"<changefreq>hourly</changefreq>" + Environment.NewLine +
					"<priority>0.9</priority> " + Environment.NewLine +
					"</url>";

			sb.Append(textoIndex);

			List<string> juegos = global::BaseDatos.Juegos.Buscar.Sitemap();

			if (juegos.Count > 0)
			{
				foreach (var enlace in juegos)
				{
                    string textoJuegos = "<url>" + Environment.NewLine +
						 "<loc>" + enlace + "</loc>" + Environment.NewLine +
						 "<changefreq>hourly</changefreq>" + Environment.NewLine +
						 "<priority>0.9</priority> " + Environment.NewLine +
						 "</url>";

                    sb.Append(textoJuegos);
                }
			}

			sb.Append("</urlset>");

			return new ContentResult
			{
				ContentType = "application/xml",
				Content = sb.ToString(),
				StatusCode = 200
			};
		}

        [HttpGet("sitemap-lastgames.xml")]
        public IActionResult SitemapUltimos()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\"\r\n        xmlns:news=\"http://www.google.com/schemas/sitemap-news/0.9\">\r\n");

            string textoIndex = "<url>" + Environment.NewLine +
                    "<loc>https://pepeizqdeals.com/</loc>" + Environment.NewLine +
                    "<changefreq>hourly</changefreq>" + Environment.NewLine +
                    "<priority>0.9</priority> " + Environment.NewLine +
                    "</url>";

            sb.Append(textoIndex);

            List<Juego> juegosConMinimos = new List<Juego>();

            SqlConnection conexion = BaseDatos.Conectar();

            using (conexion)
            {
                juegosConMinimos = global::BaseDatos.Juegos.Buscar.Ultimos(conexion, "juegos", 200);
            }

            if (juegosConMinimos.Count > 0)
            {
                int i = 0;
                while (i < 200)
                {
                    string textoJuegos = "<url>" + Environment.NewLine +
                         "<loc>https://pepeizqdeals.com/game/" + juegosConMinimos[i].Id + "/" + EnlaceAdaptador.Nombre(juegosConMinimos[i].Nombre) + "/</loc>" + Environment.NewLine +
                         "<changefreq>hourly</changefreq>" + Environment.NewLine +
                         "<priority>0.9</priority> " + Environment.NewLine +
                         "</url>";

                    sb.Append(textoJuegos);

                    i += 1;
                }
            }

            sb.Append("</urlset>");

            return new ContentResult
            {
                ContentType = "application/xml",
                Content = sb.ToString(),
                StatusCode = 200
            };
        }

        [HttpGet("sitemap-bundles.xml")]
		public IActionResult SitemapBundles()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\"\r\n        xmlns:news=\"http://www.google.com/schemas/sitemap-news/0.9\">\r\n");

			string textoIndex = "<url>" + Environment.NewLine +
					"<loc>https://pepeizqdeals.com/</loc>" + Environment.NewLine +
					"<changefreq>hourly</changefreq>" + Environment.NewLine +
					"<priority>0.9</priority> " + Environment.NewLine +
					"</url>";

			sb.Append(textoIndex);

			List<Bundle> bundles = new List<Bundle>();

			SqlConnection conexion = BaseDatos.Conectar();

			using (conexion)
			{
				bundles = global::BaseDatos.Bundles.Buscar.Ultimos("100");
			}

			if (bundles.Count > 0)
			{
				foreach (var bundle in bundles)
				{
					string textoBundles = "<url>" + Environment.NewLine +
						 "<loc>https://pepeizqdeals.com/bundle/" + bundle.Id + "/" + EnlaceAdaptador.Nombre(bundle.NombreBundle) + "/</loc>" + Environment.NewLine +
						 "<changefreq>hourly</changefreq>" + Environment.NewLine +
						 "<priority>0.9</priority> " + Environment.NewLine +
						 "</url>";

					sb.Append(textoBundles);
				}
			}

			sb.Append("</urlset>");

			return new ContentResult
			{
				ContentType = "application/xml",
				Content = sb.ToString(),
				StatusCode = 200
			};
		}

		[HttpGet("sitemap-news.xml")]
		public IActionResult SitemapNoticias()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\"\r\n        xmlns:news=\"http://www.google.com/schemas/sitemap-news/0.9\">\r\n");

			string textoIndex = "<url>" + Environment.NewLine +
					"<loc>https://pepeizqdeals.com/</loc>" + Environment.NewLine +
					"<changefreq>hourly</changefreq>" + Environment.NewLine +
					"<priority>0.9</priority> " + Environment.NewLine +
					"</url>";

			sb.Append(textoIndex);

			List<Noticia> noticias = new List<Noticia>();

			SqlConnection conexion = BaseDatos.Conectar();

			using (conexion)
			{
				noticias = global::BaseDatos.Noticias.Buscar.Ultimas("20");
			}

			if (noticias.Count > 0)
			{
				foreach (var noticia in noticias)
				{
					string textoNoticias = "<url>" + Environment.NewLine +
						 "<loc>https://pepeizqdeals.com/news/" + noticia.Id + "/" + EnlaceAdaptador.Nombre(noticia.TituloEn) + "/</loc>" + Environment.NewLine +
						 "<changefreq>hourly</changefreq>" + Environment.NewLine +
						 "<priority>0.9</priority> " + Environment.NewLine +
						 "</url>";

					sb.Append(textoNoticias);
				}
			}

			sb.Append("</urlset>");

			return new ContentResult
			{
				ContentType = "application/xml",
				Content = sb.ToString(),
				StatusCode = 200
			};
		}

		[HttpGet("sitemap-publishers.xml")]
		public IActionResult SitemapPublishers()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\"\r\n        xmlns:news=\"http://www.google.com/schemas/sitemap-news/0.9\">\r\n");

			string textoIndex = "<url>" + Environment.NewLine +
					"<loc>https://pepeizqdeals.com/</loc>" + Environment.NewLine +
					"<changefreq>hourly</changefreq>" + Environment.NewLine +
					"<priority>0.9</priority> " + Environment.NewLine +
					"</url>";

			sb.Append(textoIndex);

			List<Publisher> publishers = new List<Publisher>();

			publishers = Buscar.Todos();

			if (publishers.Count > 0)
			{
				foreach (var publisher in publishers)
				{
					string textoPublisher = "<url>" + Environment.NewLine +
						 "<loc>https://pepeizqdeals.com/publisher/" + publisher.Id + "/</loc>" + Environment.NewLine +
						 "<changefreq>hourly</changefreq>" + Environment.NewLine +
						 "<priority>0.9</priority> " + Environment.NewLine +
						 "</url>";

					sb.Append(textoPublisher);
				}
			}

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
