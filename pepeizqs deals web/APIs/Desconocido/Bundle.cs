namespace APIs.Desconocido
{
	public static class Bundle
	{
		public static Bundles2.Bundle Generar()
		{
			Bundles2.Bundle bundle = new Bundles2.Bundle()
			{
				Tipo = Bundles2.BundleTipo.Desconocido,
				Tienda = "Desconocido"
			};

			DateTime fechaBundle = DateTime.Now;
			fechaBundle = fechaBundle.AddDays(14);
			fechaBundle = new DateTime(fechaBundle.Year, fechaBundle.Month, fechaBundle.Day, 19, 0, 0);

			bundle.FechaTermina = fechaBundle;

			return bundle;
		}
	}
}
