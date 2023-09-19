#nullable disable

using Juegos;
using Microsoft.Data.SqlClient;

namespace BaseDatos.Juegos
{
	public static class Buscar
	{
		public static Juego UnJuego(string id = null, string idSteam = null, string idGog = null)
		{
			string sqlBuscar = string.Empty;
			string idParametro = string.Empty;
			string idBuscar = string.Empty;

			if (id != null)
			{
				sqlBuscar = "SELECT * FROM juegos WHERE id=@id";
				idParametro = "@id";
				idBuscar = id;
			}
			else
			{
				if (idSteam != null)
				{
					sqlBuscar = "SELECT * FROM juegos WHERE idSteam=@idSteam";
					idParametro = "@idSteam";
					idBuscar = idSteam;
				}
				else
				{
					if (idGog != null)
					{
						sqlBuscar = "SELECT * FROM juegos WHERE idGog=@idGog";
						idParametro = "@idGog";
						idBuscar = idGog;
					}
				}
			}

			if (sqlBuscar != string.Empty) 
			{
				SqlConnection conexion = Herramientas.BaseDatos.Conectar();

				using (conexion)
				{
					string buscar = sqlBuscar;

					using (SqlCommand comando = new SqlCommand(buscar, conexion))
					{
						comando.Parameters.AddWithValue(idParametro, idBuscar);

						using (SqlDataReader lector = comando.ExecuteReader())
						{
							if (lector.Read())
							{
								Juego juego = JuegoCrear.Generar();
								juego = Cargar.Ejecutar(juego, lector);

								return juego;
							}
						}
					}
				}

				conexion.Dispose();
			}	

			return null;
		}

		public static List<Juego> Nombre(string nombre)
        {
            List<Juego> juegos = new List<Juego>();

			SqlConnection conexion = Herramientas.BaseDatos.Conectar();

			using (conexion)
			{
				juegos = Nombre(nombre, conexion);
            }		

            conexion.Dispose();

			if (juegos.Count > 0)
			{
				return juegos.OrderBy(x => x.Nombre)
							 .ThenBy(x => x.Id)
							 .ToList();
            }

            return null;
        }

        public static List<Juego> Nombre(string nombre, SqlConnection conexion)
		{
            List<Juego> juegos = new List<Juego>();

            string busqueda = "SELECT TOP 100 * FROM juegos WHERE nombreCodigo LIKE '%" + Herramientas.Buscador.LimpiarNombre(nombre) + "%';";

            try
            {
                using (SqlCommand comando = new SqlCommand(busqueda, conexion))
                {
                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            Juego juego = new Juego();
                            juego = Cargar.Ejecutar(juego, lector);
                            juegos.Add(juego);
                        }
                    }
                }
            }
            catch
            {

            }

            if (juegos.Count > 0)
            {
                return juegos.OrderBy(x => x.Nombre)
                             .ThenBy(x => x.Id)
                             .ToList();
            }

            return juegos;
        }

        public static List<Juego> Todos(SqlConnection conexion, string tabla = null)
		{
			string tabla2 = string.Empty;

			if (tabla == null)
			{
				tabla2 = "juegos";
			}
			else
			{
				tabla2 = tabla;
			}

			List<Juego> juegos = new List<Juego>();

			string busqueda = "SELECT * FROM " + tabla2;

			using (SqlCommand comando = new SqlCommand(busqueda, conexion))
			{
				using (SqlDataReader lector = comando.ExecuteReader())
				{
					while (lector.Read())
					{
						Juego juego = new Juego();
						juego = Cargar.Ejecutar(juego, lector);

						juegos.Add(juego);
					}
				}
			}

			return juegos;
		}
	}
}
