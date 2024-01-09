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

			string busqueda = string.Empty;

			if (nombre.Contains(" ") == true)
			{
				if (nombre.Contains("  ") == true)
				{
					nombre = nombre.Replace("  ", " ");
				}

				string[] palabras = nombre.Split(" ");

				int i = 0;
				foreach (var palabra in palabras) 
				{
					string palabraLimpia = string.Empty;
					
					if (palabra.Length == 1)
					{
						palabraLimpia = palabra;
					}
					else
					{
						palabraLimpia = Herramientas.Buscador.LimpiarNombre(palabra, false);
                    }

                    if (i == 0)
					{
						busqueda = "SELECT TOP 30 * FROM juegos WHERE CHARINDEX('" + palabraLimpia + "', nombreCodigo) > 0 ";
					}
                    else
                    {
						bool buscar = true;

						if (palabra.ToLower() == "and")
						{
							buscar = false;
						}
						else if (palabra.ToLower() == "dlc")
						{
							buscar = false;
						}
						if (palabra.ToLower() == "expansion")
						{
							buscar = false;
						}

						if (buscar == true)
						{
                            busqueda = busqueda + " AND CHARINDEX('" + palabraLimpia + "', nombreCodigo) > 0 ";
                        }						
					}

                    i += 1;
				}
			}
			else
			{
				busqueda = "SELECT TOP 30 * FROM juegos WHERE nombreCodigo LIKE '%" + Herramientas.Buscador.LimpiarNombre(nombre) + "%'";
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

        public static List<Juego> Ultimos(SqlConnection conexion, string tabla, int cantidad)
        {
            List<Juego> juegos = new List<Juego>();

			string busqueda = "SELECT TOP (" + cantidad + ") * FROM " + tabla + " ORDER BY id DESC";

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

		public static List<Juego> DLCs(string idMaestro = null)
		{
			List<Juego> dlcs = new List<Juego>();

			SqlConnection conexion = Herramientas.BaseDatos.Conectar();

			using (conexion)
			{
				string busqueda = null;

                if (string.IsNullOrEmpty(idMaestro) == false)
				{
					busqueda = "SELECT * FROM juegos WHERE maestro='" + idMaestro + "'";
				}
				else
				{
					busqueda = "SELECT * FROM juegos WHERE (maestro IS NULL AND tipo='1') or (maestro='no' AND tipo='1')";
				}

				using (SqlCommand comando = new SqlCommand(busqueda, conexion))
				{
					using (SqlDataReader lector = comando.ExecuteReader())
					{
						while (lector.Read())
						{
							Juego dlc = new Juego();
							dlc = Cargar.Ejecutar(dlc, lector);

							dlcs.Add(dlc);
						}
					}
				}
			}

			conexion.Dispose();

			return dlcs.OrderBy(x => x.Nombre).ToList();
		}
	}
}
