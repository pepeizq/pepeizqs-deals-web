//https://embed.gog.com/public_wishlist/464176759200/search?hiddenFlag=0&mediaType=0&page=1&sortBy=date_added&totalPages=4

using Microsoft.VisualBasic;

namespace APIs.GOG
{
	public static class Cuenta
	{
		public static void UsuarioId(string usuario)
		{
			string html = "https://www.gog.com/u/" + usuario + "/wishlist";

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


					}
				}
			}
		}
	}
}
