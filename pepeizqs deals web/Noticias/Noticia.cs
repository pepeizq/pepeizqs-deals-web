#nullable disable

using Gratis2;
using Suscripciones2;

namespace Noticias
{
	public class Noticia
	{
		public int Id;
		public NoticiaTipo Tipo;
		public GratisTipo GratisTipo;
		public SuscripcionTipo SuscripcionTipo;
		public string TituloEn;
		public string TituloEs;
		public string ContenidoEn;
		public string ContenidoEs;
		public string Imagen;
		public string Enlace;
		public string Juegos;
		public DateTime FechaEmpieza;
		public DateTime FechaTermina;
	}
}
