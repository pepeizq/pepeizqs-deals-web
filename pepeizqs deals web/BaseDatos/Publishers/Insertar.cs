#nullable disable

using Microsoft.Data.SqlClient;

namespace BaseDatos.Publishers
{
    public static class Insertar
    {
        public static void Ejecutar(Publisher publisher, SqlConnection conexion = null)
        {
            if (publisher != null)
            {
                if (conexion == null)
                {
                    conexion = Herramientas.BaseDatos.Conectar();
                }

                string sqlInsertar = "INSERT INTO publishers " +
                    "(id, nombre, descripcion, imagen, ultimaModificacion) VALUES" +
                    "(@id, @nombre, @descripcion, @imagen, @ultimaModificacion)";

                using (SqlCommand comando = new SqlCommand(sqlInsertar, conexion))
                {
                    comando.Parameters.AddWithValue("@id", publisher.Id);
                    comando.Parameters.AddWithValue("@nombre", publisher.Nombre);
                    comando.Parameters.AddWithValue("@descripcion", publisher.Descripcion);
                    comando.Parameters.AddWithValue("@imagen", publisher.Imagen);
                    comando.Parameters.AddWithValue("@ultimaModificacion", DateTime.Now);

					comando.ExecuteNonQuery();
					try
                    {
                       
                    }
                    catch
                    {

                    }
                }
            }
        }
    }
}
