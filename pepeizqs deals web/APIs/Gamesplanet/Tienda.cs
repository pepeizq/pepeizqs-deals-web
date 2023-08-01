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
				AdminEnseñar = true,
				AdminInteractuar = true
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
				AdminEnseñar = true,
				AdminInteractuar = true
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
				AdminEnseñar = true,
				AdminInteractuar = true
			};

			return tienda;
		}

		public static void BuscarOfertasUk(ViewDataDictionary objeto = null)
		{
			Task<string> tareauk = Decompiladores.Estandar("https://uk.gamesplanet.com/api/v1/products/feed.xml");
			tareauk.Wait();

			string htmluk = tareauk.Result;
            tareauk.Dispose();

			if (htmluk != null)
			{
				XmlSerializer xml = new XmlSerializer(typeof(GamesplanetJuegos));
				GamesplanetJuegos listaJuegos = null;
                TextReader lector = new StringReader(htmluk);

                using (lector)
				{
					listaJuegos = (GamesplanetJuegos)xml.Deserialize(lector);
				}

                lector.Close();

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

                        BaseDatos.Tiendas.Admin.Actualizar(Tienda.GenerarUk().Id, DateTime.Now, listaJuegos.Juegos.Count.ToString() + " juegos detectados");
					}
				}
			}
        }

        public static void BuscarOfertasFr(ViewDataDictionary objeto = null)
        {
            Task<string> tareafr = Decompiladores.Estandar("https://fr.gamesplanet.com/api/v1/products/feed.xml");
            tareafr.Wait();

            string htmlfr = tareafr.Result;
            tareafr.Dispose();

            if (htmlfr != null)
            {
                XmlSerializer xml = new XmlSerializer(typeof(GamesplanetJuegos));
                GamesplanetJuegos listaJuegos = null;
                TextReader lector = new StringReader(htmlfr);

                using (lector)
                {
                    listaJuegos = (GamesplanetJuegos)xml.Deserialize(lector);
                }

                lector.Close();

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
                                    JuegoDRM drm = JuegoDRM2.Traducir(juego.DRM, GenerarFr().Id);

                                    JuegoPrecio oferta = new JuegoPrecio
                                    {
                                        Nombre = nombre,
                                        Enlace = enlace,
                                        Imagen = imagen,
                                        Moneda = JuegoMoneda.Euro,
                                        Precio = precioRebajado,
                                        Descuento = descuento,
                                        Tienda = GenerarFr().Id,
                                        DRM = drm,
                                        FechaDetectado = DateTime.Now
                                    };

                                    BaseDatos.Tiendas.Comprobar.Resto(oferta, objeto);
                                }
                            }
                        }

                        BaseDatos.Tiendas.Admin.Actualizar(Tienda.GenerarFr().Id, DateTime.Now, listaJuegos.Juegos.Count.ToString() + " juegos detectados");
                    }
                }
            }

        }

		public static void BuscarOfertasDe(ViewDataDictionary objeto = null)
		{
            Task<string> tareade = Decompiladores.Estandar("https://de.gamesplanet.com/api/v1/products/feed.xml");
            tareade.Wait();

            string htmlde = tareade.Result;
            tareade.Dispose();

            if (htmlde != null)
            {
                XmlSerializer xml = new XmlSerializer(typeof(GamesplanetJuegos));
                GamesplanetJuegos listaJuegos = null;
                TextReader lector = new StringReader(htmlde);

                using (lector)
                {
                    listaJuegos = (GamesplanetJuegos)xml.Deserialize(lector);
                }

                lector.Close();

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
                                    JuegoDRM drm = JuegoDRM2.Traducir(juego.DRM, GenerarDe().Id);

                                    JuegoPrecio oferta = new JuegoPrecio
                                    {
                                        Nombre = nombre,
                                        Enlace = enlace,
                                        Imagen = imagen,
                                        Moneda = JuegoMoneda.Euro,
                                        Precio = precioRebajado,
                                        Descuento = descuento,
                                        Tienda = GenerarDe().Id,
                                        DRM = drm,
                                        FechaDetectado = DateTime.Now
                                    };

                                    BaseDatos.Tiendas.Comprobar.Resto(oferta, objeto);
                                }
                            }
                        }

                        BaseDatos.Tiendas.Admin.Actualizar(Tienda.GenerarDe().Id, DateTime.Now, listaJuegos.Juegos.Count.ToString() + " juegos detectados");
                    }
                }
            }
        }

		public static void BuscarOfertasUs(ViewDataDictionary objeto = null)
		{
            Task<string> tareaus = Decompiladores.Estandar("https://us.gamesplanet.com/api/v1/products/feed.xml");
            tareaus.Wait();

            string htmlus = tareaus.Result;
            tareaus.Dispose();

            if (htmlus != null)
            {
                XmlSerializer xml = new XmlSerializer(typeof(GamesplanetJuegos));
                GamesplanetJuegos listaJuegos = null;
                TextReader lector = new StringReader(htmlus);

                using (lector)
                {
                    listaJuegos = (GamesplanetJuegos)xml.Deserialize(lector);
                }

                lector.Close();

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
                                    JuegoDRM drm = JuegoDRM2.Traducir(juego.DRM, GenerarUs().Id);

                                    JuegoPrecio oferta = new JuegoPrecio
                                    {
                                        Nombre = nombre,
                                        Enlace = enlace,
                                        Imagen = imagen,
                                        Moneda = JuegoMoneda.Dolar,
                                        Precio = precioRebajado,
                                        Descuento = descuento,
                                        Tienda = GenerarUs().Id,
                                        DRM = drm,
                                        FechaDetectado = DateTime.Now
                                    };

                                    BaseDatos.Tiendas.Comprobar.Resto(oferta, objeto);
                                }
                            }
                        }

                        BaseDatos.Tiendas.Admin.Actualizar(Tienda.GenerarUs().Id, DateTime.Now, listaJuegos.Juegos.Count.ToString() + " juegos detectados");
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
