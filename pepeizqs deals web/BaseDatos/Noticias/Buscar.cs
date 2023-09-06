#nullable disable

using Bundles2;
using Gratis2;
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

							if (lector.IsDBNull(2) == false)
							{
								noticia.Imagen = lector.GetString(2);
							}

							if (lector.IsDBNull(3) == false)
							{
								noticia.Enlace = lector.GetString(3);
							}

							if (lector.IsDBNull(4) == false)
							{
								noticia.Juegos = lector.GetString(4);
							}

							if (lector.IsDBNull(5) == false)
							{
								noticia.FechaEmpieza = DateTime.Parse(lector.GetString(5));
							}

							if (lector.IsDBNull(6) == false)
							{
								noticia.FechaTermina = DateTime.Parse(lector.GetString(6));
							}

							if (lector.IsDBNull(7) == false)
							{
								noticia.BundleTipo = BundlesCargar.DevolverBundle(int.Parse(lector.GetString(7))).Tipo;
							}

							if (lector.IsDBNull(8) == false)
							{
								noticia.GratisTipo = GratisCargar.DevolverGratis(int.Parse(lector.GetString(8))).Id;
							}

							if (lector.IsDBNull(9) == false)
							{
								noticia.SuscripcionTipo = SuscripcionesCargar.DevolverSuscripcion(int.Parse(lector.GetString(9))).Id;
							}

							if (lector.IsDBNull(10) == false)
							{
								noticia.TituloEn = lector.GetString(10);
							}

							if (lector.IsDBNull(11) == false)
							{
								noticia.TituloEs = lector.GetString(11);
							}

							if (lector.IsDBNull(12) == false)
							{
								noticia.ContenidoEn = lector.GetString(12);
							}

							if (lector.IsDBNull(13) == false)
							{
								noticia.ContenidoEs = lector.GetString(13);
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

							if (lector.IsDBNull(2) == false)
							{
								noticia.Imagen = lector.GetString(2);
							}

							if (lector.IsDBNull(3) == false)
							{
								noticia.Enlace = lector.GetString(3);
							}

							if (lector.IsDBNull(4) == false)
							{
								noticia.Juegos = lector.GetString(4);
							}

							if (lector.IsDBNull(5) == false)
							{
								noticia.FechaEmpieza = DateTime.Parse(lector.GetString(5));
							}

							if (lector.IsDBNull(6) == false)
							{
								noticia.FechaTermina = DateTime.Parse(lector.GetString(6));
							}

							if (lector.IsDBNull(7) == false)
							{
								noticia.BundleTipo = BundlesCargar.DevolverBundle(int.Parse(lector.GetString(7))).Tipo;
							}

							if (lector.IsDBNull(8) == false)
							{
								noticia.GratisTipo = GratisCargar.DevolverGratis(int.Parse(lector.GetString(8))).Id;
							}

							if (lector.IsDBNull(9) == false)
							{
								noticia.SuscripcionTipo = SuscripcionesCargar.DevolverSuscripcion(int.Parse(lector.GetString(9))).Id;
							}

							if (lector.IsDBNull(10) == false)
							{
								noticia.TituloEn = lector.GetString(10);
							}

							if (lector.IsDBNull(11) == false)
							{
								noticia.TituloEs = lector.GetString(11);
							}

							if (lector.IsDBNull(12) == false)
							{
								noticia.ContenidoEn = lector.GetString(12);
							}

							if (lector.IsDBNull(13) == false)
							{
								noticia.ContenidoEs = lector.GetString(13);
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
