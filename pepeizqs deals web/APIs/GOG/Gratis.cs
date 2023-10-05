namespace APIs.GOG
{
	public static class Gratis
	{
		public static Gratis2.Gratis Generar()
		{
			Gratis2.Gratis gog = new Gratis2.Gratis
			{
				Tipo = Gratis2.GratisTipo.GOG,
				Nombre = "GOG",
				Imagen = "/imagenes/tiendas/gog_300x80.webp",
				DRMDefecto = Juegos.JuegoDRM.GOG,
				DRMEnseñar = false
			};

			DateTime fechaGog = DateTime.Now;
			fechaGog = fechaGog.AddDays(3);
			fechaGog = new DateTime(fechaGog.Year, fechaGog.Month, fechaGog.Day, 15, 0, 0);

			gog.FechaSugerencia = fechaGog;

			return gog;
		}
	}
}
