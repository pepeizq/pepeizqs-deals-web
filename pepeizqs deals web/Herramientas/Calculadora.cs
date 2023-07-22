namespace Herramientas
{
	public static class Calculadora
	{
		public static int SacarDescuento(decimal precioBase, decimal precioDescontado)
		{
			int descuento = 0;

			if (precioBase > 0 && precioDescontado > 0) 
			{
				if (precioBase != precioDescontado)
				{
					decimal temp = (precioDescontado / precioBase) * 100;

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

				cantidadFinal = numCantidad.ToString() + "+ reviews";
			}

			return cantidadFinal;
		}
	}
}
