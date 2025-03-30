namespace APIs.SquareEnix
{
	public static class Gratis
	{
		public static Gratis2.Gratis Generar()
		{
			Gratis2.Gratis squareenix = new Gratis2.Gratis
			{
				Tipo = Gratis2.GratisTipo.SquareEnix,
				Nombre = "Square Enix Store",
				ImagenLogo = "/imagenes/tiendas/squareenix_logo.webp",
				ImagenIcono = "/imagenes/tiendas/squareenix_icono.webp",
				DRMDefecto = Juegos.JuegoDRM.Steam,
				DRMEnseñar = true
			};

			DateTime fechaSquare = DateTime.Now;
			fechaSquare = fechaSquare.AddDays(2);

			squareenix.FechaSugerencia = fechaSquare;

			return squareenix;
		}
	}
}