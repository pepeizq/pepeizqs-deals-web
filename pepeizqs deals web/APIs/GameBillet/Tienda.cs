//Europe: https://www.gamebillet.com/product/jsonfeed?store=eu&guid=39A6D2B7-A4EF-4E8B-AA19-350B89788365
//UK: https://www.gamebillet.com/product/jsonfeed?store=uk&guid=39A6D2B7-A4EF-4E8B-AA19-350B89788365
//US / CA / ROW: https://www.gamebillet.com/product/jsonfeed?store=us&guid=39A6D2B7-A4EF-4E8B-AA19-350B89788365
//China: https://www.gamebillet.com/product/jsonfeed?store=cn&guid=39A6D2B7-A4EF-4E8B-AA19-350B89788365
//Brazil: https://www.gamebillet.com/product/jsonfeed?store=sa&guid=39A6D2B7-A4EF-4E8B-AA19-350B89788365

#nullable disable

using Herramientas;
using Juegos;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;

namespace APIs.GameBillet
{
	public static class Tienda
	{
		public static Tiendas2.Tienda Generar()
		{
			Tiendas2.Tienda tienda = new Tiendas2.Tienda
			{
				Id = "gamebillet",
				Nombre = "GameBillet",
				ImagenLogo = "/imagenes/tiendas/gamebillet_logo.webp",
				Imagen300x80 = "/imagenes/tiendas/gamebillet_300x80.webp",
				ImagenIcono = "/imagenes/tiendas/gamebillet_icono.ico",
				Color = "#F1652A",
				AdminEnseñar = true,
				AdminInteractuar = true
			};

			return tienda;
		}

		public static string Referido(string enlace)
		{
			return enlace + "?affiliate=64e186aa-fb0e-436f-a000-069090c06fe9";
		}

		public static async Task BuscarOfertas(SqlConnection conexion, IDecompiladores decompilador)
		{
			BaseDatos.Tiendas.Admin.Actualizar(Tienda.Generar().Id, DateTime.Now, "0 ofertas detectadas", conexion);

			string html = await Decompiladores.Estandar("https://www.gamebillet.com/product/jsonfeed?store=eu&guid=39A6D2B7-A4EF-4E8B-AA19-350B89788365");

			if (string.IsNullOrEmpty(html) == false)
			{
				GameBilletProductos juegos = JsonConvert.DeserializeObject<GameBilletProductos>(html);

				if (juegos != null)
				{
					if (juegos.Juegos != null)
					{
						if (juegos.Juegos.Count > 0)
						{
							int juegos2 = 0;

							foreach (GameBilletJuego juego in juegos.Juegos)
							{
								if (string.IsNullOrEmpty(juego.PrecioBase) == false && string.IsNullOrEmpty(juego.PrecioRebajado) == false)
								{
									string sPrecioBase = juego.PrecioBase;
									sPrecioBase = sPrecioBase.Replace("€", null);

									string sPrecioRebajado = juego.PrecioRebajado;
									sPrecioRebajado = sPrecioRebajado.Replace("€", null);

									decimal precioBase = decimal.Parse(sPrecioBase);
									decimal precioRebajado = decimal.Parse(sPrecioRebajado);

									int descuento = Calculadora.SacarDescuento(precioBase, precioRebajado);

									if (descuento > 0)
									{
										JuegoDRM drm1 = JuegoDRM2.Traducir(juego.DRM1, Generar().Id);
										JuegoDRM drm2 = JuegoDRM2.Traducir(juego.DRM2, Generar().Id);

										if (drm1 != JuegoDRM.NoEspecificado || drm2 != JuegoDRM.NoEspecificado)
										{
											JuegoDRM drmFinal = drm1;

											if (drm2 != JuegoDRM.NoEspecificado)
											{
												drmFinal = drm2;
											}

											JuegoPrecio oferta = new JuegoPrecio
											{
												Nombre = juego.Nombre,
												Enlace = juego.Enlace,
												Imagen = "nada",
												Moneda = JuegoMoneda.Euro,
												Precio = precioRebajado,
												Descuento = descuento,
												Tienda = Generar().Id,
												DRM = drmFinal,
												FechaDetectado = DateTime.Now,
												FechaActualizacion = DateTime.Now
											};

											try
											{
												BaseDatos.Tiendas.Comprobar.Resto(oferta, conexion);
											}
											catch (Exception ex)
											{
												BaseDatos.Errores.Insertar.Mensaje(Tienda.Generar().Id, ex, conexion);
											}

											juegos2 += 1;

											try
											{
												BaseDatos.Tiendas.Admin.Actualizar(Tienda.Generar().Id, DateTime.Now, juegos2.ToString() + " ofertas detectadas", conexion);
											}
											catch (Exception ex)
											{
												BaseDatos.Errores.Insertar.Mensaje(Tienda.Generar().Id, ex, conexion);
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}
	}

	#region Clases

	public class GameBilletProductos
	{
		[JsonProperty("product")]
		public List<GameBilletJuego> Juegos { get; set; }
	}

	public class GameBilletJuego
	{
		[JsonProperty("name")]
		public string Nombre { get; set; }

		[JsonProperty("url")]
		public string Enlace { get; set; }

		[JsonProperty("price")]
		public string PrecioBase { get; set; }

		[JsonProperty("special_price")]
		public string PrecioRebajado { get; set; }

		[JsonProperty("sku")]
		public string Id { get; set; }

		[JsonProperty("filters1")]
		public string DRM1 { get; set; }

		[JsonProperty("filters2")]
		public string DRM2 { get; set; }
	}

	#endregion
}
