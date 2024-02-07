//https://www.humblebundle.com/store/api/recommend?recommendation_attempt=1&machine_name=americanfugitive_storefront
//https://www.humblebundle.com/store/api/search?sort=discount&filter=all&search=american&request=1
//https://www.humblebundle.com/api/v1/trove/chunk?index=4
//https://www.humblebundle.com/api/v1/subscriptions/humble_monthly/subscription_products_with_gamekeys/
//https://www.humblebundle.com/api/v1/subscriptions/humble_monthly/history?from_product=july_2020_choice
//https://www.humblebundle.com/store/api/lookup?products[]=sonic-mania&request=1

//https://scrapfly.io/
//https://www.zenrows.com

#nullable disable

using Herramientas;
using Juegos;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Net;

namespace APIs.Humble
{
	public static class Tienda
	{
		public static Tiendas2.Tienda Generar()
		{
			Tiendas2.Tienda tienda = new Tiendas2.Tienda
			{
				Id = "humblestore",
				Nombre = "Humble Store",
				ImagenLogo = "/imagenes/tiendas/humblestore_logo.webp",
				Imagen300x80 = "/imagenes/tiendas/humblestore_300x80.webp",
				ImagenIcono = "/imagenes/tiendas/humblestore_icono.ico",
				Color = "#ea9192",
				AdminEnseñar = true,
				AdminInteractuar = false
			};

			return tienda;
		}

		public static string Referido(string enlace)
		{
			return enlace + "?partner=pepeizq";
		}

		public static Tiendas2.Tienda GenerarChoice()
		{
			Tiendas2.Tienda tienda = new Tiendas2.Tienda
			{
				Id = "humblechoice",
				Nombre = "Humble Choice",
				ImagenLogo = "/imagenes/tiendas/humblechoice_logo.webp",
				Imagen300x80 = "/imagenes/tiendas/humblechoice_300x80.webp",
				ImagenIcono = "/imagenes/tiendas/humblestore_icono.ico",
				Color = "#ea9192",
				AdminEnseñar = false,
				AdminInteractuar = false
			};

			return tienda;
		}

		public static string ReferidoChoice(string enlace)
		{
			return enlace + "?refc=gXsa9X";
		}

		public static void BuscarOfertas(ViewDataDictionary objeto, string html)
		{
			if (html != null)
			{
				if (html != "null")
				{
					SqlConnection conexion = Herramientas.BaseDatos.Conectar();

					BaseDatos.Tiendas.Admin.Actualizar(Tienda.Generar().Id, DateTime.Now, "0 ofertas detectadas", conexion);

					using (conexion)
					{
						HumbleJuegos juegos = JsonConvert.DeserializeObject<HumbleJuegos>(html);

						if (juegos != null)
						{
							int juegos2 = 0;

							foreach (HumbleJuego juego in juegos.Resultados)
							{
								string nombre = WebUtility.HtmlDecode(juego.Nombre);

								string imagen = juego.ImagenGrande;

								string enlace = "https://www.humblebundle.com/store/" + juego.Enlace;

								if (juego.PrecioBase != null && juego.PrecioRebajado != null)
								{
									decimal precioRebajado = decimal.Parse(juego.PrecioRebajado.Cantidad);

									int descuento = Calculadora.SacarDescuento(decimal.Parse(juego.PrecioBase.Cantidad), precioRebajado);

									if (descuento > 0)
									{
										bool añadirChoice = true;

										if (juego.CosasIncompatibles != null)
										{
											if (juego.CosasIncompatibles.Count > 0)
											{
												foreach (var cosa in juego.CosasIncompatibles)
												{
													if (cosa == "subscriber-discount-coupons")
													{
														añadirChoice = false;
													}
												}
											}
										}

										List<JuegoPrecio> ofertas = new List<JuegoPrecio>();

										foreach (string drmTexto in juego.DRMs)
										{
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

											if (juego.FechaTermina > 0)
											{
												DateTime fechaTermina = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
												fechaTermina = fechaTermina.AddSeconds(juego.FechaTermina);
												fechaTermina = fechaTermina.ToLocalTime();

												oferta.FechaTermina = fechaTermina;
											}

											ofertas.Add(oferta);

											if (añadirChoice == true)
											{
												decimal tempChoice = decimal.Parse(juego.PrecioRebajado.Cantidad) * Convert.ToDecimal(DescuentoChoice(juego.DescuentoChoice));

												decimal precioChoice = decimal.Parse(juego.PrecioRebajado.Cantidad) - tempChoice;
												precioChoice = Math.Round(precioChoice, 2);

												if (precioChoice < precioRebajado)
												{
													int descuentoChoice = Calculadora.SacarDescuento(decimal.Parse(juego.PrecioBase.Cantidad), precioChoice);

													JuegoPrecio choice = new JuegoPrecio
													{
														Nombre = nombre,
														Enlace = enlace,
														Imagen = imagen,
														Moneda = JuegoMoneda.Euro,
														Precio = precioChoice,
														Descuento = descuentoChoice,
														Tienda = GenerarChoice().Id,
														DRM = drm,
														FechaDetectado = DateTime.Now,
                                                        FechaActualizacion = DateTime.Now
                                                    };

													if (juego.FechaTermina > 0)
													{
														DateTime fechaTermina = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
														fechaTermina = fechaTermina.AddSeconds(juego.FechaTermina);
														fechaTermina = fechaTermina.ToLocalTime();

														choice.FechaTermina = fechaTermina;
													}

													ofertas.Add(choice);
												}
											}
										}

										if (ofertas.Count > 0)
										{
											BaseDatos.Tiendas.Comprobar.Resto(ofertas, objeto, conexion);

											juegos2 += 1;
											BaseDatos.Tiendas.Admin.Actualizar(Tienda.Generar().Id, DateTime.Now, juegos2.ToString() + " ofertas detectadas", conexion);
										}
									}
								}
							}		
						}

						if (juegos != null)
						{
							objeto["Mensaje"] = objeto["Mensaje"] + "Humble Store: " + juegos.Resultados.Count();
						}
					}
				}
			}	         
        }

