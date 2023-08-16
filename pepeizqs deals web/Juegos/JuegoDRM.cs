#nullable disable

namespace Juegos
{
	//No cambiar orden
	public enum JuegoDRM
	{
		Steam,
		DRMFree,
		Ubisoft,
		EA,
		Rockstar,
		Microsoft,
		Epic,
		NoEspecificado,
		GOG
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

			DRM gog = new DRM
			{
				Id = JuegoDRM.GOG,
				Nombre = "GOG",
				Imagen = "/imagenes/drm/gog.png",
				Acepciones = new List<string> { "gog", "gog galaxy" }
			};

			drms.Add(gog);

			//----------------------------

			DRM drmfree = new DRM
			{
				Id = JuegoDRM.DRMFree,
				Nombre = "DRM Free",
				Acepciones = new List<string> { "drm free" }
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
				Acepciones = new List<string> { "ea app", "electronic arts", "origin" }
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

			DRM microsoft = new DRM
			{
				Id = JuegoDRM.Microsoft,
				Nombre = "Microsoft Store",
				Imagen = "/imagenes/drm/microsoft.png",
				Acepciones = new List<string> { "microsoft store", "microsoft" }
			};

			drms.Add(microsoft);

			//----------------------------

			DRM epic = new DRM
			{
				Id = JuegoDRM.Epic,
				Nombre = "Epic Games Store",
				Imagen = "/imagenes/drm/epic.png",
				Acepciones = new List<string> { "epic game store", "epic games store", "epic games", "epic" }
			};

			drms.Add(epic);

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

			if (drmBuscar == JuegoDRM.NoEspecificado)
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
				drmTexto = drmTexto.Replace(" ", null);
				drmTexto = drmTexto.ToLower();

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
