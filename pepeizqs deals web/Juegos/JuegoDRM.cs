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
		GOG,
		Amazon,
		ElderScrolls,
		BattleNet,
		Giants,
		PearlAbyss,
		SquareEnix
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
				Imagen = "/imagenes/drm/steam.webp",
				Acepciones = new List<string> { "steam", "steamworks" }
			};

			drms.Add(steam);

			//----------------------------

			DRM gog = new DRM
			{
				Id = JuegoDRM.GOG,
				Nombre = "GOG",
				Imagen = "/imagenes/drm/gog.webp",
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
				Imagen = "/imagenes/drm/ubisoft.webp",
				Acepciones = new List<string> { "ubisoft connect", "ubisoft", "uplay" }
			};

			drms.Add(ubisoft);

			//----------------------------

			DRM ea = new DRM
			{
				Id = JuegoDRM.EA,
				Nombre = "EA App",
				Imagen = "/imagenes/drm/ea.webp",
				Acepciones = new List<string> { "ea app", "electronic arts", "origin" }
			};

			drms.Add(ea);

			//----------------------------

			DRM rockstar = new DRM
			{
				Id = JuegoDRM.Rockstar,
				Nombre = "Rockstar Games Launcher",
				Imagen = "/imagenes/drm/rockstar.webp",
				Acepciones = new List<string> { "rockstar social club", "rockstar" }
			};

			drms.Add(rockstar);

			//----------------------------

			DRM microsoft = new DRM
			{
				Id = JuegoDRM.Microsoft,
				Nombre = "Microsoft Store",
				Imagen = "/imagenes/drm/microsoft.webp",
				Acepciones = new List<string> { "microsoft store", "microsoft" }
			};

			drms.Add(microsoft);

			//----------------------------

			DRM amazon = new DRM
			{
				Id = JuegoDRM.Amazon,
				Nombre = "Amazon Games",
				Imagen = "/imagenes/drm/amazongames.webp",
				Acepciones = new List<string> { "amazon games", "amazon" }
			};

			drms.Add(amazon);

			//----------------------------

			DRM epic = new DRM
			{
				Id = JuegoDRM.Epic,
				Nombre = "Epic Games Store",
				Imagen = "/imagenes/drm/epic.webp",
				Acepciones = new List<string> { "epic game store", "epic games store", "epic games", "epic", "epic_keyless", "epic keyless", "epicgames" }
			};

			drms.Add(epic);

			//----------------------------

			DRM elderscrolls = new DRM
			{
				Id = JuegoDRM.ElderScrolls,
				Nombre = "Elder Scrolls Online",
				Imagen = "/imagenes/drm/elderscrolls.webp",
				Acepciones = new List<string> { "esonline" }
			};

			drms.Add(elderscrolls);

            //----------------------------

            DRM battlenet = new DRM
            {
                Id = JuegoDRM.BattleNet,
                Nombre = "Battle.net",
                Imagen = "/imagenes/drm/battlenet.webp",
                Acepciones = new List<string> { "battlenet" }
            };

            drms.Add(battlenet);

            //----------------------------

            DRM giants = new DRM
            {
                Id = JuegoDRM.Giants,
                Nombre = "Giants",
                Imagen = "/imagenes/drm/giants.webp",
                Acepciones = new List<string> { "giants", "giants software" }
            };

            drms.Add(giants);

            //----------------------------

            DRM pearl = new DRM
            {
                Id = JuegoDRM.PearlAbyss,
                Nombre = "Pearl Abyss",
                Imagen = "/imagenes/drm/pearlabyss.webp",
                Acepciones = new List<string> { "pearl abyss" }
            };

            drms.Add(pearl);

			//----------------------------

			DRM square = new DRM
			{
				Id = JuegoDRM.SquareEnix,
				Nombre = "Square Enix",
				Imagen = "/imagenes/drm/squareenix.webp",
				Acepciones = new List<string> { "square-enix" }
			};

			drms.Add(square);

			//----------------------------

			return drms;
		}

		public static string DevolverDRM(JuegoDRM drmBuscar)
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

		public static JuegoDRM DevolverDRM(string drmBuscar)
		{
			List<DRM> drms = GenerarListado();

			foreach (DRM drm in drms)
			{
				if (drm.Id.ToString() == drmBuscar)
				{
					return drm.Id;
				}
			}

			return JuegoDRM.NoEspecificado;
		}

		public static JuegoDRM DevolverDRM(int posicion)
		{
			return CargarDRMs()[posicion];
		}

		public static JuegoDRM Traducir(string drmTexto, string tienda)
		{
			JuegoDRM drmFinal = JuegoDRM.NoEspecificado;

			if (string.IsNullOrEmpty(drmTexto) == false)
			{
				drmTexto = drmTexto.Replace("key", null);
				drmTexto = drmTexto.Replace("DRM &gt;", null);
				drmTexto = drmTexto.Replace("DRM >", null);
				drmTexto = drmTexto.ToLower();
				drmTexto = drmTexto.Trim();

				List<DRM> drms = GenerarListado();

				foreach (DRM drm in drms)
				{
					bool encontroDRM = false;

					foreach (string acepcion in drm.Acepciones)
					{
						if (acepcion.ToLower() == drmTexto)
						{
							drmFinal = drm.Id;

							encontroDRM = true;
							break;
						}
					}	
					
					if (encontroDRM == true)
					{
						break;
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

		public static string SacarImagen(string drmImagen)
		{
			List<DRM> drms = GenerarListado();

			foreach (DRM drm in drms)
			{
				if (drm.Id.ToString() == drmImagen)
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
