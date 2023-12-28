//https://www.indiegala.com/games/ajax/on-sale/percentage-off/1
//https://www.indiegala.com/store_games_rss?&sale=true&page=1

#nullable disable

using Herramientas;
using Juegos;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Data.SqlClient;
using System.Net;
using System.Xml;
using System.Xml.Serialization;

namespace APIs.IndieGala
{
	public static class Tienda
	{
		public static Tiendas2.Tienda Generar()
		{
			Tiendas2.Tienda tienda = new Tiendas2.Tienda
			{
				Id = "indiegala",
				Nombre = "IndieGala",
				ImagenLogo = "/imagenes/tiendas/indiegala_logo.webp",
				Imagen300x80 = "/imagenes/tiendas/indiegala_300x80.webp",
				ImagenIcono = "/imagenes/tiendas/indiegala_icono.ico",
				Color = "#ffccd4",
				AdminEnseñar = true,
				AdminInteractuar = true
			};

			return tienda;
		}

		public static string Referido(string enlace)
		{
			return enlace + "?ref=pepeizq";
		}

		public static async Task BuscarOfertas(SqlConnection conexion, ViewDataDictionary objeto = null)
		{
			int i = 1;
			while (i < 10)
			{
				await Task.Delay(1000);
				string html = Decompiladores.GZipFormato("https://www.indiegala.com/store_games_rss?&sale=true&page=" + i.ToString());

				if (html != null)
				{
					if (html == "None")
					{
						break;
					}

					XmlSerializer xml = new XmlSerializer(typeof(IndieGalaRSS));
					IndieGalaRSS listaJuegos = null;

					using (TextReader lector = new StringReader(html))
					{
						listaJuegos = (IndieGalaRSS)xml.Deserialize(lector);
					}

					if (listaJuegos != null)
					{
						if (listaJuegos.Canal.Buscador.Juegos != null)
						{
							if (listaJuegos.Canal.Buscador.Juegos.Count > 0)
							{
								int juegos2 = 0;

								foreach (IndieGalaJuego juego in listaJuegos.Canal.Buscador.Juegos)
								{
									string nombre = WebUtility.HtmlDecode(juego.Nombre);

									string enlace = juego.Enlace;

									string imagen = juego.Imagen;

									if (imagen.Contains("https://www.indiegalacdn.com/") == false)
									{
										imagen = "https://www.indiegalacdn.com/" + imagen;
									}

									decimal precioBase = decimal.Parse(juego.PrecioBase);
									decimal precioRebajado = decimal.Parse(juego.PrecioRebajado);
									int descuento = Convert.ToInt32(Math.Round(decimal.Parse(juego.Descuento), MidpointRounding.AwayFromZero));

									if (descuento > 0)
									{
										JuegoDRM drm = JuegoDRM2.Traducir(juego.DRM, Generar().Id);

										if (nombre.Contains("(Epic)") == true)
										{
											drm = JuegoDRM.Epic;
										}
										else if (ComprobarEpicFalsos(enlace) == true)
										{
											drm = JuegoDRM.Epic;
										}
										else if (enlace.Contains("_epic") == true)
										{
											drm = JuegoDRM.Epic;
										}

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
											FechaActualizacion = DateTime.Now
										};

										if (juego.Fecha != null)
										{
											DateTime fechaTermina = DateTime.Parse(juego.Fecha);
											oferta.FechaTermina = fechaTermina;
										}

										BaseDatos.Tiendas.Comprobar.Resto(oferta, objeto, conexion);

										juegos2 += 1;
										BaseDatos.Tiendas.Admin.Actualizar(Tienda.Generar().Id, DateTime.Now, juegos2.ToString() + " ofertas detectadas", conexion);
									}
								}
							}
						}
					}
				}

				i += 1;
			}
		}

		private static bool ComprobarEpicFalsos(string enlaceJuego)
		{
			List<string> enlaces = [
				"1286680_pass",
				"1286680_pre",
				"1286680_deluxe_pre",
				"1496590",
				"1496590_deluxe",
				"578650",
				"629820",
				"9b06280a",
				"f69bbded"
			];

			foreach (var enlace in enlaces)
			{
				if (enlaceJuego.Contains("/" + enlace) == true)
				{
					return true;
				}
			}

			return false;
		}
	}

	#region Clases

	[XmlRoot("rss")]
	public class IndieGalaRSS
	{
		[XmlElement("channel")]
		public IndieGalaCanal Canal { get; set; }
	}

	public class IndieGalaCanal
	{
		[XmlElement("totalPages")]
		public int TotalPaginas { get; set; }

		[XmlElement("totalGames")]
		public int TotalJuegos { get; set; }

		[XmlElement("browse")]
		public IndieGalaJuegos Buscador { get; set; }
	}

	public class IndieGalaJuegos
	{
		[XmlElement("item")]
		public List<IndieGalaJuego> Juegos { get; set; }
	}

	public class IndieGalaJuego
	{
		[XmlElement("title")]
		public string Nombre { get; set; }

		[XmlElement("link")]
		public string Enlace { get; set; }

		[XmlElement("discountPercentEUR")]
		public string Descuento { get; set; }

		[XmlElement("discountPriceEUR")]
		public string PrecioRebajado { get; set; }

		[XmlElement("priceEUR")]
		public string PrecioBase { get; set; }

		[XmlElement("boximg")]
		public string Imagen { get; set; }

		[XmlElement("drminfo")]
		public string DRM { get; set; }

		[XmlElement("discountEnd")]
		public string Fecha { get; set; }
	}

	#endregion
}
