using Microsoft.AspNetCore.Mvc;
using NETCore.MailKit.Core;

namespace Herramientas
{
	public class Correos : Controller
	{
		private readonly IEmailService servicio;

		public Correos(IEmailService _servicio)
		{
			servicio = _servicio;
		}

		public IActionResult Email()
		{
			servicio.Send("pepeizq@msn.com", "test", "test2");

			return View();
		}

		public static void Enviar()
		{
			//EmailService servicio = new EmailService();
			//servicio.Send("pepeizq@msn.com", "test", "test2");
		}
	}
}
