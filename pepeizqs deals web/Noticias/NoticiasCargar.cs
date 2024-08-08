#nullable disable

namespace Noticias
{
	public enum NoticiaTipo
	{
		Ofertas,
		Bundles,
		Gratis,
		Suscripciones,
		Eventos,
		Sorteos,
		Rumores,
		Otros,
		Web
	}

	public static class NoticiasCargar
	{
		public static List<NoticiaTipo> CargarNoticiasTipo()
		{
			List<NoticiaTipo> tipos = Enum.GetValues(typeof(NoticiaTipo)).Cast<NoticiaTipo>().ToList();

			return tipos;
		}

		public static string Traduccion(NoticiaTipo tipo, string idioma)
		{
			List<NoticiaTipo> tipos = CargarNoticiasTipo();

			int posicion = 1;
			foreach (var tipo2 in tipos) 
			{ 
				if (tipo2 == tipo)
				{
					return Herramientas.Idiomas.CogerCadena(idioma, "News.Type" + posicion.ToString());
				}

				posicion += 1;
			}

			return null;
		}
	}
}
