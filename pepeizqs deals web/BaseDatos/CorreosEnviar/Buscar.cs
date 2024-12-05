#nullable disable

using Microsoft.Data.SqlClient;

namespace BaseDatos.CorreosEnviar
{
	public static class Buscar
	{
		public static List<CorreoPendienteEnviar> PendientesEnviar(SqlConnection conexion = null)
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

			List<CorreoPendienteEnviar> lista = new List<CorreoPendienteEnviar>();

			string sqlBusqueda = "SELECT * FROM correosEnviar";

			using (SqlCommand comando = new SqlCommand(sqlBusqueda, conexion))
			{
				using (SqlDataReader lector = comando.ExecuteReader())
				{
					while (lector.Read() == true)
					{
						CorreoPendienteEnviar correo = new CorreoPendienteEnviar
						{
							Id = lector.GetInt32(0),
							Html = lector.GetString(1),
							Titulo = lector.GetString(2),
							CorreoDesde = lector.GetString(3),
							CorreoHacia = lector.GetString(4)
						};

						lista.Add(correo);
					}
				}
			}

			return lista;
		}
	}

	public class CorreoPendienteEnviar
	{
		public int Id;
		public string Html;
		public string Titulo;
		public string CorreoDesde;
		public string CorreoHacia;
	}
}
