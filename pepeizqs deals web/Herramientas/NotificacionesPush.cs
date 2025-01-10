#nullable disable

using System.Text.Json;
using WebPush;

namespace Herramientas
{
	public class NotificacionesPush
	{
		public static async void EnviarNoticia(Noticias.Noticia noticia)
		{
			WebApplicationBuilder builder = WebApplication.CreateBuilder();
			string publicKey = "BLn05UCC8XXU59DVU-Iw7lI2gJblF3jhjewYv-zBqU_AgBzMYhn3dQgFAvh3H9VFxBOVAt_sA7mFXLbnOcWz0wg";
			string privateKey = "K-6UD7EV2CwkRrqoxCH5vmx0samCcvRO0Dv7PH0Cdag";

			VapidDetails vapidDetalles = new VapidDetails("https://pepeizqdeals.com", publicKey, privateKey);
			WebPushClient webPushCliente = new WebPushClient();

			foreach (var usuario in global::BaseDatos.Usuarios.Buscar.TodosUsuariosNotificaciones())
			{
				PushSubscription suscripcion = new PushSubscription(usuario.Url, usuario.P256dh, usuario.Auth);

				try
				{
					var payload = JsonSerializer.Serialize(new
					{
						message = noticia.TituloEn,
						url = $"/news/{noticia.Id.ToString()}/"
					});

					await webPushCliente.SendNotificationAsync(suscripcion, payload, vapidDetalles);
				}
				catch (Exception ex)
				{
					global::BaseDatos.Errores.Insertar.Mensaje("Notificaciones Push", ex);
				}
			}
		}
	}

	public class NotificacionSuscripcion
	{
		public int NotificationSubscriptionId { get; set; }
		public string UserId { get; set; }
		public string Url { get; set; }
		public string P256dh { get; set; }
		public string Auth { get; set; }
	}
}
