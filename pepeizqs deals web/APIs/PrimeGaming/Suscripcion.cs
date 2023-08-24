﻿using Juegos;

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
				Imagen = "/imagenes/suscripciones/primegaming.png",
				Enlace = "https://gaming.amazon.com/",
				DRMDefecto = JuegoDRM.Amazon
			};

			DateTime fechaPrime = DateTime.Now;
			fechaPrime = fechaPrime.AddMonths(1);
			fechaPrime = new DateTime(fechaPrime.Year, fechaPrime.Month, fechaPrime.Day, 19, 0, 0);

			primeGaming.FechaSugerencia = fechaPrime;

			return primeGaming;
		}
	}
}