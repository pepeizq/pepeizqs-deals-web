#nullable disable

using Microsoft.AspNetCore.Mvc;
using Noticias;
using System.Text;

namespace Herramientas
{
	public class Sitemaps : Controller
	{
		[HttpGet("sitemap.xml")]
		public IActionResult Maestro()
		{
			List<string> sitemaps = ["https://pepeizqdeals.com/sitemap-main.xml",
				"https://pepeizqdeals.com/sitemap-curators.xml"];

			int cantidadJuegos = global::BaseDatos.Sitemaps.Buscar.Cantidad("juegos");

			if (cantidadJuegos > 0)
			{
				int segmentacion = cantidadJuegos / 1000;

				int i = 0;
				while (i <= segmentacion)
				{
					sitemaps.Add("https://pepeizqdeals.com/sitemap-games-" + i.ToString() + ".xml");

					i += 1;
				}
			}

			int cantidadBundles = global::BaseDatos.Sitemaps.Buscar.Cantidad("bundles");

			if (cantidadBundles > 0)
			{
				int segmentacion = cantidadBundles / 100;

				int i = 0;
				while (i <= segmentacion)
				{
					sitemaps.Add("https://pepeizqdeals.com/sitemap-bundles-" + i.ToString() + ".xml");

					i += 1;
				}
			}

			int cantidadNoticias = global::BaseDatos.Sitemaps.Buscar.Cantidad("noticias");

			if (cantidadNoticias > 0)
			{
				int segmentacion = cantidadNoticias / 100;

				int i = 0;
				while (i <= segmentacion)
				{
					sitemaps.Add("https://pepeizqdeals.com/sitemap-news-en-" + i.ToString() + ".xml");

					i += 1;
				}

				i = 0;
				while (i <= segmentacion)
				{
					sitemaps.Add("https://pepeizqdeals.com/sitemap-news-es-" + i.ToString() + ".xml");

					i += 1;
				}
			}

			StringBuilder sb = new StringBuilder();
			sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
			sb.Append("<sitemapindex xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">");

			foreach (var sitemap in sitemaps)
			{
				sb.Append("<sitemap>");
				sb.Append("<loc>" + sitemap + "</loc>");
				sb.Append("</sitemap>");
			}

			sb.Append("</sitemapindex>");

			return new ContentResult
			{
				ContentType = "application/xml",
				Content = sb.ToString(),
				StatusCode = 200
			};
		}

		[HttpGet("sitemap-main.xml")]
		public IActionResult Principal()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\"\r\n        xmlns:news=\"http://www.google.com/schemas/sitemap-news/0.9\">\r\n");

			string textoIndex = "<url>" + Environment.NewLine +
					"<loc>https://pepeizqdeals.com/</loc>" + Environment.NewLine +
					"<changefreq>hourly</changefreq>" + Environment.NewLine +
					"</url>";

			sb.Append(textoIndex);

			string textoBundles = "<url>" + Environment.NewLine +
					"<loc>https://pepeizqdeals.com/bundles/</loc>" + Environment.NewLine +
					"<changefreq>daily</changefreq>" + Environment.NewLine +
					"</url>";

			sb.Append(textoBundles);

			string textoGratis = "<url>" + Environment.NewLine +
					"<loc>https://pepeizqdeals.com/free/</loc>" + Environment.NewLine +
					"<changefreq>daily</changefreq>" + Environment.NewLine +
					"</url>";

			sb.Append(textoGratis);

			string textoSuscripciones = "<url>" + Environment.NewLine +
					"<loc>https://pepeizqdeals.com/subscriptions/</loc>" + Environment.NewLine +
					"<changefreq>daily</changefreq>" + Environment.NewLine +
					"</url>";

			sb.Append(textoSuscripciones);

			string textoMinimos = "<url>" + Environment.NewLine +
					"<loc>https://pepeizqdeals.com/historical-lows/</loc>" + Environment.NewLine +
					"<changefreq>hourly</changefreq>" + Environment.NewLine +
					"</url>";

			sb.Append(textoMinimos);

