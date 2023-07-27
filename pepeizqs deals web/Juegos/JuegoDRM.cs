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
		public static List<DRM> GenerarListado()
		{
			List<DRM> drms = new List<DRM>();

			//----------------------------

			DRM steam = new DRM
			{
				Id = JuegoDRM.Steam,
				Nombre = "Steam",
				Imagen = "/imagenes/drm/steam.png",
				Acepciones = new List<string> { "steam", "steamworks" }
			};

			drms.Add(steam);

			//----------------------------

			DRM drmfree = new DRM
			{
				Id = JuegoDRM.DRMFree,
				Nombre = "DRM Free",
				Acepciones = new List<string> { "drm free", "drmfree" }
			};

			drms.Add(drmfree);

			//----------------------------

			DRM ubisoft = new DRM
			{
				Id = JuegoDRM.Ubisoft,
				Nombre = "Ubisoft Connect",
				Imagen = "/imagenes/drm/ubisoft.png",
				Acepciones = new List<string> { "ubisoft connect", "ubisoft", "uplay" }
			};

			drms.Add(ubisoft);

			//----------------------------

			DRM ea = new DRM
			{
				Id = JuegoDRM.EA,
				Nombre = "EA App",
				Imagen = "/imagenes/drm/ea.png",
				Acepciones = new List<string> { "eaapp", "ea app", "electronicarts", "electronic arts", "origin" }
			};

			drms.Add(ea);

			//----------------------------

			DRM rockstar = new DRM
			{
				Id = JuegoDRM.Rockstar,
				Nombre = "Rockstar Games Launcher",
				Imagen = "/imagenes/drm/rockstar.png",
				Acepciones = new List<string> { "rockstar social club", "rockstar" }
			};

			drms.Add(rockstar);

			//----------------------------

			return drms;
		}

		public static string Nombre(JuegoDRM drmBuscar)
		{
			string texto = string.Empty;

			List<DRM> drms = GenerarListado();

			foreach (DRM drm in drms) 
			{ 
				if (drm.Id == drmBuscar)
				{
					texto = drm.Nombre;
				}
			}




			if (drmBuscar == JuegoDRM.Microsoft)
			{
				texto = "Microsoft Store";
			}
			else if (drmBuscar == JuegoDRM.Epic)
			{
				texto = "Epic Games Store";
			}
			else if (drmBuscar == JuegoDRM.NoEspecificado)
			{
				texto = "Not specified";
			}

			return texto;
		}

		public static JuegoDRM Traducir(string drmTexto, string tienda)
		{
			JuegoDRM drmFinal = JuegoDRM.NoEspecificado;

			if (string.IsNullOrEmpty(drmTexto) == false)
			{
				List<DRM> drms = GenerarListado();

				foreach (DRM drm in drms)
				{
					foreach (string acepcion in drm.Acepciones)
					{
						if (acepcion == drmTexto)
						{
							drmFinal = drm.Id;
						}
					}			
				}
			}
				

			if (string.IsNullOrEmpty(drmTexto) == false)
			{
				if (drmTexto.ToLower() == "epic game store")
				{
					drmFinal = JuegoDRM.Epic;
				}
				else if (drmTexto.ToLower() == "epic games store")
				{
					drmFinal = JuegoDRM.Epic;
				}
				else if (drmTexto.ToLower() == "epic games")
				{
					drmFinal = JuegoDRM.Epic;
				}
                else if (drmTexto.ToLower() == "epic")
                {
					drmFinal = JuegoDRM.Epic;
                }
                else
				{
					drmFinal = JuegoDRM.NoEspecificado;
				}

				//------------------------------------------------

				if (tienda == "humblestore")
				{
					if (drmTexto.ToLower() == "download")
					{
						drmFinal = JuegoDRM.DRMFree;
					}
				}
			}

			return drmFinal;
		}

		public static string SacarImagen(JuegoDRM drmImagen)
		{
			List<DRM> drms = GenerarListado();

			foreach (DRM drm in drms)
			{
				if (drm.Id == drmImagen)
				{
					return drm.Imagen;
				}
			}

			return null;
		}

		public static List<JuegoDRM> CargarDRMs()
		{
			List<JuegoDRM> drms = Enum.GetValues(typeof(JuegoDRM)).Cast<JuegoDRM>().ToList();

			return drms;
		}
	}

	public class DRM
	{
		public JuegoDRM Id;
		public string Nombre;
		public string Imagen;
		public List<string> Acepciones;
	}
}
