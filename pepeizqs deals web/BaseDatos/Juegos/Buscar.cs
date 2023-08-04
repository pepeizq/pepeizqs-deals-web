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
				try
				{
					WebApplicationBuilder builder = WebApplication.CreateBuilder();
					string conexionTexto = builder.Configuration.GetConnectionString("pepeizqs_deals_webContextConnection");
					SqlConnection conexion = new SqlConnection(conexionTexto);

					using (conexion)
					{
						conexion.Open();
						string buscar = sqlBuscar;
						SqlCommand comando = new SqlCommand(buscar, conexion);

						using (comando)
						{
							comando.Parameters.AddWithValue(idParametro, idBuscar);

							SqlDataReader lector = comando.ExecuteReader();

							using (lector)
							{
								if (lector.Read())
								{
									Juego juego = JuegoCrear.Generar();
									juego = Cargar.Ejecutar(juego, lector);

									return juego;
								}
							}

							lector.Close();
						}

						comando.Dispose();
					}

					conexion.Dispose();
				}
				catch
				{

				}
			}	

			return null;
		}

        public static List<Juego> Nombre(string nombre)
        {
            List<Juego> juegos = new List<Juego>();

            WebApplicationBuilder builder = WebApplication.CreateBuilder();
            string conexionTexto = builder.Configuration.GetConnectionString("pepeizqs_deals_webContextConnection");
			SqlConnection conexion = new SqlConnection(conexionTexto);

            using (conexion)
            {
                conexion.Open();
                string busqueda = "SELECT * FROM juegos WHERE REPLACE(REPLACE(nombre, '®',''), '™', '') LIKE '%" + nombre.ToLower() + "%'";
				SqlCommand comando = new SqlCommand(busqueda, conexion);

				using (comando)
                {
					SqlDataReader lector = comando.ExecuteReader();

					using (lector)
                    {
                        while (lector.Read())
                        {
                            Juego juego = new Juego();
                            juego = Cargar.Ejecutar(juego, lector);

                            juegos.Add(juego);
                        }
                    }

					lector.Close();
                }

				comando.Dispose();
            }

			conexion.Dispose();

			if (juegos.Count > 0)
			{
				juegos.Sort(delegate (Juego j1, Juego j2)
				{
					string j1Limpio = j1.Nombre;
					j1Limpio = j1Limpio.Replace("®", null);
                    j1Limpio = j1Limpio.Replace("™", null);

                    string j2Limpio = j2.Nombre;
                    j2Limpio = j2Limpio.Replace("®", null);
                    j2Limpio = j2Limpio.Replace("™", null);

                    return j1Limpio.CompareTo(j2Limpio);
				});
            }

            return juegos;
        }

        public static List<Juego> Todos(SqlConnection conexion)
		{
			List<Juego> juegos = new List<Juego>();

			string busqueda = "SELECT * FROM juegos";
			SqlCommand comando = new SqlCommand(busqueda, conexion);

			using (comando)
			{
				SqlDataReader lector = comando.ExecuteReader();

				using (lector)
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
