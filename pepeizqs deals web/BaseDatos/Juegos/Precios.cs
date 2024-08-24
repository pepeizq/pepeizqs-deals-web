#nullable disable

using Juegos;
using Microsoft.Data.SqlClient;

namespace BaseDatos.Juegos
{
	public static class Precios
	{
		public static void Actualizar(Juego juego, JuegoPrecio nuevaOferta, SqlConnection conexion, bool actualizarAPI = false)
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

			if (juego.PrecioMinimosHistoricos != null)
			{
				if (juego.PrecioMinimosHistoricos.Count > 0)
				{
					foreach (JuegoPrecio minimo in juego.PrecioMinimosHistoricos)
					{
						if (nuevaOferta.DRM == minimo.DRM)
						{
							decimal tempPrecio = nuevaOferta.Precio;

							if (nuevaOferta.Moneda != Herramientas.JuegoMoneda.Euro)
							{
								tempPrecio = Herramientas.Divisas.Cambio(tempPrecio, nuevaOferta.Moneda);
							}

							if (tempPrecio < minimo.Precio)
							{
								bool notificar = false;

								if (decimal.Add(tempPrecio, 0.2m) < minimo.Precio)
								{
									notificar = true;
								}

								ultimaMoficacion = true;

								minimo.Precio = tempPrecio;
								minimo.Moneda = nuevaOferta.Moneda;
								minimo.Descuento = nuevaOferta.Descuento;
								minimo.FechaDetectado = nuevaOferta.FechaDetectado;
								minimo.FechaUltimoMinimo = nuevaOferta.FechaDetectado;
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
							}
							else if (tempPrecio == minimo.Precio)
							{
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
	}
}
