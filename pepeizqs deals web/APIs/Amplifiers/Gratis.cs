namespace APIs.Amplifiers
{
	public static class Gratis
	{
		public static Gratis2.Gratis Generar()
		{
			Gratis2.Gratis amplifiers = new Gratis2.Gratis
			{
				Tipo = Gratis2.GratisTipo.Amplifiers,
				Nombre = "Amplifiers",
				ImagenLogo = "/imagenes/tiendas/amplifiers_300x80.webp",
				ImagenIcono = "/imagenes/tiendas/amplifiers_icono.webp",
				DRMDefecto = Juegos.JuegoDRM.Steam,
				DRMEnseñar = true
			};

			DateTime fechaAmplifiers = DateTime.Now;
			fechaAmplifiers = fechaAmplifiers.AddDays(7);
			fechaAmplifiers = new DateTime(fechaAmplifiers.Year, fechaAmplifiers.Month, fechaAmplifiers.Day, 17, 0, 0);

			amplifiers.FechaSugerencia = fechaAmplifiers;

			return amplifiers;
		}
	}
}
