namespace Herramientas
{
	public enum Tiempo
	{
		Atemporal,
		Actual,
		Pasado
	}

	public static class Calculadora
	{
		public static int SacarDescuento(decimal precioBase, decimal precioRebajado)
		{
			int descuento = 0;

			if (precioBase > 0 && precioRebajado > 0) 
			{
				if (precioBase != precioRebajado)
				{
					decimal temp = (precioRebajado / precioBase) * 100;

					descuento = Decimal.ToInt32(100 - temp);
				}
			}

			return descuento;
		}

		public static string RedondearAnalisis(string idioma, string cantidad)
		{
			int numCantidad = int.Parse(cantidad.Replace(",", null));
			string cantidadFinal = string.Empty;

			if (numCantidad > 0)
			{
				int divisor = 0;

				if (numCantidad > 99 && numCantidad < 1000)
				{
					divisor = 100;
				}
				else if (numCantidad > 999 && numCantidad < 10000)
				{
					divisor = 1000;
				}
				else if (numCantidad > 9999 && numCantidad < 100000)
				{
					divisor = 10000;
				}
				else if (numCantidad > 99999 && numCantidad < 1000000)
				{
					divisor = 100000;
				}
				else if (numCantidad > 999999 && numCantidad < 10000000)
				{
					divisor = 1000000;
				}
				else if (numCantidad > 9999999)
				{
					divisor = 10000000;
				}

				numCantidad = (int)(Math.Round(double.Parse(cantidad.Replace(",", null)) / divisor, 0) * divisor);

				cantidadFinal = numCantidad.ToString("N0") + "+ " + Idiomas.CogerCadena(idioma, "Game.String9");
			}

			return cantidadFinal;
		}

		public static string HaceTiempo(this DateTime fecha, string idioma)
		{
			string mensaje = string.Empty;
			TimeSpan diferenciaTiempo = DateTime.Now.Subtract(fecha);

			if (diferenciaTiempo <= TimeSpan.FromSeconds(60))
			{
				mensaje = string.Format(Idiomas.CogerCadena(idioma, "Timer.String1"), diferenciaTiempo.Seconds);
			}
			else if (diferenciaTiempo <= TimeSpan.FromMinutes(60))
			{
				if (diferenciaTiempo.Minutes == 1)
				{
					mensaje = Idiomas.CogerCadena(idioma, "Timer.String3");
				}
				else if (diferenciaTiempo.Minutes > 1)
				{
					mensaje = string.Format(Idiomas.CogerCadena(idioma, "Timer.String2"), diferenciaTiempo.Minutes);
				}

				//mensaje = diferenciaTiempo.Minutes > 1 ? string.Format(Idiomas.CogerCadena(idioma, "Timer.String2"), diferenciaTiempo.Minutes) : Idiomas.CogerCadena(idioma, "Timer.String3");
			}
			else if (diferenciaTiempo <= TimeSpan.FromHours(24))
			{
				if (diferenciaTiempo.Hours == 1)
				{
					mensaje = Idiomas.CogerCadena(idioma, "Timer.String5");
				}
				else if (diferenciaTiempo.Hours > 1)
				{
					mensaje = string.Format(Idiomas.CogerCadena(idioma, "Timer.String4"), diferenciaTiempo.Hours);
				}

				//mensaje = diferenciaTiempo.Hours > 1 ? string.Format(Idiomas.CogerCadena(idioma, "Timer.String4"), diferenciaTiempo.Hours) : Idiomas.CogerCadena(idioma, "Timer.String5");
			}
			else if (diferenciaTiempo <= TimeSpan.FromDays(30))
			{
				if (diferenciaTiempo.Days == 1)
				{
					mensaje = Idiomas.CogerCadena(idioma, "Timer.String7");
				}
				else if (diferenciaTiempo.Days > 1)
				{
					mensaje = string.Format(Idiomas.CogerCadena(idioma, "Timer.String6"), diferenciaTiempo.Days);
				}

				//mensaje = diferenciaTiempo.Days > 1 ? string.Format(Idiomas.CogerCadena(idioma, "Timer.String6"), diferenciaTiempo.Days) : Idiomas.CogerCadena(idioma, "Timer.String7");
			}
			else if (diferenciaTiempo <= TimeSpan.FromDays(365))
			{
				if (diferenciaTiempo.Days > 30 && diferenciaTiempo.Days < 60)
				{
					mensaje = Idiomas.CogerCadena(idioma, "Timer.String9");
				}
				else if (diferenciaTiempo.Days >= 60)
				{
					mensaje = string.Format(Idiomas.CogerCadena(idioma, "Timer.String8"), diferenciaTiempo.Days / 30);
				}

				//mensaje = diferenciaTiempo.Days > 30 ? string.Format(Idiomas.CogerCadena(idioma, "Timer.String8"), diferenciaTiempo.Days / 30) : Idiomas.CogerCadena(idioma, "Timer.String9");
			}
			else
			{
				if (diferenciaTiempo.Days > 365 && diferenciaTiempo.Days < 730)
				{
					mensaje = Idiomas.CogerCadena(idioma, "Timer.String11");
				}
				else if (diferenciaTiempo.Days >= 730)
				{
					mensaje = string.Format(Idiomas.CogerCadena(idioma, "Timer.String10"), diferenciaTiempo.Days / 365);
				}

				//mensaje = diferenciaTiempo.Days > 365 ? string.Format(Idiomas.CogerCadena(idioma, "Timer.String10"), diferenciaTiempo.Days / 365) : Idiomas.CogerCadena(idioma, "Timer.String11");
			}

			return mensaje;
		}

	}
}
