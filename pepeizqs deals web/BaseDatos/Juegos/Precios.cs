﻿#nullable disable

using Juegos;
using Microsoft.Data.SqlClient;

namespace BaseDatos.Juegos
{
	public static class Precios
	{
		public static void Steam(Juego juego, JuegoPrecio nuevaOferta, SqlConnection conexion, bool actualizarAPI = false)
		{
			bool ultimaMoficacion = false;

			bool añadir = true;

			if (juego.PrecioActualesTiendas != null)
			{
				if (juego.PrecioActualesTiendas.Count > 0)
				{
					foreach (JuegoPrecio precio in juego.PrecioActualesTiendas)
					{
						if (nuevaOferta.Enlace == precio.Enlace && 
							nuevaOferta.DRM == precio.DRM && 
							nuevaOferta.Tienda == precio.Tienda &&
							nuevaOferta.Moneda == precio.Moneda)
						{
							bool cambiarFechaDetectado = false;

							if (nuevaOferta.Precio < precio.Precio || nuevaOferta.Precio > precio.Precio)
							{
								cambiarFechaDetectado = true;
							}

							DateTime tempFecha = precio.FechaDetectado;
							tempFecha = tempFecha.AddDays(21);

							if (tempFecha < nuevaOferta.FechaDetectado)
							{
								cambiarFechaDetectado = true;
							}

							if (cambiarFechaDetectado == true)
							{
								precio.FechaDetectado = nuevaOferta.FechaDetectado;
							}

							precio.Precio = nuevaOferta.Precio;
							precio.Descuento = nuevaOferta.Descuento;
							precio.FechaActualizacion = nuevaOferta.FechaActualizacion;
							precio.FechaTermina = nuevaOferta.FechaTermina;
							precio.CodigoDescuento = nuevaOferta.CodigoDescuento;
							precio.CodigoTexto = nuevaOferta.CodigoTexto;
							precio.Nombre = nuevaOferta.Nombre;
							precio.Imagen = nuevaOferta.Imagen;

							añadir = false;
							break;
						}
					}
				}
			}
			else
			{
				juego.PrecioActualesTiendas = new List<JuegoPrecio>();
			}

			if (añadir == true)
			{
				juego.PrecioActualesTiendas.Add(nuevaOferta);
			}

			if (juego.PrecioMinimosHistoricos != null)
			{
				if (juego.PrecioMinimosHistoricos.Count > 0)
				{
                    bool drmEncontrado = false;

                    foreach (JuegoPrecio minimo in juego.PrecioMinimosHistoricos)
					{
						if (nuevaOferta.DRM == minimo.DRM)
						{
							drmEncontrado = true;

							decimal tempPrecio = nuevaOferta.Precio;

							if (nuevaOferta.Moneda != Herramientas.JuegoMoneda.Euro)
							{
								tempPrecio = Herramientas.Divisas.Cambio(tempPrecio, nuevaOferta.Moneda);
							}

							if (tempPrecio < minimo.Precio)
							{
								juego.Historicos = ComprobarHistoricos(juego.Historicos, nuevaOferta);

								bool notificar = false;

								if (decimal.Add(tempPrecio, 0.1m) < minimo.Precio)
								{
									notificar = true;
								}

								ultimaMoficacion = true;

								minimo.Precio = tempPrecio;
								minimo.Moneda = nuevaOferta.Moneda;
								minimo.Descuento = nuevaOferta.Descuento;
								minimo.FechaDetectado = nuevaOferta.FechaDetectado;
								minimo.FechaActualizacion = nuevaOferta.FechaActualizacion;
								minimo.FechaTermina = nuevaOferta.FechaTermina;
								minimo.CodigoDescuento = nuevaOferta.CodigoDescuento;
								minimo.CodigoTexto = nuevaOferta.CodigoTexto;
								minimo.Nombre = nuevaOferta.Nombre;
								minimo.Imagen = nuevaOferta.Imagen;
								minimo.Enlace = nuevaOferta.Enlace;
								minimo.Tienda = nuevaOferta.Tienda;

								//------------------------------------------

								if (notificar == true)
								{
                                    if (juego.UsuariosInteresados != null)
                                    {
                                        if (juego.UsuariosInteresados.Count > 0)
                                        {
                                            foreach (var usuarioInteresado in juego.UsuariosInteresados)
                                            {
                                                if (usuarioInteresado.DRM == minimo.DRM)
												{
													if (Usuarios.Buscar.UsuarioTieneJuego(usuarioInteresado.UsuarioId, juego.Id, usuarioInteresado.DRM, conexion) == false)
													{
														if (Usuarios.Buscar.UsuarioTieneDeseado(usuarioInteresado.UsuarioId, juego.Id.ToString(), usuarioInteresado.DRM, conexion) == true)
														{
															string correo = Usuarios.Buscar.UsuarioQuiereCorreos(usuarioInteresado.UsuarioId, conexion);

															if (string.IsNullOrEmpty(correo) == false)
															{
																try
																{
																	Herramientas.Correos.EnviarNuevoMinimo(usuarioInteresado.UsuarioId, juego, minimo, correo);
																}
																catch (Exception ex)
																{
																	BaseDatos.Errores.Insertar.Mensaje("Enviar Correo Minimo", ex);
																}
															}

															bool enviarPush = Usuarios.Buscar.UsuarioQuiereNotificacionesPushMinimos(usuarioInteresado.UsuarioId);

															if (enviarPush == true)
															{
																try
																{
																	Herramientas.NotificacionesPush.EnviarMinimo(usuarioInteresado.UsuarioId, juego, minimo, usuarioInteresado.DRM);
																}
																catch (Exception ex)
																{
																	BaseDatos.Errores.Insertar.Mensaje("Enviar Push Minimo", ex);
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
							else if (tempPrecio == minimo.Precio)
							{
								juego.Historicos = ComprobarHistoricos(juego.Historicos, nuevaOferta);

								ultimaMoficacion = true;

                                minimo.Imagen = nuevaOferta.Imagen;
								minimo.Enlace = nuevaOferta.Enlace;
								minimo.Tienda = nuevaOferta.Tienda;
								minimo.Moneda = nuevaOferta.Moneda;
								minimo.Descuento = nuevaOferta.Descuento;

								minimo.FechaActualizacion = nuevaOferta.FechaActualizacion;

								DateTime tempFecha = nuevaOferta.FechaDetectado;
								tempFecha = tempFecha.AddDays(30);

								bool cambiarFechaDetectado = false;

								if (tempFecha < minimo.FechaDetectado)
								{
									cambiarFechaDetectado = true;
								}

								if (cambiarFechaDetectado == true)
								{
									minimo.FechaDetectado = nuevaOferta.FechaDetectado;
								}
							}

							break;
						}
					}

					if (drmEncontrado == false)
					{
                        juego.PrecioMinimosHistoricos.Add(nuevaOferta);
                    }
				}
				else
				{
					juego.PrecioMinimosHistoricos = new List<JuegoPrecio>();
					juego.PrecioMinimosHistoricos.Add(nuevaOferta);
				}
			}
			else
			{
				juego.PrecioMinimosHistoricos = new List<JuegoPrecio>();
				juego.PrecioMinimosHistoricos.Add(nuevaOferta);
			}

			if (ultimaMoficacion == true)
			{
				juego.UltimaModificacion = DateTime.Now;
			}

			Juegos.Actualizar.Ejecutar(juego, conexion, actualizarAPI);
		}

		public static void Actualizar(int id, int idSteam, List<JuegoPrecio> ofertasActuales, List<JuegoPrecio> ofertasHistoricas, List<JuegoHistorico> historicos, JuegoPrecio nuevaOferta, SqlConnection conexion, 
			string slugGOG = null, string idGOG = null, string slugEpic = null, List<JuegoUsuariosInteresados> usuariosInteresados = null, JuegoAnalisis analisis = null)
		{
			bool ultimaModificacion = false;

			bool añadir = true;

			if (ofertasActuales != null)
			{
				if (ofertasActuales.Count > 0)
				{
					foreach (JuegoPrecio precio in ofertasActuales)
					{
						if (nuevaOferta.Enlace == precio.Enlace &&
							nuevaOferta.DRM == precio.DRM &&
							nuevaOferta.Tienda == precio.Tienda)
						{
							bool cambiarFechaDetectado = false;

							if (nuevaOferta.Precio < precio.Precio || nuevaOferta.Precio > precio.Precio)
							{
								cambiarFechaDetectado = true;
							}

							DateTime tempFecha = precio.FechaDetectado;
							tempFecha = tempFecha.AddDays(21);

							if (tempFecha < nuevaOferta.FechaDetectado)
							{
								cambiarFechaDetectado = true;
							}

							if (cambiarFechaDetectado == true)
							{
								precio.FechaDetectado = nuevaOferta.FechaDetectado;
							}

							decimal tempPrecio = nuevaOferta.Precio;

							if (string.IsNullOrEmpty(nuevaOferta.CodigoTexto) == false)
							{
								decimal descuento = (decimal)nuevaOferta.CodigoDescuento / 100;
								tempPrecio = tempPrecio - (tempPrecio * descuento);
							}

							precio.Precio = tempPrecio;
							precio.Descuento = nuevaOferta.Descuento;
							precio.FechaActualizacion = nuevaOferta.FechaActualizacion;
							precio.FechaTermina = nuevaOferta.FechaTermina;
							precio.CodigoDescuento = nuevaOferta.CodigoDescuento;
							precio.CodigoTexto = nuevaOferta.CodigoTexto;
							precio.Nombre = nuevaOferta.Nombre;
							precio.Imagen = nuevaOferta.Imagen;
							precio.Moneda = nuevaOferta.Moneda;

							añadir = false;
							break;
						}
					}
				}
			}
			else
			{
				ofertasActuales = new List<JuegoPrecio>();
			}

			if (añadir == true)
			{
				ofertasActuales.Add(nuevaOferta);
			}

			if (ofertasHistoricas != null)
			{
				if (ofertasHistoricas.Count > 0)
				{
					bool drmEncontrado = false;

					foreach (JuegoPrecio minimo in ofertasHistoricas)
					{
						if (nuevaOferta.DRM == minimo.DRM)
						{
							drmEncontrado = true;

							decimal tempPrecio = nuevaOferta.Precio;

							if (string.IsNullOrEmpty(nuevaOferta.CodigoTexto) == false)
							{
								decimal descuento = (decimal) nuevaOferta.CodigoDescuento / 100;
								tempPrecio = tempPrecio - (tempPrecio * descuento);
							}

							if (nuevaOferta.Moneda != Herramientas.JuegoMoneda.Euro)
							{
								tempPrecio = Herramientas.Divisas.Cambio(tempPrecio, nuevaOferta.Moneda);
							}

							if (tempPrecio < minimo.Precio)
							{
								historicos = ComprobarHistoricos(historicos, nuevaOferta);

								bool notificar = false;

								if (decimal.Add(tempPrecio, 0.2m) < minimo.Precio)
								{
									notificar = true;
								}

								ultimaModificacion = true;

								minimo.Precio = tempPrecio;
								minimo.Moneda = nuevaOferta.Moneda;
								minimo.Descuento = nuevaOferta.Descuento;
								minimo.FechaDetectado = nuevaOferta.FechaDetectado;
								minimo.FechaActualizacion = nuevaOferta.FechaActualizacion;
								minimo.FechaTermina = nuevaOferta.FechaTermina;
								minimo.CodigoDescuento = nuevaOferta.CodigoDescuento;
								minimo.CodigoTexto = nuevaOferta.CodigoTexto;
								minimo.Nombre = nuevaOferta.Nombre;
								minimo.Imagen = nuevaOferta.Imagen;
								minimo.Enlace = nuevaOferta.Enlace;
								minimo.Tienda = nuevaOferta.Tienda;

								//------------------------------------------

								if (notificar == true)
								{
									if (usuariosInteresados != null)
									{
										if (usuariosInteresados.Count > 0)
										{
											foreach (var usuarioInteresado in usuariosInteresados)
											{
												if (usuarioInteresado.DRM == minimo.DRM)
												{
													if (Usuarios.Buscar.UsuarioTieneJuego(usuarioInteresado.UsuarioId, id, usuarioInteresado.DRM, conexion) == false)
													{
														if (Usuarios.Buscar.UsuarioTieneDeseado(usuarioInteresado.UsuarioId, id.ToString(), usuarioInteresado.DRM, conexion) == true)
														{
															string correo = Usuarios.Buscar.UsuarioQuiereCorreos(usuarioInteresado.UsuarioId, conexion);

															if (string.IsNullOrEmpty(correo) == false)
															{
																try
																{
																	Herramientas.Correos.EnviarNuevoMinimo(usuarioInteresado.UsuarioId, id, minimo, correo);
																}
																catch (Exception ex)
																{
																	BaseDatos.Errores.Insertar.Mensaje("Enviar Correo Minimo", ex);
																}
															}

															bool enviarPush = Usuarios.Buscar.UsuarioQuiereNotificacionesPushMinimos(usuarioInteresado.UsuarioId);

															if (enviarPush == true)
															{
																try
																{
																	Herramientas.NotificacionesPush.EnviarMinimo(usuarioInteresado.UsuarioId, id, minimo, usuarioInteresado.DRM);
																}
																catch (Exception ex)
																{
																	BaseDatos.Errores.Insertar.Mensaje("Enviar Push Minimo", ex);
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
							else if (tempPrecio == minimo.Precio)
							{
								historicos = ComprobarHistoricos(historicos, nuevaOferta);

								ultimaModificacion = true;

								minimo.Imagen = nuevaOferta.Imagen;
								minimo.Enlace = nuevaOferta.Enlace;
								minimo.Tienda = nuevaOferta.Tienda;
								minimo.Moneda = nuevaOferta.Moneda;
								minimo.Descuento = nuevaOferta.Descuento;

								minimo.FechaActualizacion = nuevaOferta.FechaActualizacion;

								DateTime tempFecha = nuevaOferta.FechaDetectado;
								tempFecha = tempFecha.AddDays(30);

								bool cambiarFechaDetectado = false;

								if (tempFecha < minimo.FechaDetectado)
								{
									cambiarFechaDetectado = true;
								}

								if (cambiarFechaDetectado == true)
								{
									minimo.FechaDetectado = nuevaOferta.FechaDetectado;
								}
							}

							break;
						}
					}

					if (drmEncontrado == false)
					{
						ofertasHistoricas.Add(nuevaOferta);
					}
				}
				else
				{
					ofertasHistoricas = new List<JuegoPrecio>();
					ofertasHistoricas.Add(nuevaOferta);
				}
			}
			else
			{
				ofertasHistoricas = new List<JuegoPrecio>();
				ofertasHistoricas.Add(nuevaOferta);
			}

			DateTime? ahora = null;

			if (ultimaModificacion == true)
			{
				ahora = DateTime.Now;
			}

			Juegos.Actualizar.Comprobacion(id, ofertasActuales, ofertasHistoricas, historicos, conexion, slugGOG, idGOG, slugEpic, ahora, analisis);
		}

		private static List<JuegoHistorico> ComprobarHistoricos(List<JuegoHistorico> historicos, JuegoPrecio nuevaOferta)
		{
			if (historicos == null)
			{
				historicos = new List<JuegoHistorico>();
			}

			if (historicos.Count == 0)
			{
				JuegoHistorico nuevoHistorico = new JuegoHistorico();
				nuevoHistorico.DRM = nuevaOferta.DRM;
				nuevoHistorico.Tienda = nuevaOferta.Tienda;
				nuevoHistorico.Fecha = nuevaOferta.FechaDetectado;

				decimal tempPrecio = nuevaOferta.Precio;

				if (string.IsNullOrEmpty(nuevaOferta.CodigoTexto) == false)
				{
					decimal descuento = (decimal)nuevaOferta.CodigoDescuento / 100;
					tempPrecio = tempPrecio - (tempPrecio * descuento);
				}

				if (nuevaOferta.Moneda != Herramientas.JuegoMoneda.Euro)
				{
					tempPrecio = Herramientas.Divisas.Cambio(tempPrecio, nuevaOferta.Moneda);
				}

				nuevoHistorico.Precio = tempPrecio;

				historicos.Add(nuevoHistorico);
			}
			else if (historicos.Count > 0)
			{
				JuegoHistorico nuevoHistorico = new JuegoHistorico();
				nuevoHistorico.DRM = nuevaOferta.DRM;
				nuevoHistorico.Tienda = nuevaOferta.Tienda;
				nuevoHistorico.Fecha = nuevaOferta.FechaDetectado;

				decimal tempPrecio = nuevaOferta.Precio;

				if (string.IsNullOrEmpty(nuevaOferta.CodigoTexto) == false)
				{
					decimal descuento = (decimal)nuevaOferta.CodigoDescuento / 100;
					tempPrecio = tempPrecio - (tempPrecio * descuento);
				}

				if (nuevaOferta.Moneda != Herramientas.JuegoMoneda.Euro)
				{
					tempPrecio = Herramientas.Divisas.Cambio(tempPrecio, nuevaOferta.Moneda);
				}

				nuevoHistorico.Precio = tempPrecio;

				bool mismoDRM = false;
				decimal precioMasBajo2 = 1000000;
				DateTime fechaMasBajo2 = new DateTime();

				foreach (var historico in historicos)
				{
					if (historico.DRM == nuevoHistorico.DRM)
					{
						mismoDRM = true;

						if (precioMasBajo2 >= historico.Precio)
						{
							precioMasBajo2 = historico.Precio;
							fechaMasBajo2 = historico.Fecha;
                        }
                    }
                }

				if (mismoDRM == true)
				{
					if (precioMasBajo2 > nuevoHistorico.Precio)
					{
						historicos.Add(nuevoHistorico);
					}
					
					if (precioMasBajo2 == nuevoHistorico.Precio)
					{
						DateTime historico2 = fechaMasBajo2;
						historico2 = historico2.AddDays(22);

						if (historico2 < nuevoHistorico.Fecha)
						{
							historicos.Add(nuevoHistorico);
						}
					}
				}
                else
				{
					historicos.Add(nuevoHistorico);
				}
			}

			return historicos;
		}
	}
}
