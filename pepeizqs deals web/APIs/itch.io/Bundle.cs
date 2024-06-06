namespace APIs.itchio
{
	public static class Bundle
	{
		public static Bundles2.Bundle Generar()
		{
			Bundles2.Bundle bundle = new Bundles2.Bundle()
			{
				Tipo = Bundles2.BundleTipo.Itchio,
				NombreTienda = "itch.io",
				ImagenTienda = "/imagenes/bundles/itchio_300x80.webp",
				ImagenIcono = "/imagenes/bundles/itchio_icono.ico",
				EnlaceBase = "itch.io",
				Pick = false
			};

			DateTime fechaBundle = DateTime.Now;
			fechaBundle = fechaBundle.AddDays(14);
			fechaBundle = new DateTime(fechaBundle.Year, fechaBundle.Month, fechaBundle.Day, 17, 0, 0);

			bundle.FechaTermina = fechaBundle;

			return bundle;
		}
	}
}
