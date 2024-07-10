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
                            TimeSpan tiempoSiguiente = TimeSpan.FromMinutes(15);

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
                                                        if (juego.PrecioMinimosHistoricos != null && juego.Analisis.Cantidad.Length > 2)
                                                        {
                                                            foreach (var historico in juego.PrecioMinimosHistoricos)
                                                            {
                                                                TimeSpan actualizado = DateTime.Now.Subtract(historico.FechaActualizacion);

                                                                if (actualizado.Days == 0)
                                                                {
																	TimeSpan detectado = DateTime.Now.Subtract(historico.FechaDetectado);

																	if (actualizado.Days >= 0 && actualizado.Days <= 14)
																	{
																		Juego nuevoHistorico = juego;
																		nuevoHistorico.PrecioMinimosHistoricos = new List<JuegoPrecio>() { historico };
																		nuevoHistorico.IdMaestra = juego.Id;
																	
																		bool añadir = true;

																		if (nuevoHistorico.PrecioMinimosHistoricos[0].DRM != JuegoDRM.Steam && nuevoHistorico.PrecioMinimosHistoricos[0].DRM != JuegoDRM.GOG && nuevoHistorico.PrecioMinimosHistoricos[0].DRM != JuegoDRM.EA && nuevoHistorico.PrecioMinimosHistoricos[0].DRM != JuegoDRM.Ubisoft)
																		{
																			añadir = false;
																		}
																		
																		if (string.IsNullOrEmpty(nuevoHistorico.FreeToPlay) == false)
																		{
																			if (nuevoHistorico.FreeToPlay.ToLower() == "true")
																			{
																				añadir = false;
																			}
																		}
																		
																		if (string.IsNullOrEmpty(nuevoHistorico.MayorEdad) == false)
																		{
																			if (nuevoHistorico.MayorEdad.ToLower() == "true")
																			{
																				añadir = false;
																			}
																		}

																		if (añadir == true)
																		{
																			juegosConMinimos.Add(nuevoHistorico);
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

												juegosConMinimos.Sort(delegate (Juego j1, Juego j2)
												{
													if (j1.Analisis != null && j2.Analisis != null)
													{
														if (string.IsNullOrEmpty(j1.Analisis.Cantidad) == false && string.IsNullOrEmpty(j2.Analisis.Cantidad) == false)
														{
															int j1Analisis = int.Parse(j1.Analisis.Cantidad.Replace(",", null));
															int j2Analisis = int.Parse(j2.Analisis.Cantidad.Replace(",", null));

															return j2Analisis.CompareTo(j1Analisis);
														}
													}

													if (j1.Analisis != null)
													{
														return 0;
													}

													if (j2.Analisis != null)
													{
														return 1;
													}

													return 0;
												});										

												List<Juego> juegosDestacadosMostrar = new List<Juego>();

                                                int i = 0;

                                                foreach (var minimo in juegosConMinimos)
                                                {
                                                    bool añadir = true;

													if (minimo.Tipo != JuegoTipo.Game)
													{
														añadir = false;
													}
													else if (minimo.PrecioMinimosHistoricos[0].DRM != JuegoDRM.Steam)
													{
														añadir = false;
													}
													else if (minimo.Bundles != null)
													{
														foreach (var bundle in minimo.Bundles)
														{
															if (DateTime.Now >= bundle.FechaEmpieza && DateTime.Now <= bundle.FechaTermina)
															{
																añadir = false;
																break;
															}
														}
													}
													else if (minimo.Gratis != null)
													{
														foreach (var gratis in minimo.Gratis)
														{
															if (DateTime.Now >= gratis.FechaEmpieza && DateTime.Now <= gratis.FechaTermina)
															{
																añadir = false;
																break;
															}
														}
													}
													else if (minimo.Suscripciones != null)
													{
														foreach (var suscripcion in minimo.Suscripciones)
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
														string tempCantidad = minimo.Analisis.Cantidad;
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
                                                        Insertar.Ejecutar(juegoDestacado, conexion, "portadaJuegosDestacados");
                                                    }
                                                }

												#endregion

												#region Minimos

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

												List<Juego> juegosMinimosMostrar = new List<Juego>();

                                                foreach (var juegoConMinimo in juegosConMinimos)
                                                {
													bool descarte2 = false;

													string tempCantidad = juegoConMinimo.Analisis.Cantidad;
													tempCantidad = tempCantidad.Replace(".", null);
													tempCantidad = tempCantidad.Replace(",", null);

													if (Convert.ToInt32(tempCantidad) < 500)
													{
														descarte2 = true;
													}

													if (descarte2 == false)
													{
														bool descarte3 = false;

														if (juegoConMinimo.Gratis != null)
														{
															if (juegoConMinimo.Gratis.Count > 0)
															{
																foreach (var gratis in juegoConMinimo.Gratis)
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
														Insertar.Ejecutar(minimoMostrar, conexion, "portadaJuegosMinimos");
													}
												}

                                                if (juegosConMinimos.Count > 0)
                                                {
                                                    BaseDatos.Portada.Limpiar.Ejecutar("seccionMinimos", conexion);

													foreach (var minimo in juegosConMinimos)
													{
														Insertar.Ejecutar(minimo, conexion, "seccionMinimos");
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
}
