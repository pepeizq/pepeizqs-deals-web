using Microsoft.Data.SqlClient;

namespace BaseDatos.Noticias
{
	public static class Insertar
	{
		public static void Ejecutar(global::Noticias.Noticia noticia)
		{
			SqlConnection conexion = Herramientas.BaseDatos.Conectar();

			using (conexion)
			{
				conexion.Open();

				if (noticia.Tipo == global::Noticias.NoticiaTipo.Gratis)
				{
					string sqlInsertar = "INSERT INTO noticias " +
						"(noticiaTipo, juegos, fechaEmpieza, fechaTermina, gratisTipo, tituloEn, tituloEs, contenidoEn, contenidoEs) VALUES " +
						"(@noticiaTipo, @juegos, @fechaEmpieza, @fechaTermina, @gratisTipo, @tituloEn, @tituloEs, @contenidoEn, @contenidoEs) ";

					using (SqlCommand comando = new SqlCommand(sqlInsertar, conexion))
					{
						comando.Parameters.AddWithValue("@noticiaTipo", noticia.Tipo);
						comando.Parameters.AddWithValue("@juegos", noticia.Juegos);
						comando.Parameters.AddWithValue("@fechaEmpieza", noticia.FechaEmpieza.ToString());
						comando.Parameters.AddWithValue("@fechaTermina", noticia.FechaTermina.ToString());
						comando.Parameters.AddWithValue("@gratisTipo", noticia.GratisTipo);
						comando.Parameters.AddWithValue("@tituloEn", noticia.TituloEn);
						comando.Parameters.AddWithValue("@tituloEs", noticia.TituloEs);
						comando.Parameters.AddWithValue("@contenidoEn", noticia.ContenidoEn);
						comando.Parameters.AddWithValue("@contenidoEs", noticia.ContenidoEs);

						try
						{
							comando.ExecuteNonQuery();
						}
						catch
						{

						}
					}
				}
				else if (noticia.Tipo == global::Noticias.NoticiaTipo.Suscripciones)
				{
					string sqlInsertar = "INSERT INTO noticias " +
						"(noticiaTipo, juegos, fechaEmpieza, fechaTermina, suscripcionTipo, tituloEn, tituloEs, contenidoEn, contenidoEs) VALUES " +
						"(@noticiaTipo, @juegos, @fechaEmpieza, @fechaTermina, @suscripcionTipo, @tituloEn, @tituloEs, @contenidoEn, @contenidoEs) ";

					using (SqlCommand comando = new SqlCommand(sqlInsertar, conexion))
					{
						comando.Parameters.AddWithValue("@noticiaTipo", noticia.Tipo);
						comando.Parameters.AddWithValue("@juegos", noticia.Juegos);
						comando.Parameters.AddWithValue("@fechaEmpieza", noticia.FechaEmpieza.ToString());
						comando.Parameters.AddWithValue("@fechaTermina", noticia.FechaTermina.ToString());
						comando.Parameters.AddWithValue("@suscripcionTipo", noticia.SuscripcionTipo);
						comando.Parameters.AddWithValue("@tituloEn", noticia.TituloEn);
						comando.Parameters.AddWithValue("@tituloEs", noticia.TituloEs);
						comando.Parameters.AddWithValue("@contenidoEn", noticia.ContenidoEn);
						comando.Parameters.AddWithValue("@contenidoEs", noticia.ContenidoEs);

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

			conexion.Dispose();
		}
	}
}
