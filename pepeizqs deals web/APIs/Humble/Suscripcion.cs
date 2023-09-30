using Juegos;

namespace APIs.Humble
{
	public static class Suscripcion
	{
		public static Suscripciones2.Suscripcion Generar()
		{
			Suscripciones2.Suscripcion humbleChoice = new Suscripciones2.Suscripcion
			{
				Id = Suscripciones2.SuscripcionTipo.HumbleChoice,
				Nombre = "Humble Choice",
				Imagen = "/imagenes/suscripciones/humblechoice.webp",
				Enlace = "https://www.humblebundle.com/membership/home",
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

			return humbleChoice;
		}

		public static string Referido(string enlace)
		{
			return enlace + "?partner=pepeizq&refc=gXsa9X";
		}
	}
}
