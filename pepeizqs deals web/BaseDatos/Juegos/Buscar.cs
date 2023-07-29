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

					using (SqlConnection conexion = new SqlConnection(conexionTexto))
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

            using (SqlConnection conexion = new SqlConnection(conexionTexto))
            {
                conexion.Open();
                string busqueda = "SELECT * FROM juegos WHERE REPLACE(REPLACE(nombre, '®',''), '™', '') LIKE '%" + nombre.ToLower() + "%'";

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

        public static List<Juego> Todos()
		{
			List<Juego> juegos = new List<Juego>();

			WebApplicationBuilder builder = WebApplication.CreateBuilder();
			string conexionTexto = builder.Configuration.GetConnectionString("pepeizqs_deals_webContextConnection");

			using (SqlConnection conexion = new SqlConnection(conexionTexto))
			{
				conexion.Open();
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
			}

			return juegos;
		}
	}
}
