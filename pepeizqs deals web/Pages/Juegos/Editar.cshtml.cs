#nullable disable

using Juegos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;

namespace pepeizqs_deals_web.Pages.Juegos
{
    public class EditarModel : PageModel
    {
		public Juego juegoEditar = JuegoCrear.Generar();

		public string errorMensaje = string.Empty;
		public string exitoMensaje = string.Empty;

		public void OnGet()
        {
			string id = Request.Query["id"];
            string idSteam = Request.Query["idSteam"];
			string idGog = Request.Query["idGog"];

			string sqlBuscar = string.Empty;
			string idParametro = string.Empty;
			string idBuscar = string.Empty;

			if (id != null)
			{
				sqlBuscar = "SELECT * FROM juegos WHERE id=@id";
				idParametro = "@id";
				idBuscar = id;
			}
			else
			{
				if (idSteam != null)
				{
					sqlBuscar = "SELECT * FROM juegos WHERE idSteam=@idSteam";
					idParametro = "@idSteam";
					idBuscar = idSteam;
				}
				else
				{
					if (idGog != null)
					{
						sqlBuscar = "SELECT * FROM juegos WHERE idGog=@idGog";
						idParametro = "@idGog";
						idBuscar = idGog;
					}
				}
			}
			
            try
            {
                WebApplicationBuilder builder = WebApplication.CreateBuilder();
                string conexionTexto = builder.Configuration.GetConnectionString("pepeizqs_deals_webContextConnection");

                using (SqlConnection conexion = new SqlConnection(conexionTexto))
				{
					conexion.Open();
					String seleccionarJuego = sqlBuscar;

					using (SqlCommand comando = new SqlCommand(seleccionarJuego, conexion))
					{
						comando.Parameters.AddWithValue(idParametro, idBuscar);

						using (SqlDataReader lector = comando.ExecuteReader())
						{
							if (lector.Read())
							{
								juegoEditar.Id = lector.GetInt32(0);
								juegoEditar.Nombre = lector.GetString(1);
								juegoEditar.Tipo = Enum.Parse<JuegoTipo>(lector.GetString(2));

								juegoEditar.Imagenes = JsonConvert.DeserializeObject<JuegoImagenes>(lector.GetString(3));
								juegoEditar.PrecioMinimoActual = JsonConvert.DeserializeObject<List<JuegoPrecio>>(lector.GetString(4));
								juegoEditar.PrecioMinimoHistorico = JsonConvert.DeserializeObject<List<JuegoPrecio>>(lector.GetString(5));
								juegoEditar.PrecioActualesTiendas = JsonConvert.DeserializeObject<List<JuegoPrecio>>(lector.GetString(6));

								juegoEditar.Analisis = JsonConvert.DeserializeObject<JuegoAnalisis>(lector.GetString(7));
								juegoEditar.Caracteristicas = JsonConvert.DeserializeObject<JuegoCaracteristicas>(lector.GetString(8));
								juegoEditar.Media = JsonConvert.DeserializeObject<JuegoMedia>(lector.GetString(9));

								juegoEditar.IdSteam = lector.GetInt32(10);
								juegoEditar.IdGog = lector.GetInt32(11);
								juegoEditar.FechaSteamAPIComprobacion = DateTime.Parse(lector.GetString(12));
							}
						}
					}
				}
			}
            catch (Exception ex) 
            { 
                errorMensaje = ex.Message;
            }
        }

