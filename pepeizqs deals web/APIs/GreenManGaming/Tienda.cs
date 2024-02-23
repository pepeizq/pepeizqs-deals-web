#nullable disable

using Herramientas;
using Juegos;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Data.SqlClient;
using System.Net;
using System.Xml.Serialization;

namespace APIs.GreenManGaming
{
	public static class Tienda
	{
		public static Tiendas2.Tienda Generar()
		{
			Tiendas2.Tienda tienda = new Tiendas2.Tienda
			{
				Id = "greenmangaming",
				Nombre = "Green Man Gaming",
				ImagenLogo = "/imagenes/tiendas/greenmangaming_logo.webp",
				Imagen300x80 = "/imagenes/tiendas/greenmangaming_300x80.webp",
				ImagenIcono = "/imagenes/tiendas/greenmangaming_icono.ico",
				Color = "#97ff9a",
				AdminEnseñar = true,
				AdminInteractuar = true
			};

			return tienda;
		}

		public static string Referido(string enlace)
		{
			return enlace + "?tap_a=1964-996bbb&tap_s=608263-a851ee";
		}

		public static async Task BuscarOfertas(SqlConnection conexion, IDecompiladores decompilador, ViewDataDictionary objeto = null)
		{
			BaseDatos.Tiendas.Admin.Actualizar(Tienda.Generar().Id, DateTime.Now, "0 ofertas detectadas", conexion);

			string html = await decompilador.Estandar("https://api.greenmangaming.com/api/productfeed/prices/current?cc=es&cur=eur&lang=en");

			if (html != null)
			{
				XmlSerializer xml = new XmlSerializer(typeof(GreenManGamingJuegos));
				GreenManGamingJuegos listaJuegos = null;

				using (TextReader lector = new StringReader(html))
				{
					listaJuegos = (GreenManGamingJuegos)xml.Deserialize(lector);
				}

				if (listaJuegos != null)
				{
					if (listaJuegos.Juegos != null)
					{
						if (listaJuegos.Juegos.Count > 0)
						{
							int juegos2 = 0;

							foreach (GreenManGamingJuego juego in listaJuegos.Juegos)
							{
								string nombre = WebUtility.HtmlDecode(juego.Nombre);

								string enlace = juego.Enlace;

								string imagen = juego.Imagen;

								decimal precioBase = decimal.Parse(juego.PrecioBase);
								decimal precioRebajado = decimal.Parse(juego.PrecioRebajado);

								int descuento = Calculadora.SacarDescuento(precioBase, precioRebajado);

								if (descuento > 0)
								{
									JuegoDRM drm = JuegoDRM2.Traducir(juego.DRM, Generar().Id);

									JuegoPrecio oferta = new JuegoPrecio
									{
										Nombre = nombre,
										Enlace = enlace,
										Imagen = imagen,
										Moneda = JuegoMoneda.Euro,
										Precio = precioRebajado,
										Descuento = descuento,
										Tienda = Generar().Id,
										DRM = drm,
										FechaDetectado = DateTime.Now,
										FechaActualizacion = DateTime.Now,
										SteamID = juego.SteamId
									};

									try
									{
										BaseDatos.Tiendas.Comprobar.Resto(oferta, objeto, conexion);
									}
									catch (Exception ex)
									{
                                        BaseDatos.Errores.Insertar.Ejecutar(Tienda.Generar().Id, ex);
                                    }

									juegos2 += 1;

									try
									{
										BaseDatos.Tiendas.Admin.Actualizar(Tienda.Generar().Id, DateTime.Now, juegos2.ToString() + " ofertas detectadas", conexion);
									}
									catch (Exception ex)
									{
                                        BaseDatos.Errores.Insertar.Ejecutar(Tienda.Generar().Id, ex);
                                    }
								}
							}
						}
					}
				}
			}
		}
	}

	[XmlRoot("products")]
	public class GreenManGamingJuegos
	{
		[XmlElement("product")]
		public List<GreenManGamingJuego> Juegos { get; set; }
	}

	public class GreenManGamingJuego
	{
		[XmlElement("product_name")]
		public string Nombre { get; set; }

		[XmlElement("deep_link")]
		public string Enlace { get; set; }

		[XmlElement("image_url")]
		public string Imagen { get; set; }

		[XmlElement("price")]
		public string PrecioRebajado { get; set; }

		[XmlElement("rrp_price")]
		public string PrecioBase { get; set; }

		[XmlElement("drm")]
		public string DRM { get; set; }

		[XmlElement("steamapp_id")]
		public string SteamId { get; set; }
	}
}
