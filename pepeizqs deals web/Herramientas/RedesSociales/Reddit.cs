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

		public static void Postear(string enlace, int id, int descuento, string precio, string tipo, string tienda, string codigoDescuento = null, string codigoTexto = null)
		{
			WebApplicationBuilder builder = WebApplication.CreateBuilder();
			string cuenta = builder.Configuration.GetValue<string>("Reddit:Cuenta");
			string contraseña = builder.Configuration.GetValue<string>("Reddit:Contraseña");

			string subTexto = null;

			if (tipo == "steam")
			{
				subTexto = "/r/gamesdealssteam";
			}
			else if (tipo == "gog")
			{
				subTexto = "/r/gamesdealsgog";
			}

			if (string.IsNullOrEmpty(subTexto) == false)
			{
				RedditSharp.Reddit cliente = new RedditSharp.Reddit();
				cliente.LogIn(cuenta, contraseña);

				cliente.InitOrUpdateUser();

				if (cliente.User != null)
				{
					RedditSharp.Things.Subreddit sub = cliente.GetSubreddit(subTexto);

					string nombreTienda = string.Empty;
					foreach (var tienda2 in Tiendas2.TiendasCargar.GenerarListado())
					{
						if (tienda2.Id == tienda)
						{
							nombreTienda = tienda2.Nombre;
						}
					}

					Juego juego2 = global::BaseDatos.Juegos.Buscar.UnJuego(id);

					if (juego2 != null)
					{
						bool valido = true;

						if (tipo == "gog")
						{
							if (juego2.Tipo != JuegoTipo.Game)
							{
								valido = false;
							}
						}

						if (valido == true)
						{
							string titulo = "[" + nombreTienda + "] " + juego2.Nombre + " (" + Herramientas.Precios.Euro(decimal.Parse(precio)) + " / " + descuento + "% off)";

							RedditSharp.Things.Post post = sub.SubmitPost(titulo, enlace);

							string comentario = string.Empty;

							if (string.IsNullOrEmpty(codigoTexto) == false)
							{
								comentario = "Use code " + codigoTexto + " to obtain the price indicated in the title." + Environment.NewLine;
							}

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
										comentario = comentario + Environment.NewLine + juego2.Nombre + " is part of the following bundles:" + Environment.NewLine;

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
										comentario = comentario + Environment.NewLine + juego2.Nombre + " was part of the following bundles:" + Environment.NewLine;

										foreach (var bundle in bundlesViejunos)
										{
											Bundles2.Bundle bundle2 = global::BaseDatos.Bundles.Buscar.UnBundle(bundle);

											if (bundle2 != null)
											{
												comentario = comentario + "* [" + bundle2.NombreBundle + " • " + bundle2.NombreTienda + "](https://pepeizqdeals.com/bundle/" + bundle2.Id.ToString() + "/" + Herramientas.EnlaceAdaptador.Nombre(bundle2.NombreBundle) + "/) (" + Calculadora.DiferenciaTiempo(bundle2.FechaEmpieza, "en") + ")" + Environment.NewLine;
											}
										}
									}
								}
							}

							if (juego2.Gratis != null)
							{
								if (juego2.Gratis.Count > 0)
								{
									foreach (var gratis in juego2.Gratis)
									{
										if (gratis.FechaEmpieza <= DateTime.Now && gratis.FechaTermina >= DateTime.Now)
										{
											if (comentario.Contains(juego2.Nombre + " is currently free on:") == false)
											{
												comentario = comentario + Environment.NewLine + juego2.Nombre + " is currently free on:" + Environment.NewLine;
											}

											comentario = comentario + "* [" + Gratis2.GratisCargar.DevolverGratis(gratis.Tipo).Nombre + "](" + gratis.Enlace + ")" + Environment.NewLine;
										}
										else
										{
											if (comentario.Contains(juego2.Nombre + " was free on:") == false)
											{
												comentario = comentario + Environment.NewLine + juego2.Nombre + " was free on:" + Environment.NewLine;
											}

											comentario = comentario + "* " + Gratis2.GratisCargar.DevolverGratis(gratis.Tipo).Nombre + " (" + Calculadora.DiferenciaTiempo(gratis.FechaEmpieza, "en") + ")" + Environment.NewLine;
										}
									}
								}
							}

							if (juego2.Suscripciones != null)
							{
								if (juego2.Suscripciones.Count > 0)
								{
									foreach (var suscripcion in juego2.Suscripciones)
									{
										if (suscripcion.FechaEmpieza <= DateTime.Now && suscripcion.FechaTermina >= DateTime.Now)
										{
											if (comentario.Contains(juego2.Nombre + " is currently available on subscription services:") == false)
											{
												comentario = comentario + Environment.NewLine + juego2.Nombre + " is currently available on subscription services:" + Environment.NewLine;
											}

											comentario = comentario + "* [" + Suscripciones2.SuscripcionesCargar.DevolverSuscripcion(suscripcion.Tipo).Nombre + "](" + suscripcion.Enlace + ")" + Environment.NewLine;
										}
										else
										{
											if (comentario.Contains(juego2.Nombre + " was available on subscription services:") == false)
											{
												comentario = comentario + Environment.NewLine + juego2.Nombre + " was available on subscription services:" + Environment.NewLine;
											}

											comentario = comentario + "* " + Suscripciones2.SuscripcionesCargar.DevolverSuscripcion(suscripcion.Tipo).Nombre + " (" + Calculadora.DiferenciaTiempo(suscripcion.FechaEmpieza, "en") + ")" + Environment.NewLine;
										}
									}
								}
							}

							if (string.IsNullOrEmpty(comentario) == false)
							{
								comentario = comentario.Trim();

								post.Comment(comentario);
							}
						}
					}
				}
			}
		}
	}		
}
