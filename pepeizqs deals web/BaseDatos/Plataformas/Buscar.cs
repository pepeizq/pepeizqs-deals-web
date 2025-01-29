#nullable disable

using Microsoft.Data.SqlClient;

namespace BaseDatos.Plataformas
{
	public static class Buscar
	{
		public static void Amazon(string id, SqlConnection conexion = null)
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

			bool yaPuesto = false;

			string busqueda = "SELECT nombre FROM juegos WHERE idAmazon=@idAmazon";

			using (SqlCommand comando = new SqlCommand(busqueda, conexion))
			{
				comando.Parameters.AddWithValue("@idAmazon", id);

				using (SqlDataReader lector = comando.ExecuteReader())
				{
					if (lector.Read() == true)
					{
						yaPuesto = true;
					}
				}
			}

			if (yaPuesto == false)
			{
				bool yaTemporal = false;

				string busqueda2 = "SELECT * FROM temporalamazonjuegos WHERE id=@id";

				using (SqlCommand comando = new SqlCommand(busqueda2, conexion))
				{
					comando.Parameters.AddWithValue("@id", id);

					using (SqlDataReader lector = comando.ExecuteReader())
					{
						if (lector.Read() == true)
						{
							yaTemporal = true;
						}
					}
				}

				if (yaTemporal == false)
				{
					bool yaDescartado = false;

					string busqueda3 = "SELECT * FROM amazonDescartes WHERE id=@id";

					using (SqlCommand comando = new SqlCommand(busqueda3, conexion))
					{
						comando.Parameters.AddWithValue("@id", id);

						using (SqlDataReader lector = comando.ExecuteReader())
						{
							if (lector.Read() == true)
							{
								yaDescartado = true;
							}
						}
					}

					if (yaDescartado == false)
					{
						string sqlInsertar = "INSERT INTO temporalamazonjuegos " +
							"(id) VALUES " +
							"(@id) ";

						using (SqlCommand comando = new SqlCommand(sqlInsertar, conexion))
						{
							comando.Parameters.AddWithValue("@id", id);

							try
							{
								comando.ExecuteNonQuery();
							}
							catch (Exception ex)
							{
								BaseDatos.Errores.Insertar.Mensaje("Amazon Plataforma", ex);
							}
						}
					}
				}
			}
		}
	}
}
