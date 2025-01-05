#nullable disable

namespace Herramientas
{
    public static class Precios
    {
        public static string Euro(decimal? precio)
        {
            if (precio != null)
            {
				string precioMensaje = precio.ToString();
				precioMensaje = precioMensaje.Replace(".", ",");

				if (precioMensaje.Contains(",") == true)
				{
					int int1 = precioMensaje.IndexOf(",");

					if (int1 == precioMensaje.Length - 2)
					{
						precioMensaje = precioMensaje + "0";
					}

					if (precioMensaje.Length > int1 + 3)
					{
						precioMensaje = precioMensaje.Remove(int1 + 3, precioMensaje.Length - int1 - 3);
					}
				}
				else
				{
					precioMensaje = precioMensaje + ",00";
				}

				precioMensaje = precioMensaje + "€";

				return precioMensaje;
			}

			return null;
        }
    }
}
