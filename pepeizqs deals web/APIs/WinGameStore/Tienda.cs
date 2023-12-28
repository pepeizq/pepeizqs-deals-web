#nullable disable

using Herramientas;
using Juegos;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Net;

namespace APIs.WinGameStore
{
	public static class Tienda
	{
		public static Tiendas2.Tienda Generar()
		{
			Tiendas2.Tienda tienda = new Tiendas2.Tienda
			{
				Id = "wingamestore",
				Nombre = "WinGameStore",
				ImagenLogo = "/imagenes/tiendas/wingamestore_logo.webp",
				Imagen300x80 = "/imagenes/tiendas/wingamestore_300x80.webp",
				ImagenIcono = "/imagenes/tiendas/wingamestore_icono.webp",
				Color = "#265c92",
				AdminEnseñar = true,
				AdminInteractuar = true
			};

			return tienda;
		}

		public static string Referido(string enlace)
		{
			return enlace + "?ars=pepeizqdeals";
		}

		public static async Task BuscarOfertas(SqlConnection conexion, ViewDataDictionary objeto = null)
		{
			string html = await Decompiladores.Estandar("https://www.macgamestore.com/affiliate/feeds/p_C1B2A3.json");

			if (html != null)
			{
				List<WinGameStoreJuego> juegos = JsonConvert.DeserializeObject<List<WinGameStoreJuego>>(html);

				if (juegos != null)
				{
					if (juegos.Count > 0)
					{
						int juegos2 = 0;

						foreach (WinGameStoreJuego juego in juegos)
						{
							string nombre = WebUtility.HtmlDecode(juego.Nombre);

							string enlace = juego.Enlace;

							enlace = enlace.Replace("?ars=pepeizqdeals", null);

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
									Moneda = JuegoMoneda.Dolar,
									Precio = precioRebajado,
									Descuento = descuento,
									Tienda = Generar().Id,
									DRM = drm,
									FechaDetectado = DateTime.Now,
									FechaActualizacion = DateTime.Now
								};

								BaseDatos.Tiendas.Comprobar.Resto(oferta, objeto, conexion);

								juegos2 += 1;
								BaseDatos.Tiendas.Admin.Actualizar(Tienda.Generar().Id, DateTime.Now, juegos2.ToString() + " ofertas detectadas", conexion);
							}
						}
					}
				}
			}
		}
	}

	public class WinGameStoreJuego
	{
		[JsonProperty("title")]
		public string Nombre { get; set; }

		[JsonProperty("url")]
		public string Enlace { get; set; }

		[JsonProperty("current_price")]
		public string PrecioRebajado { get; set; }

		[JsonProperty("retail_price")]
		public string PrecioBase { get; set; }

		[JsonProperty("drm")]
		public string DRM { get; set; }

		[JsonProperty("badge")]
		public string Imagen { get; set; }
	}
}
