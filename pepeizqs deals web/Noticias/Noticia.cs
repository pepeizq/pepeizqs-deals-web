#nullable disable

using Suscripciones2;

namespace Noticias
{
	public class Noticia
	{
		public int Id;
		public NoticiaTipo Tipo;
		public SuscripcionTipo SuscripcionTipo;
		public string Titulo;
		public string Contenido;
		public string Imagen;
		public string Enlace;
		public string Juegos;
		public DateTime FechaEmpieza;
		public DateTime FechaTermina;
	}
}
