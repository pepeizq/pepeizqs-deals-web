#nullable disable

using Newtonsoft.Json;
using System.Text;
using Tweetinvi;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;

namespace Herramientas.RedesSociales
{
	public static class Twitter
	{
		public static async Task<bool> Twitear(Noticias.Noticia noticia)
		{
			WebApplicationBuilder builder = WebApplication.CreateBuilder();

			TwitterClient cliente = new TwitterClient(builder.Configuration.GetValue<string>("Twitter:ConsumerKey"),
				builder.Configuration.GetValue<string>("Twitter:ConsumerSecret"),
				builder.Configuration.GetValue<string>("Twitter:AccessToken"),
				builder.Configuration.GetValue<string>("Twitter:AccessSecret"));

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

            string ubicacion = Path.GetFullPath("./wwwroot/imagenes/twitter.png");
            IMedia imagenTweet = null;

			try
			{
				using (HttpClient descargador = new HttpClient())
				{
					using (HttpResponseMessage resultado1 = await descargador.GetAsync(noticia.Imagen))
					{
						byte[] resultado2 = await resultado1.Content.ReadAsByteArrayAsync();

						if (resultado2 != null)
						{
							await File.WriteAllBytesAsync(ubicacion, resultado2);

							imagenTweet = await cliente.Upload.UploadTweetImageAsync(resultado2);
						}
					}
				}
			}
			catch
			{

			}
            
			if (imagenTweet == null)
			{
                ITwitterResult resultado = await PonerTweet(cliente,
					new TweetV2PostRequest
					{
						Text = noticia.TituloEn + " " + Environment.NewLine + Environment.NewLine + enlace
					}
				);

				return resultado.Response.IsSuccessStatusCode;
			}
			else
			{
                ITwitterResult resultado = await PonerTweet(cliente,
					new TweetV2PostRequest
					{
						Text = noticia.TituloEn + " " + Environment.NewLine + Environment.NewLine + enlace,
						Media = imagenTweet?.Id == null ? null : new() { MediaIds = new() { imagenTweet.Id.Value } }
					}
				);

				return resultado.Response.IsSuccessStatusCode;
			}
		}

		private static Task<ITwitterResult> PonerTweet(TwitterClient cliente, TweetV2PostRequest parametros)
		{
			return cliente.Execute.AdvanceRequestAsync(
				(peticion) =>
				{
					string json = cliente.Json.Serialize(parametros);
					StringContent contenido = new StringContent(json, Encoding.UTF8, "application/json");

					peticion.Query.Url = "https://api.twitter.com/2/tweets";
					peticion.Query.HttpMethod = Tweetinvi.Models.HttpMethod.POST;
					peticion.Query.HttpContent = contenido;
				}
			);
		}

		public class TweetV2PostRequest
		{
			#nullable enable
            [JsonProperty("text")]
			public string Text { get; set; } = string.Empty;

            [JsonProperty("media", NullValueHandling = NullValueHandling.Ignore)]
            public TweetV2Media? Media { get; set; }
        }

        public class TweetV2Media
        {
			#nullable enable
            [JsonIgnore]
            public List<long>? MediaIds { get; set; }

            [JsonProperty("media_ids", NullValueHandling = NullValueHandling.Ignore)]
            public string[]? MediaIdStrings
            {
                get => MediaIds?.Select(i => JsonConvert.ToString(i)).ToArray();
                set => MediaIds = value?.Select(s => JsonConvert.DeserializeObject<long>(s)).ToList();
            }
        }
    }
}
