#nullable disable

using Microsoft.Data.SqlClient;

namespace BaseDatos.Foro
{
	public enum ForoIdioma
	{
		Ingles,
		Español
	}

	public class ForoCategoria
	{
		public int Id { get; set; }
		public bool SoloAdmin { get; set; }
		public ForoIdioma Idioma { get; set; }
	}

	public class ForoPost
	{
		public int Id { get; set; }
		public string AutorId { get; set; }
		public int CategoriaId { get; set; }
		public string Titulo { get; set; }
		public string Contenido { get; set; }
		public DateTime FechaCreacion { get; set; }
		public DateTime FechaEdicion { get; set; }
		public int RespuestaId { get; set; }
		public int CantidadRespuestas { get; set; }
		public DateTime FechaUltimaRespuesta { get; set; }
		public string AutorIdUltimaRespuesta { get; set; }
	}

	public static class Buscar
	{
		public static List<ForoCategoria> Categorias(SqlConnection conexion = null)
		{
			if (conexion == null)
			{
				conexion = Herramientas.BaseDatos.Conectar();
			}
			else
			{
				if (conexion.State != System.Data.ConnectionState.Open)
				{
					conexion = Herramientas.BaseDatos.Conectar();
				}
			}
			string busqueda = "SELECT * FROM foroCategoria";

			List<ForoCategoria> categorias = new List<ForoCategoria>();

			using (SqlCommand comando = new SqlCommand(busqueda, conexion))
			{
				using (SqlDataReader lector = comando.ExecuteReader())
				{
					while (lector.Read() == true)
					{
						categorias.Add(new ForoCategoria
						{
							Id = lector.GetInt32(0),
							SoloAdmin = lector.GetBoolean(1),
							Idioma = (ForoIdioma)lector.GetInt32(2)
						});
					}
				}
			}

			return categorias;
		}

		public static List<ForoPost> UltimosPosts(int categoria, SqlConnection conexion = null)
		{
			if (conexion == null)
			{
				conexion = Herramientas.BaseDatos.Conectar();
			}
			else
			{
				if (conexion.State != System.Data.ConnectionState.Open)
				{
					conexion = Herramientas.BaseDatos.Conectar();
				}
			}

			string busqueda = @"SELECT TOP 30 *, 
  (SELECT count(*) FROM foroPost AS V2 WHERE V2.respuestaId=V1.id) AS respuestas, 
  (SELECT MAX(fecha) FROM foroPost AS V2 WHERE V2.respuestaId=V1.id) AS fechaRespuesta, 
  (SELECT TOP 1 autorId FROM foroPost AS V2 WHERE V2.respuestaId=V1.id ORDER BY fecha DESC) AS autorId2 
FROM foroPost AS V1 WHERE categoriaId=3 AND respuestaId IS NULL  
ORDER BY CASE
WHEN (SELECT MAX(fecha) FROM foroPost AS V2 WHERE V2.respuestaId=V1.id) IS NULL THEN fecha
WHEN (SELECT MAX(fecha) FROM foroPost AS V2 WHERE V2.respuestaId=V1.id) > fecha THEN (SELECT MAX(fecha) FROM foroPost AS V2 WHERE V2.respuestaId=V1.id)
END DESC";
			
			List<ForoPost> posts = new List<ForoPost>();
			
			using (SqlCommand comando = new SqlCommand(busqueda, conexion))
			{
				comando.Parameters.AddWithValue("@categoria", categoria);

				using (SqlDataReader lector = comando.ExecuteReader())
				{
					while (lector.Read() == true)
					{
						ForoPost nuevoPost = new ForoPost
						{
							Id = lector.GetInt32(0),
							AutorId = lector.GetString(1),
							CategoriaId = lector.GetInt32(2),
							Titulo = lector.GetString(3),
							FechaCreacion = lector.GetDateTime(5)
						};

						if (lector.IsDBNull(9) == false)
						{
							nuevoPost.CantidadRespuestas = lector.GetInt32(9);
						}

						if (lector.IsDBNull(10) == false)
						{
							nuevoPost.FechaUltimaRespuesta = lector.GetDateTime(10);
						}

						if (lector.IsDBNull(11) == false)
						{
							nuevoPost.AutorIdUltimaRespuesta = lector.GetString(11);
						}

						posts.Add(nuevoPost);
					}
				}
			}

			return posts;
		}

		public static ForoPost Post(int id, SqlConnection conexion = null)
		{
			if (conexion == null)
			{
				conexion = Herramientas.BaseDatos.Conectar();
			}
			else
			{
				if (conexion.State != System.Data.ConnectionState.Open)
				{
					conexion = Herramientas.BaseDatos.Conectar();
				}
			}

			string busqueda = "SELECT * FROM foroPost WHERE id=@id";

			ForoPost post = new ForoPost();

			using (SqlCommand comando = new SqlCommand(busqueda, conexion))
			{
				comando.Parameters.AddWithValue("@id", id);
				using (SqlDataReader lector = comando.ExecuteReader())
				{
					while (lector.Read() == true)
					{
						post.Id = lector.GetInt32(0);
						post.AutorId = lector.GetString(1);
						post.CategoriaId = lector.GetInt32(2);
						post.Titulo = lector.GetString(3);
						
						if (lector.IsDBNull(4) == false)
						{
							post.Contenido = lector.GetString(4);
						}

						if (lector.IsDBNull(5) == false)
						{
							post.FechaCreacion = lector.GetDateTime(5);
						}

						if (lector.IsDBNull(6) == false)
						{
							post.FechaEdicion = lector.GetDateTime(6);
						}

						if (lector.IsDBNull(7) == false)
						{
							post.RespuestaId = lector.GetInt32(7);
						}
					}
				}
			}
			return post;
		}

		public static List<ForoPost> RespuestasPost(int postId, SqlConnection conexion = null)
		{
			if (conexion == null)
			{
				conexion = Herramientas.BaseDatos.Conectar();
			}
			else
			{
				if (conexion.State != System.Data.ConnectionState.Open)
				{
					conexion = Herramientas.BaseDatos.Conectar();
				}
			}

			string busqueda = "SELECT * FROM foroPost WHERE respuestaId=@respuestaId";

			List<ForoPost> respuestas = new List<ForoPost>();

			using (SqlCommand comando = new SqlCommand(busqueda, conexion))
			{
				comando.Parameters.AddWithValue("@respuestaId", postId);
				using (SqlDataReader lector = comando.ExecuteReader())
				{
					while (lector.Read() == true)
					{
						ForoPost respuesta = new ForoPost();
						respuesta.Id = lector.GetInt32(0);
						respuesta.AutorId = lector.GetString(1);
						respuesta.CategoriaId = lector.GetInt32(2);
						respuesta.Titulo = lector.GetString(3);

						if (lector.IsDBNull(4) == false)
						{
							respuesta.Contenido = lector.GetString(4);
						}

						if (lector.IsDBNull(5) == false)
						{
							respuesta.FechaCreacion = lector.GetDateTime(5);
						}

						if (lector.IsDBNull(6) == false)
						{
							respuesta.FechaEdicion = lector.GetDateTime(6);
						}

						if (lector.IsDBNull(7) == false)
						{
							respuesta.RespuestaId = lector.GetInt32(7);
						}

						respuestas.Add(respuesta);
					}
				}
			}

			return respuestas;
		}
	}
}
