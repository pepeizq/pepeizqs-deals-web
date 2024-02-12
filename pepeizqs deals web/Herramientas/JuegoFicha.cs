#nullable disable

using Juegos;
using Tiendas2;

namespace Herramientas
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
							bool fechaEncaja = JuegoFicha.CalcularAntiguedad(precio);

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

									if (drm == JuegoDRM.NoEspecificado)
									{
										verificacionFinal = false;
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
				preciosOrdenados.Sort(delegate (JuegoPrecio p1, JuegoPrecio p2)
				{
					if (p1.Precio == p2.Precio)
					{
						return p2.FechaDetectado.CompareTo(p1.FechaDetectado);
					}
					else
					{
						return p1.Precio.CompareTo(p2.Precio);
					}
				});
			}

			return preciosOrdenados;
		}

		public static bool CalcularAntiguedad(JuegoPrecio precio)
		{
			bool fechaEncaja = true;

			if (precio.FechaTermina.Year < 2023 && precio.FechaActualizacion.Year < 2023)
			{
				fechaEncaja = false;
			}

			if (precio.FechaTermina.Year > 2022)
			{
				if (DateTime.Now >= precio.FechaTermina)
				{
					fechaEncaja = false;
				}
			}
			else
			{
				if (precio.FechaActualizacion.Year > 2022)
				{
					if (precio.FechaActualizacion.Year != DateTime.Now.Year || precio.FechaActualizacion.DayOfYear + 2 <= DateTime.Now.DayOfYear)
					{
						fechaEncaja = false;
					}
				}
				else
				{
					if (precio.FechaDetectado.Year > 2022)
					{
						if (precio.FechaDetectado.Year != DateTime.Now.Year || precio.FechaDetectado.DayOfYear + 7 <= DateTime.Now.DayOfYear)
						{
							fechaEncaja = false;
						}
					}
				}
			}

			return fechaEncaja;
		}

		public static string IconoTiendaMinimoActualUnDRM(JuegoPrecio oferta)
		{
			if (oferta != null)
			{
				if (oferta.Precio > 0)
				{
					List<Tienda> tiendas = TiendasCargar.GenerarListado();

					foreach (var tienda in tiendas)
					{
						if (tienda.Id == oferta.Tienda)
						{
							return tienda.ImagenIcono;
						}
					}
				}
			}

			return null;
		}

		public static string PrepararPrecio(decimal precio, bool hacerCambioDivisa, JuegoMoneda moneda)
		{
			string precioTexto = string.Empty;

			if (hacerCambioDivisa == true)
			{
				if (moneda != JuegoMoneda.Euro)
				{
					precio = Divisas.Cambio(precio, moneda);
				}
			}
			
			if (precio >= 0)
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
