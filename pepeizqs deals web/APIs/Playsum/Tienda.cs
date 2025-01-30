//https://docs.google.com/document/d/1LCOqVOzMI67E-bxA8rDTaHLhqZ4xyAP80xkgzFICR8w/edit?tab=t.0

#nullable disable

using Herramientas;
using Juegos;
using Microsoft.Data.SqlClient;
using System.Net;
using System.Xml.Serialization;

namespace APIs.Playsum
{
	public static class Tienda
	{
		public static Tiendas2.Tienda Generar()
		{
			Tiendas2.Tienda tienda = new Tiendas2.Tienda
			{
				Id = "playsum",
				Nombre = "Playsum",
				ImagenLogo = "/imagenes/tiendas/playsum_logo.webp",
				Imagen300x80 = "/imagenes/tiendas/playsum_300x80.webp",
				ImagenIcono = "/imagenes/tiendas/playsum_icono.ico",
				Color = "#a91aff",
				AdminEnseñar = true,
				AdminInteractuar = true
			};

			return tienda;
		}

		public static string Referido(string enlace)
		{
			return enlace + "?plysm_ref_id=YiEguGaNJjlnglvi5JTNrVZi1z5OUoli";
		}

		public static async Task BuscarOfertas(SqlConnection conexion, IDecompiladores decompilador)
		{
			BaseDatos.Admin.Actualizar.Tiendas(Generar().Id, DateTime.Now, 0, conexion);

			int juegos2 = 0;

			string html = await Decompiladores.Estandar("https://api.playsum.live/v1/shop/products/rss");

			if (string.IsNullOrEmpty(html) == false)
			{
				PlaysumJuegos listaJuegos = new PlaysumJuegos();
				XmlSerializer xml = new XmlSerializer(typeof(PlaysumJuegos));

				using (StringReader lector = new StringReader(html))
				{
					listaJuegos = (PlaysumJuegos)xml.Deserialize(lector);
					lector.Dispose();
				}

				if (listaJuegos != null)
				{
					foreach (PlaysumJuego juego in listaJuegos.Canal.Juegos)
					{
						if (juego.Moneda == "EUR")
						{
							if (string.IsNullOrEmpty(juego.PrecioBase) == false && string.IsNullOrEmpty(juego.PrecioRebajado) == false)
							{
								decimal precioBase = decimal.Parse(juego.PrecioBase);
								decimal precioRebajado = decimal.Parse(juego.PrecioRebajado);

								int descuento = Calculadora.SacarDescuento(precioBase, precioRebajado);

								if (descuento > 0)
								{
									string nombre = WebUtility.HtmlDecode(juego.Nombre);

									string enlace = juego.Enlace;

									JuegoDRM drm = JuegoDRM2.Traducir(juego.DRM, Generar().Id);

									JuegoPrecio oferta = new JuegoPrecio
									{
										Nombre = nombre,
										Enlace = enlace,
										Moneda = JuegoMoneda.Euro,
										Precio = precioRebajado,
										Descuento = descuento,
										Tienda = Generar().Id,
										DRM = drm,
										FechaDetectado = DateTime.Now,
										FechaActualizacion = DateTime.Now,
										CodigoDescuento = 10,
										CodigoTexto = "PEPEIZQDEALS"
									};

									try
									{
										BaseDatos.Tiendas.Comprobar.Resto(oferta, conexion);
									}
									catch (Exception ex)
									{
										BaseDatos.Errores.Insertar.Mensaje(Generar().Id, ex, conexion);
									}

									juegos2 += 1;

									try
									{
										BaseDatos.Admin.Actualizar.Tiendas(Generar().Id, DateTime.Now, juegos2, conexion);
									}
									catch (Exception ex)
									{
										BaseDatos.Errores.Insertar.Mensaje(Generar().Id, ex, conexion);
									}
								}
							}
						}
					}
				}
			}
		}
	}

	[XmlRoot("rss")]
	public class PlaysumJuegos
	{
		[XmlElement("channel")]
		public PlaysumCanal Canal { get; set; }
	}

	public class PlaysumCanal
	{
		[XmlElement("item")]
		public List<PlaysumJuego> Juegos { get; set; }
	}

	public class PlaysumJuego
	{
		[XmlElement("title")]
		public string Nombre { get; set; }

		[XmlElement("link")]
		public string Enlace { get; set; }

		[XmlElement("sku")]
		public string Id { get; set; }

		[XmlElement("discountPrice")]
		public string PrecioRebajado { get; set; }

		[XmlElement("originalPrice")]
		public string PrecioBase { get; set; }

		[XmlElement("currency")]
		public string Moneda { get; set; }

		[XmlElement("keyProvider")]
		public string DRM { get; set; }

		[XmlElement("blacklistedCountries")]
		public string PaisesBloqueados { get; set; }
	}
}
