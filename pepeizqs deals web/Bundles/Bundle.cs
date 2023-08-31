#nullable disable

using Herramientas;
using Juegos;

namespace Bundles2
{
	public class Bundle
	{
		public BundleTipo Id;
		public string Nombre;
		public string Tienda;
		public string Imagen;
		public List<string> ImagenesExtra;
		public string Enlace;
		public string EnlaceBase;
		public DateTime FechaSugerencia;
		public List<BundleJuego> Juegos;
		public List<BundleTier> Tiers;
		public bool Pick;
	}

	public class BundleTier
	{
		public int Posicion;
		public string Precio;
		public JuegoMoneda Moneda;
	}

	public class BundleJuego
	{
		public string JuegoId;
		public string Nombre;
		public string Imagen;
		public BundleTier Tier;
		public JuegoDRM DRM;
	}
}
