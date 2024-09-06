#nullable disable

using Juegos;
using MailKit.Net.Imap;
using MailKit.Search;
using MailKit.Security;
using MailKit;
using MimeKit;
using Noticias;
using Sorteos2;
using Microsoft.Data.SqlClient;

namespace Herramientas
{
	public static class Correos
	{
		public static void EnviarGanadorSorteo(Juego juego, Sorteo sorteo, string correoHacia)
		{
			if (juego != null)
			{
				string titulo = "You have won the giveaway with the " + juego.Tipo.ToString().ToLower() + " " + juego.Nombre;

				string html = string.Empty;

				using (StreamReader r = new StreamReader("Plantillas/GanadorSorteo.html"))
				{
					html = r.ReadToEnd();
				}

				html = html.Replace("{{titulo}}", titulo);
				html = html.Replace("{{clave}}", sorteo.Clave);
				html = html.Replace("{{año}}", DateTime.Now.Year.ToString());

                EnviarCorreo(html, titulo, "deals@pepeizqdeals.com", correoHacia);
            }
		}

		public static void EnviarNuevaNoticia(Noticia noticia, string correoHacia, SqlConnection conexion, string idioma)
		{
			try
			{
                string titulo = noticia.TituloEn;

				if (string.IsNullOrEmpty(idioma) == false)
				{
					if (idioma == "es-ES" || idioma == "es")
					{
						titulo = noticia.TituloEs;
					}
				}

                string html = string.Empty;

                if (string.IsNullOrEmpty(noticia.Enlace) == false)
                {
                    using (StreamReader r = new StreamReader("Plantillas/NuevaNoticiaConEnlace.html"))
                    {
                        html = r.ReadToEnd();
                    }

                    string enlace = noticia.Enlace;

                    if (enlace.Contains("https://pepeizqdeals.com") == false)
                    {
                        enlace = enlace.Replace("/link/", "https://pepeizqdeals.com/link/");
                    }

                    html = html.Replace("{{enlace}}", enlace);
                }
                else
                {
                    using (StreamReader r = new StreamReader("Plantillas/NuevaNoticia.html"))
                    {
                        html = r.ReadToEnd();
                    }
                }

                string contenido = noticia.ContenidoEn;

				if (string.IsNullOrEmpty(idioma) == false)
				{
					if (idioma == "es-ES" || idioma == "es")
					{
						contenido = noticia.ContenidoEs;
					}
				}

				if (contenido.Contains("https://pepeizqdeals.com") == false)
                {
                    contenido = contenido.Replace("/link/", "https://pepeizqdeals.com/link/");
                }

                html = html.Replace("{{titulo}}", titulo);
                html = html.Replace("{{imagen}}", noticia.Imagen);
                html = html.Replace("{{contenido}}", contenido);
                html = html.Replace("{{año}}", DateTime.Now.Year.ToString());

                EnviarCorreo(html, titulo, "deals@pepeizqdeals.com", correoHacia);
            }
			catch (Exception ex) 
			{
				global::BaseDatos.Errores.Insertar.Ejecutar("Correos - Enviar Noticia", ex, conexion);
			}
        }

        public static void EnviarContraseñaReseteada(string correoHacia)
        {
            string html = string.Empty;

            using (StreamReader r = new StreamReader("Plantillas/ContraseñaReseteada.html"))
            {
                html = r.ReadToEnd();
            }

            html = html.Replace("{{año}}", DateTime.Now.Year.ToString());

            EnviarCorreo(html, "Your account password has been reset", "admin@pepeizqdeals.com", correoHacia);
        }

        public static void EnviarContraseñaOlvidada(string codigo, string correoHacia)
        {
            string html = string.Empty;

            using (StreamReader r = new StreamReader("Plantillas/ContraseñaOlvidada.html"))
            {
                html = r.ReadToEnd();
            }

            html = html.Replace("{{codigo}}", codigo);
            html = html.Replace("{{año}}", DateTime.Now.Year.ToString());

            EnviarCorreo(html, "Reset the password", "admin@pepeizqdeals.com", correoHacia);
        }

        public static void EnviarCambioContraseña(string correoHacia)
        {
            string html = string.Empty;

            using (StreamReader r = new StreamReader("Plantillas/CambioContraseña.html"))
            {
                html = r.ReadToEnd();
            }

            html = html.Replace("{{año}}", DateTime.Now.Year.ToString());

            EnviarCorreo(html, "Your account password has changed", "admin@pepeizqdeals.com", correoHacia);
        }

        public static void EnviarCambioCorreo(string codigo, string correoHacia)
		{
            string html = string.Empty;

            using (StreamReader r = new StreamReader("Plantillas/CambioCorreo.html"))
            {
                html = r.ReadToEnd();
            }

            html = html.Replace("{{codigo}}", codigo);
            html = html.Replace("{{año}}", DateTime.Now.Year.ToString());

            EnviarCorreo(html, "Confirm your email change", "admin@pepeizqdeals.com", correoHacia);
        }

        public static void EnviarConfirmacionCorreo(string codigo, string correoHacia)
		{
			string html = string.Empty;

			using (StreamReader r = new StreamReader("Plantillas/ConfirmacionCorreo.html"))
			{
				html = r.ReadToEnd();
			}

			html = html.Replace("{{codigo}}", codigo);
            html = html.Replace("{{año}}", DateTime.Now.Year.ToString());

            EnviarCorreo(html, "Confirm your email", "admin@pepeizqdeals.com", correoHacia);
		}

		public static void EnviarNuevoMinimo(int idJuego, JuegoPrecio precio, string correoHacia)
		{
			Juegos.Juego juego = global::BaseDatos.Juegos.Buscar.UnJuego(idJuego);
			EnviarNuevoMinimo(juego, precio, correoHacia);
		}

		public static void EnviarNuevoMinimo(Juego juego, JuegoPrecio precio, string correoHacia)
		{
			string html = string.Empty;

			using (StreamReader r = new StreamReader("Plantillas/NuevoMinimo.html"))
			{
				html = r.ReadToEnd();
			}

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

			string enlace = "https://pepeizqdeals.com" + EnlaceAcortador.Generar(precio.Enlace, precio.Tienda);
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

			EnviarCorreo(html, descripcion + " • " + precio2, "deals@pepeizqdeals.com", correoHacia);
		}

        private static void EnviarCorreo(string html, string titulo, string correoDesde, string correoHacia)
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
                    }
                    catch { }                   
				}
				else
				{
					cliente.Port = 25;
					cliente.Credentials = new System.Net.NetworkCredential(correoDesde, contraseña);
					cliente.EnableSsl = false;

					try
					{
						cliente.Send(mensaje);
					}
					catch { }
				}
			}
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
