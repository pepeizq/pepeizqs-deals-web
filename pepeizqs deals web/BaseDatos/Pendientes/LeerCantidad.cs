using Microsoft.Data.SqlClient;

namespace BaseDatos.Pendientes
{
    public static class LeerCantidad
    {
        public static string Ejecutar(string tienda, SqlConnection conexion)
        {
			int cantidad = 0;

			if (conexion.State == System.Data.ConnectionState.Closed)
			{
				conexion = Herramientas.BaseDatos.Conectar();
			}

			using (conexion)
			{
				string busqueda = "SELECT * FROM tienda" + tienda + " WHERE (idJuegos='0' AND descartado='no')";
				SqlCommand comando = new SqlCommand(busqueda, conexion);

				using (comando)
				{
					SqlDataReader lector = comando.ExecuteReader();

					using (lector)
					{
						while (lector.Read())
						{
							cantidad += 1;
						}
					}
				}
			}

			return cantidad.ToString();
		}
    }
}
