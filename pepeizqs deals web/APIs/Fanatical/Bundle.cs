namespace APIs.Fanatical
{
	public static class Bundle
	{
		public static Bundles2.Bundle Generar()
		{
			Bundles2.Bundle bundle = new Bundles2.Bundle()
			{
				Id = Bundles2.BundleTipo.Fanatical,
				Tienda = "Fanatical",
				EnlaceBase = "fanatical.com",
				Pick = false
			};

			DateTime fechaBundle = DateTime.Now;
			fechaBundle = fechaBundle.AddDays(14);
			fechaBundle = new DateTime(fechaBundle.Year, fechaBundle.Month, fechaBundle.Day, 17, 0, 0);

			bundle.FechaSugerencia = fechaBundle;

			return bundle;
		}
	}
}
