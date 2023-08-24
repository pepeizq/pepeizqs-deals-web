#nullable disable

using Juegos;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Data.SqlClient;

namespace BaseDatos.Juegos
{
	public static class Precios
	{
		public static void Actualizar(Juego juego, JuegoPrecio nuevoPrecio, ViewDataDictionary objeto, SqlConnection conexion)
		{
			bool añadir = true;

			if (juego.PrecioActualesTiendas != null)
			{
				if (juego.PrecioActualesTiendas.Count > 0)
				{
					foreach (JuegoPrecio precio in juego.PrecioActualesTiendas)
					{
						if (nuevoPrecio.Enlace == precio.Enlace && 
							nuevoPrecio.DRM == precio.DRM && 
							nuevoPrecio.Tienda == precio.Tienda &&
							nuevoPrecio.Moneda == precio.Moneda)
						{
							if (nuevoPrecio.Precio < precio.Precio) 
							{
								precio.FechaDetectado = nuevoPrecio.FechaDetectado;
							}

							precio.Precio = nuevoPrecio.Precio;
							precio.Descuento = nuevoPrecio.Descuento;
							precio.FechaActualizacion = nuevoPrecio.FechaActualizacion;
							precio.FechaTermina = nuevoPrecio.FechaTermina;
							precio.CodigoDescuento = nuevoPrecio.CodigoDescuento;
							precio.CodigoTexto = nuevoPrecio.CodigoTexto;
							precio.Nombre = nuevoPrecio.Nombre;
							precio.Imagen = nuevoPrecio.Imagen;

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
				juego.PrecioActualesTiendas.Add(nuevoPrecio);
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
											minimo.FechaDetectado = precio.FechaDetectado;
											minimo.FechaActualizacion = precio.FechaActualizacion;
											minimo.FechaTermina = precio.FechaTermina;
											minimo.CodigoDescuento = precio.CodigoDescuento;
											minimo.CodigoTexto = precio.CodigoTexto;
											minimo.Nombre = precio.Nombre;
											minimo.Imagen = precio.Imagen;
											minimo.Enlace = precio.Enlace;
											minimo.Tienda = precio.Tienda;
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

		public static List<Juego> DevolverMinimos()
		{
			SqlConnection conexion = Herramientas.BaseDatos.Conectar();

			using (conexion)
			{
				conexion.Open();

				List<Juego> juegos = Buscar.Todos(conexion);

				if (juegos.Count > 0)
				{
					List<Juego> juegosConMinimos = new List<Juego>();

					foreach (var juego in juegos)
					{
						if (juego.PrecioMinimosHistoricos.Count > 0)
						{
							foreach (var minimo in juego.PrecioMinimosHistoricos)
							{
								bool fechaEncaja = Herramientas.JuegoFicha.CalcularAntiguedad(minimo);

								if (fechaEncaja == true && minimo.Descuento > 0)
								{
									juegosConMinimos.Add(juego);
									break;
								}
							}
						}
					}

					juegosConMinimos.Sort(delegate (Juego j1, Juego j2)
					{
						List<JuegoPrecio> j1Ofertas = new List<JuegoPrecio>();

						if (j1.PrecioMinimosHistoricos.Count > 0)
						{
							foreach (var j in j1.PrecioMinimosHistoricos)
							{
								j1Ofertas.Add(j);
							}

							j1Ofertas.OrderBy(x => x.FechaDetectado);
						}

						List<JuegoPrecio> j2Ofertas = new List<JuegoPrecio>();

						if (j2.PrecioMinimosHistoricos.Count > 0)
						{
							foreach (var j in j2.PrecioMinimosHistoricos)
							{
								j2Ofertas.Add(j);
							}

							j2Ofertas.OrderBy(x => x.FechaDetectado);
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
			}

			return null;
		}

		public static void Limpiar(string tienda)
		{
			SqlConnection conexion = Herramientas.BaseDatos.Conectar();

			using (conexion)
			{
				conexion.Open();

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
