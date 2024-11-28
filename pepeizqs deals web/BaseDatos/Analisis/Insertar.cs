#nullable disable

using Microsoft.Data.SqlClient;

namespace BaseDatos.Analisis
{
	public static class Insertar
	{
		public static void Ejecutar(int id, int positivos, int negativos, string idioma, string contenido, SqlConnection conexion = null)
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
				bool insertar = false;

				string sqlBusqueda = "SELECT id FROM juegosAnalisis WHERE id=@id";

				using (SqlCommand comando = new SqlCommand(sqlBusqueda, conexion))
				{
					comando.Parameters.AddWithValue("@id", id);

					using (SqlDataReader lector = comando.ExecuteReader())
					{
						if (lector.Read() == false)
						{
							insertar = true;
						}
					}
				}

				if (insertar == true)
				{
					string sqlInsertar = "INSERT INTO juegosAnalisis " +
						"(id, positivos" + idioma + ", negativos" + idioma + ", fecha" + idioma + ", contenido" + idioma + ") VALUES " +
						"(@id, @positivos" + idioma + ", @negativos" + idioma + ", @fecha" + idioma + ", @contenido" + idioma + ") ";

					using (SqlCommand comando = new SqlCommand(sqlInsertar, conexion))
					{
						comando.Parameters.AddWithValue("@id", id);
						comando.Parameters.AddWithValue("@positivos" + idioma, positivos);
						comando.Parameters.AddWithValue("@negativos" + idioma, negativos);
						comando.Parameters.AddWithValue("@fecha" + idioma, DateTime.Now);
						comando.Parameters.AddWithValue("@contenido" + idioma, contenido);

						comando.ExecuteNonQuery();
						try
						{
							
						}
						catch
						{

						}
					}
				}
				else
				{
					string sqlActualizar = "UPDATE juegosAnalisis " +
						"SET positivos" + idioma + "=@positivos" + idioma + ", negativos" + idioma + "=@negativos" + idioma + ", fecha" + idioma + "=@fecha" + idioma + ", contenido" + idioma + "=@contenido" + idioma + " " +
						"WHERE id=@id";

					using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
					{
						comando.Parameters.AddWithValue("@id", id);
						comando.Parameters.AddWithValue("@positivos" + idioma, positivos);
						comando.Parameters.AddWithValue("@negativos" + idioma, negativos);
						comando.Parameters.AddWithValue("@fecha" + idioma, DateTime.Now);
						comando.Parameters.AddWithValue("@contenido" + idioma, contenido);

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
		}
	}
}