			string textoNoticias = "<url>" + Environment.NewLine +
					"<loc>https://pepeizqdeals.com/last-news/</loc>" + Environment.NewLine +
					"<changefreq>hourly</changefreq>" + Environment.NewLine +
					"</url>";

			sb.Append(textoNoticias);

			string textoAñadidos = "<url>" + Environment.NewLine +
					"<loc>https://pepeizqdeals.com/last-added/</loc>" + Environment.NewLine +
					"<changefreq>hourly</changefreq>" + Environment.NewLine +
					"</url>";

			sb.Append(textoAñadidos);

			string textoPatreon = "<url>" + Environment.NewLine +
					"<loc>https://pepeizqdeals.com/patreon/</loc>" + Environment.NewLine +
					"</url>";

			sb.Append(textoPatreon);

			string textoComparador = "<url>" + Environment.NewLine +
					"<loc>https://pepeizqdeals.com/compare/</loc>" + Environment.NewLine +
					"</url>";

			sb.Append(textoComparador);

			string textoApi = "<url>" + Environment.NewLine +
					"<loc>https://pepeizqdeals.com/api/</loc>" + Environment.NewLine +
					"</url>";

			sb.Append(textoApi);

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

		[HttpGet("sitemap-games-{i:int}.xml")]
		public IActionResult Juegos(int i)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\"\r\n        xmlns:news=\"http://www.google.com/schemas/sitemap-news/0.9\">\r\n");

			int minimo = 0;
			int maximo = 0;

			minimo = i * 1000;
			maximo = (i + 1) * 1000;

			if (i == 0)
			{
				minimo = 0;
			}

			List<string> lineas = global::BaseDatos.Sitemaps.Buscar.Juegos(minimo - 1, maximo);

			if (lineas.Count > 0)
			{
				foreach (var linea in lineas)
				{
                    sb.Append(linea);
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

        [HttpGet("sitemap-bundles-{i:int}.xml")]
		public IActionResult Bundles(int i)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\"\r\n        xmlns:news=\"http://www.google.com/schemas/sitemap-news/0.9\">\r\n");

			int minimo = 0;
			int maximo = 0;

			minimo = i * 100;
			maximo = (i + 1) * 100;

			if (i == 0)
			{
				minimo = 0;
			}

			List<string> lineas = global::BaseDatos.Sitemaps.Buscar.Bundles(minimo - 1, maximo);

			if (lineas.Count > 0)
			{
				foreach (var linea in lineas)
				{
					sb.Append(linea);
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

		[HttpGet("sitemap-news-en-{i:int}.xml")]
		public IActionResult NoticiasIngles(int i)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\"\r\n        xmlns:news=\"http://www.google.com/schemas/sitemap-news/0.9\">\r\n");

			int minimo = 0;
			int maximo = 0;

			minimo = i * 100;
			maximo = (i + 1) * 100;

			if (i == 0)
			{
				minimo = 0;
			}

			List<string> lineas = global::BaseDatos.Sitemaps.Buscar.NoticiasIngles(minimo - 1, maximo);

			if (lineas.Count > 0)
			{
				foreach (var linea in lineas)
				{
					sb.Append(linea);
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

		[HttpGet("sitemap-news-es-{i:int}.xml")]
		public IActionResult NoticiasEspañol(int i)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\"\r\n        xmlns:news=\"http://www.google.com/schemas/sitemap-news/0.9\">\r\n");

			int minimo = 0;
			int maximo = 0;

			minimo = i * 100;
			maximo = (i + 1) * 100;

			if (i == 0)
			{
				minimo = 0;
			}

			List<string> lineas = global::BaseDatos.Sitemaps.Buscar.NoticiasEspañol(minimo - 1, maximo);

			if (lineas.Count > 0)
			{
				foreach (var linea in lineas)
				{
					sb.Append(linea);
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

			List<string> lineas = global::BaseDatos.Sitemaps.Buscar.Curators();

			if (lineas.Count > 0)
			{
				foreach (var linea in lineas)
				{
					sb.Append(linea);
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
