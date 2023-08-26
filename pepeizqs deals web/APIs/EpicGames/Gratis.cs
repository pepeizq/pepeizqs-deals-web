namespace APIs.EpicGames
{
	public static class Gratis
	{
		public static Gratis2.Gratis Generar()
		{
			Gratis2.Gratis epicGames = new Gratis2.Gratis
			{
				Id = Gratis2.GratisTipo.EpicGames,
				Nombre = "Epic Games Store",
				Imagen = "/imagenes/tiendas/epic_logo.png",
				DRMDefecto = Juegos.JuegoDRM.Epic
			};

			DateTime fechaEpic = DateTime.Now;
			fechaEpic = fechaEpic.AddDays(7);
			fechaEpic = new DateTime(fechaEpic.Year, fechaEpic.Month, fechaEpic.Day, 17, 0, 0);

			epicGames.FechaSugerencia = fechaEpic;

			return epicGames;
		}
	}
}
