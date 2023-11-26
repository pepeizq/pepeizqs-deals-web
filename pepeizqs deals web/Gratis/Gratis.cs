#nullable disable

using Juegos;

namespace Gratis2
{
	public class Gratis
	{
		public GratisTipo Tipo;
		public string Nombre;
		public string ImagenLogo;
		public string ImagenIcono;
		public string Enlace;
		public DateTime FechaSugerencia;
		public JuegoDRM DRMDefecto;
		public bool DRMEnseñar;
		public int id;
		public string ImagenNoticia;
	}

	public class GratisComponente
	{
		public Gratis Tipo;
		public List<JuegoGratis> Juegos;
	}
}
