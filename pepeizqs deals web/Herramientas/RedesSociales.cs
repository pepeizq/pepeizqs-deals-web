#nullable disable

using Microsoft.AspNetCore.Mvc;
using System.ServiceModel.Syndication;
using System.Text;
using System.Xml;

namespace Herramientas
{
	public class RedesSociales : Controller
	{
		public string dominio = "https://pepeizqdeals.com";

		[ResponseCache(Duration = 2000)]
		[HttpGet("rss-en.xml")]
		public IActionResult GenerarEnRSS()
		{
			SyndicationFeed feed = new SyndicationFeed("pepeizq's deals", "RSS in English from the web", new Uri(dominio), "RSSUrl", DateTime.Now)
			{
				Copyright = new TextSyndicationContent($"{DateTime.Now.Year}")
			};

			List<SyndicationItem> items = new List<SyndicationItem>();
			List<Noticias.Noticia> noticias = global::BaseDatos.Noticias.Buscar.Todas();

			foreach (Noticias.Noticia noticia in noticias)
			{
				string enlace = string.Empty;

				if (noticia.Enlace.Contains(dominio) == false)
				{
					enlace = dominio + noticia.Enlace;
				}
				else
				{
					enlace = noticia.Enlace;
				}

				string titulo = noticia.TituloEn;
				string contenido = string.Empty;
				items.Add(new SyndicationItem(titulo, contenido, new Uri(enlace), noticia.Id.ToString(), noticia.FechaEmpieza));
			}

			feed.Items = items;

			XmlWriterSettings opciones = new XmlWriterSettings
			{
				Encoding = Encoding.UTF8,
				NewLineHandling = NewLineHandling.Entitize,
				NewLineOnAttributes = true,
				Indent = true
			};

			using (MemoryStream stream = new MemoryStream())
			{
				using (XmlWriter xmlEscritor = XmlWriter.Create(stream, opciones))
				{
					Rss20FeedFormatter rssFormateador = new Rss20FeedFormatter(feed, false);
					rssFormateador.WriteTo(xmlEscritor);
					xmlEscritor.Flush();
				}

				return File(stream.ToArray(), "application/rss+xml; charset=utf-8");
			}
		}

		[HttpGet("rss-es.xml")]
		public IActionResult GenerarEsRSS()
		{
			SyndicationFeed feed = new SyndicationFeed("pepeizq's deals", "RSS en Español de la web", new Uri(dominio), "RSSUrl", DateTime.Now)
			{
				Copyright = new TextSyndicationContent($"{DateTime.Now.Year}")
			};

			List<SyndicationItem> items = new List<SyndicationItem>();
			List<Noticias.Noticia> noticias = global::BaseDatos.Noticias.Buscar.Todas();

			foreach (Noticias.Noticia noticia in noticias)
			{
				string enlace = string.Empty;

				if (noticia.Enlace != null)
				{
					if (noticia.Enlace.Contains(dominio) == false)
					{
						enlace = dominio + noticia.Enlace;
					}
					else
					{
						enlace = noticia.Enlace;
					}		
				}
				else
				{
					enlace = dominio + "/news/" + noticia.Id.ToString();
				}

				string titulo = noticia.TituloEs;
				string contenido = string.Empty;
				items.Add(new SyndicationItem(titulo, contenido, new Uri(enlace), noticia.Id.ToString(), noticia.FechaEmpieza));
			}

			feed.Items = items;

			XmlWriterSettings opciones = new XmlWriterSettings
			{
				Encoding = Encoding.UTF8,
				NewLineHandling = NewLineHandling.Entitize,
				NewLineOnAttributes = true,
				Indent = true
			};

			using (MemoryStream stream = new MemoryStream())
			{
				using (XmlWriter xmlEscritor = XmlWriter.Create(stream, opciones))
				{
					Rss20FeedFormatter rssFormateador = new Rss20FeedFormatter(feed, false);
					rssFormateador.WriteTo(xmlEscritor);
					xmlEscritor.Flush();
				}

				return File(stream.ToArray(), "application/rss+xml; charset=utf-8");
			}
		}
	}
}
