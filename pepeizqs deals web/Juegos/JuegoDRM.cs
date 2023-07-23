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
		public static string Texto(JuegoDRM drm)
		{
			string texto = string.Empty;

			if (drm == JuegoDRM.Steam)
			{
				texto = "Steam";
			}
			else if (drm == JuegoDRM.DRMFree)
			{
				texto = "DRM Free";
			}
			else if (drm == JuegoDRM.Ubisoft)
			{
				texto = "Ubisoft Connect";
			}
			else if (drm == JuegoDRM.EA)
			{
				texto = "EA App";
			}
			else if (drm == JuegoDRM.Rockstar)
			{
				texto = "Rockstar Games Launcher";
			}
			else if (drm == JuegoDRM.Microsoft)
			{
				texto = "Microsoft Store";
			}
			else if (drm == JuegoDRM.Epic)
			{
				texto = "Epic Games Store";
			}
			else if (drm == JuegoDRM.NoEspecificado)
			{
				texto = "Not specified";
			}

			return texto;
		}

		public static JuegoDRM Traducir(string drmTexto)
		{
			JuegoDRM drm = new JuegoDRM();

			if (string.IsNullOrEmpty(drmTexto) == false)
			{
				if (drmTexto.ToLower() == "steam")
				{
					drm = JuegoDRM.Steam;
				}
				else if (drmTexto.ToLower() == "steamworks")
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
				else if (drmTexto.ToLower() == "ubisoft")
				{
					drm = JuegoDRM.Ubisoft;
				}
				else if (drmTexto.ToLower() == "rockstar social club")
				{
					drm = JuegoDRM.Rockstar;
				}
				else if (drmTexto.ToLower() == "rockstar")
				{
					drm = JuegoDRM.Rockstar;
				}
				else if (drmTexto.ToLower() == "epic game store")
				{
					drm = JuegoDRM.Epic;
				}
				else if (drmTexto.ToLower() == "epic games store")
				{
					drm = JuegoDRM.Epic;
				}
				else if (drmTexto.ToLower() == "epic games")
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
