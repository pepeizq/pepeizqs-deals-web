namespace APIs.Humble
{
	public static class Tienda
	{
		public static Tiendas2.Tienda Generar()
		{
			Tiendas2.Tienda tienda = new Tiendas2.Tienda
			{
				Id = "humblestore",
				Nombre = "Humble Store",
				ImagenLogo = "/imagenes/tiendas/humblestore_logo.png",
				Imagen300x80 = "/imagenes/tiendas/humblestore_300x80.png",
				ImagenIcono = "/imagenes/tiendas/humblestore_icono.ico",
				Color = "#ea9192",
				EnseñarAdmin = true
			};

			return tienda;
		}

		public static Tiendas2.Tienda GenerarChoice()
		{
			Tiendas2.Tienda tienda = new Tiendas2.Tienda
			{
				Id = "humblechoice",
				Nombre = "Humble Choice",
				ImagenLogo = "/imagenes/tiendas/humblechoice_logo.png",
				Imagen300x80 = "/imagenes/tiendas/humblechoice_300x80.png",
				ImagenIcono = "/imagenes/tiendas/humblestore_icono.ico",
				Color = "#ea9192",
				EnseñarAdmin = false
			};

			return tienda;
		}
	}
}
