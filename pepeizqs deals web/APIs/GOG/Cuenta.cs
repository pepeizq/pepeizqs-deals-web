//https://embed.gog.com/public_wishlist/464176759200/search?hiddenFlag=0&mediaType=0&page=1&sortBy=date_added&totalPages=4
//https://www.gog.com/u/pepeizq/games/stats?sort=recent_playtime&order=desc&page=3

#nullable disable

using Herramientas;
using Microsoft.VisualBasic;

namespace APIs.GOG
{
	public static class Cuenta
	{
		public static async Task<string> UsuarioId(string usuario)
		{
			if (string.IsNullOrEmpty(usuario) == false)
			{
				usuario = usuario.Replace("https://www.gog.com/u/", null);
				usuario = usuario.Replace("http://www.gog.com/u/", null);

				if (usuario.Contains("?") == true)
				{
					int int1 = usuario.IndexOf("?");
					usuario = usuario.Remove(int1, usuario.Length - int1);
				}

				string html = await Decompiladores.Estandar("https://www.gog.com/u/" + usuario + "/wishlist");

				if (string.IsNullOrEmpty(html) == false)
				{
					if (html.Contains("gog-user=") == true)
					{
						int int1 = html.IndexOf("gog-user=");

						if (int1 > -1)
						{
							string temp1 = html.Remove(0, int1 + 10);

							int int2 = temp1.IndexOf(Strings.ChrW(34));
							string temp2 = temp1.Remove(int2, temp1.Length - int2);

							return temp2;
						}
					}
				}
			}

			return null;
		}

		public static async Task<List<string>> ListadoJuegos(string usuario)
		{
			if (string.IsNullOrEmpty(usuario) == false)
			{
				usuario = usuario.Replace("https://www.gog.com/u/", null);
				usuario = usuario.Replace("http://www.gog.com/u/", null);

				if (usuario.Contains("?") == true)
				{
					int int1 = usuario.IndexOf("?");
					usuario = usuario.Remove(int1, usuario.Length - int1);
				}

				string html = await Decompiladores.Estandar("https://www.gog.com/u/" + usuario + "/games/stats?page=1");

				if (string.IsNullOrEmpty(html) == false)
				{

				}
			}
		}
	}
}
