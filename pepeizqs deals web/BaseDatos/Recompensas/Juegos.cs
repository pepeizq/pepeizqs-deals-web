#nullable disable

using Juegos;
using Microsoft.Data.SqlClient;

namespace BaseDatos.Recompensas
{
	public class RecompensaJuego
	{
		public int Id;
		public int JuegoId;
		public string Clave;
		public int Coins;
		public DateTime FechaEmpieza;
		public string UsuarioId;
		public global::Juegos.JuegoDRM DRM;
	}

	public static class Juegos
	{
		public static RecompensaJuego Cargar(SqlDataReader lector)
		{
			RecompensaJuego juego = new RecompensaJuego
			{
				JuegoId = lector.GetInt32(1),
				Clave = lector.GetString(2),
				Coins = lector.GetInt32(3),
				FechaEmpieza = Convert.ToDateTime(lector.GetString(5))
			};

			if (lector.IsDBNull(4) == false)
			{
				juego.UsuarioId = lector.GetString(4);
			}

			if (lector.IsDBNull(6) == false)
			{
				juego.DRM = Enum.Parse<JuegoDRM>(lector.GetString(6));
			}
			else
			{
				juego.DRM = JuegoDRM.Steam;
			}

			return juego;
		}

		public static void Insertar(RecompensaJuego recompensa)
		{
			SqlConnection conexion = Herramientas.BaseDatos.Conectar();

			using (conexion)
			{
				string sqlInsertar = "INSERT INTO recompensasJuegos " +
					"(juegoId, clave, coins, fecha) VALUES " +
					"(@juegoId, @clave, @coins, @fecha) ";

				using (SqlCommand comando = new SqlCommand(sqlInsertar, conexion))
				{
					comando.Parameters.AddWithValue("@juegoId", recompensa.JuegoId.ToString());
					comando.Parameters.AddWithValue("@clave", recompensa.Clave);
					comando.Parameters.AddWithValue("@coins", recompensa.Coins);
					comando.Parameters.AddWithValue("@fecha", recompensa.FechaEmpieza.ToString());

					comando.ExecuteNonQuery();
					try
					{

					}
					catch
					{

					}
				}
			}
		}

		public static List<RecompensaJuego> Todo()
		{
			List<RecompensaJuego> entradas = new List<RecompensaJuego>();

			SqlConnection conexion = Herramientas.BaseDatos.Conectar();

			using (conexion)
			{
				string busqueda = "SELECT * FROM recompensasJuegos";

				using (SqlCommand comando = new SqlCommand(busqueda, conexion))
				{
					using (SqlDataReader lector = comando.ExecuteReader())
					{
						while (lector.Read())
						{
							entradas.Add(Cargar(lector));
						}
					}
				}
			}

			return entradas;
		}
	}
}
