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
				AdminInteracturar = false
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
				AdminInteracturar = false
			};

			return eaPlayPro;
		}

		public static async Task Buscar(SqlConnection conexion)
		{
			BaseDatos.Tiendas.Admin.Actualizar("eaplay", DateTime.Now, "0 suscripciones detectadas", conexion);

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
										Da
									}

									if (string.IsNullOrEmpty(juego.Suscripcion.Enlace) == false)
									{
										tieneSuscripcion = true;
									}
								}

								if (juego.SuscripcionPremium != null)
								{
									if (string.IsNullOrEmpty(juego.SuscripcionPremium.Enlace) == false)
									{
										tieneSuscripcionPremium = true;
									}
								}

								if (tieneSuscripcion == true || tieneSuscripcionPremium == true)
								{
									string sqlBuscar = "SELECT idJuegos from tiendaEa WHERE enlace=@enlace";

									using (SqlCommand comando = new SqlCommand(sqlBuscar, conexion))
									{
										comando.Parameters.AddWithValue("@enlace", "https://www.origin.com/store" + juego.Enlace);

										using (SqlDataReader lector = comando.ExecuteReader())
										{
											if (lector.Read() == true)
											{
												cantidad += 1;
												BaseDatos.Tiendas.Admin.Actualizar("eaplay", DateTime.Now, cantidad.ToString() + " suscripciones detectadas", conexion);

												if (lector.IsDBNull(0) == false)
												{
													if (string.IsNullOrEmpty(lector.GetString(0)) == false)
													{
														string idJuegosTexto = lector.GetString(0);

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

																			if (actualizar == true)
																			{
																				BaseDatos.Juegos.Actualizar.Suscripciones(juegobd, conexion);

																				if (tieneSuscripcion == true)
																				{
																					JuegoSuscripcion suscripcion = BaseDatos.Suscripciones.Buscar.UnJuego(juego.Suscripcion.Enlace);
																					BaseDatos.Suscripciones.Actualizar.FechaTermina(suscripcion, conexion);
																				}

																				if (tieneSuscripcionPremium == true)
																				{
																					JuegoSuscripcion suscripcion = BaseDatos.Suscripciones.Buscar.UnJuego(juego.SuscripcionPremium.Enlace);
																					BaseDatos.Suscripciones.Actualizar.FechaTermina(suscripcion, conexion);
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
																			Nombre = juego.Titulo,
																			FechaEmpieza = DateTime.Now,
																			FechaTermina = nuevaFecha,
																			Imagen = juegobd.Imagenes.Header_460x215
																		};

																		if (tieneSuscripcion == true)
																		{
																			nuevaSuscripcion.Tipo = Suscripciones2.SuscripcionTipo.EAPlay;
																			nuevaSuscripcion.Enlace = juego.Suscripcion.Enlace;
																		}

																		if (tieneSuscripcionPremium == true)
																		{
																			nuevaSuscripcion.Tipo = Suscripciones2.SuscripcionTipo.EAPlayPro;
																			nuevaSuscripcion.Enlace = juego.SuscripcionPremium.Enlace;
																		}

																		BaseDatos.Suscripciones.Insertar.Ejecutar(juegobd.Id, juegobd.Suscripciones, nuevaSuscripcion, conexion);
																	}
																}
															}
														}
													}
												}
											}
											else
											{
												if (tieneSuscripcion == true)
												{
													BaseDatos.Errores.Insertar.Mensaje("Suscripcion no encontrada", juego.i18n.Titulo + " " + juego.Suscripcion.Enlace);
												}
												
												if (tieneSuscripcionPremium == true)
												{
													BaseDatos.Errores.Insertar.Mensaje("Suscripcion no encontrada", juego.i18n.Titulo + " " + juego.SuscripcionPremium.Enlace);
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
