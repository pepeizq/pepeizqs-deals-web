#nullable disable

using Microsoft.Data.SqlClient;
using Suscripciones2;

namespace BaseDatos.Noticias
{
	public static class Buscar
	{
		public static global::Noticias.Noticia UnaNoticia(int id)
		{
			SqlConnection conexion = Herramientas.BaseDatos.Conectar();

			using (conexion)
			{
				conexion.Open();

				string busqueda = "SELECT * FROM noticias WHERE id=@id";

				using (SqlCommand comando = new SqlCommand(busqueda, conexion))
				{
					comando.Parameters.AddWithValue("@id", id);

					using (SqlDataReader lector = comando.ExecuteReader())
					{
						while (lector.Read())
						{
							global::Noticias.Noticia noticia = new global::Noticias.Noticia();

							noticia.Id = lector.GetInt32(0);
							noticia.Tipo = global::Noticias.NoticiasCargar.CargarNoticiasTipo()[int.Parse(lector.GetString(1))];
							noticia.Titulo = lector.GetString(2);

							if (lector.IsDBNull(3) == false)
							{
								noticia.Contenido = lector.GetString(3);
							}

							if (lector.IsDBNull(4) == false)
							{
								noticia.Imagen = lector.GetString(4);
							}

							if (lector.IsDBNull(5) == false)
							{
								noticia.Enlace = lector.GetString(5);
							}

							if (lector.IsDBNull(6) == false)
							{
								noticia.Juegos = lector.GetString(6);
							}

							if (lector.IsDBNull(7) == false)
							{
								noticia.FechaEmpieza = DateTime.Parse(lector.GetString(7));
							}

							if (lector.IsDBNull(8) == false)
							{
								noticia.FechaTermina = DateTime.Parse(lector.GetString(8));
							}

							if (lector.IsDBNull(9) == false)
							{
								//bundleTipo
							}

							if (lector.IsDBNull(10) == false)
							{
								//gratisTipo
							}

							if (lector.IsDBNull(11) == false)
							{
								noticia.SuscripcionTipo = SuscripcionesCargar.DevolverSuscripcion(int.Parse(lector.GetString(11))).Id;
							}

							return noticia;
						}
					}
				}
			}

			conexion.Dispose();

			return null;
		}

        public static List<global::Noticias.Noticia> Todas()
        {
			List<global::Noticias.Noticia> noticias = new List<global::Noticias.Noticia>();

            SqlConnection conexion = Herramientas.BaseDatos.Conectar();

            using (conexion)
            {
                conexion.Open();

                string busqueda = "SELECT * FROM noticias";

                using (SqlCommand comando = new SqlCommand(busqueda, conexion))
                {
                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            global::Noticias.Noticia noticia = new global::Noticias.Noticia();

                            noticia.Id = lector.GetInt32(0);
                            noticia.Tipo = global::Noticias.NoticiasCargar.CargarNoticiasTipo()[int.Parse(lector.GetString(1))];
                            noticia.Titulo = lector.GetString(2);

                            if (lector.IsDBNull(3) == false)
                            {
                                noticia.Contenido = lector.GetString(3);
                            }

                            if (lector.IsDBNull(4) == false)
                            {
                                noticia.Imagen = lector.GetString(4);
                            }

                            if (lector.IsDBNull(5) == false)
                            {
                                noticia.Enlace = lector.GetString(5);
                            }

                            if (lector.IsDBNull(6) == false)
                            {
                                noticia.Juegos = lector.GetString(6);
                            }

                            if (lector.IsDBNull(7) == false)
                            {
                                noticia.FechaEmpieza = DateTime.Parse(lector.GetString(7));
                            }

                            if (lector.IsDBNull(8) == false)
                            {
                                noticia.FechaTermina = DateTime.Parse(lector.GetString(8));
                            }

                            if (lector.IsDBNull(9) == false)
                            {
                                //bundleTipo
                            }

                            if (lector.IsDBNull(10) == false)
                            {
                                //gratisTipo
                            }

                            if (lector.IsDBNull(11) == false)
                            {
                                noticia.SuscripcionTipo = SuscripcionesCargar.DevolverSuscripcion(int.Parse(lector.GetString(11))).Id;
                            }

                            noticias.Add(noticia);
                        }
                    }
                }
            }

            conexion.Dispose();

            return noticias;
        }

    }
}
