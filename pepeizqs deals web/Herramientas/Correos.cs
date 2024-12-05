#nullable disable

using Juegos;
using MailKit.Net.Imap;
using MailKit.Search;
using MailKit.Security;
using MailKit;
using Microsoft.Data.SqlClient;
using MimeKit;
using Noticias;

namespace Herramientas
{
	public static class Correos
	{
		public static void EnviarNuevaNoticia(Noticia noticia, string correoHacia, SqlConnection conexion, string idioma)
		{
			try
			{
                string titulo = noticia.TituloEn;
                string enlace = "https://pepeizqdeals.com/news/" + noticia.Id.ToString() + "/" + Herramientas.EnlaceAdaptador.Nombre(noticia.TituloEn) + "/";
				string contenido = noticia.ContenidoEn;

				if (string.IsNullOrEmpty(idioma) == false)
				{
					if (Herramientas.Idiomas.ComprobarEspañol(idioma) == true || Herramientas.Idiomas.ComprobarEspañolLatino(idioma) == true)
					{
						titulo = noticia.TituloEs;
                        enlace = "https://pepeizqdeals.com/news/" + noticia.Id.ToString() + "/" + Herramientas.EnlaceAdaptador.Nombre(noticia.TituloEs) + "/";
						contenido = noticia.ContenidoEs;
					}
				}

                string html = string.Empty;

                html = @"<!DOCTYPE html>
							<html>
							<head>
								<meta charset=""utf-8"" />
								<title></title>
							</head>
							<body>
								<div style=""min-width: 0; word-wrap: break-word; background-color: #002033; background-clip: border-box; border: 0px; padding: 40px; font-family: Roboto, Helevtica, Arial, sans-serif, serif, EmojiFont; font-size: 16px; color: #f5f5f5; line-height: 22px;"">
									<div style=""font-size: 18px; margin-bottom: 20px; line-height: 25px;"">
										{{titulo}}
									</div>

									<hr/>

									<div style=""margin-top: 20px; display: flex; flex-direction: column; gap: 40px; color: #f5f5f5; background-color: #0d1621; padding: 20px;"">
										<a href=""{{enlace}}"" style=""color: #f5f5f5; user-select: none; width: 100%; text-align: left; font-size: 16px; text-decoration: none;"" target=""_blank"">
											<div>
												<div style=""display: flex; align-content: center; align-items: center; justify-content: center; font-size: 18px;"">
													<img src=""{{imagen}}"" style=""max-width: 100%; max-height: 300px; margin-top: 10px;"" />
												</div>

												<div style=""margin-top: 30px"">
													{{contenido}}
												</div>
											</div>
										</a>

										<a href=""{{enlace}}"" style=""color: #95c0fe; user-select: none; width: 100%; text-align: left; font-size: 16px;"" target=""_blank"">
											{{texto1}}
										</a>
									</div>

									<div style=""margin-top: 40px;"">
										&copy; {{año}} • <a href=""https://pepeizqapps.com/"" style=""text-decoration: none; color: #f5f5f5;"" target=""_blank"">pepeizq's apps</a> • <a href=""https://pepeizqdeals.com/"" style=""text-decoration: none; color: #f5f5f5;"" target=""_blank"">pepeizq's deals</a>
									</div>
								</div>
							</body>
							</html>";

                html = html.Replace("{{enlace}}", enlace);
                html = html.Replace("{{titulo}}", titulo);
                html = html.Replace("{{imagen}}", noticia.Imagen);
                html = html.Replace("{{contenido}}", contenido);
                html = html.Replace("{{año}}", DateTime.Now.Year.ToString());
				html = html.Replace("{{texto1}}", Herramientas.Idiomas.BuscarTexto(idioma, "News1", "NewsTemplates"));

				if (html.Contains("<ul>") == true)
				{
					html = html.Replace("<ul>", @"<ul style=""line-height: 22px;"">");
				}

				global::BaseDatos.CorreosEnviar.Insertar.Ejecutar(html, titulo, "deals@pepeizqdeals.com", correoHacia);
            }
			catch (Exception ex) 
			{
                global::BaseDatos.Errores.Insertar.Mensaje("Correos - Enviar Noticia", ex, conexion);
			}
        }

