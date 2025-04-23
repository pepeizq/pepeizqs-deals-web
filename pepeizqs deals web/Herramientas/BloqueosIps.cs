namespace Herramientas
{
	public static class BloqueosIps
	{
		public static List<string> ListadoIps()
		{
			List<string> listadoIps = ["47.76.", "47.79.", "47.82.", "47.242.", "47.243.",
				"103.207.", "103.120.",  
				"8.210.", "81.177.", "113.172.", "5.35."];

			return listadoIps;
		}

		public static bool EstaBloqueada(string? ip)
		{
			if (string.IsNullOrEmpty(ip) == false)
			{
				foreach (var ip2 in ListadoIps())
				{
					if (ip.StartsWith(ip2))
					{
						return true;
					}
				}
			}

			return false;
		}
	}
}
