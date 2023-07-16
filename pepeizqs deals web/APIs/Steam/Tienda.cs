#nullable disable

namespace APIs.Steam
{
	public static class Tienda
	{
		public static Tiendas2.Tienda Generar()
		{
			Tiendas2.Tienda tienda = new Tiendas2.Tienda
			{
				Id = "steam",
				Nombre = "Steam",
				ImagenLogo = "~/imagenes/tiendas/steam_logo.png",
				Imagen300x80 = "~/imagenes/tiendas/steam_300x80.png",
				ImagenIcono = "~/imagenes/tiendas/steam_icono.ico",
				Color = "#2e4460"
			};

			return tienda;
		}
	}
}
