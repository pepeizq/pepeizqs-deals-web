namespace APIs.Steam
{
	public static class Gratis
	{
		public static Gratis2.Gratis Generar()
		{
			Gratis2.Gratis steam = new Gratis2.Gratis
			{
				Id = Gratis2.GratisTipo.Steam,
				Nombre = "Steam",
				Imagen = "/imagenes/tiendas/steam_logo.png",
				DRMDefecto = Juegos.JuegoDRM.Steam
			};

			DateTime fechaSteam = DateTime.Now;
			fechaSteam = fechaSteam.AddDays(2);
			fechaSteam = new DateTime(fechaSteam.Year, fechaSteam.Month, fechaSteam.Day, 19, 0, 0);

			steam.FechaSugerencia = fechaSteam;

			return steam;
		}
	}
}
