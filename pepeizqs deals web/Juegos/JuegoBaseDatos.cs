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

					using (SqlConnection conexion = new SqlConnection(conexionTexto))
					{
						conexion.Open();
						string buscarJuego = "SELECT * FROM juegos WHERE idSteam=@idSteam";

						using (SqlCommand comando = new SqlCommand(buscarJuego, conexion))
						{
							comando.Parameters.AddWithValue("@idSteam", idSteam);

							using (SqlDataReader lector = comando.ExecuteReader())
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
									juego = CargarJuego(juego, lector);
									
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
						ActualizarNuevosPrecios(juego, oferta, objeto);
					}
				}
			}
		}

		public static Juego CargarJuego(Juego juego, SqlDataReader lector)
		{
			juego.Id = lector.GetInt32(0);
			juego.Nombre = lector.GetString(1);

			if (lector.GetString(2) != null)
			{
				juego.Tipo = Enum.Parse<JuegoTipo>(lector.GetString(2));
			}

			if (lector.GetString(3) != null)
			{			
				try
				{
					juego.Imagenes = JsonConvert.DeserializeObject<JuegoImagenes>(lector.GetString(3));
				}
				catch { }
			}

			if (lector.GetString(4) != null)
			{			
				try
				{
					juego.PrecioMinimosHistoricos = JsonConvert.DeserializeObject<List<JuegoPrecio>>(lector.GetString(4));
				}
				catch { }
			}

			if (lector.GetString(5) != null)
			{
				try
				{
					juego.PrecioActualesTiendas = JsonConvert.DeserializeObject<List<JuegoPrecio>>(lector.GetString(5));
				}
				catch { }			
			}

			if (lector.GetString(6) != null)
			{
				try
				{
					juego.Analisis = JsonConvert.DeserializeObject<JuegoAnalisis>(lector.GetString(6));
				}
				catch { }				
			}

			if (lector.GetString(7) != null)
			{
				try
				{
					juego.Caracteristicas = JsonConvert.DeserializeObject<JuegoCaracteristicas>(lector.GetString(7));
				}
				catch { }			
			}

			if (lector.GetString(8) != null)
			{
				try
				{
					juego.Media = JsonConvert.DeserializeObject<JuegoMedia>(lector.GetString(8));
				}
				catch { }			
			}

			juego.IdSteam = lector.GetInt32(9);
			juego.IdGog = lector.GetInt32(10);

			if (lector.GetString(11) != null)
			{
				try
				{
					juego.FechaSteamAPIComprobacion = DateTime.Parse(lector.GetString(11));
				}
				catch { }			
			}

			return juego;
		}

        public static void ComprobarTienda(List<JuegoPrecio> ofertas, ViewDataDictionary objeto)
		{
			foreach (var oferta in ofertas)
			{
				ComprobarTienda(oferta, objeto);
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
					string buscarNombre = "SELECT * FROM juegos WHERE nombre=@nombre";

					using (SqlCommand comando = new SqlCommand(buscarNombre, conexion))
					{
						comando.Parameters.AddWithValue("@nombre", oferta.Nombre);

						using (SqlDataReader lector = comando.ExecuteReader())
						{
							if (lector.Read() == true)
							{
								idBuscarJuego = lector.GetInt32(0);
							}
						}
					}

					string sqlAñadir = "INSERT INTO tienda" + oferta.Tienda + " " +
						"(enlace, nombre, imagen, idJuegos) VALUES " +
						"(@enlace, @nombre, @imagen, @idJuegos)";

					using (SqlCommand comando = new SqlCommand(sqlAñadir, conexion))
					{
						comando.Parameters.AddWithValue("@enlace", oferta.Enlace);
						comando.Parameters.AddWithValue("@nombre", oferta.Nombre);
						comando.Parameters.AddWithValue("@imagen", oferta.Imagen);
						comando.Parameters.AddWithValue("@idJuegos", idBuscarJuego);

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
						comando.Parameters.AddWithValue("@id", idBuscarJuego.ToString());

						using (SqlDataReader lector = comando.ExecuteReader())
						{
							if (lector.Read() == true)
							{
								Juego juego = JuegoCrear.Generar();

								juego = CargarJuego(juego, lector);
								
								ActualizarNuevosPrecios(juego, oferta, objeto);
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

		public static void LimpiarJuegos(string tabla)
		{
			WebApplicationBuilder builder = WebApplication.CreateBuilder();
			string conexionTexto = builder.Configuration.GetConnectionString("pepeizqs_deals_webContextConnection");

			using (SqlConnection conexion = new SqlConnection(conexionTexto))
			{
				conexion.Open();

				string limpiar = "TRUNCATE TABLE " + tabla;

				using (SqlCommand comando = new SqlCommand(limpiar, conexion))
				{
					comando.ExecuteNonQuery();
				}
			}
		}

		public static void ActualizarNuevosPrecios(Juego juego, JuegoPrecio nuevoPrecio, ViewDataDictionary objeto)
		{
			bool añadir = true;

			if (juego.PrecioActualesTiendas != null)
			{
				if (juego.PrecioActualesTiendas.Count > 0)
				{
					foreach (JuegoPrecio precio in juego.PrecioActualesTiendas)
					{
						if (nuevoPrecio.Enlace == precio.Enlace && nuevoPrecio.DRM == precio.DRM && nuevoPrecio.Tienda == precio.Tienda)
						{
							precio.Precio = nuevoPrecio.Precio;
							precio.Descuento = nuevoPrecio.Descuento;
							precio.FechaDetectado = nuevoPrecio.FechaDetectado;
							precio.FechaTermina = nuevoPrecio.FechaTermina;
							precio.CodigoDescuento = nuevoPrecio.CodigoDescuento;
							precio.CodigoTexto = nuevoPrecio.CodigoTexto;
							precio.Nombre = nuevoPrecio.Nombre;
							precio.Imagen = nuevoPrecio.Imagen;

							añadir = false;
						}
					}
				}
			}
			else
			{
				juego.PrecioActualesTiendas = new List<JuegoPrecio>();
			}

			if (añadir == true)
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
