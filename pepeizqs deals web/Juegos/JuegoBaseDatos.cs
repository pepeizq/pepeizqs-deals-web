#nullable disable

using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;

namespace Juegos
{
	public static class JuegoBaseDatos
	{
		public static void ComprobarSteam(JuegoPrecio oferta, JuegoAnalisis analisis, ViewDataDictionary objeto)
		{
			Juego juego = JuegoCrear.Generar();

			bool insertar = false;
			bool actualizar = false;

			string id = string.Empty;

			if (oferta.Enlace.Contains("https://store.steampowered.com/app/") == true)
			{
				id = APIs.Steam.Juego.LimpiarID(oferta.Enlace);
			}

			if (string.IsNullOrEmpty(id) == false)
			{
				int numeroId = 0;

				try
				{
					numeroId = int.Parse(id);
				}
				catch 
				{ 
				
				}

				if (numeroId != 0)
				{
					WebApplicationBuilder builder = WebApplication.CreateBuilder();
					string conexionTexto = builder.Configuration.GetConnectionString("pepeizqs_deals_webContextConnection");

					using (SqlConnection conexion = new SqlConnection(conexionTexto))
					{
						conexion.Open();
						string seleccionarJuego = "SELECT * FROM juegos WHERE idSteam=@idSteam";

						using (SqlCommand comando = new SqlCommand(seleccionarJuego, conexion))
						{
							comando.Parameters.AddWithValue("@idSteam", id);

							using (SqlDataReader lector = comando.ExecuteReader())
							{
								if (lector.Read() == false)
								{
									try
									{
										Task<Juego> tarea = APIs.Steam.Juego.CargarDatos(id);
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
									juego.Id = lector.GetInt32(0);
									juego.Nombre = lector.GetString(1);
									juego.Tipo = Enum.Parse<JuegoTipo>(lector.GetString(2));

									juego.Imagenes = JsonConvert.DeserializeObject<JuegoImagenes>(lector.GetString(3));
									juego.PrecioMinimosHistoricos = JsonConvert.DeserializeObject<List<JuegoPrecio>>(lector.GetString(4));
									juego.PrecioActualesTiendas = JsonConvert.DeserializeObject<List<JuegoPrecio>>(lector.GetString(5));

									juego.Analisis = JsonConvert.DeserializeObject<JuegoAnalisis>(lector.GetString(6));
									juego.Caracteristicas = JsonConvert.DeserializeObject<JuegoCaracteristicas>(lector.GetString(7));
									juego.Media = JsonConvert.DeserializeObject<JuegoMedia>(lector.GetString(8));

									juego.IdSteam = lector.GetInt32(9);
									juego.IdGog = lector.GetInt32(10);
									juego.FechaSteamAPIComprobacion = DateTime.Parse(lector.GetString(11));

									actualizar = true;
								}
							}
						}
					}

					if (analisis != null)
					{
						juego.Analisis = analisis;
					}

					if (insertar == true && actualizar == false)
					{
						InsertarJuego(juego);
					}

					if (actualizar == true && insertar == false)
					{
						ActualizarNuevosPrecios(juego, oferta);
					}
				}
			}
		}

		public static void ComprobarTienda(JuegoPrecio oferta, ViewDataDictionary objeto)
		{
			WebApplicationBuilder builder = WebApplication.CreateBuilder();
			string conexionTexto = builder.Configuration.GetConnectionString("pepeizqs_deals_webContextConnection");

			using (SqlConnection conexion = new SqlConnection(conexionTexto))
			{
				conexion.Open();

				bool insertarTienda = false;
				int idBuscarJuego = 0;

				string buscarTienda = "SELECT * FROM tienda" + oferta.Tienda + " WHERE enlace=@enlace";

				using (SqlCommand comando = new SqlCommand(buscarTienda, conexion))
				{
					comando.Parameters.AddWithValue("@enlace", oferta.Enlace);

					using (SqlDataReader lector = comando.ExecuteReader())
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
				}

                if (insertarTienda == true)
                {
					string sqlAñadir = "INSERT INTO tienda" + oferta.Tienda + " " +
						"(enlace, nombre, imagen, idJuegos) VALUES " +
						"(@enlace, @nombre, @imagen, @idJuegos)";

					using (SqlCommand comando = new SqlCommand(sqlAñadir, conexion))
					{
						comando.Parameters.AddWithValue("@enlace", oferta.Enlace);
						comando.Parameters.AddWithValue("@nombre", oferta.Nombre);
						comando.Parameters.AddWithValue("@imagen", oferta.Imagen);
						comando.Parameters.AddWithValue("@idJuegos", 0);

						try
						{
							comando.ExecuteNonQuery();
						}
						catch
						{

						}
					}
				}

				if (idBuscarJuego > 0)
				{
					string buscarJuego = "SELECT * FROM juegos WHERE id=@id";

					using (SqlCommand comando = new SqlCommand(buscarJuego, conexion))
					{
						comando.Parameters.AddWithValue("@id", idBuscarJuego);

						using (SqlDataReader lector = comando.ExecuteReader())
						{
							if (lector.Read() == true)
							{
								Juego juego = JuegoCrear.Generar();

								juego.Id = lector.GetInt32(0);
								juego.Nombre = lector.GetString(1);
								juego.Tipo = Enum.Parse<JuegoTipo>(lector.GetString(2));

								juego.Imagenes = JsonConvert.DeserializeObject<JuegoImagenes>(lector.GetString(3));
								juego.PrecioMinimosHistoricos = JsonConvert.DeserializeObject<List<JuegoPrecio>>(lector.GetString(4));
								juego.PrecioActualesTiendas = JsonConvert.DeserializeObject<List<JuegoPrecio>>(lector.GetString(5));

								juego.Analisis = JsonConvert.DeserializeObject<JuegoAnalisis>(lector.GetString(6));
								juego.Caracteristicas = JsonConvert.DeserializeObject<JuegoCaracteristicas>(lector.GetString(7));
								juego.Media = JsonConvert.DeserializeObject<JuegoMedia>(lector.GetString(8));

								juego.IdSteam = lector.GetInt32(9);
								juego.IdGog = lector.GetInt32(10);
								juego.FechaSteamAPIComprobacion = DateTime.Parse(lector.GetString(11));

								ActualizarNuevosPrecios(juego, oferta);
							}
						}
					}
				}
			}
		}
	
		public static void InsertarJuego(Juego juego)
		{
			WebApplicationBuilder builder = WebApplication.CreateBuilder();
			string conexionTexto = builder.Configuration.GetConnectionString("pepeizqs_deals_webContextConnection");

			using (SqlConnection conexion = new SqlConnection(conexionTexto))
			{
				conexion.Open();

				string sqlAñadir = "INSERT INTO juegos " +
					"(idSteam, idGog, nombre, tipo, fechaSteamAPIComprobacion, imagenes, precioMinimosHistoricos, precioActualesTiendas, analisis, caracteristicas, media) VALUES " +
					"(@idSteam, @idGog, @nombre, @tipo, @fechaSteamAPIComprobacion, @imagenes, @precioMinimosHistoricos, @precioActualesTiendas, @analisis, @caracteristicas, @media) ";

				using (SqlCommand comando = new SqlCommand(sqlAñadir, conexion))
				{
					comando.Parameters.AddWithValue("@idSteam", juego.IdSteam);
					comando.Parameters.AddWithValue("@idGog", juego.IdGog);
					comando.Parameters.AddWithValue("@nombre", juego.Nombre);
					comando.Parameters.AddWithValue("@tipo", juego.Tipo);
					comando.Parameters.AddWithValue("@fechaSteamAPIComprobacion", juego.FechaSteamAPIComprobacion);
					comando.Parameters.AddWithValue("@imagenes", JsonConvert.SerializeObject(juego.Imagenes));
					comando.Parameters.AddWithValue("@precioMinimosHistoricos", JsonConvert.SerializeObject(juego.PrecioMinimosHistoricos));
					comando.Parameters.AddWithValue("@precioActualesTiendas", JsonConvert.SerializeObject(juego.PrecioActualesTiendas));
					comando.Parameters.AddWithValue("@analisis", JsonConvert.SerializeObject(juego.Analisis));
					comando.Parameters.AddWithValue("@caracteristicas", JsonConvert.SerializeObject(juego.Caracteristicas));
					comando.Parameters.AddWithValue("@media", JsonConvert.SerializeObject(juego.Media));

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

		public static void ActualizarJuego(Juego juego)
		{
			WebApplicationBuilder builder = WebApplication.CreateBuilder();
			string conexionTexto = builder.Configuration.GetConnectionString("pepeizqs_deals_webContextConnection");

			using (SqlConnection conexion = new SqlConnection(conexionTexto))
			{
				conexion.Open();

				string sqlEditar = "UPDATE juegos " +
					"SET idSteam=@idSteam, idGog=@idGog, nombre=@nombre, tipo=@tipo, fechaSteamAPIComprobacion=@fechaSteamAPIComprobacion, " +
						"imagenes=@imagenes, precioMinimosHistoricos=@precioMinimosHistoricos, precioActualesTiendas=@precioActualesTiendas, " +
						"analisis=@analisis, caracteristicas=@caracteristicas, media=@media ";

				if (juego.IdSteam > 0)
				{
					sqlEditar = sqlEditar + "WHERE idSteam=@idSteam";
				}
				else
				{
					if (juego.IdGog > 0)
					{
						sqlEditar = sqlEditar + "WHERE idGog=@idGog";
					}
					else
					{
						sqlEditar = sqlEditar + "WHERE id=@id";
					}
				}

				using (SqlCommand comando = new SqlCommand(sqlEditar, conexion))
				{
					comando.Parameters.AddWithValue("@idSteam", juego.IdSteam);
					comando.Parameters.AddWithValue("@idGog", juego.IdGog);
					comando.Parameters.AddWithValue("@nombre", juego.Nombre);
					comando.Parameters.AddWithValue("@tipo", juego.Tipo);
					comando.Parameters.AddWithValue("@fechaSteamAPIComprobacion", juego.FechaSteamAPIComprobacion);
					comando.Parameters.AddWithValue("@imagenes", JsonConvert.SerializeObject(juego.Imagenes));
					comando.Parameters.AddWithValue("@precioMinimosHistoricos", JsonConvert.SerializeObject(juego.PrecioMinimosHistoricos));
					comando.Parameters.AddWithValue("@precioActualesTiendas", JsonConvert.SerializeObject(juego.PrecioActualesTiendas));
					comando.Parameters.AddWithValue("@analisis", JsonConvert.SerializeObject(juego.Analisis));
					comando.Parameters.AddWithValue("@caracteristicas", JsonConvert.SerializeObject(juego.Caracteristicas));
					comando.Parameters.AddWithValue("@media", JsonConvert.SerializeObject(juego.Media));

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

		public static void LimpiarJuegos()
		{
			WebApplicationBuilder builder = WebApplication.CreateBuilder();
			string conexionTexto = builder.Configuration.GetConnectionString("pepeizqs_deals_webContextConnection");

			using (SqlConnection conexion = new SqlConnection(conexionTexto))
			{
				conexion.Open();

				string limpiar = "TRUNCATE TABLE juegos";

				using (SqlCommand comando = new SqlCommand(limpiar, conexion))
				{
					comando.ExecuteNonQuery();
				}
			}
		}

		public static void ActualizarNuevosPrecios(Juego juego, JuegoPrecio nuevoPrecio)
		{
			bool nuevoEncontrado = false;

			if (juego.PrecioActualesTiendas != null)
			{
				if (juego.PrecioActualesTiendas.Count > 0)
				{
					foreach (JuegoPrecio precio in juego.PrecioActualesTiendas)
					{
						if (nuevoPrecio.Enlace == precio.Enlace)
						{
							precio.Precio = nuevoPrecio.Precio;
							precio.Descuento = nuevoPrecio.Descuento;
							precio.FechaDetectado = nuevoPrecio.FechaDetectado;
							precio.FechaTermina = nuevoPrecio.FechaTermina;
							precio.CodigoDescuento = nuevoPrecio.CodigoDescuento;
							precio.CodigoTexto = nuevoPrecio.CodigoTexto;
							precio.Nombre = nuevoPrecio.Nombre;
							precio.Imagen = nuevoPrecio.Imagen;

							nuevoEncontrado = true;
						}
					}
				}
			}
			else
			{
				juego.PrecioActualesTiendas = new List<JuegoPrecio>();
			}

			if (nuevoEncontrado == false)
			{
				juego.PrecioActualesTiendas.Add(nuevoPrecio);
			}

			if (juego.PrecioActualesTiendas.Count > 0)
			{
				foreach (JuegoPrecio precio in juego.PrecioActualesTiendas)
				{
					if (precio.FechaDetectado.DayOfYear + 2 > DateTime.Now.DayOfYear)
					{
						bool drmEncontrado = false;

						if (juego.PrecioMinimosHistoricos != null)
						{
							if (juego.PrecioMinimosHistoricos.Count > 0)
							{
								foreach (JuegoPrecio minimo in juego.PrecioMinimosHistoricos)
								{
									if (precio.DRM == minimo.DRM)
									{
										if (precio.Precio <= minimo.Precio)
										{
											minimo.Precio = precio.Precio;
											minimo.Descuento = precio.Descuento;
											minimo.FechaDetectado = precio.FechaDetectado;
											minimo.FechaTermina = precio.FechaTermina;
											minimo.CodigoDescuento = precio.CodigoDescuento;
											minimo.CodigoTexto = precio.CodigoTexto;
											minimo.Nombre = precio.Nombre;
											minimo.Imagen = precio.Imagen;
											minimo.Enlace = precio.Enlace;
											minimo.Tienda = precio.Tienda;
										}

										drmEncontrado = true;
										break;
									}
								}
							}
						}
						else
						{
							juego.PrecioMinimosHistoricos = new List<JuegoPrecio>();
						}					
						
						if (drmEncontrado == false)
						{
							juego.PrecioMinimosHistoricos.Add(precio);
						}
					}				
				}
			}

			ActualizarJuego(juego);
		}
	}
}
