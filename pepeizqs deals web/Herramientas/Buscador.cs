#nullable disable

using Microsoft.VisualBasic;

namespace Herramientas
{
	public static class Buscador
	{
		public static string LimpiarNombre(string nombre, bool quitarEspacio = false)
		{
			if (nombre != null)
			{
				if (quitarEspacio == true)
				{
					nombre = nombre.Replace(" ", null);
				}

				List<string> caracteres = new List<string>
				{
					"!", "#", "$", "%", "&", "'", "(", ")", "*", "+", ",", "-", ".", "/",
					":", ";", "<", "=", ">", "?", "@", "[", "\\", "]", "^", "_", "`", "{", "|", "}", "~", "€", "‚", "ƒ", "„",
					"…", "†", "‡", "ˆ", "‰", "Š", "‹", "Œ", "Ž", "‘", "’", "“", "”", "•", "˜", "™", "š", "›", "œ", "ž", "Ÿ", "¡",
					"¢", "£", "¤", "¥", "¦", "§", "¨", "©", "ª", "«", "¬", "®", "¯", "°", "±", "²", "³", "´", "µ", "¶", "·", "¸",
					"¹", "º", "»", "¼", "½", "¾", "¿", "Æ", "Ç", "Ð", "×", "Ø", "Þ", "ß", "æ", "ç", "÷", "ø", "þ", 
					Strings.ChrW(34).ToString(), Strings.ChrW(127).ToString(), Strings.ChrW(129).ToString(), Strings.ChrW(141).ToString(),
					Strings.ChrW(143).ToString(), Strings.ChrW(144).ToString(), Strings.ChrW(150).ToString(), Strings.ChrW(151).ToString(),
					Strings.ChrW(157).ToString(), Strings.ChrW(160).ToString(), Strings.ChrW(173).ToString()
				};

				foreach (string caracter in caracteres)
				{
					nombre = nombre.Replace(caracter, null);
				}

				nombre = nombre.Replace("À", "A");
				nombre = nombre.Replace("Á", "A");
				nombre = nombre.Replace("Â", "A");
				nombre = nombre.Replace("Ã", "A");
				nombre = nombre.Replace("Ä", "A");
				nombre = nombre.Replace("Å", "A");
				nombre = nombre.Replace("à", "a");
				nombre = nombre.Replace("á", "a");
				nombre = nombre.Replace("â", "a");
				nombre = nombre.Replace("ã", "a");
				nombre = nombre.Replace("ä", "a");
				nombre = nombre.Replace("å", "a");

				nombre = nombre.Replace("È", "E");
				nombre = nombre.Replace("É", "E");
				nombre = nombre.Replace("Ê", "E");
				nombre = nombre.Replace("Ë", "E");
				nombre = nombre.Replace("è", "e");
				nombre = nombre.Replace("é", "e");
				nombre = nombre.Replace("ê", "e");
				nombre = nombre.Replace("ë", "e");

				nombre = nombre.Replace("Ì", "I");
				nombre = nombre.Replace("Í", "I");
				nombre = nombre.Replace("Î", "I");
				nombre = nombre.Replace("Ï", "I");
				nombre = nombre.Replace("ì", "i");
				nombre = nombre.Replace("í", "i");
				nombre = nombre.Replace("î", "i");
				nombre = nombre.Replace("ï", "i");

				nombre = nombre.Replace("Ò", "O");
				nombre = nombre.Replace("Ó", "O");
				nombre = nombre.Replace("Ô", "O");
				nombre = nombre.Replace("Õ", "O");
				nombre = nombre.Replace("Ö", "O");
				nombre = nombre.Replace("ð", "o");
				nombre = nombre.Replace("ò", "o");
				nombre = nombre.Replace("ó", "o");
				nombre = nombre.Replace("ô", "o");
				nombre = nombre.Replace("õ", "o");
				nombre = nombre.Replace("ö", "o");

				nombre = nombre.Replace("Ù", "U");
				nombre = nombre.Replace("Ú", "U");
				nombre = nombre.Replace("Û", "U");
				nombre = nombre.Replace("Ü", "U");
				nombre = nombre.Replace("ù", "u");
				nombre = nombre.Replace("ú", "u");
				nombre = nombre.Replace("û", "u");
				nombre = nombre.Replace("ü", "u");

				nombre = nombre.Replace("Ñ", "N");
				nombre = nombre.Replace("ñ", "n");

				nombre = nombre.Replace("Ý", "Y");
				nombre = nombre.Replace("ý", "y");
				nombre = nombre.Replace("ÿ", "y");

				nombre = nombre.ToLower();
				nombre = nombre.Trim();

				return nombre;
			}

			return null;
		}
	}
}
