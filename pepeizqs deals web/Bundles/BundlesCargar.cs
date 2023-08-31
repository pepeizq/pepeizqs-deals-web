#nullable disable

namespace Bundles2
{
	public enum BundleTipo
	{
		Desconocido,
		HumbleBundle,
		Fanatical
	}

	public class BundlesCargar
	{
		public static List<Bundle> GenerarListado()
		{
			List<Bundle> bundles = new List<Bundle>
			{
				APIs.Desconocido.Bundle.Generar(),
				APIs.Humble.Bundle.Generar(),
				APIs.Fanatical.Bundle.Generar()
			};

			return bundles;
		}

		public static async Task<Bundle> SeleccionarBundle(string enlace)
		{
			foreach (var bundle in BundlesCargar.GenerarListado())
			{
				if (string.IsNullOrEmpty(enlace) == false && string.IsNullOrEmpty(bundle.EnlaceBase) == false)
				{
					if (enlace.Contains(bundle.EnlaceBase) == true)
					{
						if (bundle.Id == BundleTipo.HumbleBundle)
						{
							Bundle nuevoBundle = bundle;
							nuevoBundle.Enlace = LimpiarEnlace(enlace);
							nuevoBundle = await APIs.Humble.Bundle.ExtraerDatos(nuevoBundle);

							return nuevoBundle;
						}
					}
				}			
			}

			return null;
		}

		private static string LimpiarEnlace(string enlace)
		{
			if (enlace.Contains("?") == true)
			{
				int int1 = enlace.IndexOf("?");
				enlace = enlace.Remove(int1, enlace.Length - int1);
			}

			return enlace;
		}
	}
}