        public static void EnviarContraseñaReseteada(string correoHacia)
        {
			string html = @"<!DOCTYPE html>
							<html>
							<head>
								<meta charset=""utf-8"" />
								<title></title>
							</head>
							<body>
								<div style=""min-width: 0; word-wrap: break-word; background-color: #002033; background-clip: border-box; border: 0px; padding: 40px; font-family: 'Lato'; font-size: 16px; color: #f5f5f5;"">
									<div>
										Your account password has been successfully reset.
									</div>

									<div style=""margin-top: 40px;"">
										&copy; {{año}} • <a href=""https://pepeizqapps.com/"" style=""text-decoration: none; color: #f5f5f5;"" target=""_blank"">pepeizq's apps</a> • <a href=""https://pepeizqdeals.com/"" style=""text-decoration: none; color: #f5f5f5;"" target=""_blank"">pepeizq's deals</a>
									</div>
								</div>
							</body>
							</html>";

            html = html.Replace("{{año}}", DateTime.Now.Year.ToString());

			global::BaseDatos.CorreosEnviar.Insertar.Ejecutar(html, "Your account password has been reset", "admin@pepeizqdeals.com", correoHacia);
        }

        public static void EnviarContraseñaOlvidada(string codigo, string correoHacia)
        {
			string html = @"<!DOCTYPE html>
							<html>
							<head>
								<meta charset=""utf-8"" />
								<title></title>
							</head>
							<body>
								<div style=""min-width: 0; word-wrap: break-word; background-color: #002033; background-clip: border-box; border: 0px; padding: 40px; font-family: 'Lato'; font-size: 16px; color: #f5f5f5;"">
									<div>
										To reset your account password click on the <a href=""{{codigo}}"" target=""_blank"">following link</a>.
									</div>

									<div style=""margin-top: 40px;"">
										&copy; {{año}} • <a href=""https://pepeizqapps.com/"" style=""text-decoration: none; color: #f5f5f5;"" target=""_blank"">pepeizq's apps</a> • <a href=""https://pepeizqdeals.com/"" style=""text-decoration: none; color: #f5f5f5;"" target=""_blank"">pepeizq's deals</a>
									</div>
								</div>
							</body>
							</html>";

            html = html.Replace("{{codigo}}", codigo);
            html = html.Replace("{{año}}", DateTime.Now.Year.ToString());

			global::BaseDatos.CorreosEnviar.Insertar.Ejecutar(html, "Reset the password", "admin@pepeizqdeals.com", correoHacia);
        }

        public static void EnviarCambioContraseña(string correoHacia)
        {
			string html = @"<!DOCTYPE html>
							<html>
							<head>
								<meta charset=""utf-8"" />
								<title></title>
							</head>
							<body>
								<div style=""min-width: 0; word-wrap: break-word; background-color: #002033; background-clip: border-box; border: 0px; padding: 40px; font-family: 'Lato'; font-size: 16px; color: #f5f5f5;"">
									<div>
										The account password has been changed, if you have not changed it, contact admin@pepeizqdeals.com immediately.
									</div>

									<div style=""margin-top: 40px;"">
										&copy; {{año}} • <a href=""https://pepeizqapps.com/"" style=""text-decoration: none; color: #f5f5f5;"" target=""_blank"">pepeizq's apps</a> • <a href=""https://pepeizqdeals.com/"" style=""text-decoration: none; color: #f5f5f5;"" target=""_blank"">pepeizq's deals</a>
									</div>
								</div>
							</body>
							</html>";

            html = html.Replace("{{año}}", DateTime.Now.Year.ToString());

			global::BaseDatos.CorreosEnviar.Insertar.Ejecutar(html, "Your account password has changed", "admin@pepeizqdeals.com", correoHacia);
        }

        public static void EnviarCambioCorreo(string codigo, string correoHacia)
		{
			string html = @"<!DOCTYPE html>
							<html>
							<head>
								<meta charset=""utf-8"" />
								<title></title>
							</head>
							<body>
								<div style=""min-width: 0; word-wrap: break-word; background-color: #002033; background-clip: border-box; border: 0px; padding: 40px; font-family: 'Lato'; font-size: 16px; color: #f5f5f5;"">
									<div>
										To confirm your email click on the <a href=""{{codigo}}"" target=""_blank"">following link</a>.
									</div>

									<div style=""margin-top: 40px;"">
										&copy; {{año}} • <a href=""https://pepeizqapps.com/"" style=""text-decoration: none; color: #f5f5f5;"" target=""_blank"">pepeizq's apps</a> • <a href=""https://pepeizqdeals.com/"" style=""text-decoration: none; color: #f5f5f5;"" target=""_blank"">pepeizq's deals</a>
									</div>
								</div>
							</body>
							</html>";

            html = html.Replace("{{codigo}}", codigo);
            html = html.Replace("{{año}}", DateTime.Now.Year.ToString());

			global::BaseDatos.CorreosEnviar.Insertar.Ejecutar(html, "Confirm your email change", "admin@pepeizqdeals.com", correoHacia);
        }

