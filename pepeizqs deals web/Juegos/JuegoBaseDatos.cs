#nullable disable

using Microsoft.Data.SqlClient;
using Newtonsoft.Json;

namespace Juegos
{
	public static class JuegoBaseDatos
	{
		public static void ComprobarSteam(List<JuegoPrecio> ofertas, string mensaje)
		{
			foreach (JuegoPrecio oferta in ofertas)
			{
				Juego juego = JuegoCrear.Generar();
				bool insertar = false;

				string id = string.Empty;

				if (oferta.Enlace.Contains("https://store.steampowered.com/app/") == true)
				{
					id = APIs.Steam.Juego.LimpiarID(oferta.Enlace);
				}
		
				mensaje = id;

				if (id != string.Empty)
				{
					if (id != null)
					{
						WebApplicationBuilder builder = WebApplication.CreateBuilder();
						string conexionTexto = builder.Configuration.GetConnectionString("pepeizqs_deals_webContextConnection");

						using (SqlConnection conexion = new SqlConnection(conexionTexto))
						{
							conexion.Open();
							string seleccionarJuego = "SELECT * FROM juegos WHERE id=@idSteam";

							using (SqlCommand comando = new SqlCommand(seleccionarJuego, conexion))
							{
								comando.Parameters.AddWithValue("@idSteam", id);

								using (SqlDataReader lector = comando.ExecuteReader())
								{
									if (lector.Read() == false)
									{
										juego = APIs.Steam.Juego.CargarDatos(id).Result;
										insertar = true;
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

										insertar = false;
									}
								}
							}
						}

						if (insertar == true)
						{
							InsertarJuego(juego);
						}
						else
						{
							ActualizarNuevoPrecio(juego, oferta);
						}
					}				
				}

				try
				{

				}
				catch
				{

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

					comando.ExecuteNonQuery();
				}
			}

			try
			{
				
			}
			catch
			{

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
				}
			}
			try
			{
				
			}
			catch 
			{

			}
		}

		public static void ActualizarNuevoPrecio(Juego juego, JuegoPrecio nuevoPrecio)
		{
			bool precioEncontrado = false;

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

					precioEncontrado = true;
					break;
				}
			}

			if (precioEncontrado == false)
			{
				juego.PrecioActualesTiendas.Add(nuevoPrecio);
			}

			if (juego.PrecioActualesTiendas.Count > 0)
			{
				foreach (JuegoPrecio precio in juego.PrecioActualesTiendas)
				{
					if (precio.FechaDetectado.DayOfYear + 1 > DateTime.Now.DayOfYear)
					{
						if (juego.PrecioMinimosHistoricos.Count > 0)
						{
							bool drmEncontrado = false;

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

							if (drmEncontrado == false)
							{
								juego.PrecioMinimosHistoricos.Add(precio);
							}
						}
					}				
				}
			}

			ActualizarJuego(juego);
		}
	}
}
