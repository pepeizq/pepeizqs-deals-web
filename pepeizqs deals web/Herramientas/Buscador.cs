#nullable disable

namespace Herramientas
{
	public static class Buscador
	{
		public static string LimpiarNombre(string nombre)
		{
			if (nombre != null)
			{
				List<string> caracteres = new List<string>
				{
					":", ",", ".", "®", "™", "_", "-", ">", "<", ";", "(", ")", "[", "]", "=", "?", "¿", "'", "¡", "!", "&", "|",
					"/", "\\", "{", "}", "#", "´", "’", "~", " "
				};

				foreach (string caracter in caracteres)
				{
					nombre = nombre.Replace(caracter, null);
				}

				nombre = nombre.ToLower();
				nombre = nombre.Trim();

				return nombre;
			}

			return null;
		}
	}
}
