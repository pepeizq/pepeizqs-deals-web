#nullable disable

using Juegos;
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic;

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
					conexion.Open();
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

        public static List<Juego> Nombre(string nombre, bool usuario)
        {
            List<Juego> juegos = new List<Juego>();

			SqlConnection conexion = Herramientas.BaseDatos.Conectar();

			using (conexion)
            {
                conexion.Open();
				string busqueda = null;
				
				if (usuario == true)
				{
					busqueda = "SELECT * FROM juegos WHERE " + ConstruirReplaces() + " LIKE '%" + nombre.ToLower() + "%'";
				}
				else
				{
					busqueda = "SELECT * FROM juegos WHERE nombre LIKE '%" + nombre.ToLower() + "%'";
				}

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

        public static List<Juego> Todos(SqlConnection conexion)
		{
			List<Juego> juegos = new List<Juego>();

			string busqueda = "SELECT * FROM juegos";

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

		private static string ConstruirReplaces()
		{
            List<string> caracteres = new List<string>
            {
                ":", ",", ".", "®", "™", "_", "-"
            };

			string mensaje = string.Empty;

			for (int i = 0; i < caracteres.Count; i += 1)
			{
				if (i == 0)
				{
					mensaje = "REPLACE(nombre, '" + caracteres[i] + "','')";
                }
				else
				{
					mensaje = "REPLACE(" + mensaje + ", '" + caracteres[i] + "', '')";
				}
			}

			return mensaje;
		}
	}
}
