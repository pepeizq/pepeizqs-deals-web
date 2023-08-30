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
				APIs.EpicGames.Gratis.Generar(),
				APIs.GOG.Gratis.Generar()
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
				if (gratis.Id.ToString() == gratisTexto)
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
				if (gratis.Id == gratisTipo)
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
				if (gratis.Id == gratis2)
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
