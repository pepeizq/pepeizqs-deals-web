using Microsoft.Data.SqlClient;

namespace BaseDatos.Pendientes
{
	public static class Actualizar
	{
		public static void Tienda(string idTienda, string enlace, string idJuegos, SqlConnection conexion)
		{
            if (string.IsNullOrEmpty(idJuegos) == false)
            {
                if (idJuegos != "0")
                {
                    string sqlActualizar = "UPDATE tienda" + idTienda + " " +
                    "SET idJuegos=@idJuegos WHERE enlace=@enlace";

                    using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
                    {
                        comando.Parameters.AddWithValue("@idJuegos", idJuegos);
                        comando.Parameters.AddWithValue("@enlace", enlace);

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

        public static void Suscripcion(string tablaInsertar, string tablaBorrar, string enlace, string nombreJuego, string imagen, List<string> idJuegos, SqlConnection conexion)
        {
            foreach (var idJuego in idJuegos)
            {
                string descartado = "no";
                global::Juegos.Juego juego = BaseDatos.Juegos.Buscar.UnJuego(idJuego);

                if (idJuego != "0")
                {
                    if (juego != null)
                    {
                        if (nombreJuego == "vacio")
                        {
                            nombreJuego = juego.Nombre;
                        }

                        if (imagen == "vacio")
                        {
                            imagen = juego.Imagenes.Header_460x215;
                        }
                    }
                }
                else
                {
                    descartado = "si";
                }

                string sqlInsertar = "INSERT INTO " + tablaInsertar +
                        "(enlace, nombre, imagen, idJuegos, descartado) VALUES" +
                        "(@enlace, @nombre, @imagen, @idJuegos, @descartado)";

                using (SqlCommand comando = new SqlCommand(sqlInsertar, conexion))
                {
                    comando.Parameters.AddWithValue("@enlace", enlace);
                    comando.Parameters.AddWithValue("@nombre", nombreJuego);
                    comando.Parameters.AddWithValue("@imagen", imagen);
                    comando.Parameters.AddWithValue("@idJuegos", idJuego);
                    comando.Parameters.AddWithValue("@descartado", descartado);

                    comando.ExecuteNonQuery();
                    try
                    {

                    }
                    catch
                    {

                    }
                }

                string sqlBorrar = "DELETE FROM " + tablaBorrar + " WHERE enlace=@enlace";

                using (SqlCommand comando = new SqlCommand(sqlBorrar, conexion))
                {
                    comando.Parameters.AddWithValue("@enlace", enlace);

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

        public static void Streaming(string idStreaming, string nombreCodigo, int idJuego, SqlConnection conexion)
        {
            if (idJuego > 0)
            {
                string sqlActualizar = "UPDATE streaming" + idStreaming + " " +
                   "SET idJuego=@idJuego WHERE nombreCodigo=@nombreCodigo";

                using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
                {
                    comando.Parameters.AddWithValue("@idJuego", idJuego);
                    comando.Parameters.AddWithValue("@nombreCodigo", nombreCodigo);

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

        public static void PlataformaAmazon(string idAmazon, int idJuego, SqlConnection conexion)
        {
            if (idJuego > 0)
            {
                bool actualizado = false;

				string sqlActualizar = "UPDATE juegos " +
				  "SET idAmazon=@idAmazon WHERE id=@id";

				using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
				{
					comando.Parameters.AddWithValue("@id", idJuego);
					comando.Parameters.AddWithValue("@idAmazon", idAmazon);

					try
					{
						comando.ExecuteNonQuery();
                        actualizado = true;
					}
					catch
					{

					}
				}

                if (actualizado == true)
                {
                    string sqlBorrar = "DELETE FROM temporalAmazonJuegos WHERE id=@id";

					using (SqlCommand comando = new SqlCommand(sqlBorrar, conexion))
					{
						comando.Parameters.AddWithValue("@id", idAmazon);

						comando.ExecuteNonQuery();
					}
				}
			}
		}

        public static void DescartarTienda(string idTienda, string enlace, SqlConnection conexion)
		{
			string sqlActualizar = "UPDATE tienda" + idTienda + " " +
					"SET descartado=@descartado WHERE enlace=@enlace";

			using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
			{
				comando.Parameters.AddWithValue("@descartado", "si");
				comando.Parameters.AddWithValue("@enlace", enlace);

				try
				{
					comando.ExecuteNonQuery();
				}
				catch
				{

				}
			}
		}

        public static void DescartarStreaming(string idStreaming, string nombreCodigo, SqlConnection conexion)
        {
            string sqlActualizar = "UPDATE streaming" + idStreaming + " " +
                    "SET descartado=@descartado WHERE nombreCodigo=@nombreCodigo";

            using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
            {
                comando.Parameters.AddWithValue("@descartado", 1);
                comando.Parameters.AddWithValue("@nombreCodigo", nombreCodigo);

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
