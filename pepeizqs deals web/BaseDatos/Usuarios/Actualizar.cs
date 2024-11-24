#nullable disable

using Microsoft.Data.SqlClient;
using Newtonsoft.Json;

namespace BaseDatos.Usuarios
{
    public static class Actualizar
    {
        public static void Claves(SqlConnection conexion, string usuarioId, Clave nuevaClave)
        {
            List<Clave> claves = new List<Clave>();

            string busqueda = "SELECT * FROM AspNetUsers WHERE Id=@Id";

            using (SqlCommand comando = new SqlCommand(busqueda, conexion))
            {
                comando.Parameters.AddWithValue("@Id", usuarioId);

                using (SqlDataReader lector = comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        if (lector.IsDBNull(24) == false)
                        {
                            if (string.IsNullOrEmpty(lector.GetString(24)) == false)
                            {
                                claves = JsonConvert.DeserializeObject<List<Clave>>(lector.GetString(24));
                            }
                        }
                    }
                }
            }

            claves.Add(nuevaClave);

            string sqlActualizar = "UPDATE AspNetUsers " +
                    "SET Keys=@Keys WHERE Id=@Id";

            using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
            {
                comando.Parameters.AddWithValue("@Id", usuarioId);
                comando.Parameters.AddWithValue("@Keys", JsonConvert.SerializeObject(claves));

                try
                {
                    comando.ExecuteNonQuery();
                }
                catch
                {

                }
            }
        }

        public static void PatreonComprobacion(string correoBuscar, DateTime fechaActualizar, SqlConnection conexion = null)
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

            string id = null;

			string busqueda = "SELECT Id FROM AspNetUsers WHERE Email=@Email OR PatreonMail=@PatreonMail";

			using (SqlCommand comando = new SqlCommand(busqueda, conexion))
			{
				comando.Parameters.AddWithValue("@Email", correoBuscar);
				comando.Parameters.AddWithValue("@PatreonMail", correoBuscar);

				using (SqlDataReader lector = comando.ExecuteReader())
				{
					while (lector.Read())
					{
						if (lector.IsDBNull(0) == false)
						{
							if (string.IsNullOrEmpty(lector.GetString(0)) == false)
							{
                                id = lector.GetString(0);	
							}
						}
					}
				}
			}

            if (string.IsNullOrEmpty(id) == false)
            {
				string sqlActualizar = "UPDATE AspNetUsers " +
					"SET PatreonLastCheck=@PatreonLastCheck WHERE Id=@Id";

				using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
				{
					comando.Parameters.AddWithValue("@Id", id);
					comando.Parameters.AddWithValue("@PatreonLastCheck", fechaActualizar);

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

		public static bool PatreonCorreo(string usuarioId, string correoNuevo, SqlConnection conexion = null)
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

			string id = null;

			string busqueda = "SELECT Id FROM AspNetUsers WHERE Email=@Email OR PatreonMail=@PatreonMail";

			using (SqlCommand comando = new SqlCommand(busqueda, conexion))
			{
				comando.Parameters.AddWithValue("@Email", correoNuevo);
				comando.Parameters.AddWithValue("@PatreonMail", correoNuevo);

				using (SqlDataReader lector = comando.ExecuteReader())
				{
					while (lector.Read())
					{
						if (lector.IsDBNull(0) == false)
						{
							if (string.IsNullOrEmpty(lector.GetString(0)) == false)
							{
								id = lector.GetString(0);
							}
						}
					}
				}
			}

			if (string.IsNullOrEmpty(id) == true)
			{
				string sqlActualizar = "UPDATE AspNetUsers " +
					"SET PatreonMail=@PatreonMail WHERE Id=@Id";

				using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
				{
					comando.Parameters.AddWithValue("@Id", usuarioId);
					comando.Parameters.AddWithValue("@PatreonMail", correoNuevo);

					try
					{
						comando.ExecuteNonQuery();
						return false;
					}
					catch
					{

					}
				}
			}

			return true;
		}
	}

    public class Clave
    {
        public string Nombre;
        public string JuegoId;
        public string Codigo;
    }
}
