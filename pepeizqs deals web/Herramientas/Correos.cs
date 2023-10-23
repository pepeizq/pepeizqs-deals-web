#nullable disable

using Juegos;
using System.Net.Mail;

namespace Herramientas
{
	public static class Correos
	{
		public static void EnviarNuevoMinimo(Juego juego, JuegoPrecio precio, string correoHacia)
		{
			WebApplicationBuilder builder = WebApplication.CreateBuilder();

			string host = builder.Configuration.GetValue<string>("Correo:Host");
			string correoDesde = builder.Configuration.GetValue<string>("Correo:CorreoDesde");
			string contraseña = builder.Configuration.GetValue<string>("Correo:Contraseña");

			string html = string.Empty;

			using (StreamReader r = new StreamReader("Plantillas/NuevoMinimo.html"))
			{
				html = r.ReadToEnd();
			}

			string descripcion = juego.Nombre + " has reached a new minimum registered price:";
			string imagen = juego.Imagenes.Capsule_231x87;
			string descuento = precio.Descuento.ToString() + "%";
			string precio2 = JuegoFicha.PrepararPrecio(precio.Precio, false, precio.Moneda);
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

			html = html.Replace("{{descripcion}}", descripcion);
			html = html.Replace("{{imagen}}", imagen);
			html = html.Replace("{{descuento}}", descuento);
			html = html.Replace("{{precio}}", precio2);
			html = html.Replace("{{enlace}}", enlace);
			html = html.Replace("{{imagenTienda}}", imagenTienda);
			html = html.Replace("{{año}}", DateTime.Now.Year.ToString());

			MailMessage mensaje = new MailMessage();
			mensaje.From = new MailAddress(correoDesde, "pepeizq's deals");
			mensaje.To.Add(correoHacia);
			mensaje.Subject = descripcion + " • " + precio2;
			mensaje.Body = html;
			mensaje.IsBodyHtml = true;

			SmtpClient cliente = new SmtpClient();
			cliente.Host = host;

			string texto1 = "gmail.com";
			string texto2 = correoDesde.ToLower();

			if (texto2.Contains(texto1) == true)
			{
				try
				{
					cliente.Port = 587;
					cliente.Credentials = new System.Net.NetworkCredential(correoDesde, contraseña);
					cliente.EnableSsl = true;
					cliente.Send(mensaje);
				}
				catch { }
			}
			else
			{
				try
				{
					cliente.Port = 25;
					cliente.Credentials = new System.Net.NetworkCredential(correoDesde, contraseña);
					cliente.EnableSsl = false;
					cliente.Send(mensaje);
				}
				catch { }
			}
		}
	}

	public class Correo
	{
		public string Host { get; set; }
		public string CorreoDesde { get; set; }
		public string Contraseña { get; set; }
	}
}
