#nullable disable

using Microsoft.Data.SqlClient;

namespace BaseDatos.Pendientes
{
	public static class Buscar
	{
		public static string Nombre(string nombre, SqlConnection conexion)
		{
            string busqueda = "SELECT * FROM juegos WHERE nombre=@nombre";

            using (SqlCommand comando = new SqlCommand(busqueda, conexion))
            {
                comando.Parameters.AddWithValue("@nombre", nombre);

                using (SqlDataReader lector = comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        return lector.GetInt32(0).ToString();
                    }
                }
            }

            return "0";
		}

        public static List<Pendiente> Todos(SqlConnection conexion)
        {
            List<Pendiente> listaPendientes = new List<Pendiente>();

            foreach (var tienda in Tiendas2.TiendasCargar.GenerarListado())
            {
                if (tienda.Id != "steam")
                {
                    string busqueda = "SELECT * FROM tienda" + tienda.Id + " WHERE (idJuegos='0' AND descartado='no')";

                    using (SqlCommand comando = new SqlCommand(busqueda, conexion))
                    {
                        SqlDataReader lector = comando.ExecuteReader();

                        using (lector)
                        {
                            while (lector.Read())
                            {
                                Pendiente pendiente = new Pendiente
                                {
                                    enlace = lector.GetString(0),
                                    nombre = lector.GetString(1),
                                    imagen = lector.GetString(2)
                                };

                                listaPendientes.Add(pendiente);
                            }
                        }
                    }
                }
            }

            return listaPendientes;
        }

		public static List<Pendiente> Tienda(string tiendaId, SqlConnection conexion)
        {
			List<Pendiente> listaPendientes = new List<Pendiente>();

			string busqueda = "SELECT * FROM tienda" + tiendaId + " WHERE (idJuegos='0' AND descartado='no')";

			using (SqlCommand comando = new SqlCommand(busqueda, conexion))
			{
				SqlDataReader lector = comando.ExecuteReader();

				using (lector)
				{
					while (lector.Read())
					{
						Pendiente pendiente = new Pendiente
						{
							enlace = lector.GetString(0),
							nombre = lector.GetString(1),
							imagen = lector.GetString(2)
						};

						listaPendientes.Add(pendiente);
					}
				}
			}

			return listaPendientes;
		}

		public static Pendiente PrimerJuego(string tiendaId, SqlConnection conexion)
		{
			string busqueda = "SELECT * FROM tienda" + tiendaId + " WHERE (idJuegos='0' AND descartado='no')";

			using (SqlCommand comando = new SqlCommand(busqueda, conexion))
			{
				SqlDataReader lector = comando.ExecuteReader();

				using (lector)
				{
					while (lector.Read())
					{
						Pendiente pendiente = new Pendiente
						{
							enlace = lector.GetString(0),
							nombre = lector.GetString(1),
							imagen = lector.GetString(2)
						};

						return pendiente;
					}
				}
			}

			return null;
		}
	}

	public class Pendiente
	{
		public string enlace;
		public string nombre;
		public string imagen;
	}

	public class PendientesTienda
	{
		public List<Pendiente> Pendientes;
		public Tiendas2.Tienda Tienda;
	}
}
