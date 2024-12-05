#nullable disable

using Microsoft.Data.SqlClient;

namespace BaseDatos.CorreosEnviar
{
	public static class Insertar
	{
		public static void Ejecutar(string html, string titulo, string correoDesde, string correoHacia, SqlConnection conexion = null)
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

			string sqlAñadir = "INSERT INTO correosEnviar " +
					 "(html, titulo, correoDesde, correoHacia) VALUES " +
					 "(@html, @titulo, @correoDesde, @correoHacia) ";

			using (SqlCommand comando = new SqlCommand(sqlAñadir, conexion))
			{
				comando.Parameters.AddWithValue("@html", html);
				comando.Parameters.AddWithValue("@titulo", titulo);
				comando.Parameters.AddWithValue("@correoDesde", correoDesde);
				comando.Parameters.AddWithValue("@correoHacia", correoHacia);

				try
				{
					comando.ExecuteNonQuery();
				}
				catch
				{

				}
			}
		}
	}
}
