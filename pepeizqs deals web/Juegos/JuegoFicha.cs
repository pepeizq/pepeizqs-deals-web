#nullable disable

using Herramientas;
using Tiendas2;

namespace Juegos
{
	public static class JuegoFicha
	{
		public static List<JuegoPrecio> OrdenarPrecios(List<JuegoPrecio> precios, JuegoDRM drm)
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
								preciosOrdenados.Add(precio);
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

				List<JuegoPrecio> preciosActualesOrdenados = OrdenarPrecios(preciosActuales, drm);

				if (preciosActuales.Count > 0)
				{
					foreach (JuegoPrecio actual in preciosActualesOrdenados)
					{
						if (actual.DRM == drm)
						{
							if (actual.Precio == minimosOrdenados[0].Precio)
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
			}

			return drmPreparado;
		}

		public static string PrepararPrecio(decimal precio, JuegoMoneda moneda)
		{
			string precioTexto = string.Empty;

			if (moneda == JuegoMoneda.Euro)
			{
				precioTexto = precio.ToString();
				precioTexto = precioTexto.Replace(".", ",") + "€";
			}

			return precioTexto;
		}
	}
}
