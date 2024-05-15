namespace APIs.Behavior
{
	public static class Gratis
	{
		public static Gratis2.Gratis Generar()
		{
			Gratis2.Gratis behavior = new Gratis2.Gratis
			{
				Tipo = Gratis2.GratisTipo.Behavior,
				Nombre = "Behavior",
				ImagenLogo = "/imagenes/tiendas/behavior_300x80.webp",
				ImagenIcono = "/imagenes/tiendas/behavior_icono.webp",
				DRMDefecto = Juegos.JuegoDRM.Steam,
				DRMEnseñar = true
			};

			DateTime fechaBehavior = DateTime.Now;
			fechaBehavior = fechaBehavior.AddDays(2);
			fechaBehavior = new DateTime(fechaBehavior.Year, fechaBehavior.Month, fechaBehavior.Day, 17, 0, 0);

			behavior.FechaSugerencia = fechaBehavior;

			return behavior;
		}
	}
}
