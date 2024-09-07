#nullable disable

using Herramientas;
using Juegos;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Net;

namespace APIs.EpicGames
{
	public class Tienda
	{
		public static Tiendas2.Tienda Generar()
		{
			Tiendas2.Tienda tienda = new Tiendas2.Tienda
			{
				Id = "epicgamesstore",
				Nombre = "Epic Games Store",
				ImagenLogo = "/imagenes/tiendas/epic_logo.webp",
				Imagen300x80 = "/imagenes/tiendas/epic_300x80.webp",
				ImagenIcono = "/imagenes/tiendas/epic_icono.ico",
				Color = "#101014",
				AdminEnseñar = true,
				AdminInteractuar = true
			};

			return tienda;
		}

		public static async Task BuscarOfertas(SqlConnection conexion, IDecompiladores decompilador, ViewDataDictionary objeto = null)
		{
			BaseDatos.Tiendas.Admin.Actualizar(Tienda.Generar().Id, DateTime.Now, "0 ofertas detectadas", conexion);

			int juegos2 = 0;

			int i = 0;
			while (i < 100)
			{
				int cantidad = i * 40;
				int juegos3 = 0;

				string html = await Decompiladores.Estandar("https://store.epicgames.com/graphql?operationName=searchStoreQuery&variables={%22allowCountries%22:%22ES%22,%22category%22:%22games/edition/base%22,%22comingSoon%22:false,%22count%22:40,%22country%22:%22ES%22,%22effectiveDate%22:%22[,2024-08-11T13:15:18.401Z]%22,%22keywords%22:%22%22,%22locale%22:%22en-GB%22,%22onSale%22:true,%22sortBy%22:%22releaseDate%22,%22sortDir%22:%22DESC%22,%22start%22:" + cantidad + ",%22tag%22:%22%22,%22withPrice%22:true}&extensions={%22persistedQuery%22:{%22version%22:1,%22sha256Hash%22:%227d58e12d9dd8cb14c84a3ff18d360bf9f0caa96bf218f2c5fda68ba88d68a437%22}}");

				if (string.IsNullOrEmpty(html) == false)
				{
					EpicGamesStorePrincipal principal = JsonConvert.DeserializeObject<EpicGamesStorePrincipal>(html);

					if (principal != null)
					{
						if (principal.Datos != null)
						{
							if (principal.Datos.Catalogo != null)
							{
								if (principal.Datos.Catalogo.Busqueda != null)
								{
									if (principal.Datos.Catalogo.Busqueda.Juegos != null)
									{
										if (principal.Datos.Catalogo.Busqueda.Juegos.Count > 0)
										{
											foreach (var juego in principal.Datos.Catalogo.Busqueda.Juegos)
											{
												if (juego.Precio != null)
												{
													if (juego.Precio.PrecioTotal != null)
													{
														if (juego.Precio.PrecioTotal.PrecioFmt != null)
														{
															if (string.IsNullOrEmpty(juego.Precio.PrecioTotal.PrecioFmt.PrecioRebajado) == false)
															{
																string textoPrecioRebajado = juego.Precio.PrecioTotal.PrecioFmt.PrecioRebajado;

																if (string.IsNullOrEmpty(textoPrecioRebajado) == false)
																{
																	textoPrecioRebajado = textoPrecioRebajado.Replace("€", null);
																	textoPrecioRebajado = textoPrecioRebajado.Replace(",", ".");
																	textoPrecioRebajado = textoPrecioRebajado.Trim();
																}

																string textoPrecioBase = juego.Precio.PrecioTotal.PrecioFmt.PrecioBase;

																if (string.IsNullOrEmpty(textoPrecioBase) == false)
																{
																	textoPrecioBase = textoPrecioBase.Replace("€", null);
																	textoPrecioBase = textoPrecioBase.Replace(",", ".");
																	textoPrecioBase = textoPrecioBase.Trim();
																}

																if (string.IsNullOrEmpty(textoPrecioRebajado) == false && string.IsNullOrEmpty(textoPrecioBase) == false)
																{
																	decimal precioRebajado = decimal.Parse(textoPrecioRebajado);
																	decimal precioBase = decimal.Parse(textoPrecioBase);

																	int descuento = Calculadora.SacarDescuento(precioBase, precioRebajado);

																	string slug = null;

                                                                    if (juego.Enlaces != null)
                                                                    {
                                                                        if (juego.Enlaces.Count > 0)
                                                                        {
                                                                            slug = juego.Enlaces[0].Slug;
                                                                        }
                                                                    }

                                                                    if (string.IsNullOrEmpty(slug) == true)
																	{
																		if (juego.Enlaces2 != null)
																		{
																			if (juego.Enlaces2.Mapeos != null)
																			{
																				slug = juego.Enlaces2.Mapeos[0].Slug;
																			}
																		}
																	}

                                                                    if (string.IsNullOrEmpty(slug) == true)
																	{
                                                                        if (string.IsNullOrEmpty(juego.Enlace) == false)
                                                                        {
                                                                            slug = juego.Enlace;
                                                                        }
                                                                    }

                                                                    if (descuento > 0 && string.IsNullOrEmpty(slug) == false && juego.Imagenes != null)
																	{
																		if (juego.Imagenes.Count > 0)
																		{
																			string nombre = juego.Nombre;
																			nombre = WebUtility.HtmlDecode(nombre);

                                                                            string enlace = "https://store.epicgames.com/p/" + slug;

																			string imagen = juego.Imagenes[0].Enlace;

																			JuegoDRM drm = JuegoDRM.Epic;

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
																				BaseDatos.Tiendas.Comprobar.Resto(oferta, conexion, null, null, slug);
																			}
																			catch (Exception ex)
																			{
                                                                                BaseDatos.Errores.Insertar.Ejecutar(Tienda.Generar().Id, ex, conexion);
																			}

																			juegos2 += 1;
																			juegos3 += 1;

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
							}
						}
					}
				}

				if (juegos3 == 0)
				{
					break;
				}

				i += 1;
			}
		}
	}

	public class EpicGamesStorePrincipal
	{
		[JsonProperty("data")]
		public EpicGamesStoreData Datos { get; set; }
	}

	public class EpicGamesStoreData
	{
		[JsonProperty("Catalog")]
		public EpicGamesStoreCatalog Catalogo { get; set; }
	}

	public class EpicGamesStoreCatalog
	{
		[JsonProperty("searchStore")]
		public EpicGamesStoreSearch Busqueda { get; set; }
	}

	public class EpicGamesStoreSearch
	{
		[JsonProperty("elements")]
		public List<EpicGamesStoreJuego> Juegos { get; set; }
	}

	public class EpicGamesStoreJuego
	{
		[JsonProperty("title")]
		public string Nombre { get; set; }

		[JsonProperty("keyImages")]
		public List<EpicGamesStoreJuegoImagen> Imagenes { get; set; }

		[JsonProperty("offerMappings")]
		public List<EpicGamesStoreJuegoEnlace> Enlaces { get; set; }

		[JsonProperty("price")]
		public EpicGamesStoreJuegoPrecio Precio { get; set; }

		[JsonProperty("productSlug")]
		public string Enlace { get; set; }

        [JsonProperty("catalogNs")]
        public EpicGamesStoreJuegoEnlaceMapeo Enlaces2 { get; set; }
    }

	public class EpicGamesStoreJuegoImagen
	{
		[JsonProperty("type")]
		public string Tipo { get; set; }

		[JsonProperty("url")]
		public string Enlace { get; set; }
	}

	public class EpicGamesStoreJuegoEnlace
	{
		[JsonProperty("pageSlug")]
		public string Slug { get; set; }
	}

	public class EpicGamesStoreJuegoPrecio
	{
		[JsonProperty("totalPrice")]
		public EpicGamesStoreJuegoPrecio2 PrecioTotal { get; set; }
	}

	public class EpicGamesStoreJuegoPrecio2
	{
		[JsonProperty("fmtPrice")]
		public EpicGamesStoreJuegoPrecio3 PrecioFmt { get; set; }
	}

	public class EpicGamesStoreJuegoPrecio3
	{
		[JsonProperty("originalPrice")]
		public string PrecioBase { get; set; }

		[JsonProperty("discountPrice")]
		public string PrecioRebajado { get; set; }
	}

    public class EpicGamesStoreJuegoEnlaceMapeo
    {
        [JsonProperty("mappings")]
        public List<EpicGamesStoreJuegoEnlace> Mapeos { get; set; }
    }
}
