//https://store.epicgames.com/graphql?operationName=searchStoreQuery&variables={%22allowCountries%22:%22ES%22,%22category%22:%22games/edition/base|addons|games/edition%22,%22count%22:40,%22country%22:%22ES%22,%22effectiveDate%22:%22[,2024-11-19T19:59:57.141Z]%22,%22keywords%22:%22%22,%22locale%22:%22en-GB%22,%22onSale%22:true,%22sortBy%22:%22relevancy,viewableDate%22,%22sortDir%22:%22DESC,DESC%22,%22start%22:40,%22tag%22:%22%22,%22withPrice%22:true}&extensions={%22persistedQuery%22:{%22version%22:1,%22sha256Hash%22:%227d58e12d9dd8cb14c84a3ff18d360bf9f0caa96bf218f2c5fda68ba88d68a437%22}}

#nullable disable

using Herramientas;
using Juegos;
using Microsoft.Data.SqlClient;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

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

		public static async Task BuscarOfertas(SqlConnection conexion, IDecompiladores decompilador)
		{
			BaseDatos.Admin.Actualizar.Tiendas(Generar().Id, DateTime.Now, 0, conexion);

			int juegos2 = 0;

			int i = 0;
			while (i < 100)
			{
				int cantidad = i * 40;
				int juegos3 = 0;

				string fecha = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString(); 

				string html = await Decompiladores.GZipFormato2("https://store.epicgames.com/graphql?operationName=searchStoreQuery&variables={%22allowCountries%22:%22ES%22,%22category%22:%22games/edition/base|addons|games/edition%22,%22count%22:40,%22country%22:%22ES%22,%22effectiveDate%22:%22[," + fecha + "T10:00:00.141Z]%22,%22keywords%22:%22%22,%22locale%22:%22en-GB%22,%22onSale%22:true,%22sortBy%22:%22relevancy,viewableDate%22,%22sortDir%22:%22DESC,DESC%22,%22start%22:" + cantidad + ",%22tag%22:%22%22,%22withPrice%22:true}&extensions={%22persistedQuery%22:{%22version%22:1,%22sha256Hash%22:%227d58e12d9dd8cb14c84a3ff18d360bf9f0caa96bf218f2c5fda68ba88d68a437%22}}");

				if (string.IsNullOrEmpty(html) == false)
				{
					EpicGamesStorePrincipal principal = null;

					try
					{
						principal = JsonSerializer.Deserialize<EpicGamesStorePrincipal>(html);
					}
					catch
					{ }

					html = await Decompiladores.GZipFormato2("https://store.epicgames.com/graphql?operationName=searchStoreQuery&variables={%22allowCountries%22:%22ES%22,%22category%22:%22games/edition/base|addons|games/edition%22,%22count%22:40,%22country%22:%22ES%22,%22effectiveDate%22:%22[," + fecha + "T10:00:00.141Z]%22,%22keywords%22:%22%22,%22locale%22:%22en-GB%22,%22onSale%22:true,%22sortBy%22:%22relevancy,viewableDate%22,%22sortDir%22:%22DESC,DESC%22,%22start%22:" + cantidad + ",%22tag%22:%22%22,%22withPrice%22:true}&extensions={%22persistedQuery%22:{%22version%22:1,%22sha256Hash%22:%227d58e12d9dd8cb14c84a3ff18d360bf9f0caa96bf218f2c5fda68ba88d68a437%22}}");

					try
					{
						principal = JsonSerializer.Deserialize<EpicGamesStorePrincipal>(html);
					}
					catch
					{ }

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

																	string slug = juego.Enlace;

																	if (string.IsNullOrEmpty(slug) == false)
																	{
																		if (slug.Contains("/") == true)
																		{
																			int int1 = slug.IndexOf("/");
																			slug = slug.Remove(int1, slug.Length - int1);
																		}
																	}
																	else
																	{
																		if (juego.Enlaces != null)
																		{
																			if (juego.Enlaces.Count > 0)
																			{
																				slug = juego.Enlaces[0].Slug;
																			}
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

                                                                    if (descuento > 0 && string.IsNullOrEmpty(slug) == false && juego.Imagenes != null)
																	{
																		if (juego.Imagenes.Count > 0)
																		{
																			string nombre = juego.Nombre;
																			nombre = WebUtility.HtmlDecode(nombre);

                                                                            string enlace = "https://store.epicgames.com/p/" + slug;

																			string imagen = juego.Imagenes[0].Enlace;

																			JuegoPrecio oferta = new JuegoPrecio
																			{
																				Nombre = nombre,
																				Enlace = enlace,
																				Imagen = imagen,
																				Moneda = JuegoMoneda.Euro,
																				Precio = precioRebajado,
																				Descuento = descuento,
																				Tienda = Generar().Id,
																				DRM = JuegoDRM.Epic,
																				FechaDetectado = DateTime.Now,
																				FechaActualizacion = DateTime.Now
																			};

                                                                            try
																			{
																				BaseDatos.Tiendas.Comprobar.Resto(oferta, conexion, null, null, slug);
																			}
																			catch (Exception ex)
																			{
                                                                                BaseDatos.Errores.Insertar.Mensaje(Generar().Id, ex, conexion);
																			}

																			juegos2 += 1;
																			juegos3 += 1;

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
		[JsonPropertyName("data")]
		public EpicGamesStoreData Datos { get; set; }
	}

	public class EpicGamesStoreData
	{
		[JsonPropertyName("Catalog")]
		public EpicGamesStoreCatalog Catalogo { get; set; }
	}

	public class EpicGamesStoreCatalog
	{
		[JsonPropertyName("searchStore")]
		public EpicGamesStoreSearch Busqueda { get; set; }
	}

	public class EpicGamesStoreSearch
	{
		[JsonPropertyName("elements")]
		public List<EpicGamesStoreJuego> Juegos { get; set; }
	}

	public class EpicGamesStoreJuego
	{
		[JsonPropertyName("title")]
		public string Nombre { get; set; }

		[JsonPropertyName("keyImages")]
		public List<EpicGamesStoreJuegoImagen> Imagenes { get; set; }

		[JsonPropertyName("offerMappings")]
		public List<EpicGamesStoreJuegoEnlace> Enlaces { get; set; }

		[JsonPropertyName("price")]
		public EpicGamesStoreJuegoPrecio Precio { get; set; }

		[JsonPropertyName("productSlug")]
		public string Enlace { get; set; }

        [JsonPropertyName("catalogNs")]
        public EpicGamesStoreJuegoEnlaceMapeo Enlaces2 { get; set; }
    }

	public class EpicGamesStoreJuegoImagen
	{
		[JsonPropertyName("type")]
		public string Tipo { get; set; }

		[JsonPropertyName("url")]
		public string Enlace { get; set; }
	}

	public class EpicGamesStoreJuegoEnlace
	{
		[JsonPropertyName("pageSlug")]
		public string Slug { get; set; }
	}

	public class EpicGamesStoreJuegoPrecio
	{
		[JsonPropertyName("totalPrice")]
		public EpicGamesStoreJuegoPrecio2 PrecioTotal { get; set; }
	}

	public class EpicGamesStoreJuegoPrecio2
	{
		[JsonPropertyName("fmtPrice")]
		public EpicGamesStoreJuegoPrecio3 PrecioFmt { get; set; }
	}

	public class EpicGamesStoreJuegoPrecio3
	{
		[JsonPropertyName("originalPrice")]
		public string PrecioBase { get; set; }

		[JsonPropertyName("discountPrice")]
		public string PrecioRebajado { get; set; }
	}

    public class EpicGamesStoreJuegoEnlaceMapeo
    {
        [JsonPropertyName("mappings")]
        public List<EpicGamesStoreJuegoEnlace> Mapeos { get; set; }
    }
}
