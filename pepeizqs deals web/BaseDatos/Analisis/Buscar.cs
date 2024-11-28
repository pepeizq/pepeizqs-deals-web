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
							if (idioma == "spanish")
							{
								if (lector.IsDBNull(2) == false)
								{
									analisis.ContenidoEspañol = JsonSerializer.Deserialize<List<SteamAnalisisAPIAnalisis>>(lector.GetString(2));
								}

								if (lector.IsDBNull(7) == false)
								{
									analisis.CantidadPositivos = lector.GetInt32(7);
								}

								if (lector.IsDBNull(8) == false)
								{
									analisis.CantidadNegativos = lector.GetInt32(8);
								}
							}

							if (idioma == "english")
							{
								if (lector.IsDBNull(4) == false)
								{
									analisis.ContenidoIngles = JsonSerializer.Deserialize<List<SteamAnalisisAPIAnalisis>>(lector.GetString(4));
								}

								if (lector.IsDBNull(9) == false)
								{
									analisis.CantidadPositivos = lector.GetInt32(9);
								}

								if (lector.IsDBNull(10) == false)
								{
									analisis.CantidadNegativos = lector.GetInt32(10);
								}
							}

							if (idioma == "latam")
							{
								if (lector.IsDBNull(6) == false)
								{
									analisis.ContenidoLatino = JsonSerializer.Deserialize<List<SteamAnalisisAPIAnalisis>>(lector.GetString(6));
								}

								if (lector.IsDBNull(11) == false)
								{
									analisis.CantidadPositivos = lector.GetInt32(11);
								}

								if (lector.IsDBNull(12) == false)
								{
									analisis.CantidadNegativos = lector.GetInt32(12);
								}
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

								if (fechaRegistrada + TimeSpan.FromDays(1) < DateTime.Now)
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
