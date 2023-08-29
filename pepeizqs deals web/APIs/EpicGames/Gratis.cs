﻿namespace APIs.EpicGames
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
			fechaEpic = fechaEpic.AddDays(1);

			int i = 1;
			while (i <= 7)
			{
				if (fechaEpic.DayOfWeek == DayOfWeek.Thursday)
				{
					break;
				}
				else
				{
					fechaEpic = fechaEpic.AddDays(1);
				}

				i += 1;
			}

			fechaEpic = new DateTime(fechaEpic.Year, fechaEpic.Month, fechaEpic.Day, 17, 0, 0);

			epicGames.FechaSugerencia = fechaEpic;

			return epicGames;
		}
	}
}
