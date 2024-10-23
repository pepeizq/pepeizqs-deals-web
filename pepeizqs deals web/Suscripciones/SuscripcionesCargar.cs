#nullable disable

namespace Suscripciones2
{
	public enum SuscripcionTipo
	{
		HumbleChoice,
		PrimeGaming,
		HumbleMonthly,
		TwitchPrime,
		EAPlay
	}

	public class SuscripcionesCargar
	{
		public static List<Suscripcion> GenerarListado()
		{
			List<Suscripcion> suscripciones = new List<Suscripcion>
			{
				APIs.Humble.Suscripcion.Generar(),
				APIs.PrimeGaming.Suscripcion.Generar(),
				APIs.Humble.Suscripcion.GenerarAntiguo(),
				APIs.PrimeGaming.Suscripcion.GenerarAntiguo(),
				APIs.EA.Suscripcion.Generar()
			};

			return suscripciones;
		}

		public static Suscripcion DevolverSuscripcion(string suscripcionTexto)
		{
			foreach (var suscripcion in GenerarListado())
			{
				if (suscripcion.Id.ToString() == suscripcionTexto)
				{
					return suscripcion;
				}
			}

			return null;
		}

		public static Suscripcion DevolverSuscripcion(SuscripcionTipo suscripcionTipo)
		{
			foreach (var suscripcion in GenerarListado())
			{
				if (suscripcion.Id == suscripcionTipo)
				{
					return suscripcion;
				}
			}

			return null;
		}

		public static Suscripcion DevolverSuscripcion(int posicion)
		{
			SuscripcionTipo suscripcion2 = CargarSuscripciones()[posicion];

			foreach (var suscripcion in GenerarListado())
			{
				if (suscripcion.Id == suscripcion2)
				{
					return suscripcion;
				}
			}

			return null;
		}

		public static List<SuscripcionTipo> CargarSuscripciones()
		{
			List<SuscripcionTipo> suscripciones = Enum.GetValues(typeof(SuscripcionTipo)).Cast<SuscripcionTipo>().ToList();

			return suscripciones;
		}
	}
}
