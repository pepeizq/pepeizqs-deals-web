//https://www.fanatical.com/api/products-group/killer-bundle/en
//https://www.fanatical.com/api/all/en
//https://www.fanatical.com/api/algolia/bundles?altRank=false
//https://www.fanatical.com/api/algolia/onsaleresults

#nullable disable

using Herramientas;
using Juegos;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System.Net;

namespace APIs.Fanatical
{
	public static class Tienda
	{
		public static Tiendas2.Tienda Generar()
		{
			Tiendas2.Tienda tienda = new Tiendas2.Tienda
			{
				Id = "fanatical",
				Nombre = "Fanatical",
				ImagenLogo = "/imagenes/tiendas/fanatical_logo.webp",
				Imagen300x80 = "/imagenes/tiendas/fanatical_300x80.webp",
				ImagenIcono = "/imagenes/tiendas/fanatical_icono.ico",
				Color = "#ffcf89",
				AdminEnseñar = true,
				AdminInteractuar = true
			};

			return tienda;
		}

		public static string Referido(string enlace)
		{
			return enlace + "?ref=pepeizq";
		}

		public static async Task BuscarOfertas(SqlConnection conexion, IDecompiladores decompilador, ViewDataDictionary objeto = null)
		{
			BaseDatos.Tiendas.Admin.Actualizar(Tienda.Generar().Id, DateTime.Now, "0 ofertas detectadas", conexion);

			string html = await Decompiladores.Estandar("https://feed.fanatical.com/feed", conexion);

			if (html != null)
			{
				html = html.Replace("{" + Strings.ChrW(34) + "title" + Strings.ChrW(34), ",{" + Strings.ChrW(34) + "title" + Strings.ChrW(34));

				html = html.Remove(0, 1);
				html = "[" + html + "]";

				List<FanaticalJuego> juegos = JsonConvert.DeserializeObject<List<FanaticalJuego>>(html);

				if (juegos != null)
				{
					if (juegos.Count > 0)
					{
						int juegos2 = 0;

						foreach (FanaticalJuego juego in juegos)
						{
							bool autorizar = true;

							if (juego.Regiones != null)
							{
								if (juego.Regiones.Count > 0)
								{
									autorizar = false;

									foreach (string region in juego.Regiones)
									{
										if (region == "ES")
										{
											autorizar = true;
										}
									}
								}
							}

							//if (juego.Tipo == "bundle")
							//{
							//	autorizar = false;
							//}

							if (autorizar == true)
							{
								string descuentoTexto = juego.Descuento;

								if (descuentoTexto != null)
								{
									if (descuentoTexto.Contains(".") == true)
									{
										int int1 = descuentoTexto.IndexOf(".");
										descuentoTexto = descuentoTexto.Remove(int1, descuentoTexto.Length - int1);
									}

									int descuento = 0;

									try
									{
										descuento = int.Parse(descuentoTexto);
									}
									catch { }

									if (descuento > 0)
									{
										string nombre = WebUtility.HtmlDecode(juego.Nombre);

										string imagen = juego.Imagen;

										string enlace = juego.Enlace;

										decimal precioRebajado = decimal.Parse(juego.PrecioRebajado.EUR);

										if (juego.DRMs != null)
										{
											if (juego.DRMs.Count > 0)
											{
												string drmTexto = juego.DRMs[0];
												JuegoDRM drm = JuegoDRM2.Traducir(drmTexto, Generar().Id);

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

												if (juego.FechaTermina != null)
												{
													if (Convert.ToDouble(juego.FechaTermina) > 0)
													{
														DateTime fechaTermina = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
														fechaTermina = fechaTermina.AddSeconds(Convert.ToDouble(juego.FechaTermina));
														fechaTermina = fechaTermina.ToLocalTime();

														oferta.FechaTermina = fechaTermina;
													}
												}

												try
												{
													BaseDatos.Tiendas.Comprobar.Resto(oferta, objeto, conexion);
												}
												catch (Exception ex)
												{
                                                    BaseDatos.Errores.Insertar.Ejecutar(Tienda.Generar().Id, ex, conexion);
                                                }

												juegos2 += 1;

												try
												{
													BaseDatos.Tiendas.Admin.Actualizar(Tienda.Generar().Id, DateTime.Now, juegos2.ToString() + " ofertas detectadas", conexion);
												}
												catch (Exception ex)
												{
                                                    BaseDatos.Errores.Insertar.Ejecutar(Tienda.Generar().Id, ex, conexion);
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
	}

    #region Clases

    public class FanaticalJuego
	{
		[JsonProperty("title")]
		public string Nombre { get; set; }

		[JsonProperty("sku")]
		public string Id { get; set; }

		[JsonProperty("drm")]
		public List<string> DRMs { get; set; }

		[JsonProperty("image")]
		public string Imagen { get; set; }

		[JsonProperty("url")]
		public string Enlace { get; set; }

		[JsonProperty("discount_percent")]
		public string Descuento { get; set; }

		[JsonProperty("expiry")]
		public string FechaTermina { get; set; }

		[JsonProperty("steam_app_id")]
		public string SteamId { get; set; }

		[JsonProperty("current_price")]
		public FanaticalJuegoPrecio PrecioRebajado { get; set; }

		[JsonProperty("regular_price")]
		public FanaticalJuegoPrecio PrecioBase { get; set; }

		[JsonProperty("regions")]
		public List<string> Regiones { get; set; }

		[JsonProperty("bundle_games")]
		public FanaticalJuegoBundle Bundle { get; set; }

		[JsonProperty("type")]
		public string Tipo { get; set; }
	}

	public class FanaticalJuegoPrecio
	{
		[JsonProperty("USD")]
		public string USD { get; set; }

		[JsonProperty("GBP")]
		public string GBP { get; set; }

		[JsonProperty("EUR")]
		public string EUR { get; set; }
	}

	public class FanaticalJuegoBundle
	{
		[JsonProperty("1")]
		public FanaticalJuegoBundleTier Tier1 { get; set; }

		[JsonProperty("2")]
		public FanaticalJuegoBundleTier Tier2 { get; set; }

		[JsonProperty("3")]
		public FanaticalJuegoBundleTier Tier3 { get; set; }
	}

	public class FanaticalJuegoBundleTier
	{
		[JsonProperty("items")]
		public List<FanaticalJuegoBundleJuego> Juegos { get; set; }
	}

	public class FanaticalJuegoBundleJuego
	{
		[JsonProperty("steam_id")]
		public string SteamId { get; set; }
	}

	//--------------------------------------------------------------

	public class FanaticalBundle
	{
		[JsonProperty("name")]
		public string Nombre { get; set; }

		[JsonProperty("slug")]
		public string Slug { get; set; }

		[JsonProperty("cover")]
		public string Imagen { get; set; }

		[JsonProperty("price")]
		public FanaticalJuegoPrecio Precio { get; set; }

		[JsonProperty("available_valid_until")]
		public string FechaTermina { get; set; }
	}

    #endregion
}
