#nullable disable

using Juegos;
using Microsoft.Data.SqlClient;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace APIs.AmazonLuna
{
	public static class Suscripcion
	{
		public static Suscripciones2.Suscripcion Generar()
		{
			Suscripciones2.Suscripcion amazon = new Suscripciones2.Suscripcion
			{
				Id = Suscripciones2.SuscripcionTipo.AmazonLunaPlus,
				Nombre = "Amazon Luna +",
				ImagenLogo = "/imagenes/suscripciones/amazonluna.webp",
				ImagenIcono = "/imagenes/streaming/amazonluna_icono.webp",
				Enlace = "https://luna.amazon.es/subscription/luna-plus/B085TRCCT6",
				DRMDefecto = JuegoDRM.AmazonLuna,
				AdminInteractuar = true,
				UsuarioEnlacesEspecificos = false,
				ParaSiempre = false,
				Precio = 9.99,
				AdminPendientes = true,
				TablaPendientes = "suscripcionamazonlunaplus"
			};

			return amazon;
		}

		public static async Task Buscar(SqlConnection conexion)
		{
			BaseDatos.Admin.Actualizar.Tiendas(Generar().Id.ToString().ToLower(), DateTime.Now, "0 suscripciones detectadas", conexion);

			int cantidad = 0;

			HttpClient cliente = new HttpClient();
			cliente.BaseAddress = new Uri("https://luna.amazon.es/");
			cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

			string peticionEnBruto = "{\"timeout\":10000,\"featureScheme\":\"WEB_V1\",\"pageContext\":{\"pageUri\":\"subscription/luna-plus/B085TRCCT6\",\"pageId\":\"default\"},\"cacheKey\":\"02ac1d69-94ae-406c-8a98-dd45d7af25e4\",\"clientContext\":{\"browserMetadata\":{\"browserType\":\"Firefox\",\"browserVersion\":\"133.0\",\"deviceModel\":\"rv:133.0\",\"deviceType\":\"unknown\",\"osName\":\"Windows\",\"osVersion\":\"10\"}},\"inputContext\":{\"gamepadTypes\":[]},\"dynamicFeatures\":[]}";

			HttpRequestMessage peticion = new HttpRequestMessage(HttpMethod.Post, "https://proxy-prod.eu-west-1.tempo.digital.a2z.com/getPage")
			{
				Content = new StringContent(peticionEnBruto, Encoding.UTF8, "application/json"),
				Headers = { { "x-amz-locale", "es_ES" },
							{ "x-amz-platform", "web" }
					}
			};

			HttpResponseMessage respuesta = await cliente.SendAsync(peticion);

			string html = string.Empty;

			try
			{
				html = await respuesta.Content.ReadAsStringAsync();
			}
			catch { }

			if (string.IsNullOrEmpty(html) == false)
			{
				AmazonLunaPlusAPI api = JsonSerializer.Deserialize<AmazonLunaPlusAPI>(html);

				if (api != null)
				{
					foreach (var juego in api.Datos.Contenido.Widgets[3].Widgets)
					{
						if (string.IsNullOrEmpty(juego.Json) == false)
						{
							AmazonLunaPlusAPIJuego apiJuego = JsonSerializer.Deserialize<AmazonLunaPlusAPIJuego>(juego.Json);

							if (apiJuego != null)
							{
								string enlace = apiJuego.Id;

								bool encontrado = false;

								string sqlBuscar = "SELECT idJuegos FROM " + Generar().TablaPendientes + " WHERE enlace=@enlace";

								using (SqlCommand comando = new SqlCommand(sqlBuscar, conexion))
								{
									comando.Parameters.AddWithValue("@enlace", enlace);

									using (SqlDataReader lector = comando.ExecuteReader())
									{
										if (lector.Read() == true)
										{
											cantidad += 1;
											BaseDatos.Admin.Actualizar.Tiendas(Generar().Id.ToString().ToLower(), DateTime.Now, cantidad.ToString() + " suscripciones detectadas", conexion);

											if (lector.IsDBNull(0) == false)
											{
												if (string.IsNullOrEmpty(lector.GetString(0)) == false)
												{
													string idJuegosTexto = lector.GetString(0);

													encontrado = true;

													if (idJuegosTexto != "0")
													{
														List<string> idJuegos = Herramientas.Listados.Generar(idJuegosTexto);

														if (idJuegos.Count > 0)
														{
															foreach (var id in idJuegos)
															{
																Juego juegobd = BaseDatos.Juegos.Buscar.UnJuego(int.Parse(id));

																if (juegobd != null)
																{
																	bool añadirSuscripcion = true;

																	if (juegobd.Suscripciones != null)
																	{
																		if (juegobd.Suscripciones.Count > 0)
																		{
																			bool actualizar = false;

																			foreach (var suscripcion in juegobd.Suscripciones)
																			{
																				if (suscripcion.Tipo == Suscripciones2.SuscripcionTipo.AmazonLunaPlus)
																				{
																					añadirSuscripcion = false;
																					actualizar = true;

																					DateTime nuevaFecha = suscripcion.FechaTermina;
																					nuevaFecha = DateTime.Now;
																					nuevaFecha = nuevaFecha + TimeSpan.FromDays(1);
																					suscripcion.FechaTermina = nuevaFecha;
																				}
																			}

																			if (actualizar == true)
																			{
																				BaseDatos.Juegos.Actualizar.Suscripciones(juegobd, conexion);

																				JuegoSuscripcion suscripcion2 = BaseDatos.Suscripciones.Buscar.UnJuego(enlace);

																				if (suscripcion2 != null)
																				{
																					DateTime nuevaFecha = suscripcion2.FechaTermina;
																					nuevaFecha = DateTime.Now;
																					nuevaFecha = nuevaFecha + TimeSpan.FromDays(1);
																					suscripcion2.FechaTermina = nuevaFecha;
																					BaseDatos.Suscripciones.Actualizar.FechaTermina(suscripcion2, conexion);
																				}
																			}
																		}
																	}

																	if (añadirSuscripcion == true)
																	{
																		DateTime nuevaFecha = DateTime.Now;
																		nuevaFecha = nuevaFecha + TimeSpan.FromDays(1);

																		JuegoSuscripcion nuevaSuscripcion = new JuegoSuscripcion
																		{
																			DRM = JuegoDRM.AmazonLuna,
																			Nombre = juegobd.Nombre,
																			FechaEmpieza = DateTime.Now,
																			FechaTermina = nuevaFecha,
																			Imagen = juegobd.Imagenes.Header_460x215,
																			ImagenNoticia = juegobd.Imagenes.Header_460x215,
																			JuegoId = juegobd.Id,
																			Enlace = enlace,
																			Tipo = Suscripciones2.SuscripcionTipo.AmazonLunaPlus
																		};

																		if (juegobd.Suscripciones == null)
																		{
																			juegobd.Suscripciones = new List<JuegoSuscripcion>();
																		}

																		juegobd.Suscripciones.Add(nuevaSuscripcion);

																		BaseDatos.Suscripciones.Insertar.Ejecutar(juegobd.Id, juegobd.Suscripciones, nuevaSuscripcion, conexion);
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

								if (encontrado == false)
								{
									BaseDatos.Suscripciones.Insertar.Temporal(conexion, Generar().Id.ToString().ToLower(), enlace, apiJuego.Nombre, apiJuego.Imagen);
								}
							}
						}
					}
				}
			}
		}
	}

	public class AmazonLunaPlusAPI
	{
		[JsonPropertyName("pageMemberGroups")]
		public AmazonLunaPlusAPIDatos Datos { get; set; }
	}

	public class AmazonLunaPlusAPIDatos
	{
		[JsonPropertyName("mainContent")]
		public AmazonLunaPlusAPIContenido Contenido { get; set; }
	}

	public class AmazonLunaPlusAPIContenido
	{
		[JsonPropertyName("widgets")]
		public List<AmazonLunaPlusAPIWidget> Widgets { get; set; }
	}

	public class AmazonLunaPlusAPIWidget
	{
		[JsonPropertyName("widgets")]
		public List<AmazonLunaPlusAPIWidget> Widgets { get; set; }

		[JsonPropertyName("presentationData")]
		public string Json { get; set; }
	}

	public class AmazonLunaPlusAPIJuego
	{
		[JsonPropertyName("gameId")]
		public string Id { get; set; }

		[JsonPropertyName("title")]
		public string Nombre { get; set; }

		[JsonPropertyName("imagePortrait")]
		public string Imagen { get; set; }
	}
}
