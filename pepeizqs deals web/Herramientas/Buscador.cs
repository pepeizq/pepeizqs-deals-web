#nullable disable

using Microsoft.VisualBasic;

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
					//":", ",", ".", "®", "™", "_", "-", ">", "<", ";", "(", ")", "[", "]", "=", "?", "¿", "'", "¡", "!", "&", "|",
					//"/", "\\", "{", "}", "#", "´", "’", "~", "∀", " ",
					//https://yorktown.cbe.wwu.edu/sandvig/shared/asciicodes.aspx
					" ", "!", Strings.ChrW(34).ToString(), "#", "$", "%", "&", "'", "(", ")", "*", "+", ",", "-", ".", "/",
					":", ";", "<"
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
