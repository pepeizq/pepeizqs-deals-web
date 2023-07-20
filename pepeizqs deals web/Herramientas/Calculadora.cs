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
	}
}
