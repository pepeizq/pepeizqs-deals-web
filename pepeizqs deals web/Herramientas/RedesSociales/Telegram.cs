#nullable disable

using System.Net;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Herramientas.RedesSociales
{
	public static class Telegram
	{
		public static async Task<bool> Postear(Noticias.Noticia noticia)
		{
			WebApplicationBuilder builder = WebApplication.CreateBuilder();
			string token = builder.Configuration.GetValue<string>("Telegram:Token");
			string chatId = builder.Configuration.GetValue<string>("Telegram:ChatID");

			if (string.IsNullOrEmpty(token) == false)
			{
				TelegramBotClient cliente = new TelegramBotClient(token);

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

				bool error = false;

				try
				{
					string imagenEnlace = noticia.Imagen;
					await cliente.SendPhoto(chatId, InputFile.FromUri(WebUtility.HtmlDecode(noticia.Imagen)), noticia.TituloEn + " " + enlace);

					return true;
				}
				catch
				{
					error = true;
				}

				if (error == true)
				{
					try
					{
						await cliente.SendMessage(chatId, noticia.TituloEn + Environment.NewLine + Environment.NewLine + enlace);

						return true;
					}
					catch
					{
						return false;
					}
				}
			}

			return false;
		}
	}
}
