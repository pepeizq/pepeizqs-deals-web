#nullable disable

namespace Juegos
{
	public class JuegoDeseado
	{
		public string IdBaseDatos { get; set; }
		public Juego Juego { get; set; }
		public string IdSteam { get; set; }
		public JuegoDRM DRM { get; set; }
	}

	public class JuegoDeseadoMostrar
	{
		public Juego Juego { get; set; }
		public JuegoPrecio Precio { get; set; }
		public JuegoDRM DRM { get; set; }
	}
}
