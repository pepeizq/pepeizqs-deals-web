#nullable disable

using Herramientas;
using Juegos;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace APIs.EpicGames
{
    public static class Juego
    {
        public static async Task<Juegos.Juego> CargarDatos(string enlace)
        {
            string html = await Decompiladores.Estandar("https://store-content-ipv4.ak.epicgames.com/api/en-US/content/products/" + enlace);

            if (html != null)
            {
                EpicJuegoAPI datos = JsonSerializer.Deserialize<EpicJuegoAPI>(html);

                if (datos != null) 
                {
                    Juegos.Juego juego = JuegoCrear.Generar();

                    juego.Nombre = datos.Nombre;

                    return juego;
                }
            }

            return null;
        }

        public static bool Detectar(string enlace)
        {
            bool resultado = false;

            if (enlace.Contains("https://store.epicgames.com/es-ES/p/") == true)
            {
                resultado = true;
            }
            else if (enlace.Contains("https://store.epicgames.com/p/") == true)
            {
                resultado = true;
            }

            return resultado;
        }

        public static string LimpiarSlug(string enlace)
        {
            enlace = enlace.Replace("https://store.epicgames.com/es-ES/p/", null);
            enlace = enlace.Replace("https://store.epicgames.com/p/", null);

            if (enlace.Contains("/") == true)
            {
                int int1 = enlace.IndexOf("/");
                enlace = enlace.Remove(int1, enlace.Length - int1);
            }

            if (enlace.Contains("?") == true)
            {
                int int1 = enlace.IndexOf("?");
                enlace = enlace.Remove(int1, enlace.Length - int1);
            }

            string id = enlace.Trim();

            return id;
        }

        public static async Task<JuegoEpicGames> EpicGamesDatos(string slug)
        {
            if (string.IsNullOrEmpty(slug) == false)
            {
                string html = await Decompiladores.Estandar("https://store.epicgames.com/graphql?operationName=getMappingByPageSlug&variables={%22pageSlug%22:%22" + slug + "%22,%22locale%22:%22es-ES%22}&extensions={%22persistedQuery%22:{%22version%22:1,%22sha256Hash%22:%22781fd69ec8116125fa8dc245c0838198cdf5283e31647d08dfa27f45ee8b1f30%22}}");
           
                if (string.IsNullOrEmpty(html) == false)
                {
					EpicGamesAPIIds ids = JsonSerializer.Deserialize<EpicGamesAPIIds>(html);

                    if (ids != null)
                    {
                        if (string.IsNullOrEmpty(ids.Datos.Mapping.Mapping2.SandboxId) == false)
                        {
							JuegoEpicGames juegoEpicGames = new JuegoEpicGames();

							string productId = ids.Datos.Mapping.Mapping2.ProductId;
							string sandboxId = ids.Datos.Mapping.Mapping2.SandboxId;

							string html1 = await Decompiladores.Estandar("https://egs-platform-service.store.epicgames.com/api/v1/egs/products/" + productId + "?country=ES&locale=es-ES&store=EGS");

							if (string.IsNullOrEmpty(html1) == false)
							{
								EpicGamesAPIDatos1 datos = JsonSerializer.Deserialize<EpicGamesAPIDatos1>(html1);

								if (datos != null)
								{
									foreach (var plataforma in datos.Etiquetas.Plataformas)
									{
										if (plataforma.Id == "9547")
										{
											juegoEpicGames.Windows = true;
										}

										if (plataforma.Id == "10719")
										{
											juegoEpicGames.Mac = true;
										}
									}

									foreach (var cosa in datos.Etiquetas.EpicCosas)
									{
										if (cosa.Id == "19847")
										{
											juegoEpicGames.Logros = true;
										}

										if (cosa.Id == "21894")
										{
											juegoEpicGames.GuardadoNube = true;
										}
									}
								}
							}

							string html2 = await Decompiladores.Estandar("https://store.epicgames.com/graphql?operationName=getStoreConfig&variables={%22locale%22:%22en-US%22,%22sandboxId%22:%22" + sandboxId + "%22}&extensions={%22persistedQuery%22:{%22version%22:1,%22sha256Hash%22:%220247771a057e44ee16627574296ad79fd48e41b4cb056465515a54ade05aa7f2%22}}");

                            if (string.IsNullOrEmpty(html2) == false)
                            {
                                EpicGamesAPIDatos datos = JsonSerializer.Deserialize<EpicGamesAPIDatos>(html2);

                                if (datos != null)
                                {
                                    if (datos.Datos.Producto.Configuracion.Configs.Count == 1)
                                    {
										foreach (var config in datos.Datos.Producto.Configuracion.Configs)
										{
											if (config != null)
											{
												if (config.Juego != null)
												{
													foreach (var etiqueta in config.Juego.Etiquetas)
													{
														if (etiqueta.Id == "9547")
														{
															juegoEpicGames.Windows = true;
														}

														if (etiqueta.Id == "10719")
														{
															juegoEpicGames.Mac = true;
														}

														if (etiqueta.Id == "21894")
														{
															juegoEpicGames.GuardadoNube = true;
														}
													}
												}	
											}
										}
									}
                                }
                            }

							string html3 = await Decompiladores.Estandar("https://store.epicgames.com/graphql?operationName=Achievement&variables=%7B%22sandboxId%22:%22" + sandboxId + "%22,%22locale%22:%22es-ES%22%7D&extensions=%7B%22persistedQuery%22:%7B%22version%22:1,%22sha256Hash%22:%229284d2fe200e351d1496feda728db23bb52bfd379b236fc3ceca746c1f1b33f2%22%7D%7D");

							if (string.IsNullOrEmpty(html3) == false)
							{
								EpicGamesAPILogros logros = JsonSerializer.Deserialize<EpicGamesAPILogros>(html3);

								if (logros != null)
								{
									if (logros.Datos.Producto.Juego.Cantidad != null)
									{
										if (logros.Datos.Producto.Juego.Cantidad > 0)
										{
											juegoEpicGames.Logros = true;
										}
									}
								}
							}

							juegoEpicGames.Fecha = DateTime.Now;

							return juegoEpicGames;
						}
                    }
				}
            }

            return null;
        }

		public static async Task<List<JuegoIdioma>> EpicGamesIdiomas(string slug, List<JuegoIdioma> listadoIdiomas)
		{
			if (string.IsNullOrEmpty(slug) == false)
			{
				string html = await Decompiladores.Estandar("https://store.epicgames.com/graphql?operationName=getMappingByPageSlug&variables={%22pageSlug%22:%22" + slug + "%22,%22locale%22:%22es-ES%22}&extensions={%22persistedQuery%22:{%22version%22:1,%22sha256Hash%22:%22781fd69ec8116125fa8dc245c0838198cdf5283e31647d08dfa27f45ee8b1f30%22}}");

				if (string.IsNullOrEmpty(html) == false)
				{
					EpicGamesAPIIds ids = JsonSerializer.Deserialize<EpicGamesAPIIds>(html);

					if (ids != null)
					{
						if (string.IsNullOrEmpty(ids.Datos.Mapping.Mapping2.SandboxId) == false)
						{
							string sandboxId = ids.Datos.Mapping.Mapping2.SandboxId;

							string html2 = await Decompiladores.Estandar("https://store.epicgames.com/graphql?operationName=getStoreConfig&variables={%22locale%22:%22en-US%22,%22sandboxId%22:%22" + sandboxId + "%22}&extensions={%22persistedQuery%22:{%22version%22:1,%22sha256Hash%22:%220247771a057e44ee16627574296ad79fd48e41b4cb056465515a54ade05aa7f2%22}}");

							if (string.IsNullOrEmpty(html2) == false)
							{
								EpicGamesAPIDatos datos = JsonSerializer.Deserialize<EpicGamesAPIDatos>(html2);

								if (datos != null)
								{
									if (datos.Datos.Producto.Configuracion.Configs.Count > 0)
									{
										foreach (var configs in datos.Datos.Producto.Configuracion.Configs)
										{ 
											if (configs != null)
											{
												if (configs.Juego != null)
												{
													List<JuegoIdioma> idiomas = Herramientas.Idiomas.EpicGamesSacarIdiomas(configs.Juego.IdiomasAudios, configs.Juego.IdiomasTexto);

													if (idiomas.Count > 0)
													{
														if (listadoIdiomas == null)
														{
															listadoIdiomas = idiomas;
														}
														else
														{
															List<JuegoIdioma> listadoActualizar = listadoIdiomas;

															foreach (var nuevoIdioma in idiomas)
															{
																bool existe = false;

																foreach (var viejoIdioma in listadoActualizar)
																{
																	if (viejoIdioma.DRM == nuevoIdioma.DRM && nuevoIdioma.Idioma == viejoIdioma.Idioma)
																	{
																		existe = true;

																		viejoIdioma.Audio = nuevoIdioma.Audio;
																		viejoIdioma.Texto = nuevoIdioma.Texto;

																		break;
																	}
																}

																if (existe == false)
																{
																	listadoActualizar.Add(nuevoIdioma);
																}
															}

															return listadoActualizar;
														}
													}
												}
											}
										}
									}
								}
							}

							string html3 = await Decompiladores.Estandar("https://store-content-ipv4.ak.epicgames.com/api/en-US/content/products/" + slug);

							if (string.IsNullOrEmpty(html3) == false)
							{
								EpicGamesAPIDatos3 datos = JsonSerializer.Deserialize<EpicGamesAPIDatos3>(html3);

								if (datos != null)
								{
									if (datos.Paginas != null)
									{
										if (datos.Paginas.Count > 0)
										{
											if (datos.Paginas[0].Datos.Requisitos.Idiomas.Count == 2)
											{
												List<JuegoIdioma> idiomas = Herramientas.Idiomas.EpicGamesSacarIdiomas(datos.Paginas[0].Datos.Requisitos.Idiomas[0], datos.Paginas[0].Datos.Requisitos.Idiomas[1]);

												if (listadoIdiomas == null)
												{
													listadoIdiomas = idiomas;
												}
												else
												{
													List<JuegoIdioma> listadoActualizar = listadoIdiomas;

													foreach (var nuevoIdioma in idiomas)
													{
														bool existe = false;

														foreach (var viejoIdioma in listadoActualizar)
														{
															if (viejoIdioma.DRM == nuevoIdioma.DRM && nuevoIdioma.Idioma == viejoIdioma.Idioma)
															{
																existe = true;

																viejoIdioma.Audio = nuevoIdioma.Audio;
																viejoIdioma.Texto = nuevoIdioma.Texto;

																break;
															}
														}

														if (existe == false)
														{
															listadoActualizar.Add(nuevoIdioma);
														}
													}

													return listadoActualizar;
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}

			return listadoIdiomas;
		}
	}

    #region API Vieja

    public class EpicJuegoAPI
    {
        [JsonPropertyName("productName")]
        public string Nombre { get; set; }
    }

	#endregion

	#region API Ids

	public class EpicGamesAPIIds
    {
		[JsonPropertyName("data")]
		public EpicGamesAPIIdsMapping Datos { get; set; }
	}

	public class EpicGamesAPIIdsMapping
	{
		[JsonPropertyName("StorePageMapping")]
		public EpicGamesAPIIdsMapping2 Mapping { get; set; }
	}

	public class EpicGamesAPIIdsMapping2
	{
		[JsonPropertyName("mapping")]
		public EpicGamesAPIIdsDatos Mapping2 { get; set; }
	}

	public class EpicGamesAPIIdsDatos
	{
		[JsonPropertyName("sandboxId")]
		public string SandboxId { get; set; }

		[JsonPropertyName("productId")]
		public string ProductId { get; set; }
	}

	#endregion

	#region API Datos (1ra Opcion)

	public class EpicGamesAPIDatos1
	{
		[JsonPropertyName("tags")]
		public EpicGamesAPIDatos1Etiquetas Etiquetas { get; set; }
	}

	public class EpicGamesAPIDatos1Etiquetas
	{
		[JsonPropertyName("epicFeatures")]
		public List<EpicGamesAPIDatos1Etiqueta> EpicCosas { get; set; }

		[JsonPropertyName("platforms")]
		public List<EpicGamesAPIDatos1Etiqueta> Plataformas { get; set; }
	}

	public class EpicGamesAPIDatos1Etiqueta
	{
		[JsonPropertyName("id")]
		public string Id { get; set; }
	}

	#endregion

	#region API Datos (2da Opcion)

	public class EpicGamesAPIDatos
	{
		[JsonPropertyName("data")]
		public EpicGamesAPIDatosProducto Datos { get; set; }
	}

	public class EpicGamesAPIDatosProducto
	{
		[JsonPropertyName("Product")]
		public EpicGamesAPIDatosSandbox Producto { get; set; }
	}

	public class EpicGamesAPIDatosSandbox
	{
		[JsonPropertyName("sandbox")]
		public EpicGamesAPIDatosConfiguration Configuracion { get; set; }
	}

	public class EpicGamesAPIDatosConfiguration
	{
		[JsonPropertyName("configuration")]
		public List<EpicGamesAPIDatosConfigs> Configs { get; set; }
	}

	public class EpicGamesAPIDatosConfigs
	{
		[JsonPropertyName("configs")]
		public EpicGamesAPIDatosJuego Juego { get; set; }
	}

	public class EpicGamesAPIDatosJuego
	{
		[JsonPropertyName("supportedAudio")]
		public List<string> IdiomasAudios { get; set; }

		[JsonPropertyName("supportedText")]
		public List<string> IdiomasTexto { get; set; }

		[JsonPropertyName("tags")]
		public List<EpicGamesAPIDatosJuegoEtiqueta> Etiquetas { get; set; }
	}

	public class EpicGamesAPIDatosJuegoEtiqueta
    {
		[JsonPropertyName("id")]
		public string Id { get; set; }
	}

	#endregion

	#region API Datos (3ra Opcion)

	public class EpicGamesAPIDatos3
	{
		[JsonPropertyName("pages")]
		public List<EpicGamesAPIDatos3Pagina> Paginas { get; set; }
	}

	public class EpicGamesAPIDatos3Pagina
	{
		[JsonPropertyName("data")]
		public EpicGamesAPIDatos3PaginaDatos Datos { get; set; }
	}

	public class EpicGamesAPIDatos3PaginaDatos
	{
		[JsonPropertyName("requirements")]
		public EpicGamesAPIDatos3Requisitos Requisitos { get; set; }
	}

	public class EpicGamesAPIDatos3Requisitos
	{
		[JsonPropertyName("languages")]
		public List<string> Idiomas { get; set; }
	}

	#endregion

	#region API Logros

	public class EpicGamesAPILogros
	{
		[JsonPropertyName("data")]
		public EpicGamesAPILogrosDatos Datos { get; set; }
	}

	public class EpicGamesAPILogrosDatos
	{
		[JsonPropertyName("Achievement")]
		public EpicGamesAPILogrosProducto Producto { get; set; }
	}

	public class EpicGamesAPILogrosProducto
	{
		[JsonPropertyName("productAchievementsRecordBySandbox")]
		public EpicGamesAPILogrosJuego Juego { get; set; }
	}

	public class EpicGamesAPILogrosJuego
	{
		[JsonPropertyName("totalAchievements")]
		public int? Cantidad { get; set; }
	}

	#endregion
}