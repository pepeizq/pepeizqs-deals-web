#nullable disable

using Herramientas;
using Juegos;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Net;
using System.Xml.Serialization;

namespace APIs.Gamesplanet
{
	public static class Tienda
	{
		public static Tiendas2.Tienda GenerarUk()
		{
			Tiendas2.Tienda tienda = new Tiendas2.Tienda
			{
				Id = "gamesplanetuk",
				Nombre = "Gamesplanet (UK)",
				ImagenLogo = "/imagenes/tiendas/gamesplanet_logo.png",
				Imagen300x80 = "/imagenes/tiendas/gamesplanetuk_300x80.png",
				ImagenIcono = "/imagenes/tiendas/gamesplanet_icono.ico",
				Color = "#000",
				AdminEnseñar = true,
				AdminInteractuar = true
			};

			return tienda;
		}

		public static Tiendas2.Tienda GenerarFr()
		{
			Tiendas2.Tienda tienda = new Tiendas2.Tienda
			{
				Id = "gamesplanetfr",
				Nombre = "Gamesplanet (FR)",
				ImagenLogo = "/imagenes/tiendas/gamesplanet_logo.png",
				Imagen300x80 = "/imagenes/tiendas/gamesplanetfr_300x80.png",
				ImagenIcono = "/imagenes/tiendas/gamesplanet_icono.ico",
				Color = "#000",
				AdminEnseñar = false,
				AdminInteractuar = false
			};

			return tienda;
		}

		public static Tiendas2.Tienda GenerarDe()
		{
			Tiendas2.Tienda tienda = new Tiendas2.Tienda
			{
				Id = "gamesplanetde",
				Nombre = "Gamesplanet (DE)",
				ImagenLogo = "/imagenes/tiendas/gamesplanet_logo.png",
				Imagen300x80 = "/imagenes/tiendas/gamesplanetde_300x80.png",
				ImagenIcono = "/imagenes/tiendas/gamesplanet_icono.ico",
				Color = "#000",
				AdminEnseñar = false,
				AdminInteractuar = false
			};

			return tienda;
		}

		public static Tiendas2.Tienda GenerarUs()
		{
			Tiendas2.Tienda tienda = new Tiendas2.Tienda
			{
				Id = "gamesplanetus",
				Nombre = "Gamesplanet (US)",
				ImagenLogo = "/imagenes/tiendas/gamesplanet_logo.png",
				Imagen300x80 = "/imagenes/tiendas/gamesplanetus_300x80.png",
				ImagenIcono = "/imagenes/tiendas/gamesplanet_icono.ico",
				Color = "#000",
				AdminEnseñar = false,
				AdminInteractuar = false
			};

			return tienda;
		}

		public static void BuscarOfertas(ViewDataDictionary objeto)
		{
			BaseDatos.Tiendas.Tiempo.Actualizar(Tienda.GenerarUk().Id, DateTime.Now);

			Task<string> tareauk = Decompiladores.Estandar("https://uk.gamesplanet.com/api/v1/products/feed.xml");
			tareauk.Wait();

			string htmluk = tareauk.Result;

			if (htmluk != null)
			{
				XmlSerializer xml = new XmlSerializer(typeof(GamesplanetJuegos));
				GamesplanetJuegos listaJuegos = null;

				using (TextReader lector = new StringReader(htmluk))
				{
					listaJuegos = (GamesplanetJuegos)xml.Deserialize(lector);
				}

				if (listaJuegos != null)
				{
					if (listaJuegos.Juegos != null)
					{
						if (listaJuegos.Juegos.Count > 0)
						{
							foreach (GamesplanetJuego juego in listaJuegos.Juegos)
							{
								string nombre = WebUtility.HtmlDecode(juego.Nombre);

								string enlace = juego.Enlace;

								string imagen = juego.Imagen;

								decimal precioBase = decimal.Parse(juego.PrecioBase);
								decimal precioRebajado = decimal.Parse(juego.PrecioRebajado);

								int descuento = Calculadora.SacarDescuento(precioBase, precioRebajado);

								if (descuento > 0)
								{
									JuegoDRM drm = JuegoDRM2.Traducir(juego.DRM, GenerarUk().Id);

									JuegoPrecio oferta = new JuegoPrecio
									{
										Nombre = nombre,
										Enlace = enlace,
										Imagen = imagen,
										Moneda = JuegoMoneda.Libra,
										Precio = precioRebajado,
										Descuento = descuento,
										Tienda = GenerarUk().Id,
										DRM = drm,
										FechaDetectado = DateTime.Now
									};

									BaseDatos.Tiendas.Comprobar.Resto(oferta, objeto);
								}
							}
						}

						objeto["Mensaje"] = objeto["Mensaje"] + "GamersGate: " + listaJuegos.Juegos.Count.ToString() + " juegos detectados" + Environment.NewLine;
					}
				}
			}
		}
	}

	[XmlRoot("products")]
	public class GamesplanetJuegos
	{
		[XmlElement("product")]
		public List<GamesplanetJuego> Juegos { get; set; }
	}

	public class GamesplanetJuego
	{
		[XmlElement("name")]
		public string Nombre { get; set; }

		[XmlElement("uid")]
		public string Id { get; set; }

		[XmlElement("link")]
		public string Enlace { get; set; }

		[XmlElement("price")]
		public string PrecioRebajado { get; set; }

		[XmlElement("price_base")]
		public string PrecioBase { get; set; }

		[XmlElement("teaser620")]
		public string Imagen { get; set; }

		[XmlElement("delivery_type")]
		public string DRM { get; set; }

		[XmlElement("steam_id")]
		public string SteamId { get; set; }

		[XmlElement("country_whitelist")]
		public string Paises { get; set; }
	}
}
