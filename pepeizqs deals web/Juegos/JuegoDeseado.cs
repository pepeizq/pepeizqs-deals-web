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
		public string Nombre { get; set; }
		public string Imagen { get; set; }
		public JuegoPrecio Precio { get; set; }
		public JuegoDRM DRM { get; set; }
		public bool Historico { get; set; }
		public string HistoricoPrecio { get; set; }
		public string ReseñasPorcentaje { get; set; }
		public string ReseñasCantidad { get; set; }
	}
}
