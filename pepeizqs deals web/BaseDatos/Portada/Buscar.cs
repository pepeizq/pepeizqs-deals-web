#nullable disable

using Juegos;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;

namespace BaseDatos.Portada
{
	public static class Buscar
	{
		public static List<Juego> Minimos(SqlConnection conexion = null)
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
				string busqueda = @"SELECT * FROM juegos
									WHERE ultimaModificacion >= DATEADD(day, -3, GETDATE()) AND JSON_PATH_EXISTS(analisis, '$.Cantidad') > 0 AND 
									CONVERT(bigint, REPLACE(JSON_VALUE(analisis, '$.Cantidad'),',','')) > 99 AND 
									((mayorEdad IS NOT NULL AND mayorEdad = 'false') OR (mayorEdad IS NULL)) AND 
									(freeToPlay = 'false' OR freeToPlay IS NULL) AND
									JSON_VALUE(precioMinimosHistoricos, '$[0].Descuento') > 14";

				using (SqlCommand comando = new SqlCommand(busqueda, conexion))
				{
					using (SqlDataReader lector = comando.ExecuteReader())
					{
						while (lector.Read())
						{
							Juego juego = new Juego();
							juego = BaseDatos.Juegos.Buscar.Cargar(juego, lector);

							resultados.Add(juego);
						}
					}
				}
			}
			
			return resultados;
		}

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
				string busqueda = @"SELECT TOP 6 idMaestra, nombre, imagenes, precioMinimosHistoricos, JSON_VALUE(media, '$.Video') as video, idSteam FROM seccionMinimos
									WHERE tipo = 0 AND JSON_VALUE(precioMinimosHistoricos, '$[0].DRM') = 0 AND CONVERT(bigint, REPLACE(JSON_VALUE(analisis, '$.Cantidad'),',','')) > 4999 AND bundles IS NULL AND gratis IS NULL AND suscripciones IS NULL
									ORDER BY NEWID()";

				using (SqlCommand comando = new SqlCommand(busqueda, conexion))
				{
					using (SqlDataReader lector = comando.ExecuteReader())
					{
						while (lector.Read())
						{
							Juego juego = new Juego();

							if (lector.IsDBNull(0) == false)
							{
								juego.Id = lector.GetInt32(0);
								juego.IdMaestra = lector.GetInt32(0);
							}

							if (lector.IsDBNull(1) == false)
							{
								if (string.IsNullOrEmpty(lector.GetString(1)) == false)
								{
									juego.Nombre = lector.GetString(1);
								}
							}

							if (lector.IsDBNull(2) == false)
							{
								if (string.IsNullOrEmpty(lector.GetString(2)) == false)
								{
									juego.Imagenes = JsonConvert.DeserializeObject<JuegoImagenes>(lector.GetString(2));
								}
							}

							if (lector.IsDBNull(3) == false)
							{
								if (string.IsNullOrEmpty(lector.GetString(3)) == false)
								{
									juego.PrecioMinimosHistoricos = JsonConvert.DeserializeObject<List<JuegoPrecio>>(lector.GetString(3));
								}
							}

							if (lector.IsDBNull(4) == false)
							{
								if (string.IsNullOrEmpty(lector.GetString(4)) == false)
								{
									JuegoMedia media = new JuegoMedia();
									media.Video = lector.GetString(4);

									juego.Media = media;
								}
							}

							if (lector.IsDBNull(5) == false)
							{
								juego.IdSteam = lector.GetInt32(5);
							}

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

				string busqueda = @"SELECT DISTINCT TOP @cantidad idMaestra, nombre, imagenes, precioMinimosHistoricos, JSON_VALUE(media, '$.Video'), bundles, gratis, suscripciones, idSteam, CONVERT(datetime2, JSON_VALUE(precioMinimosHistoricos, '$[0].FechaDetectado')) AS Fecha FROM seccionMinimos 
                                    WHERE CONVERT(bigint, REPLACE(JSON_VALUE(analisis, '$.Cantidad'),',','')) > 199 AND CONVERT(datetime2, JSON_VALUE(precioMinimosHistoricos, '$[0].FechaDetectado')) > DATEADD(day, -4, CAST(GETDATE() AS date)) @categoria @drm
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

							if (lector.IsDBNull(0) == false)
							{
								juego.Id = lector.GetInt32(0);
								juego.IdMaestra = lector.GetInt32(0);
							}

							if (lector.IsDBNull(1) == false)
							{
								if (string.IsNullOrEmpty(lector.GetString(1)) == false)
								{
									juego.Nombre = lector.GetString(1);
								}
							}

							if (lector.IsDBNull(2) == false)
							{
								if (string.IsNullOrEmpty(lector.GetString(2)) == false)
								{
									juego.Imagenes = JsonConvert.DeserializeObject<JuegoImagenes>(lector.GetString(2));
								}
							}

							if (lector.IsDBNull(3) == false)
							{
								if (string.IsNullOrEmpty(lector.GetString(3)) == false)
								{
									juego.PrecioMinimosHistoricos = JsonConvert.DeserializeObject<List<JuegoPrecio>>(lector.GetString(3));
								}
							}

							if (lector.IsDBNull(4) == false)
							{
								if (string.IsNullOrEmpty(lector.GetString(4)) == false)
								{
									JuegoMedia media = new JuegoMedia();
									media.Video = lector.GetString(4);

									juego.Media = media;
								}
							}

							if (lector.IsDBNull(5) == false)
							{
								if (string.IsNullOrEmpty(lector.GetString(5)) == false)
								{
									juego.Bundles = JsonConvert.DeserializeObject<List<JuegoBundle>>(lector.GetString(5));
								}
							}

							if (lector.IsDBNull(6) == false)
							{
								if (string.IsNullOrEmpty(lector.GetString(6)) == false)
								{
									juego.Gratis = JsonConvert.DeserializeObject<List<JuegoGratis>>(lector.GetString(6));
								}
							}

							if (lector.IsDBNull(7) == false)
							{
								if (string.IsNullOrEmpty(lector.GetString(7)) == false)
								{
									juego.Suscripciones = JsonConvert.DeserializeObject<List<JuegoSuscripcion>>(lector.GetString(7));
								}
							}

							if (lector.IsDBNull(8) == false)
							{
								juego.IdSteam = lector.GetInt32(8);
							}

							resultados.Add(juego);
						}
					}
				}
			}

			return resultados;
		}
	}
}
