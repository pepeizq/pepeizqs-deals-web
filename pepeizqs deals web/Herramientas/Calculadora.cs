namespace Herramientas
{
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

		public static string RedondearAnalisis(string cantidad)
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

				cantidadFinal = numCantidad.ToString("N0") + "+ reviews";
			}

			return cantidadFinal;
		}

		public static string HaceTiempo(this DateTime fecha)
		{
			string resultado = string.Empty;
			TimeSpan diferenciaTiempo = DateTime.Now.Subtract(fecha);

			if (diferenciaTiempo <= TimeSpan.FromSeconds(60))
			{
				resultado = string.Format("{0} seconds ago", diferenciaTiempo.Seconds);
			}
			else if (diferenciaTiempo <= TimeSpan.FromMinutes(60))
			{
				resultado = diferenciaTiempo.Minutes > 1 ? string.Format("about {0} minutes ago", diferenciaTiempo.Minutes) : "about a minute ago";
			}
			else if (diferenciaTiempo <= TimeSpan.FromHours(24))
			{
				resultado = diferenciaTiempo.Hours > 1 ? string.Format("about {0} hours ago", diferenciaTiempo.Hours) : "about an hour ago";
			}
			else if (diferenciaTiempo <= TimeSpan.FromDays(30))
			{
				resultado = diferenciaTiempo.Days > 1 ? string.Format("about {0} days ago", diferenciaTiempo.Days) : "yesterday";
			}
			else if (diferenciaTiempo <= TimeSpan.FromDays(365))
			{
				resultado = diferenciaTiempo.Days > 30 ? string.Format("about {0} months ago", diferenciaTiempo.Days / 30) : "about a month ago";
			}
			else
			{
				resultado = diferenciaTiempo.Days > 365 ? string.Format("about {0} years ago", diferenciaTiempo.Days / 365) : "about a year ago";
			}

			return resultado;
		}

	}
}
