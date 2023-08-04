using Microsoft.Data.SqlClient;

namespace BaseDatos.Pendientes
{
    public static class LeerCantidad
    {
        public static string Ejecutar(string tienda)
        {
			int cantidad = 0;

			SqlConnection conexion = Herramientas.BaseDatos.Conectar();

			using (conexion)
			{
				conexion.Open();

				string busqueda = "SELECT * FROM tienda" + tienda;
				SqlCommand comando = new SqlCommand(busqueda, conexion);

				using (comando)
				{
					SqlDataReader lector = comando.ExecuteReader();

					using (lector)
					{
						while (lector.Read())
						{
							if (lector.GetString(3) == "0")
							{
								cantidad += 1;
							}
						}
					}
				}
			}

			conexion.Dispose();

			return cantidad.ToString();
		}
    }
}
