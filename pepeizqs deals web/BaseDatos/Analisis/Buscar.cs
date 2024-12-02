#nullable disable

using APIs.Steam;
using Juegos;
using Microsoft.Data.SqlClient;
using System.Text.Json;

namespace BaseDatos.Analisis
{
	public static class Buscar
	{
		public static JuegoAnalisisAmpliado Cargar(int id, string idioma, SqlConnection conexion = null)
		{
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

			JuegoAnalisisAmpliado analisis = new JuegoAnalisisAmpliado();

			using (conexion)
			{
				string sqlBusqueda = "SELECT * FROM juegosAnalisis WHERE id=@id";

				using (SqlCommand comando = new SqlCommand(sqlBusqueda, conexion))
				{
					comando.Parameters.AddWithValue("@id", id);

					using (SqlDataReader lector = comando.ExecuteReader())
					{
						if (lector.Read() == true)
						{
							if (lector.IsDBNull(lector.GetOrdinal("contenido" + idioma)) == false)
							{
								analisis.Contenido = JsonSerializer.Deserialize<List<SteamAnalisisAPIAnalisis>>(lector.GetString(lector.GetOrdinal("contenido" + idioma)));
							}

							if (lector.IsDBNull(lector.GetOrdinal("positivos" + idioma)) == false)
							{
								analisis.CantidadPositivos = lector.GetInt32(lector.GetOrdinal("positivos" + idioma));
							}

							if (lector.IsDBNull(lector.GetOrdinal("negativos" + idioma)) == false)
							{
								analisis.CantidadNegativos = lector.GetInt32(lector.GetOrdinal("negativos" + idioma));
							}
						}
					}
				}
			}

			return analisis;
		}

		public static bool DebeModificarse(int id, string idioma, SqlConnection conexion = null)
		{
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
				string sqlBusqueda = "SELECT fecha" + idioma + " FROM juegosAnalisis WHERE id=@id";

				using (SqlCommand comando = new SqlCommand(sqlBusqueda, conexion))
				{
					comando.Parameters.AddWithValue("@id", id);

					using (SqlDataReader lector = comando.ExecuteReader())
					{
						if (lector.Read() == false)
						{
							return true;
						}
						else
						{
							if (lector.IsDBNull(0) == true)
							{
								return true;
							}
							else
							{
								DateTime fechaRegistrada = lector.GetDateTime(0);

								if (fechaRegistrada + TimeSpan.FromDays(7) < DateTime.Now)
								{
									return true;
								}
							}
						}
					}
				}

				return false;
			}
		}
	}
}
