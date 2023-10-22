#nullable disable

using System.Net.Mail;

//https://blog.christian-schou.dk/send-emails-with-asp-net-core-with-mailkit/

namespace Herramientas
{
	public static class Correos
	{
		public static void Enviar(string correoHacia)
		{
			WebApplicationBuilder builder = WebApplication.CreateBuilder();

			string host = builder.Configuration.GetValue<string>("Correo:Host");
			string correoDesde = builder.Configuration.GetValue<string>("Correo:CorreoDesde");
			string contraseña = builder.Configuration.GetValue<string>("Correo:Contraseña");

			MailMessage mensaje = new MailMessage();
			mensaje.From = new MailAddress(correoDesde);
			mensaje.To.Add(correoHacia);
			mensaje.Subject = "This is a test";
			mensaje.Body = "This is a sample message using SMTP authentication";

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
		public string Contraseña { get; set; }
	}
}
