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

				if (noticia.Tipo == global::Noticias.NoticiaTipo.Suscripciones)
				{
					string sqlInsertar = "INSERT INTO noticias " +
						"(noticiaTipo, titulo, contenido, juegos, fechaEmpieza, fechaTermina, suscripcionTipo) VALUES " +
						"(@noticiaTipo, @titulo, @contenido, @juegos, @fechaEmpieza, @fechaTermina, @suscripcionTipo) ";

					using (SqlCommand comando = new SqlCommand(sqlInsertar, conexion))
					{
						comando.Parameters.AddWithValue("@noticiaTipo", noticia.Tipo);
						comando.Parameters.AddWithValue("@titulo", noticia.Titulo);
						comando.Parameters.AddWithValue("@contenido", noticia.Contenido);
						comando.Parameters.AddWithValue("@juegos", noticia.Juegos);
						comando.Parameters.AddWithValue("@fechaEmpieza", noticia.FechaEmpieza.ToString());
						comando.Parameters.AddWithValue("@fechaTermina", noticia.FechaTermina.ToString());
						comando.Parameters.AddWithValue("@suscripcionTipo", noticia.SuscripcionTipo);

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
