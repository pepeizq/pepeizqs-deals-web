#nullable disable

using Microsoft.AspNetCore.Mvc;
using Noticias;
using System.Text;

namespace Herramientas
{
	public class Sitemaps : Controller
	{
		[HttpGet("sitemap.xml")]
		public IActionResult Principal()
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

			string textoCurators = "<url>" + Environment.NewLine +
					"<loc>https://pepeizqdeals.com/curators/</loc>" + Environment.NewLine +
					"<changefreq>hourly</changefreq>" + Environment.NewLine +
					"<priority>0.9</priority> " + Environment.NewLine +
					"</url>";

			sb.Append(textoCurators);

			string textoComparador = "<url>" + Environment.NewLine +
					"<loc>https://pepeizqdeals.com/compare/</loc>" + Environment.NewLine +
					"<changefreq>hourly</changefreq>" + Environment.NewLine +
					"<priority>0.9</priority> " + Environment.NewLine +
					"</url>";

			sb.Append(textoComparador);

			List<Noticia> noticias = global::BaseDatos.Noticias.Buscar.Ultimas(20);

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
		public IActionResult JuegosMinimos()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\"\r\n        xmlns:news=\"http://www.google.com/schemas/sitemap-news/0.9\">\r\n");

			List<string> juegos = global::BaseDatos.Sitemaps.Buscar.JuegosMinimos();

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

		[HttpGet("sitemap-gamesrandom.xml")]
		public IActionResult JuegosAzar()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\"\r\n        xmlns:news=\"http://www.google.com/schemas/sitemap-news/0.9\">\r\n");

			List<string> juegos = global::BaseDatos.Sitemaps.Buscar.JuegosAzar();

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
        public IActionResult JuegosUltimos()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\"\r\n        xmlns:news=\"http://www.google.com/schemas/sitemap-news/0.9\">\r\n");

			List<string> juegos = global::BaseDatos.Sitemaps.Buscar.JuegosUltimos();

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

        [HttpGet("sitemap-bundles.xml")]
		public IActionResult BundlesUltimos()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\"\r\n        xmlns:news=\"http://www.google.com/schemas/sitemap-news/0.9\">\r\n");

			List<string> bundles = global::BaseDatos.Sitemaps.Buscar.BundlesUltimos();

			if (bundles.Count > 0)
			{
				foreach (var enlace in bundles)
				{
					string texto = "<url>" + Environment.NewLine +
						 "<loc>" + enlace + "</loc>" + Environment.NewLine +
						 "<changefreq>hourly</changefreq>" + Environment.NewLine +
						 "<priority>0.9</priority> " + Environment.NewLine +
						 "</url>";

					sb.Append(texto);
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

		[HttpGet("sitemap-bundlesrandom.xml")]
		public IActionResult BundlesAzar()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\"\r\n        xmlns:news=\"http://www.google.com/schemas/sitemap-news/0.9\">\r\n");

			List<string> bundles = global::BaseDatos.Sitemaps.Buscar.BundlesAzar();

			if (bundles.Count > 0)
			{
				foreach (var enlace in bundles)
				{
					string texto = "<url>" + Environment.NewLine +
						 "<loc>" + enlace + "</loc>" + Environment.NewLine +
						 "<changefreq>hourly</changefreq>" + Environment.NewLine +
						 "<priority>0.9</priority> " + Environment.NewLine +
						 "</url>";

					sb.Append(texto);
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
		public IActionResult NoticiasUltimasIngles()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\"\r\n        xmlns:news=\"http://www.google.com/schemas/sitemap-news/0.9\">\r\n");

			List<string> noticias = global::BaseDatos.Sitemaps.Buscar.NoticiasUltimasIngles();

			if (noticias.Count > 0)
			{
				foreach (var enlace in noticias)
				{
					string texto = "<url>" + Environment.NewLine +
						 "<loc>" + enlace + "</loc>" + Environment.NewLine +
						 "<changefreq>hourly</changefreq>" + Environment.NewLine +
						 "<priority>0.9</priority> " + Environment.NewLine +
						 "</url>";

					sb.Append(texto);
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

		[HttpGet("sitemap-newses.xml")]
		public IActionResult NoticiasUltimasEspañol()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\"\r\n        xmlns:news=\"http://www.google.com/schemas/sitemap-news/0.9\">\r\n");

			List<string> noticias = global::BaseDatos.Sitemaps.Buscar.NoticiasUltimasEspañol();

			if (noticias.Count > 0)
			{
				foreach (var enlace in noticias)
				{
					string texto = "<url>" + Environment.NewLine +
						 "<loc>" + enlace + "</loc>" + Environment.NewLine +
						 "<changefreq>hourly</changefreq>" + Environment.NewLine +
						 "<priority>0.9</priority> " + Environment.NewLine +
						 "</url>";

					sb.Append(texto);
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

		[HttpGet("sitemap-curators.xml")]
		public IActionResult Curators()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\"\r\n        xmlns:news=\"http://www.google.com/schemas/sitemap-news/0.9\">\r\n");

			List<string> curators = global::BaseDatos.Sitemaps.Buscar.Curators();

			if (curators.Count > 0)
			{
				foreach (var enlace in curators)
				{
					string texto = "<url>" + Environment.NewLine +
						 "<loc>" + enlace + "</loc>" + Environment.NewLine +
						 "<changefreq>hourly</changefreq>" + Environment.NewLine +
						 "<priority>0.9</priority> " + Environment.NewLine +
						 "</url>";

					sb.Append(texto);
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
