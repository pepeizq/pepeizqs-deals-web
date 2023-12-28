//https://partner.dlgamer.com/secure/affiliation

//https://static.dlgamer.com/feeds/general_feed_en.json
//https://static.dlgamer.com/feeds/general_feed_us.json
//https://static.dlgamer.com/feeds/general_feed_eu.json

#nullable disable

using Herramientas;
using Juegos;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Net;

namespace APIs.DLGamer
{
	public class Tienda
	{
		public static Tiendas2.Tienda Generar()
		{
			Tiendas2.Tienda tienda = new Tiendas2.Tienda
			{
				Id = "dlgamer",
				Nombre = "DLGamer",
				ImagenLogo = "/imagenes/tiendas/dlgamer_logo.webp",
				Imagen300x80 = "/imagenes/tiendas/dlgamer_300x80.webp",
				ImagenIcono = "/imagenes/tiendas/dlgamer_icono.webp",
				Color = "#b9aa21",
				AdminEnseñar = true,
				AdminInteractuar = true
			};

			return tienda;
		}

		public static string Referido(string enlace)
		{
			return enlace + "?affil=pepeizqdeals";
		}

		public static async Task BuscarOfertas(SqlConnection conexion, ViewDataDictionary objeto = null)
		{
			int juegos2 = 0;

			string html = await Decompiladores.Estandar("https://static.dlgamer.com/feeds/general_feed_eu.json");

			if (html != null)
			{
				DLGamerJuegos basedatos = JsonConvert.DeserializeObject<DLGamerJuegos>(html);

				if (basedatos != null)
				{
					foreach (var juegoDL in basedatos.Datos)
					{
						decimal precioRebajado = decimal.Parse(juegoDL.Value.PrecioRebajado);
						decimal precioBase = decimal.Parse(juegoDL.Value.PrecioBase);

						int descuento = Calculadora.SacarDescuento(precioBase, precioRebajado);

						if (descuento > 0)
						{
							string nombre = WebUtility.HtmlDecode(juegoDL.Value.Nombre);

							string enlace = juegoDL.Value.Enlace;

							string imagen = juegoDL.Value.Imagen;

							JuegoDRM drm = JuegoDRM2.Traducir(juegoDL.Value.DRM, Generar().Id);

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

							BaseDatos.Tiendas.Comprobar.Resto(oferta, objeto, conexion);

							juegos2 += 1;
							BaseDatos.Tiendas.Admin.Actualizar(Tienda.Generar().Id, DateTime.Now, juegos2.ToString() + " ofertas detectadas", conexion);
						}
					}
				}
			}
		}
	}

	#region Clases

	public class DLGamerJuegos
	{
		[JsonProperty("products")]
		public Dictionary<string, DLGamerJuego> Datos { get; set; }
	}

	public class DLGamerJuego
	{
		[JsonProperty("name")]
		public string Nombre { get; set; }

		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("price")]
		public string PrecioRebajado { get; set; }

		[JsonProperty("price_strike")]
		public string PrecioBase { get; set; }

		[JsonProperty("price_purcent")]
		public string Descuento { get; set; }

		[JsonProperty("link")]
		public string Enlace { get; set; }

		[JsonProperty("image_box")]
		public string Imagen { get; set; }

		[JsonProperty("drm")]
		public string DRM { get; set; }

		[JsonProperty("id_steam")]
		public string SteamID { get; set; }

		[JsonProperty("discount_end_at")]
		public string FechaTermina { get; set; }
	}

	#endregion
}
