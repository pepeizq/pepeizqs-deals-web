#nullable disable

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ServiceModel.Syndication;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Tweetinvi;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;

namespace Herramientas
{
	public class Rss : Controller
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

			if (noticias.Count > 0)
			{
				noticias = noticias.OrderBy(x => x.FechaEmpieza).Reverse().ToList();

				foreach (Noticias.Noticia noticia in noticias)
				{
					if (DateTime.Now >= noticia.FechaEmpieza && DateTime.Now <= noticia.FechaTermina)
					{
						string enlace = noticia.Enlace;

						if (enlace != null)
						{
							if (enlace.Contains(dominio) == false)
							{
								enlace = dominio + noticia.Enlace;
							}
						}

						string titulo = noticia.TituloEn;
						string contenido = noticia.ContenidoEn;
						Uri enlaceUri = null;

						if (enlace != null)
						{
							enlaceUri = new Uri(enlace);
						}

						SyndicationItem item = new SyndicationItem(titulo, contenido, enlaceUri, noticia.Id.ToString(), noticia.FechaEmpieza);
						
						if (noticia.Imagen != null)
						{
							item.ElementExtensions.Add(new XElement("image", dominio + "/imagenes/noticias/" + noticia.Id.ToString() + "/header.webp"));
						}
						
						items.Add(item);
					}
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

			return null;
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

			if (noticias.Count > 0)
			{
				noticias = noticias.OrderBy(x => x.FechaEmpieza).Reverse().ToList();

				foreach (Noticias.Noticia noticia in noticias)
				{
					if (DateTime.Now >= noticia.FechaEmpieza && DateTime.Now <= noticia.FechaTermina)
					{
						string enlace = noticia.Enlace;

						if (enlace != null)
						{
							if (enlace.Contains(dominio) == false)
							{
								enlace = dominio + noticia.Enlace;
							}
						}

						string titulo = noticia.TituloEs;
						string contenido = noticia.ContenidoEs;
						Uri enlaceUri = null;

						if (enlace != null)
						{
							enlaceUri = new Uri(enlace);
						}

						SyndicationItem item = new SyndicationItem(titulo, contenido, enlaceUri, noticia.Id.ToString(), noticia.FechaEmpieza);
						
						if (noticia.Imagen != null)
						{
							item.ElementExtensions.Add(new XElement("image", dominio + "/imagenes/noticias/" + noticia.Id.ToString() + "/header.webp"));
						}

						items.Add(item);
					}
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

			return null;
		}
	}

	public static class Twitter
	{
		public static async void Twitear(Noticias.Noticia noticia)
		{
			WebApplicationBuilder builder = WebApplication.CreateBuilder();

			TwitterClient cliente = new TwitterClient(builder.Configuration.GetValue<string>("Twitter:ConsumerKey"),
				builder.Configuration.GetValue<string>("Twitter:ConsumerSecret"),
				builder.Configuration.GetValue<string>("Twitter:AccessToken"),
				builder.Configuration.GetValue<string>("Twitter:AccessSecret"));

			ITwitterResult resultado = await PonerTweet(cliente,
				new TweetV2PostRequest
				{
					Text = noticia.TituloEn + " " + Environment.NewLine + Environment.NewLine + noticia.Enlace
				}
			);
		}

		public static Task<ITwitterResult> PonerTweet(TwitterClient cliente, TweetV2PostRequest parametros)
		{
			return cliente.Execute.AdvanceRequestAsync(
				(ITwitterRequest peticion) =>
				{
					string json = cliente.Json.Serialize(parametros);
					StringContent contenido = new StringContent(json, Encoding.UTF8, "application/json");

					peticion.Query.Url = "https://api.twitter.com/2/tweets";
					peticion.Query.HttpMethod = Tweetinvi.Models.HttpMethod.POST;
					peticion.Query.HttpContent = contenido;
				}
			);
		}

		public class TweetV2PostRequest
		{
			[JsonProperty("text")]
			public string Text { get; set; } = string.Empty;
		}
	}
}