        public static void EnviarConfirmacionCorreo(string codigo, string correoHacia)
		{
			string html = @"<!DOCTYPE html>
							<html>
							<head>
								<meta charset=""utf-8"" />
								<title></title>
							</head>
							<body>
								<div style=""min-width: 0; word-wrap: break-word; background-color: #002033; background-clip: border-box; border: 0px; padding: 40px; font-family: 'Lato'; font-size: 16px; color: #f5f5f5;"">
									<div>
										To confirm your email click on the <a href=""{{codigo}}"" target=""_blank"">following link</a>.
									</div>

									<div style=""margin-top: 40px;"">
										&copy; {{año}} • <a href=""https://pepeizqapps.com/"" style=""text-decoration: none; color: #f5f5f5;"" target=""_blank"">pepeizq's apps</a> • <a href=""https://pepeizqdeals.com/"" style=""text-decoration: none; color: #f5f5f5;"" target=""_blank"">pepeizq's deals</a>
									</div>
								</div>
							</body>
							</html>";

			html = html.Replace("{{codigo}}", codigo);
            html = html.Replace("{{año}}", DateTime.Now.Year.ToString());

			global::BaseDatos.CorreosEnviar.Insertar.Ejecutar(html, "Confirm your email", "admin@pepeizqdeals.com", correoHacia);
		}

		public static void EnviarNuevoMinimo(int idJuego, JuegoPrecio precio, string correoHacia)
		{
			Juego juego = global::BaseDatos.Juegos.Buscar.UnJuego(idJuego);
			EnviarNuevoMinimo(juego, precio, correoHacia);
		}

		public static void EnviarNuevoMinimo(Juego juego, JuegoPrecio precio, string correoHacia)
		{
			string html = @"<!DOCTYPE html>
							<html>
							<head>
								<meta charset=""utf-8"" />
								<title></title>
							</head>
							<body>
								<div style=""min-width: 0; word-wrap: break-word; background-color: #002033; background-clip: border-box; border: 0px; padding: 40px; font-family: 'Lato'; font-size: 16px; color: #f5f5f5;"">
									<div>
										{{descripcion}}
									</div>

									<div style=""margin-top: 40px;"">
										<a href=""{{enlace}}"" style=""color: #f5f5f5; background-color: #0d1621; display: inline-block; user-select: none; width: 100%; padding: 6px; text-align: left; font-size: 16px; border: 0px; text-decoration: none; "">
											<div style=""display: flex; align-content: center; align-items: center; justify-content: center; font-size: 18px;"">
												<img src=""{{imagen}}"" style=""max-width: 100%; max-height: 100%; margin-top: 10px;"" />
											</div>
											<div style=""display: flex; align-content: center; align-items: center; justify-content: center; font-size: 18px; margin-top: 10px;"">
												<img src=""{{imagenTienda}}"" style=""width: 120px; margin-right: 10px;"" />
												<div class=""juego-descuento"" style=""margin: 10px; padding: 10px; background-color: darkgreen;"">
													{{descuento}}
												</div>
												<div style=""padding: 5px 10px;"">
													{{precio}}
												</div>
											</div>
										</a>
									</div>

									<div style=""margin-top: 20px;"">
										<a href=""{{enlace}}"" style=""color: #f5f5f5; background-color: #0d1621; display: inline-block; user-select: none; width: 100%; padding: 6px; text-align: center; border: 0px; text-decoration: none; "">
											<div style=""display: flex; align-content: center; align-items: center; justify-content: center; font-size: 17px; margin: 15px;"">
												{{mensaje}}
											</div>
										</a>
									</div>

									<div style=""margin-top: 40px;"">
										&copy; {{año}} • <a href=""https://pepeizqapps.com/"" style=""text-decoration: none; color: #f5f5f5;"" target=""_blank"">pepeizq's apps</a> • <a href=""https://pepeizqdeals.com/"" style=""text-decoration: none; color: #f5f5f5;"" target=""_blank"">pepeizq's deals</a>
									</div>
								</div>
							</body>
							</html>";

			string descripcion = juego.Nombre + " has reached a new minimum price registered on pepeizqdeals.com:";
			string imagen = juego.Imagenes.Header_460x215;
			string descuento = precio.Descuento.ToString() + "%";

			string precio2 = string.Empty;

			precio2 = precio.Precio.ToString();
			precio2 = precio2.Replace(".", ",");

			int int1 = precio2.IndexOf(",");

			if (int1 == precio2.Length - 2)
			{
				precio2 = precio2 + "0";
			}

			precio2 = precio2 + "€";

			string enlace = "https://pepeizqdeals.com/game/" + juego.Id.ToString() + "/" + Herramientas.EnlaceAdaptador.Nombre(juego.Nombre) + "/";
			string imagenTienda = string.Empty;

			List<Tiendas2.Tienda> tiendas = Tiendas2.TiendasCargar.GenerarListado();

			foreach (var tienda in tiendas)
			{
				if (tienda.Id == precio.Tienda)
				{
					imagenTienda = "https://pepeizqdeals.com/" + tienda.Imagen300x80;
				}
			}

			string mensajeAbrir = string.Empty;

			if (juego.Tipo == JuegoTipo.Game)
			{
				mensajeAbrir = "Open Game";
			}
			else if (juego.Tipo == JuegoTipo.DLC)
			{
				mensajeAbrir = "Open DLC";
			}

			html = html.Replace("{{descripcion}}", descripcion);
			html = html.Replace("{{imagen}}", imagen);
			html = html.Replace("{{descuento}}", descuento);
			html = html.Replace("{{precio}}", precio2);
			html = html.Replace("{{enlace}}", enlace);
			html = html.Replace("{{imagenTienda}}", imagenTienda);
			html = html.Replace("{{mensaje}}", mensajeAbrir);
			html = html.Replace("{{año}}", DateTime.Now.Year.ToString());

			global::BaseDatos.CorreosEnviar.Insertar.Ejecutar(html, descripcion + " • " + precio2, "deals@pepeizqdeals.com", correoHacia);
		}

