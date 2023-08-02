#nullable disable

using Juegos;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Data.SqlClient;

namespace BaseDatos.Tiendas
{
	public static class Comprobar
	{
		public static void Steam(JuegoPrecio oferta, JuegoAnalisis analisis, ViewDataDictionary objeto)
		{
			Juego juego = JuegoCrear.Generar();

			bool insertar = false;
			bool actualizar = false;

			string idSteam = string.Empty;

			if (oferta.Enlace.Contains("https://store.steampowered.com/app/") == true)
			{
				idSteam = APIs.Steam.Juego.LimpiarID(oferta.Enlace);
			}

			if (string.IsNullOrEmpty(idSteam) == false)
			{
				int numeroId = 0;

				try
				{
					numeroId = int.Parse(idSteam);
				}
				catch
				{

				}

				if (numeroId > 0)
				{
					WebApplicationBuilder builder = WebApplication.CreateBuilder();
					string conexionTexto = builder.Configuration.GetConnectionString("pepeizqs_deals_webContextConnection");
					SqlConnection conexion = new SqlConnection(conexionTexto);

                    using (conexion)
					{
						conexion.Open();
						string buscarJuego = "SELECT * FROM juegos WHERE idSteam=@idSteam";
						SqlCommand comando = new SqlCommand(buscarJuego, conexion);

                        using (comando)
						{
							comando.Parameters.AddWithValue("@idSteam", idSteam);

							SqlDataReader lector = comando.ExecuteReader();

                            using (lector)
							{
								if (lector.Read() == false)
								{
									try
									{
										Task<Juego> tarea = APIs.Steam.Juego.CargarDatos(idSteam);
										tarea.Wait();

										juego = tarea.Result;
									}
									catch
									{

									}

									if (juego != null)
									{
										if (juego.PrecioActualesTiendas == null)
										{
											juego.PrecioActualesTiendas = new List<JuegoPrecio>();
											juego.PrecioMinimosHistoricos = new List<JuegoPrecio>();
										}

										if (juego.PrecioActualesTiendas.Count == 0)
										{
											juego.PrecioActualesTiendas.Add(oferta);
											juego.PrecioMinimosHistoricos.Add(oferta);
										}

										insertar = true;
									}
									else
									{
										insertar = false;
									}
								}
								else
								{
									juego = Juegos.Cargar.Ejecutar(juego, lector);

									actualizar = true;
								}
							}

							lector.Close();
						}

						comando.Dispose();
					}

					conexion.Dispose();

					if (analisis != null)
					{
						juego.Analisis = analisis;
					}

					if (insertar == true && actualizar == false)
					{
						Juegos.Insertar.Ejecutar(juego);
					}

					if (actualizar == true && insertar == false)
					{
						Juegos.Precios.Actualizar(juego, oferta, objeto);
					}
				}
			}
		}

		public static void Resto(List<JuegoPrecio> ofertas, ViewDataDictionary objeto)
		{
			foreach (var oferta in ofertas)
			{
				Resto(oferta, objeto);
			}
		}

		public static void Resto(JuegoPrecio oferta, ViewDataDictionary objeto)
		{
			WebApplicationBuilder builder = WebApplication.CreateBuilder();
			string conexionTexto = builder.Configuration.GetConnectionString("pepeizqs_deals_webContextConnection");
			SqlConnection conexion = new SqlConnection(conexionTexto);

            using (conexion)
			{
				conexion.Open();

				bool insertarTienda = false;
				int idBuscarJuego = 0;

				string buscarTienda = "SELECT * FROM tienda" + oferta.Tienda + " WHERE enlace=@enlace";
				SqlCommand comandoBuscar = new SqlCommand(buscarTienda, conexion);

                using (comandoBuscar)
				{
                    comandoBuscar.Parameters.AddWithValue("@enlace", oferta.Enlace);

					SqlDataReader lector = comandoBuscar.ExecuteReader();

                    using (lector)
					{
						if (lector.Read() == false)
						{
							insertarTienda = true;
						}
						else
						{
							idBuscarJuego = lector.GetInt32(3);
						}
					}

					lector.Close();
				}

				comandoBuscar.Dispose();

				if (insertarTienda == true)
				{
					string buscarNombre = "SELECT * FROM juegos WHERE nombre=@nombre";
					SqlCommand comandoBuscar2 = new SqlCommand(buscarNombre, conexion);

                    using (comandoBuscar2)
					{
                        comandoBuscar2.Parameters.AddWithValue("@nombre", oferta.Nombre);

						SqlDataReader lector = comandoBuscar2.ExecuteReader();

                        using (lector)
						{
							if (lector.Read() == true)
							{
								idBuscarJuego = lector.GetInt32(0);
							}
						}

						lector.Close();
					}

					string sqlAñadir = "INSERT INTO tienda" + oferta.Tienda + " " +
						"(enlace, nombre, imagen, idJuegos) VALUES " +
						"(@enlace, @nombre, @imagen, @idJuegos)";
					SqlCommand comandoInsertar = new SqlCommand(sqlAñadir, conexion);

                    using (comandoInsertar)
					{
                        comandoInsertar.Parameters.AddWithValue("@enlace", oferta.Enlace);
                        comandoInsertar.Parameters.AddWithValue("@nombre", oferta.Nombre);
                        comandoInsertar.Parameters.AddWithValue("@imagen", oferta.Imagen);
                        comandoInsertar.Parameters.AddWithValue("@idJuegos", idBuscarJuego);

						try
						{
                            comandoInsertar.ExecuteNonQuery();
						}
						catch
						{

						}
					}
				}

				if (idBuscarJuego > 0)
				{
					string buscarJuego = "SELECT * FROM juegos WHERE id=@id";
					SqlCommand comandoBuscar3 = new SqlCommand(buscarJuego, conexion);

                    using (comandoBuscar3)
					{
                        comandoBuscar3.Parameters.AddWithValue("@id", idBuscarJuego.ToString());

						SqlDataReader lector = comandoBuscar3.ExecuteReader();

                        using (lector)
						{
							if (lector.Read() == true)
							{
								Juego juego = JuegoCrear.Generar();

								juego = Juegos.Cargar.Ejecutar(juego, lector);

								Juegos.Precios.Actualizar(juego, oferta, objeto);
							}
						}

						lector.Close();
					}

					comandoBuscar3.Dispose();
				}
			}

			conexion.Dispose();
		}
	}
}
