#nullable disable

using Herramientas;
using Juegos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text;

namespace pepeizqs_deals_web.Pages.Admin.Juegos
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
					string seleccionarJuego = sqlBuscar;

					using (SqlCommand comando = new SqlCommand(seleccionarJuego, conexion))
					{
						comando.Parameters.AddWithValue(idParametro, idBuscar);

						using (SqlDataReader lector = comando.ExecuteReader())
						{
							if (lector.Read())
							{
								juegoEditar = BaseDatos.Juegos.Cargar.Ejecutar(juegoEditar, lector);
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

					Encoding Utf8 = Encoding.UTF8;
					byte[] utf8Bytes = Utf8.GetBytes(Request.Form["nombre"]);
					string nombreJuego = Utf8.GetString(utf8Bytes);

					juegoEditar.Nombre = nombreJuego;
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

					juegoEditar.PrecioMinimosHistoricos = juegoEditar.PrecioActualesTiendas;

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

					i = 0;
					while (i < 20)
					{
						if (Request.Form["miniatura_" + i.ToString()] != string.Empty)
						{
							string miniatura = Request.Form["miniatura_" + i.ToString()];

							if (media.Miniaturas == null)
							{
								media.Miniaturas = new List<string>();
							}

							if (miniatura != null)
							{
								media.Miniaturas.Add(miniatura);
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

					SqlConnection conexion = Herramientas.BaseDatos.Conectar();

					using (conexion)
					{
						conexion.Open();

						BaseDatos.Juegos.Actualizar.Ejecutar(juegoEditar, conexion);
					}

					conexion.Dispose();				
				}
			}
			else
			{
				errorMensaje = "error id";
				return null;
			}

			return null;
		}
    }
}
