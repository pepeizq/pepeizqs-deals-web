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
using Microsoft.Data.SqlClient;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

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
				AdminInteractuar = true
			};

			return tienda;
		}

		public static string Referido(string enlace)
		{
            enlace = enlace.Replace(":", "%3A");
            enlace = enlace.Replace("/", "%2F");
            enlace = enlace.Replace("/", "%2F");
            enlace = enlace.Replace("?", "%3F");
            enlace = enlace.Replace("=", "%3D");

            return "https://humblebundleinc.sjv.io/c/1382810/2059850/25796?u=" + enlace;
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
            enlace = enlace + "?refc=gXsa9X&partner=pepeizq";

            enlace = enlace.Replace(":", "%3A");
            enlace = enlace.Replace("/", "%2F");
            enlace = enlace.Replace("/", "%2F");
            enlace = enlace.Replace("?", "%3F");
            enlace = enlace.Replace("=", "%3D");

            return "https://humblebundleinc.sjv.io/c/1382810/2059850/25796?u=" + enlace;
		}

		public static void RecopilarOfertas(string html)
		{
			if (string.IsNullOrEmpty(html) == false)
			{
				if (html != "null")
				{
					SqlConnection conexion = Herramientas.BaseDatos.Conectar();

					using (conexion)
					{
						HumbleJuegos juegos = JsonSerializer.Deserialize<HumbleJuegos>(html);

						if (juegos != null)
						{
							foreach (HumbleJuego juego in juegos.Resultados)
							{
								string sqlAñadir = "INSERT INTO temporalhumble " +
										"(contenido, fecha, enlace) VALUES " +
										"(@contenido, @fecha, @enlace) ";

								using (SqlCommand comando = new SqlCommand(sqlAñadir, conexion))
								{
									comando.Parameters.AddWithValue("@contenido", JsonSerializer.Serialize(juego));
									comando.Parameters.AddWithValue("@fecha", DateTime.Now.ToString());
									comando.Parameters.AddWithValue("@enlace", juego.Enlace);

									try
									{
                                        comando.ExecuteNonQuery();
                                    }
									catch (Exception ex) 
									{
                                        global::BaseDatos.Errores.Insertar.Mensaje("Humble Recopilación", ex, conexion);
                                    }								
								}
							}		
						}
					}
				}
			}	         
        }

		public static async Task BuscarOfertas(SqlConnection conexion)
		{
			await Task.Delay(1000);

			BaseDatos.Tiendas.Admin.Actualizar(Tienda.Generar().Id, DateTime.Now, "0 ofertas detectadas", conexion);

			DateTime fechaRecopilado = new DateTime();
			List<HumbleJuego> juegos = new List<HumbleJuego>();

			string busqueda = "SELECT * FROM temporalhumble";

			using (SqlCommand comando = new SqlCommand(busqueda, conexion))
			{
				using (SqlDataReader lector = comando.ExecuteReader())
				{
					while (lector.Read())
					{
						if (lector.IsDBNull(1) == false)
						{
							if (string.IsNullOrEmpty(lector.GetString(1)) == false)
							{
								HumbleJuego juego = new HumbleJuego();
								juego = JsonSerializer.Deserialize<HumbleJuego>(lector.GetString(1));

								juegos.Add(juego);
							}
						}

						if (lector.IsDBNull(2) == false)
						{
							if (string.IsNullOrEmpty(lector.GetString(2)) == false)
							{
								fechaRecopilado = DateTime.Parse(lector.GetString(2));
							}
						}
					}
				}
			}

			if (juegos.Count > 0) 
			{
				int juegos2 = 0;

				foreach (var juego in juegos)
				{
					string nombre = WebUtility.HtmlDecode(juego.Nombre);

					string imagen = WebUtility.HtmlDecode(juego.ImagenGrande);

					string enlace = "https://www.humblebundle.com/store/" + juego.Enlace;

					if (juego.PrecioBase != null && juego.PrecioRebajado != null)
					{
						decimal precioRebajado = decimal.Parse(juego.PrecioRebajado.Cantidad.ToString());

						int descuento = Calculadora.SacarDescuento(decimal.Parse(juego.PrecioBase.Cantidad.ToString()), precioRebajado);

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
									decimal tempChoice = decimal.Parse(juego.PrecioRebajado.Cantidad.ToString()) * Convert.ToDecimal(DescuentoChoice(juego.DescuentoChoice));

									decimal precioChoice = decimal.Parse(juego.PrecioRebajado.Cantidad.ToString()) - tempChoice;
									precioChoice = Math.Round(precioChoice, 2);

									if (precioChoice < precioRebajado)
									{
										int descuentoChoice = Calculadora.SacarDescuento(decimal.Parse(juego.PrecioBase.Cantidad.ToString()), precioChoice);

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
								foreach (var oferta in ofertas)
								{
									try
									{
										BaseDatos.Tiendas.Comprobar.Resto(oferta, conexion);

										string limpieza = "DELETE FROM temporalhumble WHERE enlace=@enlace";

										using (SqlCommand comando = new SqlCommand(limpieza, conexion))
										{
											comando.Parameters.AddWithValue("@enlace", juego.Enlace);

											comando.ExecuteNonQuery();
										}
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

	public class HumbleJuegos
	{
		[JsonPropertyName("num_pages")]
		public int Numero { get; set; }

		[JsonPropertyName("results")]
		public List<HumbleJuego> Resultados { get; set; }
	}

	public class HumbleJuego
	{
		[JsonPropertyName("human_name")]
		public string Nombre { get; set; }

		[JsonPropertyName("machine_name")]
		public string Id { get; set; }

		[JsonPropertyName("standard_carousel_image")]
		public string ImagenPequeña { get; set; }

		[JsonPropertyName("large_capsule")]
		public string ImagenGrande { get; set; }

		[JsonPropertyName("current_price")]
		public HumbleJuegoPrecio PrecioRebajado { get; set; }

		[JsonPropertyName("full_price")]
		public HumbleJuegoPrecio PrecioBase { get; set; }

		[JsonPropertyName("human_url")]
		public string Enlace { get; set; }

		[JsonPropertyName("delivery_methods")]
		public List<string> DRMs { get; set; }

		[JsonPropertyName("platforms")]
		public List<string> Sistemas { get; set; }

		[JsonPropertyName("sale_end")]
		public double FechaTermina { get; set; }

		[JsonPropertyName("rewards_split")]
		public double DescuentoChoice { get; set; }

		[JsonPropertyName("incompatible_features")]
		public List<string> CosasIncompatibles { get; set; }
	}

	public class HumbleJuegoPrecio
	{
		[JsonPropertyName("currency")]
		public string Moneda { get; set; }

		[JsonPropertyName("amount")]
		public object Cantidad { get; set; }
	}
}