        public static bool EnviarCorreo(string html, string titulo, string correoDesde, string correoHacia)
		{
			if (string.IsNullOrEmpty(correoDesde) == false && string.IsNullOrEmpty(correoHacia) == false) 
			{
				WebApplicationBuilder builder = WebApplication.CreateBuilder();

				string host = builder.Configuration.GetValue<string>("Correo:Host");
				string contraseña = builder.Configuration.GetValue<string>("Correo:Contraseña");

				System.Net.Mail.MailMessage mensaje = new System.Net.Mail.MailMessage();
				mensaje.From = new System.Net.Mail.MailAddress(correoDesde, "pepeizq's deals");
				mensaje.To.Add(correoHacia);
				mensaje.Subject = titulo;
				mensaje.Body = html;
				mensaje.IsBodyHtml = true;

				System.Net.Mail.SmtpClient cliente = new System.Net.Mail.SmtpClient();
				cliente.Host = host;

				string texto1 = "gmail.com";
				string texto2 = correoDesde.ToLower();

				if (texto2.Contains(texto1) == true)
				{
					cliente.Port = 587;
					cliente.Credentials = new System.Net.NetworkCredential(correoDesde, contraseña);
					cliente.EnableSsl = true;

					try
					{
						cliente.Send(mensaje);
						return true;
					}
					catch (Exception ex)
					{
						global::BaseDatos.Errores.Insertar.Mensaje("Correo Enviar", ex);
						return false;
					}
				}
				else
				{
					cliente.Port = 8889;
					cliente.Credentials = new System.Net.NetworkCredential(correoDesde, contraseña);
					cliente.EnableSsl = false;

					try
					{
						cliente.Send(mensaje);
						return true;
					}
					catch (Exception ex)
					{
						global::BaseDatos.Errores.Insertar.Mensaje("Correo Enviar", ex);
						return false;
					}
				}
			}

			return false;
		}

		//tipos
		//0 pepeizqdeals
		//1 pepeizqapps
		public static List<CorreoConId> ComprobarNuevosCorreos(int tipo)
		{
			List<CorreoConId> correos = new List<CorreoConId>();

            WebApplicationBuilder builder = WebApplication.CreateBuilder();

			string host = string.Empty;
			string contraseña = string.Empty;

            if (tipo == 0) 
			{ 
				host = builder.Configuration.GetValue<string>("Correo:Host");
                contraseña = builder.Configuration.GetValue<string>("Correo:Contraseña");
            }
			else if (tipo == 1)
            {
                host = builder.Configuration.GetValue<string>("Correo2:Host");
                contraseña = builder.Configuration.GetValue<string>("Correo2:Contraseña");
            }

            using (ImapClient cliente = new ImapClient())
            {
				try
				{
                    cliente.Connect(host, 143, SecureSocketOptions.Auto);

					if (tipo == 0)
					{
                        cliente.Authenticate("admin@pepeizqdeals.com", contraseña);
                    }
					else if (tipo == 1) 
					{
                        cliente.Authenticate("admin@pepeizqapps.com", contraseña);
                    }
                    
                    cliente.Inbox.Open(FolderAccess.ReadOnly);

                    foreach (var id in cliente.Inbox.Search(SearchQuery.NotSeen))
                    {
                        MimeMessage correo = cliente.Inbox.GetMessage(id);

						CorreoConId nuevoCorreo = new CorreoConId();
						nuevoCorreo.Correo = correo;
						nuevoCorreo.Id = id;

                        correos.Add(nuevoCorreo);
                    }
                }
				catch { }

				cliente.Disconnect(true);
			}

			if (correos.Count > 0)
			{
				correos = correos.OrderByDescending(x => x.Correo.Date).ToList();
            }

            return correos;
        }
	}

	public class CorreoConId
	{
		public MimeMessage Correo;
		public UniqueId Id;
	}
}
