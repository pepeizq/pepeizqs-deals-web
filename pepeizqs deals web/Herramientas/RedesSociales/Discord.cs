#nullable disable

using Discord;
using Discord.Webhook;
using System.Net;

namespace Herramientas.RedesSociales
{
	public static class Discord
	{
		public static void Postear(Noticias.Noticia noticia)
		{
			WebApplicationBuilder builder = WebApplication.CreateBuilder();

			string ingles = builder.Configuration.GetValue<string>("Discord:Ingles");
			string español = builder.Configuration.GetValue<string>("Discord:Español");

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

			Enviar(noticia.TituloEn, enlace, noticia.Imagen, ingles);
			Enviar(noticia.TituloEs, enlace, noticia.Imagen, español);
		}

		private static async void Enviar(string titulo, string enlace, string imagen, string hook)
		{
			using (DiscordWebhookClient cliente = new DiscordWebhookClient(hook))
			{
				EmbedBuilder constructor = new EmbedBuilder();

				constructor.Title = titulo;
				constructor.Url = enlace;

				if (string.IsNullOrEmpty(imagen) == false)
				{
					constructor.ImageUrl = WebUtility.HtmlDecode(imagen);
				}

				List<Embed> lista = new List<Embed>
				{
					constructor.Build()
				};

				await cliente.SendMessageAsync(titulo + Environment.NewLine + enlace, false, lista, "pepebot5");
			}
		}
	}
}
