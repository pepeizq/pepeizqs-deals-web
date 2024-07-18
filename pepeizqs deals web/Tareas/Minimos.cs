#nullable disable

using BaseDatos.Juegos;
using BaseDatos.Tiendas;
using Herramientas;
using Juegos;
using Microsoft.Data.SqlClient;

namespace Tareas
{
    public class Minimos : BackgroundService
    {
        private readonly ILogger<Minimos> _logger;
        private readonly IServiceScopeFactory _factoria;
        private readonly IDecompiladores _decompilador;

        public Minimos(ILogger<Minimos> logger, IServiceScopeFactory factory, IDecompiladores decompilador)
        {
            _logger = logger;
            _factoria = factory;
            _decompilador = decompilador;
        }

		protected override async Task ExecuteAsync(CancellationToken tokenParar)
		{
			using PeriodicTimer timer = new PeriodicTimer(TimeSpan.FromSeconds(5));

			while (await timer.WaitForNextTickAsync(tokenParar))
			{
                WebApplicationBuilder builder = WebApplication.CreateBuilder();
                string piscinaApp = builder.Configuration.GetValue<string>("PoolWeb:Contenido");
                string piscinaUsada = Environment.GetEnvironmentVariable("APP_POOL_ID", EnvironmentVariableTarget.Process);

				if (piscinaApp == piscinaUsada)
				{
                    SqlConnection conexion = new SqlConnection();

                    try
                    {
                        conexion = Herramientas.BaseDatos.Conectar();
                    }
                    catch { }

                    if (conexion.State == System.Data.ConnectionState.Open)
                    {
                        try
                        {
                            TimeSpan tiempoSiguiente = TimeSpan.FromMinutes(20);

                            if (Admin.ComprobarTareaUso(conexion, "minimos", tiempoSiguiente) == true)
                            {
                                Admin.ActualizarTareaUso(conexion, "minimos", DateTime.Now);

                                List<Juego> juegos = Buscar.Todos(conexion, "juegos");

                                if (juegos != null)
                                {
                                    if (juegos.Count > 0)
                                    {
										List<MinimoListado> juegosConMinimos = new List<MinimoListado>();

                                        foreach (var juego in juegos)
                                        {
                                            if (juego != null)
                                            {
                                                if (juego.Analisis != null)
                                                {
                                                    if (string.IsNullOrEmpty(juego.Analisis.Porcentaje) == false && string.IsNullOrEmpty(juego.Analisis.Cantidad) == false)
                                                    {
                                                        if (juego.Analisis.Cantidad.Length > 2)
                                                        {
															if (juego.PrecioMinimosHistoricos != null)
															{
																if (juego.PrecioMinimosHistoricos.Count > 0)
																{
																	List<JuegoPrecio> historicosFinales = new List<JuegoPrecio>();

																	foreach (var historico in juego.PrecioMinimosHistoricos)
																	{
																		TimeSpan actualizado = DateTime.Now.Subtract(historico.FechaActualizacion);

																		if (actualizado.Days == 0)
																		{
																			bool añadir = true;

																			if (historico.DRM != JuegoDRM.Steam && historico.DRM != JuegoDRM.GOG && historico.DRM != JuegoDRM.EA && historico.DRM != JuegoDRM.Ubisoft)
																			{
																				añadir = false;
																			}

																			if (añadir == true)
																			{
																				historicosFinales.Add(historico);
																			}
																		}
																	}

																	if (historicosFinales.Count > 0)
																	{
																		foreach (var historicoFinal in historicosFinales)
																		{
																			Juego nuevoJuego = new Juego();
																			nuevoJuego = juego;
																			nuevoJuego.PrecioMinimosHistoricos = null;
																			nuevoJuego.PrecioActualesTiendas = null;
																			nuevoJuego.IdMaestra = juego.Id;

																			bool añadir = true;

																			if (string.IsNullOrEmpty(nuevoJuego.FreeToPlay) == false)
																			{
																				if (nuevoJuego.FreeToPlay.ToLower() == "true")
																				{
																					añadir = false;
																				}
																			}

																			if (string.IsNullOrEmpty(nuevoJuego.MayorEdad) == false)
																			{
																				if (nuevoJuego.MayorEdad.ToLower() == "true")
																				{
																					añadir = false;
																				}
																			}

																			if (nuevoJuego.Gratis != null)
																			{
																				if (nuevoJuego.Gratis.Count > 0)
																				{
																					foreach (var gratis in nuevoJuego.Gratis)
																					{
																						if (gratis.DRM == historicoFinal.DRM)
																						{
																							añadir = false;
																						}
																					}
																				}
																			}

																			if (añadir == true)
																			{
																				MinimoListado minimoListado = new MinimoListado();
																				minimoListado.Juego = nuevoJuego;
																				minimoListado.Historico = historicoFinal;

																				juegosConMinimos.Add(minimoListado);
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

                                        if (juegosConMinimos != null)
                                        {
                                            if (juegosConMinimos.Count > 0)
                                            {
												#region Destacados

												juegosConMinimos = juegosConMinimos.OrderByDescending(x => int.Parse(x.Juego.Analisis.Cantidad.Replace(",", null))).ThenBy(x => x.Juego.Nombre).ToList();							

												List<MinimoListado> juegosDestacadosMostrar = new List<MinimoListado>();

                                                int i = 0;

                                                foreach (var minimo in juegosConMinimos)
                                                {
                                                    bool añadir = true;

													if (minimo.Juego.Tipo != JuegoTipo.Game)
													{
														añadir = false;
													}
													else if (minimo.Historico.DRM != JuegoDRM.Steam)
													{
														añadir = false;
													}
													else if (minimo.Juego.Bundles != null)
													{
														foreach (var bundle in minimo.Juego.Bundles)
														{
															if (DateTime.Now >= bundle.FechaEmpieza && DateTime.Now <= bundle.FechaTermina)
															{
																añadir = false;
																break;
															}
														}
													}
													else if (minimo.Juego.Gratis != null)
													{
														foreach (var gratis in minimo.Juego.Gratis)
														{
															if (DateTime.Now >= gratis.FechaEmpieza && DateTime.Now <= gratis.FechaTermina)
															{
																añadir = false;
																break;
															}
														}
													}
													else if (minimo.Juego.Suscripciones != null)
													{
														foreach (var suscripcion in minimo.Juego.Suscripciones)
														{
															if (DateTime.Now >= suscripcion.FechaEmpieza && DateTime.Now <= suscripcion.FechaTermina)
															{
																añadir = false;
																break;
															}
														}
													}
													
                                                    if (añadir == true && minimo != null)
                                                    {
														string tempCantidad = minimo.Juego.Analisis.Cantidad;
														tempCantidad = tempCantidad.Replace(".", null);
														tempCantidad = tempCantidad.Replace(",", null);

														if (Convert.ToInt32(tempCantidad) >= 5000)
														{
															if (i < 150)
															{
																juegosDestacadosMostrar.Add(minimo);
																i += 1;

																if (i == 150)
																{
																	break;
																}
															}
														}
													}
                                                }

                                                if (juegosDestacadosMostrar.Count > 0)
                                                {
                                                    BaseDatos.Portada.Limpiar.Ejecutar("portadaJuegosDestacados", conexion);

                                                    foreach (var juegoDestacado in juegosDestacadosMostrar)
                                                    {
														Juego juegoDestacadoFinal = juegoDestacado.Juego;
														juegoDestacadoFinal.PrecioMinimosHistoricos = [juegoDestacado.Historico];

														Insertar.Ejecutar(juegoDestacadoFinal, conexion, "portadaJuegosDestacados");
                                                    }
                                                }

												#endregion

												#region Minimos

												juegosConMinimos = juegosConMinimos.OrderByDescending(x => x.Historico.FechaDetectado).ThenBy(x => x.Juego.Nombre).ToList();

												List<MinimoListado> juegosMinimosMostrar = new List<MinimoListado>();

                                                foreach (var juegoConMinimo in juegosConMinimos)
                                                {
													bool descarte2 = false;

													string tempCantidad = juegoConMinimo.Juego.Analisis.Cantidad;
													tempCantidad = tempCantidad.Replace(".", null);
													tempCantidad = tempCantidad.Replace(",", null);

													if (Convert.ToInt32(tempCantidad) < 500)
													{
														descarte2 = true;
													}

													if (descarte2 == false)
													{
														bool descarte3 = false;

														if (juegoConMinimo.Juego.Gratis != null)
														{
															if (juegoConMinimo.Juego.Gratis.Count > 0)
															{
																foreach (var gratis in juegoConMinimo.Juego.Gratis)
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
															juegosMinimosMostrar.Add(juegoConMinimo);

															if (juegosMinimosMostrar.Count >= 80)
															{
																break;
															}
														}
													}
												}

												if (juegosMinimosMostrar.Count > 0)
												{
													BaseDatos.Portada.Limpiar.Ejecutar("portadaJuegosMinimos", conexion);

													foreach (var minimoMostrar in juegosMinimosMostrar)
													{
														Juego minimoMostrarFinal = minimoMostrar.Juego;
														minimoMostrarFinal.PrecioMinimosHistoricos = [minimoMostrar.Historico];

														Insertar.Ejecutar(minimoMostrarFinal, conexion, "portadaJuegosMinimos");
													}
												}

												if (juegosConMinimos.Count > 0)
                                                {
                                                    BaseDatos.Portada.Limpiar.Ejecutar("seccionMinimos", conexion);

													foreach (var minimo in juegosConMinimos)
													{
														Juego minimoFinal = minimo.Juego;
														minimoFinal.PrecioMinimosHistoricos = [minimo.Historico];

														try
														{
															Insertar.Ejecutar(minimoFinal, conexion, "seccionMinimos");
														}
														catch 
														{
															BaseDatos.Errores.Insertar.Mensaje("Tarea - Minimos Insertar", minimo.Juego.Nombre, conexion);
														}														
													}
												}

                                                #endregion
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            BaseDatos.Errores.Insertar.Ejecutar("Tarea - Minimos", ex, conexion);
                        }
                    }
                }                   
			}
		}

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            await base.StopAsync(stoppingToken);
        }
    }

	public class MinimoListado
	{
		public Juego Juego { get; set; }
		public JuegoPrecio Historico { get; set; }
	}
}
