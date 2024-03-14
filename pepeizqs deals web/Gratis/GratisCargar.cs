#nullable disable

namespace Gratis2
{
	public enum GratisTipo
	{
		Desconocido,
		Steam,
		GOG,
		Fanatical,
		Ubisoft,
		EpicGames,
        Amplifiers,
		SEGA,
		Humble
    }

	public class GratisCargar
	{
		public static List<Gratis> GenerarListado()
		{
			List<Gratis> gratis = new List<Gratis>
			{
				APIs.Desconocido.Gratis.Generar(),
				APIs.Steam.Gratis.Generar(),
				APIs.EpicGames.Gratis.Generar(),
				APIs.GOG.Gratis.Generar(),
				APIs.Fanatical.Gratis.Generar(),
				APIs.Ubisoft.Gratis.Generar(),
				APIs.Amplifiers.Gratis.Generar(),
                APIs.SEGA.Gratis.Generar(),
				APIs.Humble.Gratis.Generar()
            };

			return gratis;
		}

		public static string LimpiarEnlace(string enlace)
		{
			enlace = enlace.Replace("store.epicgames.com/en-US/", "store.epicgames.com/");
			enlace = enlace.Replace("store.epicgames.com/es-ES/", "store.epicgames.com/");

			return enlace;
		}

		public static Gratis DevolverGratis(string gratisTexto)
		{
			foreach (var gratis in GenerarListado())
			{
				if (gratis.Tipo.ToString() == gratisTexto)
				{
					return gratis;
				}
			}

			return null;
		}

		public static Gratis DevolverGratis(GratisTipo gratisTipo)
		{
			foreach (var gratis in GenerarListado())
			{
				if (gratis.Tipo == gratisTipo)
				{
					return gratis;
				}
			}

			return null;
		}

		public static Gratis DevolverGratis(int posicion)
		{
			GratisTipo gratis2 = CargarGratis()[posicion];

			foreach (var gratis in GenerarListado())
			{
				if (gratis.Tipo == gratis2)
				{
					return gratis;
				}
			}

			return null;
		}

		public static List<GratisTipo> CargarGratis()
		{
			List<GratisTipo> gratis = Enum.GetValues(typeof(GratisTipo)).Cast<GratisTipo>().ToList();

			return gratis;
		}
	}
}
