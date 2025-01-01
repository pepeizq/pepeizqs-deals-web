#nullable disable

using Herramientas;
using Juegos;
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace APIs.XboxGamePass
{
    public static class Suscripcion
    {
        public static Suscripciones2.Suscripcion Generar()
        {
            Suscripciones2.Suscripcion gamepass = new Suscripciones2.Suscripcion
            {
                Id = Suscripciones2.SuscripcionTipo.PCGamePass,
                Nombre = "PC Game Pass",
                ImagenLogo = "/imagenes/suscripciones/pcgamepass.webp",
                ImagenIcono = "/imagenes/suscripciones/pcgamepass_icono.webp",
                Enlace = "https://www.xbox.com/xbox-game-pass/pc-game-pass",
                DRMDefecto = JuegoDRM.Microsoft,
                AdminInteractuar = true,
                UsuarioEnlacesEspecificos = true,
                ParaSiempre = false,
                Precio = 11.99,
				AdminPendientes = true,
				TablaPendientes = "tiendamicrosoftstore"
            };

            return gamepass;
        }

        public static async Task Buscar(SqlConnection conexion)
        {
			BaseDatos.Admin.Actualizar.Tiendas(Generar().Id.ToString().ToLower(), DateTime.Now, 0, conexion);

            int cantidad = 0;

			await Task.Delay(5000);

            string html = await Decompiladores.Estandar("https://catalog.gamepass.com/sigls/v2?id=fdd9e2a7-0fee-49f6-ad69-4354098401ff&language=en-us&market=US");

            if (string.IsNullOrEmpty(html) == false)
            {
                int int1 = html.IndexOf("{" + Strings.ChrW(34) + "id" + Strings.ChrW(34));
                string temp1 = html.Remove(0, int1);
                html = "[" + temp1;

                List<XboxGamePassJuego> juegos = JsonSerializer.Deserialize<List<XboxGamePassJuego>>(html);

                if (juegos != null)
                {
                    foreach (var juego in juegos)
                    {
                        string enlace = "https://www.microsoft.com/store/productid/" + juego.Id;

						bool encontrado = false;

						string sqlBuscar = "SELECT idJuegos FROM tiendamicrosoftstore WHERE enlace=@enlace";

                        using (SqlCommand comando = new SqlCommand(sqlBuscar, conexion))
                        {
                            comando.Parameters.AddWithValue("@enlace", enlace);

                            using (SqlDataReader lector = comando.ExecuteReader())
                            {
                                if (lector.Read() == true)
                                {
									cantidad += 1;
									BaseDatos.Admin.Actualizar.Tiendas(Generar().Id.ToString().ToLower(), DateTime.Now, cantidad, conexion);

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
																		if (suscripcion.Tipo == Suscripciones2.SuscripcionTipo.PCGamePass)
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
																	DRM = JuegoDRM.Microsoft,
																	Nombre = juegobd.Nombre,
																	FechaEmpieza = DateTime.Now,
																	FechaTermina = nuevaFecha,
																	Imagen = juegobd.Imagenes.Header_460x215,
																	ImagenNoticia = juegobd.Imagenes.Header_460x215,
																	JuegoId = juegobd.Id,
																	Enlace = enlace,
																	Tipo = Suscripciones2.SuscripcionTipo.PCGamePass
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
							BaseDatos.Suscripciones.Insertar.Temporal(conexion, Generar().Id.ToString().ToLower(), enlace);
						}
					}
                }
			}
        }
    }

    public class XboxGamePassJuego
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
    }
}
