#nullable disable

using Juegos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;

namespace pepeizqs_deals_web.Pages.Juegos
{
    public class AñadirModel : PageModel
    {
		public Juego juegoAñadir = JuegoCrear.Generar();

        public string errorMensaje = string.Empty;
        public string exitoMensaje = string.Empty;

        public void OnGet()
        {
			string plataforma = Request.Query["plataforma"];
			string id = Request.Query["id"];

            if (id != null)
            {
				if (id.Length > 0)
				{
					if (plataforma != null)
					{
						if (plataforma == "steam")
						{
							juegoAñadir = Steam.Juego.CargarDatos(id).Result;
						}
					}					
				}
            }

			
		}

        public IActionResult OnPost() 
        {
            if (Request.Form["precarga"] != string.Empty) 
            {
				if (Steam.Juego.Detectar(Request.Form["precarga"])  == true) 
				{
					string idPrecarga = Steam.Juego.LimpiarID(Request.Form["precarga"]);

					return RedirectToPage("./Añadir", new { id = idPrecarga, plataforma = "steam" });
				}
			}
            else 
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
								juegoAñadir.IdSteam = int.Parse(Request.Form["idSteam"]);
							}
						}

						if (Request.Form["idGog"] != string.Empty)
						{
							if (int.Parse(Request.Form["idGog"]) > 0)
							{
								juegoAñadir.IdGog = int.Parse(Request.Form["idGog"]);
							}
						}


						juegoAñadir.Nombre = Request.Form["nombre"];
						juegoAñadir.Tipo = Enum.Parse<JuegoTipo>(Request.Form["tipo"]);
						juegoAñadir.FechaSteamAPIComprobacion = DateTime.Parse(Request.Form["fechacomprobacion"]);

						//----------------------------

						int l = 0;
						while (l < 20)
						{
							if (Request.Form["precio_" + l.ToString()] != string.Empty)
							{
								string precioMirar = Request.Form["precio_" + l.ToString()];

								if (juegoAñadir.PrecioActualesTiendas == null)
								{
									juegoAñadir.PrecioActualesTiendas = new List<JuegoPrecio>();
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

									juegoAñadir.PrecioActualesTiendas.Add(precio);
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

						juegoAñadir.PrecioMinimoActual = juegoAñadir.PrecioActualesTiendas;
						juegoAñadir.PrecioMinimoHistorico = juegoAñadir.PrecioActualesTiendas;

						//----------------------------

						JuegoImagenes imagenes = new JuegoImagenes
						{
							Header_460x215 = Request.Form["imagenheader"],
							Capsule_231x87 = Request.Form["imagencapsule"],
							Logo = Request.Form["imagenlogo"],
							Library_600x900 = Request.Form["imagenlibrary1"],
							Library_1920x620 = Request.Form["imagenlibrary2"]
						};

						juegoAñadir.Imagenes = imagenes;

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

						juegoAñadir.Caracteristicas = caracteristicas;

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
							juegoAñadir.Media = media;
						}

						//----------------------------

						try
						{
							WebApplicationBuilder builder = WebApplication.CreateBuilder();
							string conexionTexto = builder.Configuration.GetConnectionString("pepeizqs_deals_webContextConnection");

							using (SqlConnection conexion = new SqlConnection(conexionTexto))
							{
								conexion.Open();

								string sqlAñadir = "INSERT INTO juegos " +
									"(idSteam, idGog, nombre, tipo, fechaSteamAPIComprobacion, imagenes, precioMinimoActual, precioMinimoHistorico, precioActualesTiendas, analisis, caracteristicas, media) VALUES " +
									"(@idSteam, @idGog, @nombre, @tipo, @fechaSteamAPIComprobacion, @imagenes, @precioMinimoActual, @precioMinimoHistorico, @precioActualesTiendas, @analisis, @caracteristicas, @media) ";

								using (SqlCommand comando = new SqlCommand(sqlAñadir, conexion))
								{
									comando.Parameters.AddWithValue("@idSteam", juegoAñadir.IdSteam);
									comando.Parameters.AddWithValue("@idGog", juegoAñadir.IdGog);
									comando.Parameters.AddWithValue("@nombre", juegoAñadir.Nombre);
									comando.Parameters.AddWithValue("@tipo", juegoAñadir.Tipo);
									comando.Parameters.AddWithValue("@fechaSteamAPIComprobacion", juegoAñadir.FechaSteamAPIComprobacion);
									comando.Parameters.AddWithValue("@imagenes", JsonConvert.SerializeObject(juegoAñadir.Imagenes));
									comando.Parameters.AddWithValue("@precioMinimoActual", JsonConvert.SerializeObject(juegoAñadir.PrecioMinimoActual));
									comando.Parameters.AddWithValue("@precioMinimoHistorico", JsonConvert.SerializeObject(juegoAñadir.PrecioMinimoHistorico));
									comando.Parameters.AddWithValue("@precioActualesTiendas", JsonConvert.SerializeObject(juegoAñadir.PrecioActualesTiendas));
									comando.Parameters.AddWithValue("@analisis", JsonConvert.SerializeObject(juegoAñadir.Analisis));
									comando.Parameters.AddWithValue("@caracteristicas", JsonConvert.SerializeObject(juegoAñadir.Caracteristicas));
									comando.Parameters.AddWithValue("@media", JsonConvert.SerializeObject(juegoAñadir.Media));

									comando.ExecuteNonQuery();
								}

								if (juegoAñadir.IdSteam > 0)
								{
									return RedirectToPage("./Editar", new { idSteam = juegoAñadir.IdSteam });
								}
								else
								{
									if (juegoAñadir.IdGog > 0)
									{
										return RedirectToPage("./Editar", new { idGog = juegoAñadir.IdGog });
									}
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
			}

			return null;
		}
    }
}
