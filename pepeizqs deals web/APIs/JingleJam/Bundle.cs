namespace APIs.JingleJam
{
	public static class Bundle
	{
		public static Bundles2.Bundle Generar()
		{
			Bundles2.Bundle bundle = new Bundles2.Bundle()
			{
				Tipo = Bundles2.BundleTipo.JingleJam,
				NombreTienda = "Jingle Jam",
				ImagenTienda = "/imagenes/bundles/jinglejam_300x80.webp",
				ImagenIcono = "/imagenes/bundles/jinglejam_icono.webp",
				EnlaceBase = "jinglejam.tiltify.com",
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
