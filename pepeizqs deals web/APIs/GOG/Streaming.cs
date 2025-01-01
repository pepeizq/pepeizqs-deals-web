#nullable disable

using Herramientas;
using Microsoft.Data.SqlClient;
using System.Text.Json;

namespace APIs.GOG
{
	public static class Streaming
	{
		public static Streaming2.Streaming Generar()
		{
			Streaming2.Streaming amazonluna = new Streaming2.Streaming
			{
				Id = Streaming2.StreamingTipo.AmazonLuna,
				Nombre = "Amazon Luna (GOG)",
				ImagenLogo = "/imagenes/streaming/amazonluna_logo.webp",
				ImagenIcono = "/imagenes/streaming/amazonluna_icono.webp"
			};

			return amazonluna;
		}

		public static async Task Buscar(SqlConnection conexion)
		{
			BaseDatos.Admin.Actualizar.Tiendas(Generar().Id.ToString(), DateTime.Now, 0, conexion);

			int cantidad = 0;

			int i = 1;
			int limite = 10;
			while (i < limite + 1)
			{
				string html = await Decompiladores.Estandar("https://catalog.gog.com/v1/filtered-catalog?limit=48&order=desc:discount&productType=in:game,pack,dlc,extras&page=" + i.ToString() + "&pageId=2f70726f6d6f2f706c61792d6f6e2d6c756e61&sectionId=7efd2a6a-0831-43af-bc1f-616509912d24&countryCode=ES&locale=en-US&currencyCode=EUR");

				if (string.IsNullOrEmpty(html) == false)
				{
					GOGOfertas datos = JsonSerializer.Deserialize<GOGOfertas>(html);

					if (datos != null)
					{
						limite = datos.Paginas;

						foreach (var juego in datos.Juegos)
						{
							if (juego.Tipo == "game" || juego.Tipo == "pack")
							{
								DateTime fecha = DateTime.Now;
								fecha = fecha + TimeSpan.FromDays(1);

								bool encontrado = false;

								string sqlBuscar = "SELECT * FROM streamingamazonluna WHERE nombreCodigo=@nombreCodigo";

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

								using (SqlCommand comando = new SqlCommand(sqlBuscar, conexion))
								{
									comando.Parameters.AddWithValue("@nombreCodigo", juego.Id);

									using (SqlDataReader lector = comando.ExecuteReader())
									{
										encontrado = lector.Read();

										cantidad += 1;
										BaseDatos.Admin.Actualizar.Tiendas(Generar().Id.ToString(), DateTime.Now, cantidad, conexion);
									}
								}

								if (encontrado == true)
								{
									string sqlActualizar = "UPDATE streamingamazonluna " +
															"SET fecha=@fecha WHERE nombreCodigo=@nombreCodigo";

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

									using (SqlCommand comandoActualizar = new SqlCommand(sqlActualizar, conexion))
									{
										comandoActualizar.Parameters.AddWithValue("@nombreCodigo", juego.Id);
										comandoActualizar.Parameters.AddWithValue("@fecha", fecha);

										try
										{
											comandoActualizar.ExecuteNonQuery();
										}
										catch
										{

										}
									}
								}
								else
								{
									string sqlInsertar = "INSERT INTO streamingamazonluna " +
														"(nombreCodigo, nombre, drms, fecha) VALUES " +
														"(@nombreCodigo, @nombre, @drms, @fecha) ";

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

									using (SqlCommand comandoInsertar = new SqlCommand(sqlInsertar, conexion))
									{
										comandoInsertar.Parameters.AddWithValue("@nombreCodigo", juego.Id);
										comandoInsertar.Parameters.AddWithValue("@nombre", juego.Nombre);
										comandoInsertar.Parameters.AddWithValue("@drms", "gog");
										comandoInsertar.Parameters.AddWithValue("@fecha", fecha);

										try
										{
											comandoInsertar.ExecuteNonQuery();
										}
										catch (Exception ex)
										{
											BaseDatos.Errores.Insertar.Mensaje("Insertar Amazon Luna " + juego.Nombre, ex);
										}
									}
								}
							}
						}						
					}
				}

				i += 1;
			}
		}
	}
}
