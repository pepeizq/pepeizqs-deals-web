#nullable disable

using Juegos;
using Microsoft.Data.SqlClient;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace APIs.Ubisoft
{
    public static class Suscripcion
    {
        public static Suscripciones2.Suscripcion Generar()
        {
            Suscripciones2.Suscripcion ubisoft = new Suscripciones2.Suscripcion
            {
                Id = Suscripciones2.SuscripcionTipo.UbisoftPlusClassics,
                Nombre = "Ubisoft+ Classics",
                ImagenLogo = "/imagenes/suscripciones/ubisoftplusclassics.webp",
                ImagenIcono = "/imagenes/tiendas/ubisoft_icono.webp",
                Enlace = "https://store.ubisoft.com/ubisoftplus",
                DRMDefecto = JuegoDRM.Ubisoft,
                AdminInteractuar = true,
                UsuarioEnlacesEspecificos = false,
                ParaSiempre = false,
                Precio = 7.99,
                AdminPendientes = true,
                TablaPendientes = "tiendaubisoft"
            };

            return ubisoft;
        }

        public static Suscripciones2.Suscripcion GenerarPremium()
        {
            Suscripciones2.Suscripcion ubisoft = new Suscripciones2.Suscripcion
            {
                Id = Suscripciones2.SuscripcionTipo.UbisoftPlusPremium,
                Nombre = "Ubisoft+ Premium",
                ImagenLogo = "/imagenes/suscripciones/ubisoftpluspremium.webp",
                ImagenIcono = "/imagenes/tiendas/ubisoft_icono.webp",
                Enlace = "https://store.ubisoft.com/ubisoftplus",
                DRMDefecto = JuegoDRM.Ubisoft,
                AdminInteractuar = true,
                UsuarioEnlacesEspecificos = false,
                ParaSiempre = false,
                Precio = 17.99,
                IncluyeSuscripcion = Suscripciones2.SuscripcionTipo.UbisoftPlusClassics,
                AdminPendientes = true,
                TablaPendientes = "tiendaubisoft"
            };

            return ubisoft;
        }

        public static async Task Buscar(SqlConnection conexion)
        {
            BaseDatos.Tiendas.Admin.Actualizar(Generar().Id.ToString().ToLower(), DateTime.Now, "0 suscripciones detectadas", conexion);

            int cantidad = 0;

            HttpClient cliente = new HttpClient();
            cliente.BaseAddress = new Uri("https://store.ubisoft.com/");
            cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpRequestMessage peticion = new HttpRequestMessage(HttpMethod.Post, "https://xely3u4lod-dsn.algolia.net/1/indexes/production__us_ubisoft__products__en_US__release_date/query?x-algolia-agent=Algolia for JavaScript (4.13.1); Browser (lite)&x-algolia-api-key=5638539fd9edb8f2c6b024b49ec375bd&x-algolia-application-id=XELY3U4LOD");
            peticion.Content = new StringContent("{\"query\":\"\",\"attributesToRetrieve\":[\"title\",\"image_link\",\"short_title\",\"id\",\"MasterID\",\"Genre\",\"release_date\",\"partOfUbisoftPlus\",\"anywherePlatforms\",\"subscriptionExpirationDate\",\"Edition\",\"adult\",\"partofSubscriptionOffer\"],\"hitsPerPage\":9999,\"facetFilters\":[[\"partOfUbisoftPlus:true\"],[],[],[\"partofSubscriptionOfferID:5ebe5e920d253c3638a9521b\"]],\"clickAnalytics\":true}",
                                                Encoding.UTF8, "application/json");

            HttpResponseMessage respuesta = await cliente.SendAsync(peticion);

            string html = string.Empty;
            
            try
            {
                html = await respuesta.Content.ReadAsStringAsync();
            }
            catch { }
            
            if (string.IsNullOrEmpty(html) == false)
            {
                UbisoftSuscripcion datos = JsonSerializer.Deserialize<UbisoftSuscripcion>(html);

                if (datos != null)
                {
                    foreach (var juego in datos.Juegos)
                    {
                        string enlace = "https://store.ubisoft.com/" + juego.Id + ".html";

                        bool encontrado = false;

                        string sqlBuscar = "SELECT idJuegos FROM tiendaubisoft WHERE enlace=@enlace";

                        using (SqlCommand comando = new SqlCommand(sqlBuscar, conexion))
                        {
                            comando.Parameters.AddWithValue("@enlace", enlace);

                            using (SqlDataReader lector = comando.ExecuteReader())
                            {
                                if (lector.Read() == true)
                                {
                                    cantidad += 1;

                                    BaseDatos.Tiendas.Admin.Actualizar(Generar().Id.ToString(), DateTime.Now, cantidad.ToString() + " suscripciones detectadas", conexion);

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
                                                                        if (suscripcion.Tipo == Suscripciones2.SuscripcionTipo.UbisoftPlusClassics)
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
                                                                    DRM = JuegoDRM.Ubisoft,
                                                                    Nombre = juegobd.Nombre,
                                                                    FechaEmpieza = DateTime.Now,
                                                                    FechaTermina = nuevaFecha,
                                                                    Imagen = juegobd.Imagenes.Header_460x215,
                                                                    ImagenNoticia = juegobd.Imagenes.Header_460x215,
                                                                    JuegoId = juegobd.Id,
                                                                    Enlace = enlace,
                                                                    Tipo = Suscripciones2.SuscripcionTipo.UbisoftPlusClassics
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

                            if (encontrado == false)
                            {
                                BaseDatos.Suscripciones.Insertar.Temporal(conexion, Generar().Id.ToString().ToLower(), enlace, juego.Nombre, juego.Imagen);
                            }
                        }
                    }
                }
            }
        }

		public static async Task BuscarPremium(SqlConnection conexion)
		{
			BaseDatos.Tiendas.Admin.Actualizar(GenerarPremium().Id.ToString().ToLower(), DateTime.Now, "0 suscripciones detectadas", conexion);

			int cantidad = 0;

			HttpClient cliente = new HttpClient();
			cliente.BaseAddress = new Uri("https://store.ubisoft.com/");
			cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

			HttpRequestMessage peticion = new HttpRequestMessage(HttpMethod.Post, "https://xely3u4lod-dsn.algolia.net/1/indexes/production__us_ubisoft__products__en_US__release_date/query?x-algolia-agent=Algolia for JavaScript (4.13.1); Browser (lite)&x-algolia-api-key=5638539fd9edb8f2c6b024b49ec375bd&x-algolia-application-id=XELY3U4LOD");
			peticion.Content = new StringContent("{\"query\":\"\",\"attributesToRetrieve\":[\"title\",\"image_link\",\"short_title\",\"id\",\"MasterID\",\"Genre\",\"release_date\",\"partOfUbisoftPlus\",\"anywherePlatforms\",\"subscriptionExpirationDate\",\"Edition\",\"adult\",\"partofSubscriptionOffer\"],\"hitsPerPage\":9999,\"facetFilters\":[[\"partOfUbisoftPlus:true\"],[],[],[\"partofSubscriptionOfferID:5f44de7b5cdf9a0c2027ca78\"]],\"clickAnalytics\":true}",
												Encoding.UTF8, "application/json");

			HttpResponseMessage respuesta = await cliente.SendAsync(peticion);

			string html = string.Empty;

			try
			{
				html = await respuesta.Content.ReadAsStringAsync();
			}
			catch { }

			if (string.IsNullOrEmpty(html) == false)
			{
				UbisoftSuscripcion datos = JsonSerializer.Deserialize<UbisoftSuscripcion>(html);

				if (datos != null)
				{
					foreach (var juego in datos.Juegos)
					{
						string enlace = "https://store.ubisoft.com/" + juego.Id + ".html";

						bool encontrado = false;

						string sqlBuscar = "SELECT idJuegos FROM tiendaubisoft WHERE enlace=@enlace";

						using (SqlCommand comando = new SqlCommand(sqlBuscar, conexion))
						{
							comando.Parameters.AddWithValue("@enlace", enlace);

							using (SqlDataReader lector = comando.ExecuteReader())
							{
								if (lector.Read() == true)
								{
									cantidad += 1;

									BaseDatos.Tiendas.Admin.Actualizar(GenerarPremium().Id.ToString().ToLower(), DateTime.Now, cantidad.ToString() + " suscripciones detectadas", conexion);

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
																		if (suscripcion.Tipo == Suscripciones2.SuscripcionTipo.UbisoftPlusPremium)
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
																	DRM = JuegoDRM.Ubisoft,
																	Nombre = juegobd.Nombre,
																	FechaEmpieza = DateTime.Now,
																	FechaTermina = nuevaFecha,
																	Imagen = juegobd.Imagenes.Header_460x215,
																	ImagenNoticia = juegobd.Imagenes.Header_460x215,
																	JuegoId = juegobd.Id,
																	Enlace = enlace,
																	Tipo = Suscripciones2.SuscripcionTipo.UbisoftPlusPremium
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

							if (encontrado == false)
							{
                                BaseDatos.Suscripciones.Insertar.Temporal(conexion, GenerarPremium().Id.ToString(), enlace, juego.Nombre, juego.Imagen);
                            }
						}
					}
				}
			}
		}
	}

    public class UbisoftSuscripcion
    {
        [JsonPropertyName("hits")]
        public List<UbisoftSuscripcionJuego> Juegos { get; set; }
    }

    public class UbisoftSuscripcionJuego
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("title")]
        public string Nombre { get; set; }

        [JsonPropertyName("image_link")]
        public string Imagen { get; set; }
    }
}
