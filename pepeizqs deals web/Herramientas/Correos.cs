using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net.Mail;

//https://blog.christian-schou.dk/send-emails-with-asp-net-core-with-mailkit/

namespace Herramientas
{
	public class Correos : ICorreos
	{
		private readonly Correo _configuracion;

		public Correos(IOptions<Correo> configuracion)
		{
			_configuracion = configuracion.Value;
		}

		public async Task<bool> SendAsync(CorreoDatos datos, CancellationToken ct = default)
		{
			try
			{
				// Initialize a new instance of the MimeKit.MimeMessage class
				var mail = new MimeMessage();

				#region Sender / Receiver
				// Sender
				mail.From.Add(new MailboxAddress(_configuracion.DisplayName, datos.From ?? _configuracion.From));
				mail.Sender = new MailboxAddress(datos.DisplayName ?? _configuracion.DisplayName, datos.From ?? _configuracion.From);

				// Receiver
				foreach (string mailAddress in datos.To)
					mail.To.Add(MailboxAddress.Parse(mailAddress));

				// Set Reply to if specified in mail data
				if (!string.IsNullOrEmpty(datos.ReplyTo))
					mail.ReplyTo.Add(new MailboxAddress(datos.ReplyToName, datos.ReplyTo));

				// BCC
				// Check if a BCC was supplied in the request
				if (datos.Bcc != null)
				{
					// Get only addresses where value is not null or with whitespace. x = value of address
					foreach (string mailAddress in datos.Bcc.Where(x => !string.IsNullOrWhiteSpace(x)))
						mail.Bcc.Add(MailboxAddress.Parse(mailAddress.Trim()));
				}

				// CC
				// Check if a CC address was supplied in the request
				if (datos.Cc != null)
				{
					foreach (string mailAddress in datos.Cc.Where(x => !string.IsNullOrWhiteSpace(x)))
						mail.Cc.Add(MailboxAddress.Parse(mailAddress.Trim()));
				}
				#endregion

				#region Content

				// Add Content to Mime Message
				var body = new BodyBuilder();
				mail.Subject = datos.Subject;
				body.HtmlBody = datos.Body;
				mail.Body = body.ToMessageBody();

				#endregion

				#region Send Mail

				using var smtp = new SmtpClient();

				if (_configuracion.UseSSL)
				{
					await smtp.ConnectAsync(_configuracion.Host, _configuracion.Port, SecureSocketOptions.SslOnConnect, ct);
				}
				else if (_configuracion.UseStartTls)
				{
					await smtp.ConnectAsync(_configuracion.Host, _configuracion.Port, SecureSocketOptions.StartTls, ct);
				}
				await smtp.AuthenticateAsync(_configuracion.UserName, _configuracion.Password, ct);
				await smtp.SendAsync(mail, ct);
				await smtp.DisconnectAsync(true, ct);

				#endregion

				return true;

			}
			catch (Exception)
			{
				return false;
			}
		}

	}

	public interface ICorreos
	{
		Task<bool> SendAsync(CorreoDatos datos, CancellationToken ct);
	}

	public class Correo
	{
		public string? NombreMostrar { get; set; }
		public string? Desde { get; set; }
		public string? Usuario { get; set; }
		public string? Contraseña { get; set; }
		public string? Host { get; set; }
		public int Puerto { get; set; }
		public bool UsarSSL { get; set; }
		public bool UsarStartTls { get; set; }
	}

	public class CorreoDatos
	{
		public List<string> Hacia { get; }
		public List<string> Bcc { get; }

		public List<string> Cc { get; }

		public string? Desde { get; }

		public string? NombreMostrar { get; }

		public string? ResponderA { get; }

		public string? ResponderANombre { get; }

		public string Titulo { get; }

		public string? Contenido { get; }

		public CorreoDatos(List<string> hacia, string titulo, string? contenido = null, string? desde = null, string? nombreMostrar = null, string? responderA = null, string? responderANombre = null, List<string>? bcc = null, List<string>? cc = null)
		{
			Hacia = hacia;
			Bcc = bcc ?? new List<string>();
			Cc = cc ?? new List<string>();

			Desde = desde;
			NombreMostrar = nombreMostrar;
			ResponderA = responderA;
			ResponderANombre = responderANombre;

			Titulo = titulo;
			Contenido = contenido;
		}
	}
}
