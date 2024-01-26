#nullable disable

using Juegos;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Data.SqlClient;

namespace BaseDatos.Juegos
{
	public static class Precios
	{
		public static void Actualizar(Juego juego, JuegoPrecio nuevaOferta, ViewDataDictionary objeto, SqlConnection conexion)
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
							if (nuevaOferta.Precio < precio.Precio) 
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

										if (tempPrecio <= minimo.Precio)
										{
											minimo.Precio = tempPrecio;
											minimo.Moneda = precio.Moneda;
											minimo.Descuento = precio.Descuento;											
											minimo.FechaActualizacion = precio.FechaActualizacion;
											minimo.FechaTermina = precio.FechaTermina;
											minimo.CodigoDescuento = precio.CodigoDescuento;
											minimo.CodigoTexto = precio.CodigoTexto;
											minimo.Nombre = precio.Nombre;
											minimo.Imagen = precio.Imagen;
											minimo.Enlace = precio.Enlace;
											minimo.Tienda = precio.Tienda;

											//------------------------------------------

											if (tempPrecio < minimo.Precio)
											{
												if (juego.UsuariosInteresados != null)
												{
													if (juego.UsuariosInteresados.Count > 0)
													{
														foreach (var usuarioInteresado in juego.UsuariosInteresados)
														{
															string correo = Usuarios.Buscar.UnUsuarioDeseados(usuarioInteresado.UsuarioId, juego.Id.ToString(), usuarioInteresado.DRM);

															if (correo != null)
															{
																Herramientas.Correos.EnviarNuevoMinimo(juego, minimo, correo);
															}
														}
													}
												}
											}

											//------------------------------------------

											minimo.FechaDetectado = precio.FechaDetectado;
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

		public static List<Juego> DevolverMinimos(List<Juego> juegos)
		{		
			if (juegos.Count > 0)
			{
				List<Juego> juegosConMinimos = new List<Juego>();

				foreach (var juego in juegos)
				{
					if (juego != null)
					{
						if (juego.PrecioActualesTiendas != null)
						{
							if (juego.PrecioActualesTiendas.Count > 0)
							{
								foreach (var minimo2 in juego.PrecioActualesTiendas)
								{
									if (minimo2.Moneda != Herramientas.JuegoMoneda.Euro)
									{
										minimo2.Precio = Herramientas.Divisas.Cambio(minimo2.Precio, minimo2.Moneda);
									}

									if (minimo2.DRM == JuegoDRM.NoEspecificado)
									{
										minimo2.Precio = 100000000;
									}

									foreach (var historico2 in juego.PrecioMinimosHistoricos)
									{
										if (historico2.DRM == minimo2.DRM)
										{
											if (historico2.Precio < minimo2.Precio)
											{
												minimo2.Precio = 100000000;
											}
										}
									}
								}

								juego.PrecioActualesTiendas = juego.PrecioActualesTiendas.OrderBy(x => x.Precio).ToList();

								if (juego.PrecioActualesTiendas[0] != null)
								{
									bool fechaEncaja = Herramientas.JuegoFicha.CalcularAntiguedad(juego.PrecioActualesTiendas[0]);

									if (fechaEncaja == true && juego.PrecioActualesTiendas[0].Descuento > 0 && juego.PrecioActualesTiendas[0].Precio < 1000 && juego.Analisis != null)
									{
										juegosConMinimos.Add(juego);
									}
								}
							}
						}						
					}                   
                }

				juegosConMinimos.Sort(delegate (Juego j1, Juego j2)
				{
					List<JuegoPrecio> j1Ofertas = new List<JuegoPrecio>();

					if (j1.PrecioActualesTiendas.Count > 0)
					{
						decimal precio = 10000000;
						JuegoPrecio minimo = null;

						foreach (var minimo2 in j1.PrecioActualesTiendas)
						{
							decimal precio2 = minimo2.Precio;

							//if (minimo2.Moneda != Herramientas.JuegoMoneda.Euro)
							//{
							//	precio2 = Herramientas.Divisas.Cambio(precio2, minimo2.Moneda);
							//}

							if (precio2 < precio)
							{
								if (minimo2.DRM != JuegoDRM.NoEspecificado)
								{
									precio = precio2;

									minimo = minimo2;
								}
							}
						}

						if (minimo != null)
						{
							j1Ofertas.Add(minimo);
						}
					}

					List<JuegoPrecio> j2Ofertas = new List<JuegoPrecio>();

					if (j2.PrecioActualesTiendas.Count > 0)
					{
						decimal precio = 10000000;
						JuegoPrecio minimo = null;

						foreach (var minimo2 in j2.PrecioActualesTiendas)
						{
							decimal precio2 = minimo2.Precio;

							//if (minimo2.Moneda != Herramientas.JuegoMoneda.Euro)
							//{
							//	precio2 = Herramientas.Divisas.Cambio(precio2, minimo2.Moneda);
							//}

							if (precio2 < precio)
							{
								if (minimo2.DRM != JuegoDRM.NoEspecificado)
								{
									precio = precio2;

									minimo = minimo2;
								}
							}
						}

						if (minimo != null)
						{
							j2Ofertas.Add(minimo);
						}
					}

					if (j1Ofertas.Count > 0 && j2Ofertas.Count > 0)
					{
						return j2Ofertas[0].FechaDetectado.CompareTo(j1Ofertas[0].FechaDetectado);
					}
					else
					{
						if (j1Ofertas.Count == 0 && j2Ofertas.Count > 0)
						{
							return 1;
						}
						else if (j1Ofertas.Count > 0 && j2Ofertas.Count == 0)
						{
							return -1;
						}
					}

					return 0;
				});

				return juegosConMinimos;
			}

			return null;
		}

		public static void Limpiar(string tienda)
		{
			SqlConnection conexion = Herramientas.BaseDatos.Conectar();

			using (conexion)
			{
				List<Juego> juegos = Buscar.Todos(conexion);

				if (juegos.Count > 0)
				{
					foreach (var juego in juegos)
					{
						juego.PrecioActualesTiendas.Clear();
						Juegos.Actualizar.Ejecutar(juego, conexion);

						//int posicionActual = 0;
						//bool borrarActual = false;

						//int i = 0;
						//foreach (var precio in juego.PrecioActualesTiendas)
						//{
						//	if (precio.Tienda == tienda)
						//	{
						//		posicionActual = i;
						//		borrarActual = true;
						//	}
						//	i += 1;
						//}

						//if (borrarActual == true)
						//{
						//	juego.PrecioActualesTiendas.RemoveAt(posicionActual);

						//	Juegos.Actualizar.Ejecutar(juego, conexion);
						//}

						//int posicionHistorico = 0;
						//bool borrarHistorico = false;

						//int j = 0;
						//foreach (var precio in juego.PrecioMinimosHistoricos)
						//{
						//	if (precio.Tienda == tienda)
						//	{
						//		posicionHistorico = j;
						//		borrarHistorico = true;
						//	}
						//	j += 1;
						//}

						//if (borrarHistorico == true)
						//{
						//	juego.PrecioMinimosHistoricos.RemoveAt(posicionHistorico);

						//	Juegos.Actualizar.Ejecutar(juego, conexion);
						//}
					}
				}

				//------------------------------------------------------

				//string limpiar = "TRUNCATE TABLE tienda" + tienda;

				//using (SqlCommand comando = new SqlCommand(limpiar, conexion))
				//{
				//	comando.ExecuteNonQuery();
				//}
			}

			conexion.Dispose();
		}
	}
}
