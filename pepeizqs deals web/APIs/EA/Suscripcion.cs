#nullable disable

using Herramientas;
using Juegos;
using Microsoft.Data.SqlClient;
using System.Text.Json;

namespace APIs.EA
{
	public static class Suscripcion
	{
		public static Suscripciones2.Suscripcion Generar()
		{
			Suscripciones2.Suscripcion eaPlay = new Suscripciones2.Suscripcion
			{
				Id = Suscripciones2.SuscripcionTipo.EAPlay,
				Nombre = "EA Play",
				ImagenLogo = "/imagenes/suscripciones/eaplay.webp",
				ImagenIcono = "/imagenes/tiendas/ea_icono.webp",
				Enlace = "https://www.ea.com/ea-play",
				DRMDefecto = JuegoDRM.EA,
				AdminInteractuar = true,
				UsuarioEnlacesEspecificos = false,
				ParaSiempre = false,
				Precio = 5.99,
				AdminPendientes = true,
				TablaPendientes = "tiendaea"
			};

			return eaPlay;
		}

		public static Suscripciones2.Suscripcion GenerarPro()
		{
			Suscripciones2.Suscripcion eaPlayPro = new Suscripciones2.Suscripcion
			{
				Id = Suscripciones2.SuscripcionTipo.EAPlayPro,
				Nombre = "EA Play Pro",
				ImagenLogo = "/imagenes/suscripciones/eaplaypro.webp",
				ImagenIcono = "/imagenes/tiendas/ea_icono.webp",
				Enlace = "https://www.ea.com/ea-play",
				DRMDefecto = JuegoDRM.EA,
				AdminInteractuar = true,
				UsuarioEnlacesEspecificos = false,
				ParaSiempre = false,
				IncluyeSuscripcion = Suscripciones2.SuscripcionTipo.EAPlay,
                Precio = 16.99,
                AdminPendientes = false,
                TablaPendientes = "tiendaea"
            };

			return eaPlayPro;
		}

		public static async Task Buscar(SqlConnection conexion)
		{
			BaseDatos.Admin.Actualizar.Tiendas(Generar().Id.ToString().ToLower(), DateTime.Now, "0", conexion);

			int cantidad = 0;

			string html = await Decompiladores.Estandar("https://api3.origin.com/supercat/GB/en_GB/supercat-PCWIN_MAC-GB-en_GB.json.gz");

			if (string.IsNullOrEmpty(html) == false)
			{
				EABD basedatos = JsonSerializer.Deserialize<EABD>(html);

				if (basedatos != null)
				{
					if (basedatos.Juegos != null)
					{
						if (basedatos.Juegos.Count > 0)
						{
							foreach (var juego in basedatos.Juegos)
							{
								bool tieneSuscripcion = false;
								bool tieneSuscripcionPremium = false;

								if (juego.Suscripcion != null)
								{
									bool dentroDeFecha = false;

									if (string.IsNullOrEmpty(juego.Suscripcion.FechaAcaba) == false)
									{
										dentroDeFecha = true;
									}
									else
									{
										try
										{
											DateTime fecha = DateTime.Parse(juego.Suscripcion.FechaAcaba);

											if (fecha > DateTime.Now)
											{
												dentroDeFecha = true;
											}
										}
										catch 
										{
											dentroDeFecha = true;
										}
									}

									if (dentroDeFecha == true)
									{
										if (string.IsNullOrEmpty(juego.Suscripcion.Enlace) == false)
										{
											tieneSuscripcion = true;
										}
									}									
								}

								if (tieneSuscripcion == false)
								{
									if (juego.SuscripcionPremium != null)
									{
										bool dentroDeFecha = false;

										if (string.IsNullOrEmpty(juego.SuscripcionPremium.FechaAcaba) == false)
										{
											dentroDeFecha = true;
										}
										else
										{
											try
											{
												DateTime fecha = DateTime.Parse(juego.SuscripcionPremium.FechaAcaba);

												if (fecha > DateTime.Now)
												{
													dentroDeFecha = true;
												}
											}
											catch
											{
												dentroDeFecha = true;
											}
										}

										if (dentroDeFecha == true)
										{
											if (string.IsNullOrEmpty(juego.SuscripcionPremium.Enlace) == false)
											{
												tieneSuscripcionPremium = true;
											}
										}
									}
								}

								if (tieneSuscripcion == true || tieneSuscripcionPremium == true)
								{
									string enlace = string.Empty;

									if (tieneSuscripcion == true)
									{
										enlace = "https://www.ea.com/games" + juego.Suscripcion.Enlace;
									}

									if (tieneSuscripcionPremium == true)
									{
										enlace = "https://www.ea.com/games" + juego.SuscripcionPremium.Enlace;
									}

									bool encontrado = false;

									string sqlBuscar = "SELECT idJuegos FROM tiendaea WHERE enlace=@enlace";

									using (SqlCommand comando = new SqlCommand(sqlBuscar, conexion))
									{
										comando.Parameters.AddWithValue("@enlace", enlace);

										using (SqlDataReader lector = comando.ExecuteReader())
										{
											if (lector.Read() == true)
											{
												cantidad += 1;
												BaseDatos.Admin.Actualizar.Tiendas(Generar().Id.ToString().ToLower(), DateTime.Now, cantidad.ToString(), conexion);

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
																					if (suscripcion.Tipo == Suscripciones2.SuscripcionTipo.EAPlay && tieneSuscripcion == true)
																					{
																						añadirSuscripcion = false;
																						actualizar = true;

																						DateTime nuevaFecha = suscripcion.FechaTermina;
																						nuevaFecha = DateTime.Now;
																						nuevaFecha = nuevaFecha + TimeSpan.FromDays(1);
																						suscripcion.FechaTermina = nuevaFecha;
																					}
																					else
																					{
																						if (suscripcion.Tipo == Suscripciones2.SuscripcionTipo.EAPlayPro && tieneSuscripcionPremium == true)
																						{
																							añadirSuscripcion = false;
																							actualizar = true;

																							DateTime nuevaFecha = suscripcion.FechaTermina;
																							nuevaFecha = DateTime.Now;
																							nuevaFecha = nuevaFecha + TimeSpan.FromDays(1);
																							suscripcion.FechaTermina = nuevaFecha;
																						}
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
																				DRM = JuegoDRM.EA,
																				Nombre = juego.i18n.Titulo,
																				FechaEmpieza = DateTime.Now,
																				FechaTermina = nuevaFecha,
																				Imagen = juegobd.Imagenes.Header_460x215,
																				ImagenNoticia = juegobd.Imagenes.Header_460x215,
																				JuegoId = juegobd.Id
																			};

																			if (tieneSuscripcion == true)
																			{
																				nuevaSuscripcion.Tipo = Suscripciones2.SuscripcionTipo.EAPlay;
																				nuevaSuscripcion.Enlace = enlace;
																			}
																			else
																			{
																				if (tieneSuscripcionPremium == true)
																				{
																					nuevaSuscripcion.Tipo = Suscripciones2.SuscripcionTipo.EAPlayPro;
																					nuevaSuscripcion.Enlace = enlace;
																				}
																			}

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
										if (tieneSuscripcion == true)
										{
                                            BaseDatos.Suscripciones.Insertar.Temporal(conexion, Generar().Id.ToString().ToLower(), enlace, juego.i18n.Titulo);
										}
										else
										{
											if (tieneSuscripcionPremium == true)
											{
                                                BaseDatos.Suscripciones.Insertar.Temporal(conexion, GenerarPro().Id.ToString().ToLower(), enlace, juego.i18n.Titulo);
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
