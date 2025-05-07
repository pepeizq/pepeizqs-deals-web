//https://store.steampowered.com/search/results/?query&start=350&count=50&dynamic_data=&sort_by=Price_ASC&force_infinite=1&supportedlang=english&specials=1&hidef2p=1&ndl=1&infinite=1&ignore_preferences=1
//https://api.steampowered.com/IStoreQueryService/Query/v1/?input_json={%22query%22:{%22filters%22:{%22tagids_must_match%22:[{%22tagids%22:[%229%22]}]}},%22context%22:{%22language%22:%22english%22,%22country_code%22:%22US%22,%22steam_realm%22:%221%22},%22data_request%22:{%22include_basic_info%22:true}}
//https://store.steampowered.com/saleaction/ajaxgetdeckappcompatibilityreport?nAppID=1868140&l=spanish&cc=ES
//https://api.steampowered.com/ISteamUserStats/GetNumberOfCurrentPlayers/v1/?appid=730
//https://api.steampowered.com/IStoreService/GetAppList/v1/?key=[devkey]&max_results=50000
//https://store.steampowered.com/appreviews/730?json=1
//https://store.steampowered.com/curator/185907/ajaxgetcreatorhomeinfo?get_appids=true
//https://steamcommunity.com/gid/103582791462511166/ajaxgetvanityandclanid/
//https://store.steampowered.com/actions/ajaxresolvebundles?bundleids=45867&cc=ES&l=english

#nullable disable

