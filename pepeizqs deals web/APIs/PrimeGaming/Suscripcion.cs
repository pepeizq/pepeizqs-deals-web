using Juegos;

namespace APIs.PrimeGaming
{
	public static class Suscripcion
	{
		public static Suscripciones2.Suscripcion Generar()
		{
			Suscripciones2.Suscripcion primeGaming = new Suscripciones2.Suscripcion
			{
				Id = Suscripciones2.SuscripcionTipo.PrimeGaming,
				Nombre = "Prime Gaming",
				ImagenLogo = "/imagenes/suscripciones/primegaming.webp",
				ImagenIcono = "/imagenes/suscripciones/primegaming_icono.webp",
				ImagenFondo = "/imagenes/suscripciones/primegaming_fondo.webp",
				Enlace = "https://gaming.amazon.com/",
				DRMDefecto = JuegoDRM.Amazon,
				AdminInteractuar = true,
				UsuarioEnlacesEspecificos = true,
				ParaSiempre = true,
                Precio = 4.99
            };

			DateTime fechaPrime = DateTime.Now;
			fechaPrime = fechaPrime.AddMonths(1);
			fechaPrime = new DateTime(fechaPrime.Year, fechaPrime.Month, fechaPrime.Day, 19, 0, 0);

			primeGaming.FechaSugerencia = fechaPrime;

			return primeGaming;
		}

		public static Suscripciones2.Suscripcion GenerarAntiguo()
		{
			Suscripciones2.Suscripcion twitchPrime = new Suscripciones2.Suscripcion
			{
				Id = Suscripciones2.SuscripcionTipo.TwitchPrime,
				Nombre = "Twitch Prime",
				ImagenLogo = "/imagenes/suscripciones/twitchprime_300x80.webp",
				ImagenIcono = "/imagenes/suscripciones/twitchprime_icono.webp",
				Enlace = "https://gaming.amazon.com/",
				DRMDefecto = JuegoDRM.Amazon,
				AdminInteractuar = false,
                UsuarioEnlacesEspecificos = false,
                ParaSiempre = true
            };

			DateTime fechaPrime = DateTime.Now;
			fechaPrime = fechaPrime.AddMonths(1);
			fechaPrime = new DateTime(fechaPrime.Year, fechaPrime.Month, fechaPrime.Day, 19, 0, 0);

			twitchPrime.FechaSugerencia = fechaPrime;

			return twitchPrime;
		}
	}
}
