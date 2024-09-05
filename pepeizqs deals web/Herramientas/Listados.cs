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
					string datos2 = datos;

					int i = 0;
					int j = 100000;

					while (i < j)
					{
						if (datos2.Contains(",") == true)
						{
							int int1 = datos2.IndexOf(",");

							string añadir = datos2.Remove(int1, datos2.Length - int1);

							if (añadir.Length > 0)
							{
								lista.Add(añadir);
							}

							datos2 = datos2.Remove(0, int1 + 1);
						}
						else
						{
							if (datos2.Length > 0)
							{
								lista.Add(datos2);
							}

							break;
						}

						i += 1;
					}

					if (lista.Count > 0)
					{
						return lista;
					}
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
