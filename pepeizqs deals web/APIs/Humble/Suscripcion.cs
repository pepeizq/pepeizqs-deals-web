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
				ImagenLogo = "/imagenes/suscripciones/humblechoice.webp",
				ImagenIcono = "/imagenes/tiendas/humblestore_icono.ico",
				Enlace = "https://www.humblebundle.com/membership/home",
				DRMDefecto = JuegoDRM.Steam,
				AdminInteracturar = true
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
			return enlace + "?partner=pepeizq";
		}

		public static Suscripciones2.Suscripcion GenerarAntiguo()
		{
			Suscripciones2.Suscripcion humbleMonthly = new Suscripciones2.Suscripcion
			{
				Id = Suscripciones2.SuscripcionTipo.HumbleMonthly,
				Nombre = "Humble Monthly",
				ImagenLogo = "/imagenes/suscripciones/humblemonthly.webp",
				ImagenIcono = "/imagenes/tiendas/humblestore_icono.ico",
				Enlace = "https://www.humblebundle.com/membership/home",
				DRMDefecto = JuegoDRM.Steam,
				AdminInteracturar = false
			};

			DateTime fechaHumbleMonthly = DateTime.Now;
			fechaHumbleMonthly = fechaHumbleMonthly.AddMonths(1);
			fechaHumbleMonthly = new DateTime(fechaHumbleMonthly.Year, fechaHumbleMonthly.Month, 1, 19, 0, 0);

			int i = 1;
			while (i <= 7)
			{
				if (fechaHumbleMonthly.DayOfWeek == DayOfWeek.Tuesday)
				{
					break;
				}
				else
				{
					fechaHumbleMonthly = fechaHumbleMonthly.AddDays(1);
				}

				i += 1;
			}

			fechaHumbleMonthly = new DateTime(fechaHumbleMonthly.Year, fechaHumbleMonthly.Month, i, 19, 0, 0);

			humbleMonthly.FechaSugerencia = fechaHumbleMonthly;

			return humbleMonthly;
		}
	}
}
