#nullable disable

using Juegos;
using Microsoft.Data.SqlClient;

namespace BaseDatos.Juegos
{
	public static class Precios
	{
		public static void Actualizar(Juego juego, JuegoPrecio nuevaOferta, SqlConnection conexion)
		{
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

							if (precio.FechaUltimoMinimo.Year == 0001)
							{
								precio.FechaUltimoMinimo = precio.FechaDetectado;
							}

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

			if (juego.PrecioActualesTiendas.Count > 0)
			{
				foreach (JuegoPrecio precio in juego.PrecioActualesTiendas)
				{
					if (precio.FechaActualizacion.DayOfYear + 2 > DateTime.Now.DayOfYear)
					{
						bool drmEncontrado = false;

						if (juego.PrecioMinimosHistoricos != null)
						{
							if (juego.PrecioMinimosHistoricos.Count > 0)
							{
								foreach (JuegoPrecio minimo in juego.PrecioMinimosHistoricos)
								{
									if (precio.DRM == minimo.DRM)
									{
										decimal tempPrecio = precio.Precio;

										if (precio.Moneda != Herramientas.JuegoMoneda.Euro)
										{
											tempPrecio = Herramientas.Divisas.Cambio(tempPrecio, precio.Moneda);
										}

										if (tempPrecio < minimo.Precio)
										{
											minimo.Precio = tempPrecio;
											minimo.Moneda = precio.Moneda;
											minimo.Descuento = precio.Descuento;
											minimo.FechaDetectado = precio.FechaDetectado;
											minimo.FechaUltimoMinimo = precio.FechaDetectado;
											minimo.FechaActualizacion = precio.FechaActualizacion;
											minimo.FechaTermina = precio.FechaTermina;
											minimo.CodigoDescuento = precio.CodigoDescuento;
											minimo.CodigoTexto = precio.CodigoTexto;
											minimo.Nombre = precio.Nombre;
											minimo.Imagen = precio.Imagen;
											minimo.Enlace = precio.Enlace;
											minimo.Tienda = precio.Tienda;

											//------------------------------------------
											
											if (juego.UsuariosInteresados != null)
											{
												if (juego.UsuariosInteresados.Count > 0)
												{
													foreach (var usuarioInteresado in juego.UsuariosInteresados)
													{
														if (usuarioInteresado.DRM == minimo.DRM)
														{
															string correo = Usuarios.Buscar.UnUsuarioDeseados(usuarioInteresado.UsuarioId, juego.Id.ToString(), usuarioInteresado.DRM, juego.IdSteam);

															if (correo != null)
															{
																Herramientas.Correos.EnviarNuevoMinimo(juego, minimo, correo);
															}
														}														
													}
												}
											}
                                        }
                                        else if (tempPrecio == minimo.Precio)
										{
											minimo.FechaActualizacion = precio.FechaActualizacion;

											DateTime tempFecha = precio.FechaDetectado;
											tempFecha = tempFecha.AddDays(30);

											bool cambiarFechaDetectado = false;

											if (tempFecha < minimo.FechaDetectado)
											{
												cambiarFechaDetectado = true;
											}

											if (cambiarFechaDetectado == true)
											{
												minimo.FechaDetectado = precio.FechaDetectado;
											}
										}

										drmEncontrado = true;
										break;
									}
								}
							}
						}
						else
						{
							juego.PrecioMinimosHistoricos = new List<JuegoPrecio>();
						}

						if (drmEncontrado == false)
						{
							juego.PrecioMinimosHistoricos.Add(precio);
						}
					}
				}
			}

			Juegos.Actualizar.Ejecutar(juego, conexion);
		}
	}
}
