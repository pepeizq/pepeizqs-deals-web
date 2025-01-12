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
			string publicKey = builder.Configuration.GetValue<string>("NotificacionesPush:PublicKey"); 
			string privateKey = builder.Configuration.GetValue<string>("NotificacionesPush:PrivateKey");

			VapidDetails vapidDetalles = new VapidDetails("https://pepeizqdeals.com", publicKey, privateKey);
			WebPushClient webPushCliente = new WebPushClient();

			foreach (var usuario in global::BaseDatos.Usuarios.Buscar.TodosUsuariosNotificacionesPush())
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
		public string UserAgent { get; set; }
	}
}
