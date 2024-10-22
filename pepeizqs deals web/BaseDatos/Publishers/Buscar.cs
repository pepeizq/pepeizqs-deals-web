#nullable disable

using Juegos;
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic;
using Newtonsoft.Json;

namespace BaseDatos.Publishers
{
    public class Publisher
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Imagen { get; set; }
        public List<string> Acepciones { get; set; }
        public DateTime UltimaModificacion { get; set; }
    }

    public static class Buscar
	{
        public static Publisher Cargar(SqlDataReader lector)
        {
            Publisher publisher = new Publisher();

            if (lector.IsDBNull(0) == false)
            {
                if (string.IsNullOrEmpty(lector.GetString(0)) == false)
                {
                    publisher.Id = lector.GetString(0);
                }
            }

            if (lector.IsDBNull(1) == false)
            {
                if (string.IsNullOrEmpty(lector.GetString(1)) == false)
                {
                    publisher.Nombre = lector.GetString(1);
                }
            }

            if (lector.IsDBNull(2) == false)
            {
                if (string.IsNullOrEmpty(lector.GetString(2)) == false)
                {
                    publisher.Descripcion = lector.GetString(2);
                }
            }

            if (lector.IsDBNull(3) == false)
            {
                if (string.IsNullOrEmpty(lector.GetString(3)) == false)
                {
                    publisher.Imagen = lector.GetString(3);
                }
            }

            if (lector.IsDBNull(4) == false)
            {
                if (string.IsNullOrEmpty(lector.GetString(4)) == false)
                {
                    publisher.Acepciones = JsonConvert.DeserializeObject<List<string>>(lector.GetString(4));
                }
            }

            if (lector.IsDBNull(5) == false)
            {
                publisher.UltimaModificacion = lector.GetDateTime(5);
            }

            return publisher;
        }


        public static Publisher Id(string cadena, SqlConnection conexion = null)
		{
            if (conexion == null)
            {
                conexion = Herramientas.BaseDatos.Conectar();
            }

            if (string.IsNullOrEmpty(cadena) == false)
            {
                using (conexion)
                {
                    string busqueda = "SELECT * FROM publishers WHERE id = '" + cadena + "'";

                    using (SqlCommand comando = new SqlCommand(busqueda, conexion))
                    {
                        using (SqlDataReader lector = comando.ExecuteReader())
                        {
                            while (lector.Read())
                            {
                                return Cargar(lector);
                            }
                        }
                    }
                }
            }

            return null;
		}

		public static List<Publisher> Todos(SqlConnection conexion = null)
		{
			List<Publisher> lista = new List<Publisher>();

			if (conexion == null)
			{
				conexion = Herramientas.BaseDatos.Conectar();
			}

			using (conexion)
			{
				string busqueda = "SELECT * FROM publishers";

				using (SqlCommand comando = new SqlCommand(busqueda, conexion))
				{
					using (SqlDataReader lector = comando.ExecuteReader())
					{
						while (lector.Read())
						{
							lista.Add(Cargar(lector));
						}
					}
				}
			}

			return lista;
		}

		public static Publisher NombreExacto(string cadena, SqlConnection conexion = null)
		{
			if (conexion == null)
			{
				conexion = Herramientas.BaseDatos.Conectar();
			}

			if (string.IsNullOrEmpty(cadena) == false)
			{
				using (conexion)
				{
					string busqueda = "SELECT * FROM publishers WHERE nombre = N'" + cadena + "'";

					using (SqlCommand comando = new SqlCommand(busqueda, conexion))
					{
						using (SqlDataReader lector = comando.ExecuteReader())
						{
							while (lector.Read())
							{
								return Cargar(lector);
							}
						}
					}
				}
			}

			return null;
		}

        public static List<Publisher> NombreContiene(string cadena, int cantidad = 30, SqlConnection conexion = null)
        {
            List<Publisher> lista = new List<Publisher>();

            if (conexion == null)
            {
                conexion = Herramientas.BaseDatos.Conectar();
            }

            if (string.IsNullOrEmpty(cadena) == false)
            {
                using (conexion)
                {
                    string busqueda = "SELECT TOP " + cantidad + " * FROM publishers WHERE nombre LIKE '%" + cadena + "%'";

                    using (SqlCommand comando = new SqlCommand(busqueda, conexion))
                    {
                        using (SqlDataReader lector = comando.ExecuteReader())
                        {
                            while (lector.Read())
                            {
                                lista.Add(Cargar(lector));
                            }
                        }
                    }
                }
            }

            return lista;
        }

        public static Publisher Acepcion(string cadena, SqlConnection conexion = null)
		{
			if (conexion == null)
			{
				conexion = Herramientas.BaseDatos.Conectar();
			}

			if (string.IsNullOrEmpty(cadena) == false)
			{
				using (conexion)
				{
					string busqueda = "SELECT * FROM publishers WHERE acepciones LIKE N'%" + cadena + "%'";

					using (SqlCommand comando = new SqlCommand(busqueda, conexion))
					{
						using (SqlDataReader lector = comando.ExecuteReader())
						{
							while (lector.Read())
							{
								return Cargar(lector);
							}
						}
					}
				}
			}

			return null;
		}

		public static List<Juego> Juegos(string publisher, SqlConnection conexion = null)
		{
			List<Juego> juegos = new List<Juego>();

			if (conexion == null)
			{
                conexion = Herramientas.BaseDatos.Conectar();
            }

            if (string.IsNullOrEmpty(publisher) == false)
            {
                using (conexion)
                {
                    string busqueda = @"SELECT id, nombre, imagenes, precioMinimosHistoricos, precioActualesTiendas, bundles, gratis, suscripciones, tipo, idSteam FROM juegos 
                                        WHERE ISJSON(caracteristicas) > 0 AND JSON_QUERY(caracteristicas, '$.Publishers') LIKE N'%" + Strings.ChrW(34) + publisher + Strings.ChrW(34) + "%'";

                    using (SqlCommand comando = new SqlCommand(busqueda, conexion))
                    {
                        using (SqlDataReader lector = comando.ExecuteReader())
                        {
                            while (lector.Read())
                            {
                                Juego juego = new Juego();

								if (lector.IsDBNull(0) == false)
								{
									juego.Id = lector.GetInt32(0);
									juego.IdMaestra = lector.GetInt32(0);
								}

								if (lector.IsDBNull(1) == false)
								{
									if (string.IsNullOrEmpty(lector.GetString(1)) == false)
									{
										juego.Nombre = lector.GetString(1);
										juego.NombreCodigo = Herramientas.Buscador.LimpiarNombre(juego.Nombre);
									}
								}

								if (lector.IsDBNull(2) == false)
								{
									if (string.IsNullOrEmpty(lector.GetString(2)) == false)
									{
										juego.Imagenes = JsonConvert.DeserializeObject<JuegoImagenes>(lector.GetString(2));
									}
								}

								if (lector.IsDBNull(3) == false)
								{
									if (string.IsNullOrEmpty(lector.GetString(3)) == false)
									{
										juego.PrecioMinimosHistoricos = JsonConvert.DeserializeObject<List<JuegoPrecio>>(lector.GetString(3));
									}
								}

								if (lector.IsDBNull(4) == false)
								{
									if (string.IsNullOrEmpty(lector.GetString(4)) == false)
									{
										juego.PrecioActualesTiendas = JsonConvert.DeserializeObject<List<JuegoPrecio>>(lector.GetString(4));
									}
								}

								if (lector.IsDBNull(5) == false)
								{
									if (string.IsNullOrEmpty(lector.GetString(5)) == false)
									{
										juego.Bundles = JsonConvert.DeserializeObject<List<JuegoBundle>>(lector.GetString(5));
									}
								}

								if (lector.IsDBNull(6) == false)
								{
									if (string.IsNullOrEmpty(lector.GetString(6)) == false)
									{
										juego.Gratis = JsonConvert.DeserializeObject<List<JuegoGratis>>(lector.GetString(6));
									}
								}

								if (lector.IsDBNull(7) == false)
								{
									if (string.IsNullOrEmpty(lector.GetString(7)) == false)
									{
										juego.Suscripciones = JsonConvert.DeserializeObject<List<JuegoSuscripcion>>(lector.GetString(7));
									}
								}

								if (lector.IsDBNull(8) == false)
								{
									juego.Tipo = Enum.Parse<JuegoTipo>(lector.GetString(8));
								}

								if (lector.IsDBNull(9) == false)
								{
									juego.IdSteam = lector.GetInt32(9);
								}

								juegos.Add(juego);
                            }
                        }
                    }
                }                
            }

			return juegos;
		}
	}
}
