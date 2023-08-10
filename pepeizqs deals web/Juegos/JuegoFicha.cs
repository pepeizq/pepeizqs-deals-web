﻿#nullable disable

using Herramientas;
using Tiendas2;

namespace Juegos
{
	public static class JuegoFicha
	{
		public static List<JuegoPrecio> OrdenarPrecios(List<JuegoPrecio> precios, JuegoDRM drm, bool cambioMoneda)
		{
			List<JuegoPrecio> preciosOrdenados = new List<JuegoPrecio>();

			if (precios != null) 
			{
				if (precios.Count > 0)
				{
					foreach (JuegoPrecio precio in precios)
					{
						if (precio.DRM == drm)
						{
							bool fechaEncaja = true;

							if (precio.FechaTermina.Year > 2022)
							{
								if (DateTime.Now > precio.FechaTermina)
								{
									fechaEncaja = false;
								}
							}
							else
							{
								if (precio.FechaDetectado.DayOfYear + 1 < DateTime.Now.DayOfYear)
								{
									fechaEncaja = false;
								}
							}
							
							if (fechaEncaja == true)
							{
								JuegoPrecio nuevoPrecio = precio;

								if (cambioMoneda == true)
								{
									if (nuevoPrecio != null)
									{
										if (nuevoPrecio.Moneda != JuegoMoneda.Euro)
										{
											nuevoPrecio.Precio = Divisas.Cambio(nuevoPrecio.Precio, nuevoPrecio.Moneda);
										}
									}
								}								

								if (nuevoPrecio.Descuento > 0)
								{
									bool verificacionFinal = true;

									if (preciosOrdenados.Count > 0)
									{
										foreach (var ordenado in preciosOrdenados)
										{
											if (ordenado.Enlace == nuevoPrecio.Enlace && ordenado.Tienda == nuevoPrecio.Tienda && ordenado.DRM == nuevoPrecio.DRM)
											{
												verificacionFinal = false;
												break;
											}
										}
									}

									if (verificacionFinal == true) 
									{
										preciosOrdenados.Add(nuevoPrecio);
									}								
								}				
							}						
						}
					}
				}
			}
			
			if (preciosOrdenados.Count > 0)
			{
				preciosOrdenados.Sort(delegate (JuegoPrecio p1, JuegoPrecio p2) { 
					return p1.Precio.CompareTo(p2.Precio); 
				});
			}

			return preciosOrdenados;
		}

		public static string LimpiarImagenJuego(string enlace)
		{
			if (enlace.Contains("/header_alt_") == true)
			{
				int int1 = enlace.IndexOf("/header_alt_");
				enlace = enlace.Remove(int1, enlace.Length - int1);

				enlace = enlace + "/header.jpg";
			}

			return enlace;
		}

		public static string SacarImagenTienda(string codigo)
		{
			string imagen = string.Empty;

			List<Tienda> tiendas = TiendasCargar.GenerarListado();

			foreach (var tienda in tiendas)
			{
				if (tienda.Id == codigo)
				{
					imagen = tienda.Imagen300x80;
				}
			}

			return imagen;
		}

		public static string PrepararMinimo(JuegoDRM drm, List<JuegoPrecio> minimos, List<JuegoPrecio> preciosActuales)
		{
			string drmPreparado = string.Empty;

			List<JuegoPrecio> minimosOrdenados = new List<JuegoPrecio>();

			if (minimos != null)
			{
				if (minimos.Count > 0)
				{
					foreach (JuegoPrecio minimo in minimos)
					{
						if (minimo.DRM == drm)
						{
							minimosOrdenados.Add(minimo);
						}
					}
				}
			}
			
			if (minimosOrdenados.Count == 0)
			{
				preciosActuales.Sort(delegate (JuegoPrecio p1, JuegoPrecio p2) {
					return p1.Precio.CompareTo(p2.Precio);
				});

				foreach (JuegoPrecio actual in preciosActuales)
				{
					if (actual.DRM == drm)
					{
						minimosOrdenados.Add(actual);
						break;
					}
				}
			}

			if (minimosOrdenados.Count > 0)
			{
				if (minimosOrdenados.Count > 1)
				{
					minimosOrdenados.Sort(delegate (JuegoPrecio p1, JuegoPrecio p2) {
						return p1.Precio.CompareTo(p2.Precio);
					});
				}
				
				drmPreparado = "Historical low for " + JuegoDRM2.Nombre(drm) + ": " + PrepararPrecio(minimosOrdenados[0].Precio, minimosOrdenados[0].Moneda);

				bool incluirTiempo = true;

				List<JuegoPrecio> preciosActualesOrdenados = OrdenarPrecios(preciosActuales, drm, false);

				if (preciosActuales.Count > 0)
				{
					foreach (JuegoPrecio actual in preciosActualesOrdenados)
					{
						if (actual.DRM == drm)
						{
							decimal precio = actual.Precio;

							if (actual.Moneda != JuegoMoneda.Euro)
							{
								precio = Divisas.Cambio(precio, actual.Moneda);
							}

							if (precio == minimosOrdenados[0].Precio)
							{
								incluirTiempo = false;
							}
						}
					}
				}
				
				if (incluirTiempo == true)
				{
					List<Tienda> tiendas = TiendasCargar.GenerarListado();
					string tiendaFinal = string.Empty;

					foreach (var tienda in tiendas)
					{
						if (tienda.Id == minimosOrdenados[0].Tienda)
						{
							tiendaFinal = tienda.Nombre;
						}
					}

					drmPreparado = drmPreparado + " (" + Calculadora.HaceTiempo(minimosOrdenados[0].FechaDetectado) + " on " + tiendaFinal + ")";
				}
				else
				{
					drmPreparado = drmPreparado + " (Active Now)";
				}
			}

			return drmPreparado;
		}

		public static string BuscadorMinimo(Juego juego)
		{
			List<JuegoDRM> drms = JuegoDRM2.CargarDRMs();
			decimal minimo = 10000000;
			
			foreach (var drm in drms)
			{
                List<JuegoPrecio> ordenados = OrdenarPrecios(juego.PrecioActualesTiendas, drm, true);

				if (ordenados.Count > 0)
				{
					foreach (var precio in ordenados)
					{
						if (precio.Precio < minimo)
						{
							minimo = precio.Precio;
						}
					}
				}
            }

			string mensaje = string.Empty;

            if (minimo != 10000000)
			{
                mensaje = "From " + PrepararPrecio(minimo, JuegoMoneda.Euro);
            }
			else
			{
                mensaje = "Not in Sale";
            }
			
			return mensaje;
		}

		public static string PrepararPrecio(decimal precio, JuegoMoneda moneda)
		{
			string precioTexto = string.Empty;

			if (precio > 0)
			{
				precioTexto = precio.ToString();
				precioTexto = precioTexto.Replace(".", ",");

				int int1 = precioTexto.IndexOf(",");

				if (int1 == precioTexto.Length - 2)
				{
					precioTexto = precioTexto + "0";
				}

				precioTexto = precioTexto + "€";
			}

            return precioTexto;
		}
	}
}
