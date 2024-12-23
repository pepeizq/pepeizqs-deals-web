#nullable disable

namespace Juegos
{
	public class JuegoDeseado
	{
		public string IdBaseDatos { get; set; }
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
        public int Id { get; set; }
		public int IdSteam { get; set; }
		public int IdGog { get; set; }
		public string SlugEpic { get; set; }
		public bool Importado { get; set; }
		public int IdBundle { get; set; }
		public int IdSuscripcion { get; set; }
	}

	public class JuegoDeseadoExportar
	{
		public string Nombre { get; set; }
		public int Id { get; set; }
		public JuegoDRM DRM { get; set; }
		public int IdSteam { get; set; }
		public int IdGog { get; set; }
		public string SlugEpic { get; set; }
	}
}
