namespace APIs.Steam
{
	public static class Gratis
	{
		public static Gratis2.Gratis Generar()
		{
			Gratis2.Gratis steam = new Gratis2.Gratis
			{
				Tipo = Gratis2.GratisTipo.Steam,
				Nombre = "Steam",
				Imagen = "/imagenes/tiendas/steam_300x80.webp",
				DRMDefecto = Juegos.JuegoDRM.Steam,
				DRMEnseñar = false
			};

			DateTime fechaSteam = DateTime.Now;
			fechaSteam = fechaSteam.AddDays(2);
			fechaSteam = new DateTime(fechaSteam.Year, fechaSteam.Month, fechaSteam.Day, 19, 0, 0);

			steam.FechaSugerencia = fechaSteam;

			return steam;
		}

        public static string Referido(string enlace)
        {
            return enlace + "?curator_clanid=33500256";
        }
    }
}
