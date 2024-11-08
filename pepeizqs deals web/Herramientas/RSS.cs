#nullable disable

using Microsoft.AspNetCore.Mvc;
using Noticias;
using System.ServiceModel.Syndication;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using System.Security.Claims;

namespace Herramientas
{
    public class Rss : Controller
    {
        public string dominio = "https://pepeizqdeals.com";

        #region Noticias

        [ResponseCache(Duration = 3000)]
        [HttpGet("rss-en.xml")]
        public IActionResult GenerarEnRSS()
        {
            SyndicationFeed feed = new SyndicationFeed("pepeizq's deals", "RSS in English from the web", new Uri(dominio), "RSSUrl", DateTime.Now)
            {
                Copyright = new TextSyndicationContent($"{DateTime.Now.Year}")
            };

            List<SyndicationItem> items = new List<SyndicationItem>();
            List<Noticias.Noticia> noticias = global::BaseDatos.Noticias.Buscar.Actuales();

            if (noticias.Count > 0)
            {
                noticias = noticias.OrderBy(x => x.FechaEmpieza).Reverse().ToList();

                foreach (Noticias.Noticia noticia in noticias)
                {
                    string enlace = dominio + "/news/" + noticia.Id.ToString() + "/" + Herramientas.EnlaceAdaptador.Nombre(noticia.TituloEn) + "/";

                    string titulo = noticia.TituloEn;
                    string contenido = noticia.ContenidoEn;
                    Uri enlaceUri = null;

                    if (enlace != null)
                    {
                        enlaceUri = new Uri(enlace);
                    }

                    SyndicationItem item = new SyndicationItem(titulo, contenido, enlaceUri, noticia.Id.ToString(), noticia.FechaEmpieza);

                    if (string.IsNullOrEmpty(noticia.Imagen) == false)
                    {
                        item.ElementExtensions.Add(new XElement("image", noticia.Imagen));
                    }

                    items.Add(item);
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

        [ResponseCache(Duration = 3000)]
        [HttpGet("rss-es.xml")]
        public IActionResult GenerarEsRSS()
        {
            SyndicationFeed feed = new SyndicationFeed("pepeizq's deals", "RSS en Español de la web", new Uri(dominio), "RSSUrl", DateTime.Now)
            {
                Copyright = new TextSyndicationContent($"{DateTime.Now.Year}")
            };

            List<SyndicationItem> items = new List<SyndicationItem>();
            List<Noticias.Noticia> noticias = global::BaseDatos.Noticias.Buscar.Actuales();

            if (noticias.Count > 0)
            {
                noticias = noticias.OrderBy(x => x.FechaEmpieza).Reverse().ToList();

                foreach (Noticias.Noticia noticia in noticias)
                {
                    string enlace = dominio + "/news/" + noticia.Id.ToString() + "/" + Herramientas.EnlaceAdaptador.Nombre(noticia.TituloEs) + "/";

                    string titulo = noticia.TituloEs;
                    string contenido = noticia.ContenidoEs;
                    Uri enlaceUri = null;

                    if (enlace != null)
                    {
                        enlaceUri = new Uri(enlace);
                    }

                    SyndicationItem item = new SyndicationItem(titulo, contenido, enlaceUri, noticia.Id.ToString(), noticia.FechaEmpieza);

                    if (string.IsNullOrEmpty(noticia.Imagen) == false)
                    {
                        item.ElementExtensions.Add(new XElement("image", noticia.Imagen));
                    }

                    items.Add(item);
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

        [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Any, NoStore = false)]
        [HttpGet("news-rss")]
        public IActionResult CogerNoticiasRSS()
        {
            if (User.Identity.IsAuthenticated == true)
            {
                if (global::BaseDatos.Usuarios.Buscar.RolDios(User.FindFirst(ClaimTypes.NameIdentifier).Value) == true)
                {
                    List<Noticia> noticias = global::BaseDatos.Noticias.Buscar.Ultimas("10");

                    if (noticias.Count > 0)
                    {
                        return Ok(noticias);
                    }
                }
            }

            return Redirect("~/");
        }

        #endregion

        [ResponseCache(Duration = 3000)]
        [HttpGet("rss/{drm}/{cantidadAnalisis}")]
        public IActionResult GenerarUltimasOfertas(string drm, int cantidadAnalisis)
        {
            List<string> drmsUsar = new List<string>();

            foreach (var drm2 in Juegos.JuegoDRM2.GenerarListado())
            {
                foreach (var acepcion in drm2.Acepciones)
                {
                    if (drm.ToLower().Contains(acepcion) == true)
                    {
                        int posicion = Array.IndexOf(Enum.GetValues(typeof(Juegos.JuegoDRM)), drm2.Id);

						drmsUsar.Add(posicion.ToString());
                    }
                }
            }

            if (drmsUsar.Count > 0)
            {
				if (cantidadAnalisis < 200)
				{
					cantidadAnalisis = 199;
				}

				if (cantidadAnalisis > 10000)
				{
					cantidadAnalisis = 9999;
				}

				SyndicationFeed feed = new SyndicationFeed("pepeizq's deals", "RSS Last Deals", new Uri(dominio), "RSSUrl", DateTime.Now)
				{
					Copyright = new TextSyndicationContent($"{DateTime.Now.Year}")
				};

				List<SyndicationItem> items = new List<SyndicationItem>();

				List<Juegos.Juego> juegos = global::BaseDatos.Portada.Buscar.UltimosMinimos(50, null, drmsUsar, null, cantidadAnalisis);

				if (juegos.Count > 0)
				{
					foreach (Juegos.Juego juego in juegos)
					{
						string enlace = dominio + "/game/" + juego.Id.ToString() + "/" + Herramientas.EnlaceAdaptador.Nombre(juego.Nombre) + "/";

						string titulo = juego.Nombre;
						string contenido = juego.PrecioMinimosHistoricos[0].Descuento.ToString() + "% - " + Herramientas.Precios.Euro(juego.PrecioMinimosHistoricos[0].Precio);
						Uri enlaceUri = null;

						if (enlace != null)
						{
							enlaceUri = new Uri(enlace);
						}

						SyndicationItem item = new SyndicationItem(titulo, contenido, enlaceUri);

						if (string.IsNullOrEmpty(juego.Imagenes.Header_460x215) == false)
						{
							item.ElementExtensions.Add(new XElement("image", juego.Imagenes.Header_460x215));
						}

						items.Add(item);
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

            return null;
        }
    }
}
