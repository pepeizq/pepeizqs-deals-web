#nullable disable

using Herramientas;
using Microsoft.Data.SqlClient;

namespace BaseDatos.Enlaces
{
	public static class Insertar
	{
		public static Enlace Ejecutar(string enlace)
		{
            SqlConnection conexion = Herramientas.BaseDatos.Conectar();

            using (conexion)
			{
				string sqlAñadir = "INSERT INTO enlaces " +
					"(base) VALUES " +
					"(@base) ";

				using (SqlCommand comando = new SqlCommand(sqlAñadir, conexion))
				{
					comando.Parameters.AddWithValue("@base", enlace);

					try
					{
						comando.ExecuteNonQuery();
					}
					catch
					{

					}
				}
			}

            conexion.Dispose();

            return Buscar.Base(enlace);
		}
	}
}