		private static double DescuentoChoice(double cantidad)
		{
			double descuento = 0;

			if (cantidad == 0.1)
			{
				descuento = 0.2;
			}
			else if (cantidad == 0.05)
			{
				descuento = 0.15;
			}
			else if (cantidad == 0.03)
			{
				descuento = 0.13;
			}
			else if (cantidad == 0.02)
			{
				descuento = 0.12;
			}
			else if (cantidad == 0)
			{
				descuento = 0.10;
			}

			return descuento;
		}
	}

	public class Scraper
	{
		[JsonProperty("result")]
		public ScraperContenido Resultado { get; set; }
	}

	public class ScraperContenido
	{
		[JsonProperty("content")]
		public string Contenido { get; set; }
	}

	public class HumblePaginas
	{
		[JsonProperty("num_pages")]
		public string Numero { get; set; }
	}

	public class HumbleJuegos
	{
		[JsonProperty("results")]
		public List<HumbleJuego> Resultados { get; set; }
	}

	public class HumbleJuego
	{
		[JsonProperty("human_name")]
		public string Nombre { get; set; }

		[JsonProperty("machine_name")]
		public string Id { get; set; }

		[JsonProperty("standard_carousel_image")]
		public string ImagenPequeña { get; set; }

		[JsonProperty("large_capsule")]
		public string ImagenGrande { get; set; }

		[JsonProperty("current_price")]
		public HumbleJuegoPrecio PrecioRebajado { get; set; }

		[JsonProperty("full_price")]
		public HumbleJuegoPrecio PrecioBase { get; set; }

		[JsonProperty("human_url")]
		public string Enlace { get; set; }

		[JsonProperty("delivery_methods")]
		public List<string> DRMs { get; set; }

		[JsonProperty("platforms")]
		public List<string> Sistemas { get; set; }

		[JsonProperty("sale_end")]
		public double FechaTermina { get; set; }

		[JsonProperty("rewards_split")]
		public double DescuentoChoice { get; set; }

		[JsonProperty("incompatible_features")]
		public List<string> CosasIncompatibles { get; set; }
	}

	public class HumbleJuegoPrecio
	{
		[JsonProperty("currency")]
		public string Moneda { get; set; }

		[JsonProperty("amount")]
		public string Cantidad { get; set; }
	}
}
