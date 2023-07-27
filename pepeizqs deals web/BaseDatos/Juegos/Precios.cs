using Juegos;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace BaseDatos.Juegos
{
	public static class Precios
	{
		public static void Actualizar(Juego juego, JuegoPrecio nuevoPrecio, ViewDataDictionary objeto)
		{
			bool añadir = true;

			if (juego.PrecioActualesTiendas != null)
			{
				if (juego.PrecioActualesTiendas.Count > 0)
				{
					foreach (JuegoPrecio precio in juego.PrecioActualesTiendas)
					{
						if (nuevoPrecio.Enlace == precio.Enlace && nuevoPrecio.DRM == precio.DRM && nuevoPrecio.Tienda == precio.Tienda)
						{
							precio.Precio = nuevoPrecio.Precio;
							precio.Descuento = nuevoPrecio.Descuento;
							precio.FechaDetectado = nuevoPrecio.FechaDetectado;
							precio.FechaTermina = nuevoPrecio.FechaTermina;
							precio.CodigoDescuento = nuevoPrecio.CodigoDescuento;
							precio.CodigoTexto = nuevoPrecio.CodigoTexto;
							precio.Nombre = nuevoPrecio.Nombre;
							precio.Imagen = nuevoPrecio.Imagen;

							añadir = false;
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
					if (precio.FechaDetectado.DayOfYear + 2 > DateTime.Now.DayOfYear)
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
										if (precio.Precio <= minimo.Precio)
										{
											minimo.Precio = precio.Precio;
											minimo.Descuento = precio.Descuento;
											minimo.FechaDetectado = precio.FechaDetectado;
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

			Juegos.Actualizar.Ejecutar(juego);
		}

		public static void Limpiar(string tienda)
		{
			List<Juego> juegos = Buscar.Todos();

			if (juegos.Count > 0) 
			{
				foreach (var juego in juegos)
				{
					int posicion = 0;
					bool borrar = false;

					int i = 0;
					foreach (var precio in juego.PrecioActualesTiendas)
					{
						if (precio.Tienda == tienda)
						{
							posicion = i;
							borrar = true;
							break;
						}
						i += 1;
					}

					if (borrar == true)
					{
						juego.PrecioActualesTiendas.RemoveAt(posicion);

						Juegos.Actualizar.Ejecutar(juego);
					}
				}
			}
			
		}
	}
}
