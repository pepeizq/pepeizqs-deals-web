#nullable disable

using Herramientas;
using Juegos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;

namespace pepeizqs_deals_web.Pages.Admin.Juegos
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
							juegoAñadir = APIs.Steam.Juego.CargarDatos(id).Result;
						}
						else if (plataforma == "gog")
						{
							juegoAñadir = APIs.GOG.Juego.CargarDatos(id).Result;
						}
                        else if (plataforma == "epic")
                        {
                            juegoAñadir = APIs.EpicGames.Juego.CargarDatos(id).Result;
                        }
                    }					
				}
            }	
		}

        public IActionResult OnPost() 
        {
            if (Request.Form["precarga"] != string.Empty) 
            {
				if (APIs.Steam.Juego.Detectar(Request.Form["precarga"])  == true) 
				{
					string idPrecarga = APIs.Steam.Juego.LimpiarID(Request.Form["precarga"]);

					return RedirectToPage("./Añadir", new { id = idPrecarga, plataforma = "steam" });
				}
				else if (APIs.GOG.Juego.Detectar(Request.Form["precarga"]) == true)
				{
					string idPrecarga = APIs.GOG.Juego.LimpiarSlug(Request.Form["precarga"]);

					return RedirectToPage("./Añadir", new { id = idPrecarga, plataforma = "gog" });
				}
                else if (APIs.EpicGames.Juego.Detectar(Request.Form["precarga"]) == true)
                {
                    string idPrecarga = APIs.EpicGames.Juego.LimpiarSlug(Request.Form["precarga"]);

                    return RedirectToPage("./Añadir", new { id = idPrecarga, plataforma = "epic" });
                }
            }
            else 
            {
				if (string.IsNullOrEmpty(Request.Form["nombre"]) == false)
				{
					string nombre = Request.Form["nombre"];

					if (nombre.Length > 0)
					{
						if (string.IsNullOrEmpty(Request.Form["idSteam"]) == false)
						{
							if (int.Parse(Request.Form["idSteam"]) > 0)
							{
								juegoAñadir.IdSteam = int.Parse(Request.Form["idSteam"]);
							}
						}

						if (string.IsNullOrEmpty(Request.Form["idGog"]) == false)
						{
							if (int.Parse(Request.Form["idGog"]) > 0)
							{
								juegoAñadir.IdGog = int.Parse(Request.Form["idGog"]);
							}
						}

						juegoAñadir.Nombre = Request.Form["nombre"];
						juegoAñadir.Tipo = Enum.Parse<JuegoTipo>(Request.Form["tipo"]);
						juegoAñadir.FechaSteamAPIComprobacion = DateTime.Parse(Request.Form["fechacomprobacion"]);

						if (juegoAñadir.Tipo == JuegoTipo.DLC)
						{
							juegoAñadir.Maestro = Request.Form["maestro"];
						}
						else if (juegoAñadir.Tipo == JuegoTipo.Game)
						{
							juegoAñadir.FreeToPlay = Request.Form["f2p"];
						}

						//----------------------------

						int l = 0;
						while (l < 20)
						{
							if (string.IsNullOrEmpty(Request.Form["precio_" + l.ToString()]) == false)
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

						juegoAñadir.PrecioMinimosHistoricos = juegoAñadir.PrecioActualesTiendas;

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

                        if (string.IsNullOrEmpty(Request.Form["windows"]) == false)
						{
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
                                if (string.IsNullOrEmpty(Request.Form["desarrollador_" + j.ToString()]) == false)
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
                                if (string.IsNullOrEmpty(Request.Form["publisher_" + k.ToString()]) == false)
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
                        }
                            
						//----------------------------

						JuegoMedia media = new JuegoMedia();

						if (string.IsNullOrEmpty(Request.Form["video"]) == false)
						{
							media.Video = Request.Form["video"];
						}

						int i = 0;
						while (i < 20)
						{
							if (string.IsNullOrEmpty(Request.Form["captura_" + i.ToString()]) == false) 
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
							if (string.IsNullOrEmpty(Request.Form["miniatura_" + i.ToString()]) == false)
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

						if (media.Video != null || media.Capturas != null)
						{
							juegoAñadir.Media = media;
						}

						//----------------------------

						SqlConnection conexion = Herramientas.BaseDatos.Conectar();

						using (conexion)
						{
							BaseDatos.Juegos.Insertar.Ejecutar(juegoAñadir, conexion);
						}

						conexion.Dispose();

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
