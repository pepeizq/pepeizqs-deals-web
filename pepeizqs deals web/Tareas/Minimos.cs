#nullable disable

using BaseDatos.Juegos;
using BaseDatos.Tiendas;
using Herramientas;
using Juegos;
using Microsoft.Data.SqlClient;

namespace Tareas
{
	public class Minimos
	{
		public static async Task Ejecutar(SqlConnection conexion)
		{
			await Task.Delay(1000);

			TimeSpan tiempoSiguiente = TimeSpan.FromMinutes(10);

			if (Admin.ComprobarTareaUso(conexion, "minimos", tiempoSiguiente) == true)
			{
				Admin.ActualizarTareaUso(conexion, "minimos", DateTime.Now);

				List<Juego> juegos = Buscar.Todos(conexion, "juegos");

				if (juegos != null)
				{
					if (juegos.Count > 0)
					{
						List<Juego> juegosConMinimos = new List<Juego>();

						foreach (var juego in juegos)
						{
							if (juego != null)
							{
								if (juego.Analisis != null)
								{
									if (string.IsNullOrEmpty(juego.Analisis.Porcentaje) == false && string.IsNullOrEmpty(juego.Analisis.Cantidad) == false)
									{
										if (juego.PrecioMinimosHistoricos != null)
										{
											for (int i = 0; i < juego.PrecioMinimosHistoricos.Count; i += 1)
											{
												bool añadir = false;

												if (JuegoFicha.CalcularAntiguedad(juego.PrecioMinimosHistoricos[i]) == true)
												{
													añadir = true;
												}

												if (añadir == true)
												{
													Juego nuevoHistorico = juego;
													nuevoHistorico.PrecioMinimosHistoricos = [juego.PrecioMinimosHistoricos[i]];

													juegosConMinimos.Add(nuevoHistorico);
												}
											}
										}
									}
								}
							}
						}

						if (juegosConMinimos != null)
						{
							if (juegosConMinimos.Count > 0)
							{
								juegosConMinimos.Sort(delegate (Juego j1, Juego j2)
								{
									JuegoPrecio j1Oferta = null;

									if (j1.PrecioMinimosHistoricos.Count > 0)
									{
										j1Oferta = j1.PrecioMinimosHistoricos[0];
									}

									JuegoPrecio j2Oferta = null;

									if (j2.PrecioMinimosHistoricos.Count > 0)
									{
										j2Oferta = j2.PrecioMinimosHistoricos[0];
									}

									if (j1Oferta != null && j2Oferta != null)
									{
										return j2Oferta.FechaDetectado.CompareTo(j1Oferta.FechaDetectado);
									}
									else
									{
										if (j1Oferta == null && j2Oferta != null)
										{
											return 1;
										}
										else if (j1Oferta != null && j2Oferta == null)
										{
											return -1;
										}
									}

									return 0;
								});

								#region Destacados

								List<Juego> juegosDestacadosMostrar = new List<Juego>();

								int i = 0;

								foreach (var minimo in juegosConMinimos)
								{
									bool añadir = true;

									if (minimo.Analisis == null)
									{
										añadir = false;
									}
									else if (minimo.Tipo != JuegoTipo.Game)
									{
										añadir = false;
									}
									else if (minimo.Bundles != null)
									{
										añadir = false;
									}
									else if (minimo.Gratis != null)
									{
										añadir = false;
									}
									else if (minimo.Suscripciones != null)
									{
										añadir = false;
									}
									else if (string.IsNullOrEmpty(minimo.FreeToPlay) == false)
									{
										if (minimo.FreeToPlay.ToLower() == "true")
										{
											añadir = false;
										}
									}

									if (añadir == true && minimo != null)
									{
										if (minimo.Analisis != null)
										{
											if (string.IsNullOrEmpty(minimo.Analisis.Cantidad) == false)
											{
												string tempCantidad = minimo.Analisis.Cantidad;
												tempCantidad = tempCantidad.Replace(".", null);
												tempCantidad = tempCantidad.Replace(",", null);

												if (Convert.ToInt32(tempCantidad) >= 5000)
												{
													if (i < 60)
													{
														juegosDestacadosMostrar.Add(minimo);
														i += 1;
													}
												}
											}
										}
									}
								}

								if (juegosDestacadosMostrar.Count > 0)
								{
									BaseDatos.Portada.Limpiar.Ejecutar("portadaJuegosDestacados", conexion);

									foreach (var juego in juegosDestacadosMostrar)
									{
										BaseDatos.Portada.Insertar.Juego(juego, "portadaJuegosDestacados", conexion);
									}
								}

								#endregion

								#region Minimos

								List<Juego> juegosMinimosMostrar = new List<Juego>();

								foreach (var minimo in juegosConMinimos)
								{
									bool descarte1 = false;

									if (juegosDestacadosMostrar.Count > 0)
									{
										foreach (var destacado in juegosDestacadosMostrar)
										{
											if (destacado.IdMaestra == minimo.IdMaestra)
											{
												descarte1 = true;
											}
										}
									}

									if (descarte1 == false)
									{
										bool descarte2 = false;

										if (minimo.Analisis == null)
										{
											descarte2 = true;
										}
										else
										{
											if (string.IsNullOrEmpty(minimo.Analisis.Cantidad) == false)
											{
												string tempCantidad = minimo.Analisis.Cantidad;
												tempCantidad = tempCantidad.Replace(".", null);
												tempCantidad = tempCantidad.Replace(",", null);

												if (Convert.ToInt32(tempCantidad) < 500)
												{
													descarte2 = true;
												}
											}
											else
											{
												descarte2 = true;
											}
										}

										if (descarte2 == false)
										{
											bool descarte3 = false;

											if (minimo.Gratis != null)
											{
												if (minimo.Gratis.Count > 0)
												{
													foreach (var gratis in minimo.Gratis)
													{
														if (DateTime.Now >= gratis.FechaEmpieza && DateTime.Now <= gratis.FechaTermina)
														{
															descarte3 = true;
														}
													}
												}
											}

											if (descarte3 == false)
											{
												bool descarte4 = false;

												if (string.IsNullOrEmpty(minimo.FreeToPlay) == false)
												{
													if (minimo.FreeToPlay.ToLower() == "true")
													{
														descarte4 = true;
													}
												}

												if (descarte4 == false)
												{
													juegosMinimosMostrar.Add(minimo);

													if (juegosMinimosMostrar.Count == 100)
													{
														break;
													}
												}
											}
										}
									}
								}

								if (juegosMinimosMostrar.Count > 0)
								{
									BaseDatos.Portada.Limpiar.Ejecutar("portadaJuegosMinimos", conexion);

									foreach (var juego in juegosMinimosMostrar)
									{
										BaseDatos.Portada.Insertar.Juego(juego, "portadaJuegosMinimos", conexion);
									}
								}

								if (juegosConMinimos.Count > 0)
								{
									BaseDatos.Portada.Limpiar.Ejecutar("seccionMinimos", conexion);

									foreach (var juego in juegosConMinimos)
									{
										BaseDatos.Portada.Insertar.Juego(juego, "seccionMinimos", conexion);
									}
								}

								#endregion
							}
						}
					}
				}
			}
		}
	}
}
