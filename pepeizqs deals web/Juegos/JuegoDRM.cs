#nullable disable

namespace Juegos
{
	public enum JuegoDRM
	{
		Steam,
		DRMFree,
		Ubisoft,
		EA,
		Rockstar,
		Microsoft,
		Epic,
		NoEspecificado
	}

	public static class JuegoDRM2
	{
		public static JuegoDRM Traducir(string drmTexto)
		{
			JuegoDRM drm = new JuegoDRM();

			if (string.IsNullOrEmpty(drmTexto) == false)
			{
				if (drmTexto.ToLower() == "steam")
				{
					drm = JuegoDRM.Steam;
				}
				else if (drmTexto.ToLower() == "drm free")
				{
					drm = JuegoDRM.DRMFree;
				}
				else if (drmTexto.ToLower() == "ubisoft connect")
				{
					drm = JuegoDRM.Ubisoft;
				}
				else if (drmTexto.ToLower() == "rockstar social club")
				{
					drm = JuegoDRM.Rockstar;
				}
				else if (drmTexto.ToLower() == "epic game store")
				{
					drm = JuegoDRM.Epic;
				}
				else
				{
					drm = JuegoDRM.NoEspecificado;
				}
			}

			return drm;
		}

		public static List<JuegoDRM> CargarDRMs()
		{
			List<JuegoDRM> drms = Enum.GetValues(typeof(JuegoDRM)).Cast<JuegoDRM>().ToList();

			return drms;
		}
	}
}
