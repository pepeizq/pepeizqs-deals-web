using Juegos;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;

namespace BaseDatos.Suscripciones
{
	public static class Insertar
	{
		public static void Ejecutar(int id, List<JuegoSuscripcion> suscripcionesJuego, JuegoSuscripcion suscripcion)
		{
			SqlConnection conexion = Herramientas.BaseDatos.Conectar();

			using (conexion)
			{
				conexion.Open();

				string sqlActualizarJuego = "UPDATE juegos " +
					"SET suscripciones=@suscripciones WHERE id=@id";

				using (SqlCommand comando = new SqlCommand(sqlActualizarJuego, conexion))
				{
					comando.Parameters.AddWithValue("@id", id);
					comando.Parameters.AddWithValue("@suscripciones", JsonConvert.SerializeObject(suscripcionesJuego));

					try
					{
						comando.ExecuteNonQuery();
					}
					catch
					{

					}
				}

				string sqlInsertar = "INSERT INTO suscripciones " +
					"(suscripcion, juegoId, nombre, imagen, drm, enlace, fechaEmpieza, fechaTermina) VALUES " +
					"(@suscripcion, @juegoId, @nombre, @imagen, @drm, @enlace, @fechaEmpieza, @fechaTermina) ";

				using (SqlCommand comando = new SqlCommand(sqlInsertar, conexion))
				{
					comando.Parameters.AddWithValue("@suscripcion", suscripcion.Suscripcion);
					comando.Parameters.AddWithValue("@juegoId", suscripcion.JuegoId);
					comando.Parameters.AddWithValue("@nombre", suscripcion.Nombre);
					comando.Parameters.AddWithValue("@imagen", suscripcion.Imagen);
					comando.Parameters.AddWithValue("@drm", suscripcion.DRM);
					comando.Parameters.AddWithValue("@enlace", suscripcion.Enlace);
					comando.Parameters.AddWithValue("@fechaEmpieza", suscripcion.FechaEmpieza.ToString());
					comando.Parameters.AddWithValue("@fechaTermina", suscripcion.FechaTermina.ToString());

					try
					{
						comando.ExecuteNonQuery();
					}
					catch
					{

					}
				}
			}

			conexion.Dispose();
		}
	}
}
