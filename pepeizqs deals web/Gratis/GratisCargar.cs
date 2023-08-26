namespace Gratis2
{
	public enum GratisTipo
	{
		Desconocido,
		Steam,
		GOG,
		Fanatical,
		Ubisoft,
		EpicGames
	}

	public class GratisCargar
	{
		public static List<Gratis> GenerarListado()
		{
			List<Gratis> gratis = new List<Gratis>
			{
				APIs.Desconocido.Gratis.Generar(),
				APIs.Steam.Gratis.Generar(),
				APIs.EpicGames.Gratis.Generar()
			};

			return gratis;
		}

		public static List<GratisTipo> CargarGratis()
		{
			List<GratisTipo> gratis = Enum.GetValues(typeof(GratisTipo)).Cast<GratisTipo>().ToList();

			return gratis;
		}
	}
}
