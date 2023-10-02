#nullable disable

using Juegos;

namespace Gratis2
{
	public class Gratis
	{
		public GratisTipo Id;
		public string Nombre;
		public string Imagen;
		public string Enlace;
		public DateTime FechaSugerencia;
		public JuegoDRM DRMDefecto;
		public bool DRMEnseñar;
	}

	public class GratisComponente
	{
		public Gratis Tipo;
		public List<JuegoGratis> Juegos;
	}
}
