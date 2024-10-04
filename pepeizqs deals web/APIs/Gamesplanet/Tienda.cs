#nullable disable

using Herramientas;
using Juegos;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Data.SqlClient;
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
				ImagenLogo = "/imagenes/tiendas/gamesplanet_logo.webp",
				Imagen300x80 = "/imagenes/tiendas/gamesplanetuk_300x80.webp",
				ImagenIcono = "/imagenes/tiendas/gamesplanet_icono.webp",
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
				ImagenLogo = "/imagenes/tiendas/gamesplanet_logo.webp",
				Imagen300x80 = "/imagenes/tiendas/gamesplanetfr_300x80.webp",
				ImagenIcono = "/imagenes/tiendas/gamesplanet_icono.webp",
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
				ImagenLogo = "/imagenes/tiendas/gamesplanet_logo.webp",
				Imagen300x80 = "/imagenes/tiendas/gamesplanetde_300x80.webp",
				ImagenIcono = "/imagenes/tiendas/gamesplanet_icono.webp",
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
				ImagenLogo = "/imagenes/tiendas/gamesplanet_logo.webp",
				Imagen300x80 = "/imagenes/tiendas/gamesplanetus_300x80.webp",
				ImagenIcono = "/imagenes/tiendas/gamesplanet_icono.webp",
				Color = "#000",
				AdminEnseñar = true,
				AdminInteractuar = true
			};

			return tienda;
		}

		public static string Referido(string enlace)
		{
			return enlace + "?ref=pepeizq";
		}

		public static async Task BuscarOfertasUk(SqlConnection conexion, IDecompiladores decompilador, ViewDataDictionary objeto = null)
		{
			BaseDatos.Tiendas.Admin.Actualizar(Tienda.GenerarUk().Id, DateTime.Now, "0 ofertas detectadas", conexion);

			List<JuegoPrecio> ofertas = new List<JuegoPrecio>();

			string htmluk = await Decompiladores.Estandar("https://uk.gamesplanet.com/api/v1/products/feed.xml");

			if (string.IsNullOrEmpty(htmluk) == false)
			{
                GamesplanetJuegos listaJuegos = new GamesplanetJuegos();

                XmlSerializer xml = new XmlSerializer(typeof(GamesplanetJuegos));
                listaJuegos = (GamesplanetJuegos)xml.Deserialize(new StringReader(htmluk));

                if (listaJuegos != null)
                {
                    if (listaJuegos.Juegos != null)
                    {
                        if (listaJuegos.Juegos.Count > 0)
                        {
                            foreach (GamesplanetJuego juego in listaJuegos.Juegos)
                            {
                                decimal precioBase = decimal.Parse(juego.PrecioBase);
                                decimal precioRebajado = decimal.Parse(juego.PrecioRebajado);

                                int descuento = Calculadora.SacarDescuento(precioBase, precioRebajado);

                                if (descuento > 0)
                                {
                                    string nombre = WebUtility.HtmlDecode(juego.Nombre);

                                    string enlace = juego.Enlace;

                                    string imagen = juego.Imagen;

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
                                        FechaDetectado = DateTime.Now,
                                        FechaActualizacion = DateTime.Now
                                    };

                                    ofertas.Add(oferta);
                                }
                            }
                        }
                    }
                }
            }

			int juegos2 = 0;

			if (ofertas.Count > 0)
			{
				foreach (var oferta in ofertas)
				{
					try
					{
						BaseDatos.Tiendas.Comprobar.Resto(oferta, conexion);
					}
					catch (Exception ex)
					{
						BaseDatos.Errores.Insertar.Mensaje(Tienda.GenerarUk().Id, ex, conexion);
					}

					juegos2 += 1;

					try
					{
						BaseDatos.Tiendas.Admin.Actualizar(Tienda.GenerarUk().Id, DateTime.Now, juegos2.ToString() + " ofertas detectadas", conexion);
					}
					catch (Exception ex)
					{
						BaseDatos.Errores.Insertar.Mensaje(Tienda.GenerarUk().Id, ex, conexion);
					}
				}
			}
		}

        public static async Task BuscarOfertasFr(SqlConnection conexion, IDecompiladores decompilador, ViewDataDictionary objeto = null)
        {
			BaseDatos.Tiendas.Admin.Actualizar(Tienda.GenerarFr().Id, DateTime.Now, "0 ofertas detectadas", conexion);

			List<JuegoPrecio> ofertas = new List<JuegoPrecio>();

			string htmlfr = await Decompiladores.Estandar("https://fr.gamesplanet.com/api/v1/products/feed.xml");

			if (string.IsNullOrEmpty(htmlfr) == false)
			{
                GamesplanetJuegos listaJuegos = new GamesplanetJuegos();

                XmlSerializer xml = new XmlSerializer(typeof(GamesplanetJuegos));
                listaJuegos = (GamesplanetJuegos)xml.Deserialize(new StringReader(htmlfr));

                if (listaJuegos != null)
                {
                    if (listaJuegos.Juegos != null)
                    {
                        if (listaJuegos.Juegos.Count > 0)
                        {
                            foreach (GamesplanetJuego juego in listaJuegos.Juegos)
                            {
                                decimal precioBase = decimal.Parse(juego.PrecioBase);
                                decimal precioRebajado = decimal.Parse(juego.PrecioRebajado);

                                int descuento = Calculadora.SacarDescuento(precioBase, precioRebajado);

                                if (descuento > 0)
                                {
                                    string nombre = WebUtility.HtmlDecode(juego.Nombre);

                                    string enlace = juego.Enlace;

                                    string imagen = juego.Imagen;

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
                                        FechaDetectado = DateTime.Now,
                                        FechaActualizacion = DateTime.Now
                                    };

                                    ofertas.Add(oferta);
                                }
                            }
                        }
                    }
                }
            }

			int juegos2 = 0;

			if (ofertas.Count > 0)
			{
				foreach (var oferta in ofertas)
				{
					try
					{
						BaseDatos.Tiendas.Comprobar.Resto(oferta, conexion);
					}
					catch (Exception ex)
					{
						BaseDatos.Errores.Insertar.Mensaje(Tienda.GenerarFr().Id, ex, conexion);
					}

					juegos2 += 1;

					try
					{
						BaseDatos.Tiendas.Admin.Actualizar(Tienda.GenerarFr().Id, DateTime.Now, juegos2.ToString() + " ofertas detectadas", conexion);
					}
					catch (Exception ex)
					{
						BaseDatos.Errores.Insertar.Mensaje(Tienda.GenerarFr().Id, ex, conexion);
					}
				}
			}
		}

		public static async Task BuscarOfertasDe(SqlConnection conexion, IDecompiladores decompilador, ViewDataDictionary objeto = null)
		{
			BaseDatos.Tiendas.Admin.Actualizar(Tienda.GenerarDe().Id, DateTime.Now, "0 ofertas detectadas", conexion);

			List<JuegoPrecio> ofertas = new List<JuegoPrecio>();

			string htmlde = await Decompiladores.Estandar("https://de.gamesplanet.com/api/v1/products/feed.xml");

			if (string.IsNullOrEmpty(htmlde) == false)
			{
                GamesplanetJuegos listaJuegos = new GamesplanetJuegos();

                XmlSerializer xml = new XmlSerializer(typeof(GamesplanetJuegos));
                listaJuegos = (GamesplanetJuegos)xml.Deserialize(new StringReader(htmlde));

                if (listaJuegos != null)
                {
                    if (listaJuegos.Juegos != null)
                    {
                        if (listaJuegos.Juegos.Count > 0)
                        {
                            foreach (GamesplanetJuego juego in listaJuegos.Juegos)
                            {
                                decimal precioBase = decimal.Parse(juego.PrecioBase);
                                decimal precioRebajado = decimal.Parse(juego.PrecioRebajado);

                                int descuento = Calculadora.SacarDescuento(precioBase, precioRebajado);

                                if (descuento > 0)
                                {
                                    string nombre = WebUtility.HtmlDecode(juego.Nombre);

                                    string enlace = juego.Enlace;

                                    string imagen = juego.Imagen;

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
                                        FechaDetectado = DateTime.Now,
                                        FechaActualizacion = DateTime.Now
                                    };

                                    ofertas.Add(oferta);
                                }
                            }
                        }
                    }
                }
            }

			int juegos2 = 0;

			if (ofertas.Count > 0)
			{
				foreach (var oferta in ofertas)
				{
					try
					{
						BaseDatos.Tiendas.Comprobar.Resto(oferta, conexion);
					}
					catch (Exception ex)
					{
						BaseDatos.Errores.Insertar.Mensaje(Tienda.GenerarDe().Id, ex, conexion);
					}

					juegos2 += 1;

					try
					{
						BaseDatos.Tiendas.Admin.Actualizar(Tienda.GenerarDe().Id, DateTime.Now, juegos2.ToString() + " ofertas detectadas", conexion);
					}
					catch (Exception ex)
					{
						BaseDatos.Errores.Insertar.Mensaje(Tienda.GenerarDe().Id, ex, conexion);
					}
				}
			}
		}

		public static async Task BuscarOfertasUs(SqlConnection conexion, IDecompiladores decompilador, ViewDataDictionary objeto = null)
		{
			BaseDatos.Tiendas.Admin.Actualizar(Tienda.GenerarUs().Id, DateTime.Now, "0 ofertas detectadas", conexion);

			List<JuegoPrecio> ofertas = new List<JuegoPrecio>();

			string htmlus = await Decompiladores.Estandar("https://us.gamesplanet.com/api/v1/products/feed.xml");

			if (string.IsNullOrEmpty(htmlus) == false)
			{
                GamesplanetJuegos listaJuegos = new GamesplanetJuegos();

                XmlSerializer xml = new XmlSerializer(typeof(GamesplanetJuegos));
                listaJuegos = (GamesplanetJuegos)xml.Deserialize(new StringReader(htmlus));

                if (listaJuegos != null)
                {
                    if (listaJuegos.Juegos != null)
                    {
                        if (listaJuegos.Juegos.Count > 0)
                        {
                            foreach (GamesplanetJuego juego in listaJuegos.Juegos)
                            {
                                decimal precioBase = decimal.Parse(juego.PrecioBase);
                                decimal precioRebajado = decimal.Parse(juego.PrecioRebajado);

                                int descuento = Calculadora.SacarDescuento(precioBase, precioRebajado);

                                if (descuento > 0)
                                {
                                    string nombre = WebUtility.HtmlDecode(juego.Nombre);

                                    string enlace = juego.Enlace;

                                    string imagen = juego.Imagen;

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
                                        FechaDetectado = DateTime.Now,
                                        FechaActualizacion = DateTime.Now
                                    };

                                    ofertas.Add(oferta);
                                }
                            }
                        }
                    }
                }
            }

			int juegos2 = 0;

			if (ofertas.Count > 0)
			{
				foreach (var oferta in ofertas)
				{
					try
					{
						BaseDatos.Tiendas.Comprobar.Resto(oferta, conexion);
					}
					catch (Exception ex)
					{
						BaseDatos.Errores.Insertar.Mensaje(Tienda.GenerarUs().Id, ex, conexion);
					}

					juegos2 += 1;

					try
					{
						BaseDatos.Tiendas.Admin.Actualizar(Tienda.GenerarUs().Id, DateTime.Now, juegos2.ToString() + " ofertas detectadas", conexion);
					}
					catch (Exception ex)
					{
						BaseDatos.Errores.Insertar.Mensaje(Tienda.GenerarUs().Id, ex, conexion);
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