using Herramientas;
using Juegos;
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace APIs.Steam
{
	public static class Tienda
	{
        public static Tiendas2.Tienda Generar()
		{
			Tiendas2.Tienda tienda = new Tiendas2.Tienda
			{
				Id = "steam",
				Nombre = "Steam",
				ImagenLogo = "/imagenes/tiendas/steam_logo.webp",
				Imagen300x80 = "/imagenes/tiendas/steam_300x80.webp",
				ImagenIcono = "/imagenes/tiendas/steam_icono.webp",
				Color = "#2e4460",
                AdminEnseñar = true,
				AdminInteractuar = true
			};

			return tienda;
		}

		public static Tiendas2.Tienda GenerarBundles()
		{
			Tiendas2.Tienda tienda = new Tiendas2.Tienda
			{
				Id = "steambundles",
				Nombre = "Steam (Bundles)",
				ImagenLogo = "/imagenes/tiendas/steam_logo.webp",
				Imagen300x80 = "/imagenes/tiendas/steam_300x80.webp",
				ImagenIcono = "/imagenes/tiendas/steam_icono.webp",
				Color = "#2e4460",
				AdminEnseñar = false,
				AdminInteractuar = true
			};

			return tienda;
		}

		public static string Referido(string enlace)
        {
            return enlace + "?curator_clanid=33500256";
        }

		public static async Task BuscarOfertas(SqlConnection conexion, IDecompiladores decompilador, bool mirarOfertas)
		{
			BaseDatos.Admin.Actualizar.Tiendas(Generar().Id, DateTime.Now, 0, conexion);

			string añadirBundles = string.Empty;

			if (DateTime.Now.Hour > 0 && DateTime.Now.Hour < 19)
			{
				añadirBundles = "%2C996";
			}

			int juegos = 0;

			int arranque = BaseDatos.Admin.Buscar.TiendasValorAdicional(Generar().Id, "valorAdicional", conexion);
			int tope = BaseDatos.Admin.Buscar.TiendasValorAdicional(Generar().Id, "valorAdicional2", conexion);

			if (DateTime.Now.Hour == 19 && DateTime.Now.Minute >= 0 && DateTime.Now.Minute < 10)
			{
				arranque = 0;
			}

			if (arranque >= tope - 50)
			{
				arranque = 0;
			}

			int i = arranque;
            while (i < tope)
            {
				string html = null;

				if (mirarOfertas == true)
				{
					string html2 = await Decompiladores.Estandar("https://store.steampowered.com/search/results/?query&start=" + i.ToString() + "&count=50&dynamic_data=&force_infinite=1&supportedlang=english&specials=1&hidef2p=1&ndl=1&infinite=1&ignore_preferences=1&l=english&category1=998%2C21%2C990%2C994" + añadirBundles);

					try
					{
						SteamQueryAPI datos = JsonSerializer.Deserialize<SteamQueryAPI>(html2);

						if (datos != null)
						{
							html = datos.Html;
						}

						tope = datos.Total;
					}
					catch { }

					if (html == null)
					{
						html = html2;
					}

					if (html2.Contains("total_count") == true)
					{
						int int1 = html2.IndexOf("total_count");

						if (int1 != -1)
						{
							string temp1 = html2.Remove(0, int1);

							int int2 = temp1.IndexOf(":");
							string temp2 = temp1.Remove(0, int2 + 1);

							int int3 = temp2.IndexOf(",");
							string temp3 = temp2.Remove(int3, temp2.Length - int3);

							tope = int.Parse(temp3.Trim());

							if (arranque > tope)
							{
								i = 0;
							}
						}
					}

					if (tope < 1000)
					{
						tope = 10000;
					}
				}
				else
				{
					string html2 = await Decompiladores.Estandar("https://store.steampowered.com/search/results/?query&start=" + i.ToString() + "&count=50&dynamic_data=&force_infinite=1&supportedlang=english&hidef2p=1&ndl=1&infinite=1&l=english");

					try
					{
						SteamQueryAPI datos = JsonSerializer.Deserialize<SteamQueryAPI>(html2);

						if (datos != null)
						{
							html = datos.Html;
						}
					}
					catch { }

					if (html == null)
					{
						html = html2;
					}
				}

				BaseDatos.Admin.Actualizar.TiendasValorAdicional(Generar().Id, "valorAdicional2", tope, conexion);

				if (string.IsNullOrEmpty(html) == false)
				{
					try
					{
						if (html.Contains("<!-- List Items -->") == true)
						{
							int int1 = html.IndexOf("<!-- List Items -->");
							html = html.Remove(0, int1);

							int int2 = html.IndexOf("<!-- End List Items -->");
							html = html.Remove(int2, html.Length - int2);

							int j = 0;
							while (j < 50)
							{
								if (html.Contains("<a href=" + Strings.ChrW(34) + "https://store.steampowered.com/") == true)
								{
									int int3 = html.IndexOf("<a href=" + Strings.ChrW(34) + "https://store.steampowered.com/");
									string temp3 = html.Remove(0, int3 + 5);

									html = temp3;

									int int4 = temp3.IndexOf("</a>");
									string temp4 = temp3.Remove(int4, temp3.Length - int4);

									int int5 = temp4.IndexOf("<span class=" + Strings.ChrW(34) + "title" + Strings.ChrW(34) + ">");
									string temp5 = temp4.Remove(0, int5);

									int int6 = temp5.IndexOf("</span>");
									string temp6 = temp5.Remove(int6, temp5.Length - int6);

									int5 = temp6.IndexOf(">");
									temp6 = temp6.Remove(0, int5 + 1);

									string titulo = temp6.Trim();
									titulo = WebUtility.HtmlDecode(titulo);

									int int7 = temp4.IndexOf("https://");
									string temp7 = temp4.Remove(0, int7);

									int int8 = temp7.IndexOf("?");
									string temp8 = temp7.Remove(int8, temp7.Length - int8);

									string enlace = temp8.Trim();

									if (enlace.Contains("https://store.steampowered.com/app/") == true)
									{
										enlace = "https://store.steampowered.com/app/" + Juego.LimpiarID(enlace);
									}
									else if (enlace.Contains("https://store.steampowered.com/bundle/") == true)
									{
										enlace = "https://store.steampowered.com/bundle/" + Juego.LimpiarID(enlace);
									}

									int int9 = temp4.IndexOf("<img src=");
									string temp9 = temp4.Remove(0, int9 + 10);

									int int10 = temp9.IndexOf("?");
									string temp10 = temp9.Remove(int10, temp9.Length - int10);

									string imagen = temp10.Trim();

									JuegoAnalisis analisis = new JuegoAnalisis
									{
										Cantidad = "0",
										Porcentaje = "0"
									};

									if (enlace.Contains("https://store.steampowered.com/app/") == true || enlace.Contains("https://store.steampowered.com/bundle/") == true)
									{
										int int11 = temp4.IndexOf("data-tooltip-html=");

										if (int11 != -1)
										{
											string temp11 = temp4.Remove(0, int11);

											int int12 = temp11.IndexOf("%");
											string temp12 = temp11.Remove(int12, temp11.Length - int12);

											temp12 = temp12.Remove(0, temp12.Length - 2);
											temp12 = temp12.Trim();

											if (temp12.Contains(";") == true)
											{
												temp12 = temp12.Replace(";", "0");
											}

											if (temp12 == "00")
											{
												temp12 = "100";
											}

											string porcentaje = temp12;

											int int13 = temp4.IndexOf("data-tooltip-html=");
											string temp13 = temp4.Remove(0, int13);

											int int14 = temp13.IndexOf("user reviews");
											string temp14 = temp13.Remove(int14, temp13.Length - int14);

											int14 = temp14.IndexOf("of the");
											temp14 = temp14.Remove(0, int14 + 6);

											string cantidad = temp14.Trim();

											if (cantidad.Length > 1)
											{
												analisis.Cantidad = cantidad;
												analisis.Porcentaje = porcentaje;
											}
										}
									}

									bool suficientesReseñas = false;

									if (enlace.Contains("https://store.steampowered.com/app/") == true)
									{
										if (analisis.Cantidad.Length > 1)
										{
											suficientesReseñas = true;
										}
									}
									else if (enlace.Contains("https://store.steampowered.com/bundle/") == true)
									{
										if (analisis.Cantidad.Length > 3)
										{
											suficientesReseñas = true;
										}
									}

									if (suficientesReseñas == true)
									{
										int int11 = temp4.IndexOf("data-discount=" + Strings.ChrW(34));

										if (int11 != -1)
										{
											string temp11 = temp4.Remove(0, int11);

											int11 = temp11.IndexOf(Strings.ChrW(34));
											temp11 = temp11.Remove(0, int11 + 1);

											int int12 = temp11.IndexOf(Strings.ChrW(34));
											string temp12 = temp11.Remove(int12, temp11.Length - int12);

											int descuento = 0;

											if (int12 != -1)
											{
												temp12 = temp12.Replace("-", null);
												temp12 = temp12.Replace("%", null);

												descuento = int.Parse(temp12.Trim());
											}

											if (descuento >= 0)
											{
												int int13 = temp4.IndexOf(Strings.ChrW(34) + "discount_final_price" + Strings.ChrW(34));
												string temp13 = temp4.Remove(0, int13);

												int13 = temp13.IndexOf(Strings.ChrW(34) + ">");
												temp13 = temp13.Remove(0, int13 + 2);

												int int14 = temp13.IndexOf("</div>");
												string temp14 = temp13.Remove(int14, temp13.Length - int14);

												if (temp14 != null)
												{
													temp14 = temp14.Replace("--", "00");
													temp14 = temp14.Replace(",", ".");
													temp14 = temp14.Replace("€", null);
													temp14 = temp14.Replace(" ", null);
												}

												bool precioFormato = true;

												if (temp14.Contains("Free") == true)
												{
													precioFormato = false;
												}
												else if (temp14.Length == 0)
												{
													precioFormato = false;
												}

												if (precioFormato == true)
												{
													decimal precio = decimal.Parse(temp14.Trim());

													JuegoPrecio oferta = new JuegoPrecio
													{
														Nombre = titulo,
														Imagen = imagen,
														Tienda = Generar().Id,
														DRM = JuegoDRM.Steam,
														Descuento = descuento,
														Precio = precio,
														Moneda = JuegoMoneda.Euro,
														Enlace = enlace,
														FechaDetectado = DateTime.Now,
														FechaActualizacion = DateTime.Now
													};

													if (enlace.Contains("https://store.steampowered.com/app/") == true)
													{
														try
														{
															BaseDatos.Tiendas.Comprobar.Steam(oferta, analisis, conexion);
														}
														catch (Exception ex)
														{
															BaseDatos.Errores.Insertar.Mensaje(Generar().Id, ex, conexion);
														}
													}
													
													if (enlace.Contains("https://store.steampowered.com/bundle/") == true)
													{
														oferta.Tienda = GenerarBundles().Id;

														try
														{
															BaseDatos.Tiendas.Comprobar.Resto(oferta, conexion);
														}
														catch (Exception ex)
														{
															BaseDatos.Errores.Insertar.Mensaje(GenerarBundles().Id, ex, conexion);
														}
													}

													juegos += 1;

													try
													{
														BaseDatos.Admin.Actualizar.Tiendas(Generar().Id, DateTime.Now, juegos, conexion);
													}
													catch (Exception ex)
													{
														BaseDatos.Errores.Insertar.Mensaje(Generar().Id, ex, conexion);
													}
												}
											}
										}
									}
								}

								j += 1;
							}
						}
					}
					catch (Exception ex)
					{
						BaseDatos.Errores.Insertar.Mensaje("Steam Ofertas", ex);
					}
				}

				i += 50;
				BaseDatos.Admin.Actualizar.TiendasValorAdicional(Generar().Id, "valorAdicional", i, conexion);
			}
		}

		public static string IdsEspeciales(string id)
		{
			#region Total War: SHOGUN 2 DLC es juego

			if (id == "201270")
			{
				id = "34330";
			}

			#endregion

			#region Call of Duty®: Modern Warfare® 3(2011) Config es juego

			if (id == "115300")
			{
				id = "42680";
			}

			#endregion

			#region The Elder Scrolls IV: Oblivion® Game of the Year Edition Deluxe

			if (id == "900883")
			{
				id = "22330";
			}

			#endregion

			return id;
		}
	}

	public class SteamQueryAPI
	{
		[JsonPropertyName("results_html")]
		public string Html { get; set; }

		[JsonPropertyName("total_count")]
		public int Total { get; set; }
	}
}
