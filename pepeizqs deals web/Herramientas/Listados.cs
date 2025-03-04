#nullable disable

namespace Herramientas
{
	public static class Listados
	{
		public static List<string> Generar(string datos)
		{
			try
			{
				if (datos != null)
				{
					List<string> lista = new List<string>();

					string[] datosPartidos = datos.Split(',');
					lista.AddRange(datosPartidos);

					return lista;
				}
			}
			catch 
			{
				Environment.Exit(1);
			}
		
			return null;
		}
	}
}
