using Juegos;
using Microsoft.Data.SqlClient;
using System.Text.Json;

namespace BaseDatos.Suscripciones
{
	public static class Insertar
	{
		public static void Ejecutar(int juegoId, List<JuegoSuscripcion> listaVecesSuscripciones, JuegoSuscripcion actual, SqlConnection conexion)
		{
			string sqlActualizarJuego = "UPDATE juegos " +
								"SET suscripciones=@suscripciones WHERE id=@id";

			using (SqlCommand comando = new SqlCommand(sqlActualizarJuego, conexion))
			{
				comando.Parameters.AddWithValue("@id", juegoId);
				comando.Parameters.AddWithValue("@suscripciones", JsonSerializer.Serialize(listaVecesSuscripciones));

				try
				{
					comando.ExecuteNonQuery();
				}
				catch
				{

				}
			}

			string sqlInsertar = "INSERT INTO suscripciones " +
				"(suscripcion, juegoId, nombre, imagen, drm, enlace, fechaEmpieza, fechaTermina, imagenNoticia) VALUES " +
				"(@suscripcion, @juegoId, @nombre, @imagen, @drm, @enlace, @fechaEmpieza, @fechaTermina, @imagenNoticia) ";

			using (SqlCommand comando = new SqlCommand(sqlInsertar, conexion))
			{
				comando.Parameters.AddWithValue("@suscripcion", actual.Tipo);
				comando.Parameters.AddWithValue("@juegoId", actual.JuegoId);
				comando.Parameters.AddWithValue("@nombre", actual.Nombre);
				comando.Parameters.AddWithValue("@imagen", actual.Imagen);
				comando.Parameters.AddWithValue("@drm", actual.DRM);
				comando.Parameters.AddWithValue("@enlace", actual.Enlace);
				comando.Parameters.AddWithValue("@fechaEmpieza", actual.FechaEmpieza.ToString());
				comando.Parameters.AddWithValue("@fechaTermina", actual.FechaTermina.ToString());
				comando.Parameters.AddWithValue("@imagenNoticia", actual.ImagenNoticia);

				try
				{
					comando.ExecuteNonQuery();
				}
				catch
				{

				}
			}
		}

		public static void Temporal(SqlConnection conexion, string nombreTabla, string enlace, string nombreJuego = "vacio", string imagen = "vacio")
		{
			bool encontrado = false;
			string sqlBuscar = "SELECT enlace FROM temporal" + nombreTabla + " WHERE enlace=@enlace";

			using (SqlCommand comando = new SqlCommand(sqlBuscar, conexion))
			{
				comando.Parameters.AddWithValue("@enlace", enlace);

				using (SqlDataReader lector = comando.ExecuteReader())
				{
					if (lector.Read() == true)
					{
						encontrado = true;
					}
				}
			}

			if (encontrado == false)
			{
				string sqlInsertar = "INSERT INTO temporal" + nombreTabla + " " +
					"(enlace, nombre, imagen) VALUES " +
					"(@enlace, @nombre, @imagen) ";

				using (SqlCommand comando = new SqlCommand(sqlInsertar, conexion))
				{
					comando.Parameters.AddWithValue("@enlace", enlace);
					comando.Parameters.AddWithValue("@nombre", nombreJuego);
					comando.Parameters.AddWithValue("@imagen", imagen);

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
	}
}
