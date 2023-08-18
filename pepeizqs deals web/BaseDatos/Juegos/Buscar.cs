﻿#nullable disable

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
				catch
				{

				}
			}	

			return null;
		}

        public static List<Juego> Nombre(string nombre)
        {
            List<Juego> juegos = new List<Juego>();

			SqlConnection conexion = Herramientas.BaseDatos.Conectar();

			using (conexion)
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
