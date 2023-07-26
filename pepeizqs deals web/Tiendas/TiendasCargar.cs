namespace Tiendas2
{
	public static class TiendasCargar
	{
		public static List<Tienda> GenerarListado()
		{
			List<Tienda> tiendas = new List<Tienda>
			{
				APIs.Steam.Tienda.Generar(),
				APIs.GamersGate.Tienda.Generar(),
				APIs.Humble.Tienda.Generar(),
				APIs.Humble.Tienda.GenerarChoice()
			};

			return tiendas;
		}
	}
}
