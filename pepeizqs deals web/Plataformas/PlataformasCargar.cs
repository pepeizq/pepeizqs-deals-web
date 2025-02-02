#nullable disable

namespace Plataformas2
{
	public enum PlataformaTipo
	{
		Amazon,
		Epic
	}

	public class PlataformasCargar
	{
		public static List<Plataforma> GenerarListado()
		{
			List<Plataforma> listado = new List<Plataforma>();

			Plataforma amazon = new Plataforma
			{
				Id = PlataformaTipo.Amazon,
				Nombre = "Amazon Games",
				ImagenIcono = "/imagenes/plataformas/amazongames_icono.webp",
				ImagenLogo = "/imagenes/plataformas/amazongames_logo.webp"
			};

			listado.Add(amazon);

			Plataforma epic = new Plataforma
			{
				Id = PlataformaTipo.Epic,
				Nombre = "Epic Games",
				ImagenIcono = "/imagenes/drm/epic.webp",
				ImagenLogo = "/imagenes/tiendas/epic_300x80.webp"
			};

			listado.Add(epic);

			return listado;
		}
	}
}
