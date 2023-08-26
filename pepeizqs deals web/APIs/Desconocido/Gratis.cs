namespace APIs.Desconocido
{
	public static class Gratis
	{
		public static Gratis2.Gratis Generar()
		{
			Gratis2.Gratis desconocido = new Gratis2.Gratis
			{
				Id = Gratis2.GratisTipo.Desconocido,
				Nombre = "Desconocido",
				DRMDefecto = Juegos.JuegoDRM.Steam
			};

			DateTime fechaDesconocido = DateTime.Now;
			fechaDesconocido = fechaDesconocido.AddDays(2);
			fechaDesconocido = new DateTime(fechaDesconocido.Year, fechaDesconocido.Month, fechaDesconocido.Day, 19, 0, 0);

			desconocido.FechaSugerencia = fechaDesconocido;

			return desconocido;
		}
	}
}