        public IActionResult OnPost() 
        {
			if (Request.Form["nombre"] != string.Empty)
			{
				string nombre = Request.Form["nombre"];

				if (nombre.Length > 0)
				{
					if (Request.Form["idSteam"] != string.Empty)
					{
						if (int.Parse(Request.Form["idSteam"]) > 0)
						{
							juegoEditar.IdSteam = int.Parse(Request.Form["idSteam"]);
						}
					}

					if (Request.Form["idGog"] != string.Empty)
					{
						if (int.Parse(Request.Form["idGog"]) > 0)
						{
							juegoEditar.IdGog = int.Parse(Request.Form["idGog"]);
						}
					}


					juegoEditar.Nombre = Request.Form["nombre"];
					juegoEditar.Tipo = Enum.Parse<JuegoTipo>(Request.Form["tipo"]);
					juegoEditar.FechaSteamAPIComprobacion = DateTime.Parse(Request.Form["fechacomprobacion"]);

					//----------------------------

					int l = 0;
					while (l < 20)
					{
						if (Request.Form["precio_" + l.ToString()] != string.Empty)
						{
							string precioMirar = Request.Form["precio_" + l.ToString()];

							if (juegoEditar.PrecioActualesTiendas == null)
							{
								juegoEditar.PrecioActualesTiendas = new List<JuegoPrecio>();
							}

							if (precioMirar != null)
							{
								JuegoPrecio precio = new JuegoPrecio
								{
									Descuento = int.Parse(Request.Form["descuento_" + l.ToString()]),
									DRM = Enum.Parse<JuegoDRM>(Request.Form["drm_" + l.ToString()]),
									Precio = decimal.Parse(Request.Form["precio_" + l.ToString()]),
									Moneda = Enum.Parse<JuegoMoneda>(Request.Form["moneda_" + l.ToString()]),
									FechaDetectado = DateTime.Parse(Request.Form["fechadetectado_" + l.ToString()]),
									Enlace = Request.Form["enlace_" + l.ToString()],
									Tienda = Request.Form["tienda_" + l.ToString()]
								};

								juegoEditar.PrecioActualesTiendas.Add(precio);
							}
							else
							{
								break;
							}
						}
						else
						{
							break;
						}

						l += 1;
					}

					juegoEditar.PrecioMinimoActual = juegoEditar.PrecioActualesTiendas;
					juegoEditar.PrecioMinimoHistorico = juegoEditar.PrecioActualesTiendas;

					//----------------------------

					JuegoImagenes imagenes = new JuegoImagenes
					{
						Header_460x215 = Request.Form["imagenheader"],
						Capsule_231x87 = Request.Form["imagencapsule"],
						Logo = Request.Form["imagenlogo"],
						Library_600x900 = Request.Form["imagenlibrary1"],
						Library_1920x620 = Request.Form["imagenlibrary2"]
					};

					juegoEditar.Imagenes = imagenes;

					//----------------------------

					JuegoCaracteristicas caracteristicas = new JuegoCaracteristicas
					{
						Windows = bool.Parse(Request.Form["windows"]),
						Mac = bool.Parse(Request.Form["mac"]),
						Linux = bool.Parse(Request.Form["linux"]),
						Descripcion = Request.Form["descripcion"]
					};

					int j = 0;
					while (j < 20)
					{
						if (Request.Form["desarrollador_" + j.ToString()] != string.Empty)
						{
							string desarrollador = Request.Form["desarrollador_" + j.ToString()];

							if (caracteristicas.Desarrolladores == null)
							{
								caracteristicas.Desarrolladores = new List<string>();
							}

							if (desarrollador != null)
							{
								caracteristicas.Desarrolladores.Add(desarrollador);
							}
							else
							{
								break;
							}
						}
						else
						{
							break;
						}

						j += 1;
					}

					int k = 0;
					while (k < 20)
					{
						if (Request.Form["publisher_" + k.ToString()] != string.Empty)
						{
							string publisher = Request.Form["publisher_" + k.ToString()];

							if (caracteristicas.Publishers == null)
							{
								caracteristicas.Publishers = new List<string>();
							}

							if (publisher != null)
							{
								caracteristicas.Publishers.Add(publisher);
							}
							else
							{
								break;
							}
						}
						else
						{
							break;
						}

						k += 1;
					}

					juegoEditar.Caracteristicas = caracteristicas;

					//----------------------------

					JuegoMedia media = new JuegoMedia();

					if (Request.Form["video"] != string.Empty)
					{
						media.Video = Request.Form["video"];
					}

					int i = 0;
					while (i < 20)
					{
						if (Request.Form["captura_" + i.ToString()] != string.Empty)
						{
							string captura = Request.Form["captura_" + i.ToString()];

							if (media.Capturas == null)
							{
								media.Capturas = new List<string>();
							}

							if (captura != null)
							{
								media.Capturas.Add(captura);
							}
							else
							{
								break;
							}
						}
						else
						{
							break;
						}

						i += 1;
					}

					if (media.Video != null & media.Capturas != null)
					{
						juegoEditar.Media = media;
					}

					//----------------------------

					try
					{
						WebApplicationBuilder builder = WebApplication.CreateBuilder();
						string conexionTexto = builder.Configuration.GetConnectionString("pepeizqs_deals_webContextConnection");

						using (SqlConnection conexion = new SqlConnection(conexionTexto))
						{
							conexion.Open();

							string sqlEditar = "UPDATE juegos " +
								"SET idSteam=@idSteam, idGog=@idGog, nombre=@nombre, tipo=@tipo, fechaSteamAPIComprobacion=@fechaSteamAPIComprobacion, " +
									"imagenes=@imagenes, precioMinimoActual=@precioMinimoActual, precioMinimoHistorico=@precioMinimoHistorico, precioActualesTiendas=@precioActualesTiendas, " +
									"analisis=@analisis, caracteristicas=@caracteristicas, media=@media ";

							if (juegoEditar.IdSteam > 0)
							{
								sqlEditar = sqlEditar + "WHERE idSteam=@idSteam";
							}
							else
							{
								if (juegoEditar.IdGog > 0)
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
								comando.Parameters.AddWithValue("@idSteam", juegoEditar.IdSteam);
								comando.Parameters.AddWithValue("@idGog", juegoEditar.IdGog);
								comando.Parameters.AddWithValue("@nombre", juegoEditar.Nombre);
								comando.Parameters.AddWithValue("@tipo", juegoEditar.Tipo);
								comando.Parameters.AddWithValue("@fechaSteamAPIComprobacion", juegoEditar.FechaSteamAPIComprobacion);
								comando.Parameters.AddWithValue("@imagenes", JsonConvert.SerializeObject(juegoEditar.Imagenes));
								comando.Parameters.AddWithValue("@precioMinimoActual", JsonConvert.SerializeObject(juegoEditar.PrecioMinimoActual));
								comando.Parameters.AddWithValue("@precioMinimoHistorico", JsonConvert.SerializeObject(juegoEditar.PrecioMinimoHistorico));
								comando.Parameters.AddWithValue("@precioActualesTiendas", JsonConvert.SerializeObject(juegoEditar.PrecioActualesTiendas));
								comando.Parameters.AddWithValue("@analisis", JsonConvert.SerializeObject(juegoEditar.Analisis));
								comando.Parameters.AddWithValue("@caracteristicas", JsonConvert.SerializeObject(juegoEditar.Caracteristicas));
								comando.Parameters.AddWithValue("@media", JsonConvert.SerializeObject(juegoEditar.Media));

								comando.ExecuteNonQuery();
							}

							
						}
					}
					catch (Exception ex)
					{
						errorMensaje = ex.Message;
						return null;
					}
				}
			}
			else
			{
				errorMensaje = "error id";
				return null;
			}












			//juegoEditar.Id = int.Parse(Request.Form["id"]);
			//juegoEditar.Nombre = Request.Form["nombre"];
			//juego.Imagen = Request.Form["imagen"];
			//juego.Drm = Request.Form["drm"];
			//juego.Enlace = Request.Form["enlace"];

			//if (juego.Id == 0 || juego.Nombre == string.Empty || juego.Imagen == string.Empty || juego.Drm == string.Empty || juego.Enlace == string.Empty)
			//{
			//	errorMensaje = "error";
			//	return;
			//}

			//try
			//{
			//	WebApplicationBuilder builder = WebApplication.CreateBuilder();
			//	string conexionTexto = builder.Configuration.GetConnectionString("pepeizqs_deals_webContextConnection");

			//	using (SqlConnection conexion = new SqlConnection(conexionTexto))
			//	{
			//		conexion.Open();

			//		string sqlActualizar = "UPDATE juegos " +
			//			"SET nombre=@nombre, imagen=@imagen, drm=@drm, enlace=@enlace " +
			//			"WHERE id=@id";

			//		using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
			//		{
			//			comando.Parameters.AddWithValue("@id", juegoEditar.Id);
			//			comando.Parameters.AddWithValue("@nombre", juegoEditar.Nombre);
			//			//comando.Parameters.AddWithValue("@imagen", juego.Imagen);
			//			//comando.Parameters.AddWithValue("@drm", juego.Drm);
			//			//comando.Parameters.AddWithValue("@enlace", juego.Enlace);

			//			comando.ExecuteNonQuery();
			//		}
			//	}
			//}
			//catch (Exception ex)
			//{
			//	errorMensaje = ex.Message;
			//	return;
			//}

			//juegoEditar.Id = 0;
			//juegoEditar.Nombre = string.Empty;
			////juego.Imagen = string.Empty;
			////juego.Drm = string.Empty;
			////juego.Enlace = string.Empty;

			//exitoMensaje = "exito";

			//Response.Redirect("/Juegos/Index");

			return null;
		}
    }
}
