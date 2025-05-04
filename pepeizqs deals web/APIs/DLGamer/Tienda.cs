//https://partner.dlgamer.com/secure/affiliation

//https://static.dlgamer.com/feeds/general_feed_en.json
//https://static.dlgamer.com/feeds/general_feed_us.json
//https://static.dlgamer.com/feeds/general_feed_eu.json

#nullable disable

using Herramientas;
using Juegos;
using Microsoft.Data.SqlClient;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

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

		public static async Task BuscarOfertas(SqlConnection conexion, IDecompiladores decompilador)
		{
			BaseDatos.Admin.Actualizar.Tiendas(Generar().Id, DateTime.Now, 0, conexion);

			int juegos2 = 0;

			string html = await Decompiladores.Estandar("http://static.dlgamer.com/feeds/general_feed_eu.json");

			if (string.IsNullOrEmpty(html) == false)
			{
				DLGamerJuegos basedatos = JsonSerializer.Deserialize<DLGamerJuegos>(html);

				if (basedatos != null)
				{
					foreach (var juegoDL in basedatos.Datos)
					{
						decimal precioRebajado = juegoDL.Value.PrecioRebajado;
						decimal precioBase = juegoDL.Value.PrecioBase;

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

	#region Clases

	public class DLGamerJuegos
	{
		[JsonPropertyName("products")]
		public Dictionary<string, DLGamerJuego> Datos { get; set; }
	}

	public class DLGamerJuego
	{
		[JsonPropertyName("name")]
		public string Nombre { get; set; }

		[JsonPropertyName("id")]
		public int Id { get; set; }

		[JsonPropertyName("price")]
		public decimal PrecioRebajado { get; set; }

		[JsonPropertyName("price_strike")]
		public decimal PrecioBase { get; set; }

		[JsonPropertyName("price_purcent")]
		public string Descuento { get; set; }

		[JsonPropertyName("link")]
		public string Enlace { get; set; }

		[JsonPropertyName("image_box")]
		public string Imagen { get; set; }

		[JsonPropertyName("drm")]
		public string DRM { get; set; }

		[JsonPropertyName("id_steam")]
		public int SteamID { get; set; }

		[JsonPropertyName("discount_end_at")]
		public string FechaTermina { get; set; }
	}

	#endregion
}
