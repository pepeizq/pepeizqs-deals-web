using Juegos;

namespace Herramientas
{
	public static class Precios
	{
		public static bool CalcularAntiguedad(JuegoPrecio precio)
		{
			bool fechaEncaja = true;

			if (precio.FechaTermina.Year > 2022)
			{
				if (DateTime.Now > precio.FechaTermina)
				{
					fechaEncaja = false;
				}
			}
			else
			{
				if (precio.FechaActualizacion.Year > 2022)
				{
					if (precio.FechaActualizacion.DayOfYear + 1 < DateTime.Now.DayOfYear)
					{
						fechaEncaja = false;
					}
				}
			}

			return fechaEncaja;
		}
	}
}
