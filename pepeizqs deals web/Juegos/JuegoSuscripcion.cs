#nullable disable

namespace Juegos
{
    public enum JuegoSuscripcion
    {
        HumbleChoice,
        PrimeGaming
    }

	public class JuegoSuscripcion2
	{
		public JuegoSuscripcion Suscripcion { get; set; }
		public int JuegoId { get; set; }
		public DateTime FechaEmpieza { get; set; }
		public DateTime FechaTermina { get; set; }
		public JuegoDRM DRM { get; set; }
		public string Enlace { get; set; }
	}

	public static class Suscripcion2
	{
        public static List<Suscripcion> GenerarListado()
        {
			List<Suscripcion> suscripciones = new List<Suscripcion>();

			//----------------------------

			Suscripcion humbleChoice = new Suscripcion
			{
				Id = JuegoSuscripcion.HumbleChoice,
				Nombre = "Humble Choice",
				Imagen = "/imagenes/suscripciones/humblechoice.png",
				Enlace = "https://www.humblebundle.com/membership/",
				DRMDefecto = JuegoDRM.Steam
			};

			DateTime fechaHumbleChoice = DateTime.Now;
			fechaHumbleChoice = fechaHumbleChoice.AddMonths(1);
			fechaHumbleChoice = new DateTime(fechaHumbleChoice.Year, fechaHumbleChoice.Month, 1, 19, 0, 0);

			int i = 1;
			while (i <= 7)
			{
				if (fechaHumbleChoice.DayOfWeek == DayOfWeek.Tuesday) 
				{
					break;
				}
				else
				{
					fechaHumbleChoice = fechaHumbleChoice.AddDays(1);
				}

				i += 1;
			}

			fechaHumbleChoice = new DateTime(fechaHumbleChoice.Year, fechaHumbleChoice.Month, i, 19, 0, 0);

			humbleChoice.FechaSugerencia = fechaHumbleChoice;

			suscripciones.Add(humbleChoice);

			//----------------------------

			Suscripcion primeGaming = new Suscripcion
			{
				Id = JuegoSuscripcion.PrimeGaming,
				Nombre = "Prime Gaming",
				Imagen = "/imagenes/suscripciones/primegaming.png",
				Enlace = "https://gaming.amazon.com/",
				DRMDefecto = JuegoDRM.Amazon
			};

			DateTime fechaPrime = DateTime.Now;
			fechaPrime = fechaPrime.AddMonths(1);
			fechaPrime = new DateTime(fechaPrime.Year, fechaPrime.Month, fechaPrime.Day, 19, 0, 0);

			primeGaming.FechaSugerencia = fechaPrime;

			suscripciones.Add(primeGaming);

			//----------------------------

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
    }

	public class Suscripcion
	{
		public JuegoSuscripcion Id;
		public string Nombre;
		public string Imagen;
        public string Enlace;
		public DateTime FechaSugerencia;
		public JuegoDRM DRMDefecto;
	}
}
