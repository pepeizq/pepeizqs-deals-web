#nullable disable

using Herramientas;
using Microsoft.Data.SqlClient;

namespace BaseDatos.Tiendas
{
	public static class Admin
	{
		public static void Actualizar(string tienda, DateTime fecha, string mensaje)
		{
			WebApplicationBuilder builder = WebApplication.CreateBuilder();
			string conexionTexto = builder.Configuration.GetConnectionString("pepeizqs_deals_webContextConnection");
			SqlConnection conexion = new SqlConnection(conexionTexto);

            using (conexion)
			{
				conexion.Open();

				bool insertar = false;
				bool actualizar = false;

				string seleccionarJuego = "SELECT * FROM adminTiendas WHERE id=@id";

				using (SqlCommand comando = new SqlCommand(seleccionarJuego, conexion))
				{
					comando.Parameters.AddWithValue("@id", tienda);

					SqlDataReader lector = comando.ExecuteReader();

                    using (lector)
					{
						if (lector.Read() == false)
						{
							insertar = true;
						}
						else
						{
							actualizar = true;
						}
					}

					lector.Close();
				}

				if (insertar == true)
				{
					string sqlAñadir = "INSERT INTO adminTiendas " +
						"(id, fecha, mensaje) VALUES " +
						"(@id, @fecha, @mensaje)";

					SqlCommand comando = new SqlCommand(sqlAñadir, conexion);

                    using (comando)
					{
						comando.Parameters.AddWithValue("@id", tienda);
						comando.Parameters.AddWithValue("@fecha", fecha.ToString());
						comando.Parameters.AddWithValue("@mensaje", mensaje);

						try
						{
							comando.ExecuteNonQuery();
						}
						catch
						{

						}
					}
				}

				if (actualizar == true)
				{
					string sqlActualizar = "UPDATE adminTiendas " +
							"SET id=@id, fecha=@fecha, mensaje=@mensaje WHERE id=@id";

					SqlCommand comando = new SqlCommand(sqlActualizar, conexion);

                    using (comando)
					{
						comando.Parameters.AddWithValue("@id", tienda);
						comando.Parameters.AddWithValue("@fecha", fecha.ToString());
						comando.Parameters.AddWithValue("@mensaje", mensaje);

						SqlDataReader lector = comando.ExecuteReader();

                        try
						{
							comando.ExecuteNonQuery();
						}
						catch
						{

						}

						lector.Close();
					}

					comando.Dispose();
				}
			}

			conexion.Dispose();
		}

		public static string ComprobacionMensaje(string tienda)
		{
			string mensaje = string.Empty;

			WebApplicationBuilder builder = WebApplication.CreateBuilder();
			string conexionTexto = builder.Configuration.GetConnectionString("pepeizqs_deals_webContextConnection");
			SqlConnection conexion = new SqlConnection(conexionTexto);

            using (conexion)
			{
				conexion.Open();

				string seleccionarJuego = "SELECT * FROM adminTiendas WHERE id=@id";
				SqlCommand comando = new SqlCommand(seleccionarJuego, conexion);

                using (comando)
				{
					comando.Parameters.AddWithValue("@id", tienda);

					SqlDataReader lector = comando.ExecuteReader();

                    using (lector)
					{
						if (lector.Read() == true)
						{
							mensaje = Calculadora.HaceTiempo(DateTime.Parse(lector.GetString(1)));
							
							try
							{
                                if (lector.GetString(2) != null)
                                {
                                    mensaje = mensaje + " • " + lector.GetString(2);
                                }
                            }
							catch
							{

							}									
						}
					}

					lector.Close();
				}

				comando.Dispose();
			}

			conexion.Dispose();

			return mensaje;
		}

		public static int CronLeerOrden()
		{
			int orden = 0;

            WebApplicationBuilder builder = WebApplication.CreateBuilder();
            string conexionTexto = builder.Configuration.GetConnectionString("pepeizqs_deals_webContextConnection");
            SqlConnection conexion = new SqlConnection(conexionTexto);

            using (conexion)
            {
                conexion.Open();

                string seleccionarJuego = "SELECT * FROM cronGestionador WHERE id=@id";
                SqlCommand comando = new SqlCommand(seleccionarJuego, conexion);

                using (comando)
                {
                    comando.Parameters.AddWithValue("@id", "0");

                    SqlDataReader lector = comando.ExecuteReader();

                    using (lector)
                    {
                        if (lector.Read() == true)
                        {
							orden = lector.GetInt32(1);
                        }
                    }

                    lector.Close();
                }

                comando.Dispose();
            }

            conexion.Dispose();

			return orden;
        }

		public static void CronAumentarOrden(int orden)
		{
            WebApplicationBuilder builder = WebApplication.CreateBuilder();
            string conexionTexto = builder.Configuration.GetConnectionString("pepeizqs_deals_webContextConnection");
            SqlConnection conexion = new SqlConnection(conexionTexto);

            using (conexion)
            {
                conexion.Open();

                string sqlActualizar = "UPDATE cronGestionador " +
                            "SET id=@id, posicion=@posicion WHERE id=@id";

                SqlCommand comando = new SqlCommand(sqlActualizar, conexion);

                using (comando)
                {
                    comando.Parameters.AddWithValue("@id", "0");
                    comando.Parameters.AddWithValue("@posicion", orden);

                    SqlDataReader lector = comando.ExecuteReader();

                    try
                    {
                        comando.ExecuteNonQuery();
                    }
                    catch
                    {

                    }

                    lector.Close();
                }

                comando.Dispose();
            }

            conexion.Dispose();
        }
    }
}
