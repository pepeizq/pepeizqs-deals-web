#nullable disable

using Juegos;
using Microsoft.Data.SqlClient;

namespace BaseDatos.Portada
{
	public static class Buscar
	{
		public static List<Juego> Destacados(List<string> idsSteam = null, SqlConnection conexion = null)
		{
			List<Juego> resultados = new List<Juego>();

			if (conexion == null)
			{
				conexion = Herramientas.BaseDatos.Conectar();
			}
			else
			{
				if (conexion.State != System.Data.ConnectionState.Open)
				{
					conexion = Herramientas.BaseDatos.Conectar();
				}
			}

			using (conexion)
			{
				string descartarSteam = null;

				//if (idsSteam != null)
				//{
				//	if (idsSteam.Count > 0)
				//	{
				//		int i = 0;
				//		foreach (var id in idsSteam)
				//		{
				//			if (i == 0)
				//			{
				//				descartarSteam = "AND idSteam NOT IN (" + id;
				//			}
				//			else if (i > 0)
				//			{
				//				descartarSteam = descartarSteam + "," + id;
				//			}

				//			i += 1;
				//		}

				//		descartarSteam = descartarSteam + ")";
				//	}
				//}

				string busqueda = @"SELECT TOP 6 * FROM seccionMinimos
									WHERE tipo = 0 AND JSON_VALUE(precioMinimosHistoricos, '$[0].DRM') = 0 AND CONVERT(bigint, REPLACE(JSON_VALUE(analisis, '$.Cantidad'),',','')) > 4999 AND bundles IS NULL AND gratis IS NULL AND suscripciones IS NULL @descartarSteam
									ORDER BY NEWID()";

				busqueda = busqueda.Replace("@descartarSteam", descartarSteam);

				using (SqlCommand comando = new SqlCommand(busqueda, conexion))
				{
					using (SqlDataReader lector = comando.ExecuteReader())
					{
						while (lector.Read())
						{
							Juego juego = new Juego();
							juego = Juegos.Buscar.Cargar(juego, lector);

							resultados.Add(juego);
						}
					}
				}
			}

			return resultados;
		}

		public static List<Juego> UltimosMinimos(int cantidad, List<string> categorias = null, List<string> drms = null, SqlConnection conexion = null)
		{
			List<Juego> resultados = new List<Juego>();

			if (conexion == null)
			{
				conexion = Herramientas.BaseDatos.Conectar();
			}
			else
			{
				if (conexion.State != System.Data.ConnectionState.Open)
				{
					conexion = Herramientas.BaseDatos.Conectar();
				}
			}

			using (conexion)
			{
				string categoria = null;

				if (categorias != null)
				{
					if (categorias.Count > 0)
					{
						int i = 0;
						foreach (var valor in categorias)
						{
							if (i == 0)
							{
								categoria = categoria + " AND (tipo = " + valor;
							}
							else if (i > 0)
							{
								categoria = categoria + " OR tipo = " + valor;
							}

							i += 1;
						}

						if (string.IsNullOrEmpty(categoria) == false)
						{
							categoria = categoria + ")";
						}
					}
				}

				string drm = null;

				if (drms != null)
				{
					if (drms.Count > 0)
					{
						int i = 0;
						foreach (var valor in drms)
						{
							if (i == 0)
							{
								drm = drm + " AND (JSON_VALUE(precioMinimosHistoricos, '$[0].DRM') = " + valor;
							}
							else if (i > 0)
							{
								drm = drm + " OR JSON_VALUE(precioMinimosHistoricos, '$[0].DRM') = " + valor;
							}

							i += 1;
						}

						if (string.IsNullOrEmpty(drm) == false)
						{
							drm = drm + ")";
						}
					}
				}

				string busqueda = @"SELECT TOP @cantidad *, CONVERT(datetime2, JSON_VALUE(precioMinimosHistoricos, '$[0].FechaDetectado')) AS Fecha FROM seccionMinimos 
                                    WHERE CONVERT(bigint, REPLACE(JSON_VALUE(analisis, '$.Cantidad'),',','')) > 199 AND JSON_VALUE(precioMinimosHistoricos, '$[0].FechaDetectado') > DATEADD(day, -21, CAST(GETDATE() AS date)) @categoria @drm
                                    ORDER BY Fecha DESC";

				busqueda = busqueda.Replace("@cantidad", cantidad.ToString());
				busqueda = busqueda.Replace("@categoria", categoria);
				busqueda = busqueda.Replace("@drm", drm);

				using (SqlCommand comando = new SqlCommand(busqueda, conexion))
				{
					using (SqlDataReader lector = comando.ExecuteReader())
					{
						while (lector.Read())
						{
							Juego juego = new Juego();
							juego = Juegos.Buscar.Cargar(juego, lector);

							resultados.Add(juego);
						}
					}
				}
			}

			return resultados;
		}
	}
}
