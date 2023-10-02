#nullable disable

using Gratis2;
using Juegos;
using Microsoft.Data.SqlClient;

namespace BaseDatos.Gratis
{
	public static class Buscar
	{
		public static List<JuegoGratis> Todos()
		{
			List<JuegoGratis> listaGratis = new List<JuegoGratis>();

			SqlConnection conexion = Herramientas.BaseDatos.Conectar();

			using (conexion)
			{
				string busqueda = "SELECT * FROM gratis";

				using (SqlCommand comando = new SqlCommand(busqueda, conexion))
				{
					using (SqlDataReader lector = comando.ExecuteReader())
					{
						while (lector.Read())
						{
							JuegoGratis gratis = new JuegoGratis
							{
								Tipo = GratisCargar.DevolverGratis(lector.GetInt32(0)).Id,
								JuegoId = lector.GetInt32(1),
								Nombre = lector.GetString(2),
								Imagen = lector.GetString(3),
								DRM = JuegoDRM2.DevolverDRM(lector.GetInt32(4)),
								Enlace = lector.GetString(5),
								FechaEmpieza = Convert.ToDateTime(lector.GetString(6)),
								FechaTermina = Convert.ToDateTime(lector.GetString(7))
							};

							listaGratis.Add(gratis);
						}
					}
				}
			}

			conexion.Dispose();

			return listaGratis;
		}

		public static List<JuegoGratis> UnTipo(string gratisTexto, Herramientas.Tiempo tiempo)
		{
			List<JuegoGratis> listaGratis = new List<JuegoGratis>();

			SqlConnection conexion = Herramientas.BaseDatos.Conectar();

			using (conexion)
			{
				string busqueda = "SELECT * FROM gratis WHERE gratis=@gratis";

				using (SqlCommand comando = new SqlCommand(busqueda, conexion))
				{
					comando.Parameters.AddWithValue("@gratis", GratisCargar.DevolverGratis(gratisTexto).Id);

					using (SqlDataReader lector = comando.ExecuteReader())
					{
						while (lector.Read())
						{
							JuegoGratis gratis = new JuegoGratis
							{
								Tipo = GratisCargar.DevolverGratis(lector.GetInt32(0)).Id,
								JuegoId = lector.GetInt32(1),
								Nombre = lector.GetString(2),
								Imagen = lector.GetString(3),
								DRM = JuegoDRM2.DevolverDRM(lector.GetInt32(4)),
								Enlace = lector.GetString(5),
								FechaEmpieza = Convert.ToDateTime(lector.GetString(6)),
								FechaTermina = Convert.ToDateTime(lector.GetString(7))
							};

							if (tiempo == Herramientas.Tiempo.Atemporal)
							{
								listaGratis.Add(gratis);
							}
							else if (tiempo == Herramientas.Tiempo.Actual)
							{
								if (DateTime.Now >= gratis.FechaEmpieza && DateTime.Now <= gratis.FechaTermina)
								{
									listaGratis.Add(gratis);
								}
							}
							else if (tiempo == Herramientas.Tiempo.Pasado)
							{
								if (DateTime.Now > gratis.FechaTermina)
								{
									listaGratis.Add(gratis);
								}
							}
						}
					}
				}
			}

			conexion.Dispose();

			return listaGratis;
		}

		public static JuegoGratis UnJuego(int juegoId)
		{
			SqlConnection conexion = Herramientas.BaseDatos.Conectar();

			using (conexion)
			{
				string busqueda = "SELECT * FROM gratis WHERE juegoId=(SELECT max(@juegoId) FROM gratis)";

				using (SqlCommand comando = new SqlCommand(busqueda, conexion))
				{
					comando.Parameters.AddWithValue("@juegoId", juegoId);

					using (SqlDataReader lector = comando.ExecuteReader())
					{
						while (lector.Read())
						{
							JuegoGratis gratis = new JuegoGratis
							{
								Tipo = GratisCargar.DevolverGratis(lector.GetInt32(0)).Id,
								JuegoId = lector.GetInt32(1),
								Nombre = lector.GetString(2),
								Imagen = lector.GetString(3),
								DRM = JuegoDRM2.DevolverDRM(lector.GetInt32(4)),
								Enlace = lector.GetString(5),
								FechaEmpieza = Convert.ToDateTime(lector.GetString(6)),
								FechaTermina = Convert.ToDateTime(lector.GetString(7))
							};

							return gratis;
						}
					}
				}
			}

			conexion.Dispose();

			return null;
		}
	}
}
