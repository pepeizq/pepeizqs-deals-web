#nullable disable

using Herramientas;
using Juegos;
using Microsoft.Data.SqlClient;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace APIs.PlanetPlay
{
	public static class Tienda
	{
		public static Tiendas2.Tienda Generar()
		{
			Tiendas2.Tienda tienda = new Tiendas2.Tienda
			{
				Id = "planetplay",
				Nombre = "PlanetPlay",
				ImagenLogo = "/imagenes/tiendas/planetplay_logo.webp",
				Imagen300x80 = "/imagenes/tiendas/planetplay_300x80.webp",
				ImagenIcono = "/imagenes/tiendas/planetplay_icono.webp",
				Color = "#00CC7E",
				AdminEnseñar = true,
				AdminInteractuar = true
			};

			return tienda;
		}

		public static async Task BuscarOfertas(SqlConnection conexion, IDecompiladores decompilador)
		{
			BaseDatos.Admin.Actualizar.Tiendas(Generar().Id, DateTime.Now, 0, conexion);

			int tope = 10;
			int juegos2 = 0;

			int i = 0;
			while (i < tope)
			{
				string html = await Decompiladores.Estandar("https://api.planetplay.com/platform/fetch?order=relevancy&offer=discount&page=" + i.ToString());

				if (string.IsNullOrEmpty(html) == false)
				{
					PlanetPlayDatos datos = JsonSerializer.Deserialize<PlanetPlayDatos>(html);

					if (datos != null)
					{
						tope = datos.Total / 20;

						if (datos.Juegos != null)
						{
							if (datos.Juegos.Count > 0)
							{
								foreach (var juego in datos.Juegos)
								{
									if (juego.Activo == true)
									{
										decimal precioBase = juego.PrecioBase;
										decimal precioRebajado = juego.PrecioRebajado;

										int descuento = Calculadora.SacarDescuento(precioBase, precioRebajado);

										if (descuento > 0)
										{
											string nombre = WebUtility.HtmlDecode(juego.Nombre);

											string enlace = juego.Enlace;

											bool parar = false;

											if (enlace.Contains("/store-mobile/games/") == true)
											{
												parar = true;
											}

											if (parar == false)
											{
												string imagen = juego.Imagen;

												JuegoDRM drm = JuegoDRM2.Traducir(juego.DRM.Nombre, Generar().Id);

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
					}
				}

				i += 1;
			}
		}
	}

	public class PlanetPlayDatos
	{
		[JsonPropertyName("results")]
		public List<PlanetPlayJuego> Juegos { get; set; }

		[JsonPropertyName("total")]
		public int Total { get; set; }
	}

	public class PlanetPlayJuego
	{
		[JsonPropertyName("title")]
		public string Nombre { get; set; }

		[JsonPropertyName("url")]
		public string Enlace { get; set; }

		[JsonPropertyName("image")]
		public string Imagen { get; set; }

		[JsonPropertyName("price")]
		public decimal PrecioBase { get; set; }

		[JsonPropertyName("discountPrice")]
		public decimal PrecioRebajado { get; set; }

		[JsonPropertyName("platform")]
		public PlanetPlayJuegoPlataforma DRM { get; set; }

		[JsonPropertyName("enabled")]
		public bool Activo { get; set; }
	}

	public class PlanetPlayJuegoPlataforma
	{
		[JsonPropertyName("name")]
		public string Nombre { get; set; }
	}
}
