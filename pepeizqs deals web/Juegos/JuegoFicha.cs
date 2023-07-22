#nullable disable

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
							preciosOrdenados.Add(precio);
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
	}
}
