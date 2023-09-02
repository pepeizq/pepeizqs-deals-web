#nullable disable

using Herramientas;
using Juegos;
using System.Security.Cryptography.X509Certificates;

namespace Bundles2
{
	public class Bundle
	{
		public int Id;
		public BundleTipo Tipo;
		public string NombreBundle;
		public string NombreTienda;
		public string ImagenBundle;
		public string ImagenTienda;
		public List<string> ImagenesExtra;
		public string Enlace;
		public string EnlaceBase;
		public DateTime FechaEmpieza;
		public DateTime FechaTermina;
		public List<BundleJuego> Juegos;
		public List<BundleTier> Tiers;
		public bool Pick;
	}

	public class BundleTier
	{
		public int Posicion;
		public string Precio;
		public JuegoMoneda Moneda;
		public int CantidadJuegos;
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
