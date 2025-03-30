namespace Herramientas
{
	public static class BloqueosIps
	{
		public static List<string> ListadoIps()
		{
			List<string> listadoIps = ["47.79.", "47.82.", "103.207."];

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
