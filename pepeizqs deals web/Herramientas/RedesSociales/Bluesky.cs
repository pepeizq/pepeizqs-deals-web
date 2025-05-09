#nullable disable

using X.Bluesky;

namespace Herramientas.RedesSociales
{
	public static class Bluesky
	{
		public static async Task<bool> Postear(Noticias.Noticia noticia)
		{
			WebApplicationBuilder builder = WebApplication.CreateBuilder();

			string correo = builder.Configuration.GetValue<string>("Bluesky:Correo");
			string contraseña = builder.Configuration.GetValue<string>("Bluesky:Contraseña");

			if (string.IsNullOrEmpty(correo) == false && string.IsNullOrEmpty(contraseña) == false)
			{
				IBlueskyClient cliente = new BlueskyClient(correo, contraseña);

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

				Uri enlaceFinal = new Uri(enlace);

				bool error = false;

				try
				{
					string imagenEnlace = noticia.Imagen;
					HttpClient clienteWeb = new HttpClient();

					using (HttpResponseMessage respuesta = await clienteWeb.GetAsync(imagenEnlace, HttpCompletionOption.ResponseContentRead))
					{
						byte[] imageBytes = await respuesta.Content.ReadAsByteArrayAsync();

						X.Bluesky.Models.Image imagen = new X.Bluesky.Models.Image
						{
							Content = imageBytes,
							Alt = noticia.TituloEn,
							MimeType = "image/jpeg"
						};

						await cliente.Post(noticia.TituloEn + Environment.NewLine + Environment.NewLine + enlaceFinal, enlaceFinal, imagen);

						return true;
					}
				}
				catch
				{
					error = true;
				}

				if (error == true)
				{
					try
					{
						await cliente.Post(noticia.TituloEn + Environment.NewLine + Environment.NewLine + enlaceFinal);
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
