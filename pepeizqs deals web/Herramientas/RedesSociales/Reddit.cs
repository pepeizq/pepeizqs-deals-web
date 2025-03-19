#nullable disable

using Juegos;

namespace Herramientas.RedesSociales
{
	public static class Reddit
	{
		public static void Postear(Noticias.Noticia noticia)
		{
			WebApplicationBuilder builder = WebApplication.CreateBuilder();
			string cuenta = builder.Configuration.GetValue<string>("Reddit:Cuenta");
			string contraseña = builder.Configuration.GetValue<string>("Reddit:Contraseña");

			RedditSharp.Reddit cliente = new RedditSharp.Reddit();
			cliente.LogIn(cuenta, contraseña);

			cliente.InitOrUpdateUser();

			if (cliente.User != null)
			{
				RedditSharp.Things.Subreddit sub = cliente.GetSubreddit("/r/pepeizqdeals");

				string enlace = string.Empty;

				if (string.IsNullOrEmpty(noticia.Enlace) == false)
				{
					enlace = noticia.Enlace;
				}
				else
				{
					if (noticia.Id == 0)
					{
						enlace = "/news/" + noticia.IdMaestra.ToString() + "/";
					}
					else
					{
						enlace = "/news/" + noticia.Id.ToString() + "/";
					}
				}

				if (string.IsNullOrEmpty(enlace) == false)
				{
					if (enlace.Contains("https://pepeizqdeals.com") == false)
					{
						enlace = "https://pepeizqdeals.com" + enlace;
					}
				}

				sub.SubmitPost(noticia.TituloEn, enlace);
			}
		}

		public static void Postear(int id, JuegoPrecio juego, JuegoAnalisis analisis)
		{
			if (juego != null)
			{
				if (juego.DRM == JuegoDRM.Steam)
				{
					if (analisis != null)
					{
						if (string.IsNullOrEmpty(analisis.Cantidad) == false)
						{
							if (analisis.Cantidad.Length > 3)
							{
								WebApplicationBuilder builder = WebApplication.CreateBuilder();
								string cuenta = builder.Configuration.GetValue<string>("Reddit:Cuenta");
								string contraseña = builder.Configuration.GetValue<string>("Reddit:Contraseña");

								RedditSharp.Reddit cliente = new RedditSharp.Reddit();
								cliente.LogIn(cuenta, contraseña);

								cliente.InitOrUpdateUser();

								if (cliente.User != null)
								{
									RedditSharp.Things.Subreddit sub = cliente.GetSubreddit("/r/gamesdealssteam");

									string nombreTienda = string.Empty;
									foreach (var tienda in Tiendas2.TiendasCargar.GenerarListado())
									{
										if (tienda.Id == juego.Tienda)
										{
											nombreTienda = tienda.Nombre;
										}
									}

									string titulo = "[" + nombreTienda + "] " + juego.Nombre + " (" + Herramientas.Precios.Euro(juego.Precio) + " / " + juego.Descuento + "% off)";

									RedditSharp.Things.Post post = sub.SubmitPost(titulo, juego.Enlace);

									string comentario = string.Empty;

									if (juego.CodigoDescuento > 0)
									{
										comentario = "Use code " + juego.CodigoTexto + " to obtain the price indicated in the title." + Environment.NewLine + Environment.NewLine;
									}

									Juego juego2 = global::BaseDatos.Juegos.Buscar.UnJuego(id);

									if (juego2 != null)
									{
										if (juego2.Bundles != null)
										{
											if (juego2.Bundles.Count > 0)
											{
												List<int> bundlesActivos = new List<int>();
												List<int> bundlesViejunos = new List<int>();

												foreach (var bundle in juego2.Bundles)
												{
													if (bundle.FechaEmpieza <= DateTime.Now && bundle.FechaTermina >= DateTime.Now)
													{
														bundlesActivos.Add(bundle.BundleId);
													}
													else
													{
														bundlesViejunos.Add(bundle.BundleId);
													}
												}
												
												if (bundlesActivos.Count > 0)
												{
													comentario = comentario + "This title is part of the following bundles:" + Environment.NewLine;

													foreach (var bundle in bundlesActivos)
													{
														Bundles2.Bundle bundle2 = global::BaseDatos.Bundles.Buscar.UnBundle(bundle);
														
														if (bundle2 != null)
														{
															comentario = comentario + "* [" + bundle2.NombreBundle + " • " + bundle2.NombreTienda + "](" + bundle2.Enlace + ")" + Environment.NewLine;
														}
													}
												}

												if (bundlesViejunos.Count > 0)
												{
													comentario = comentario + "This title was part of the following bundles:" + Environment.NewLine;

													foreach (var bundle in bundlesViejunos)
													{
														Bundles2.Bundle bundle2 = global::BaseDatos.Bundles.Buscar.UnBundle(bundle);

														if (bundle2 != null)
														{
															comentario = comentario + "* [" + bundle2.NombreBundle + " • " + bundle2.NombreTienda + "](https://pepeizqdeals.com/bundle/" + bundle2.Id.ToString() + "/" + Herramientas.EnlaceAdaptador.Nombre(bundle2.NombreBundle) + "/)" + Environment.NewLine;
														}
													}
												}
											}
										}
									}

									if (string.IsNullOrEmpty(comentario) == false)
									{
										RedditSharp.Things.Comment comentario2 = post.Comment(comentario);
										comentario2.Distinguish(RedditSharp.Things.VotableThing.DistinguishType.Moderator);
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
