using Microsoft.AspNetCore.Mvc;

//https://blog.christian-schou.dk/send-emails-with-asp-net-core-with-mailkit/

namespace Herramientas
{
	public class Correos : IEmailService
	{
		private readonly Email servicio;

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
