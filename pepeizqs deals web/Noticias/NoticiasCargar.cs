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
		Otros
	}

	public static class NoticiasCargar
	{
		public static List<NoticiaTipo> CargarNoticiasTipo()
		{
			List<NoticiaTipo> tipos = Enum.GetValues(typeof(NoticiaTipo)).Cast<NoticiaTipo>().ToList();

			return tipos;
		}
	}
}
