//https://embed.gog.com/public_wishlist/464176759200/search?hiddenFlag=0&mediaType=0&page=1&sortBy=date_added&totalPages=4
//https://www.gog.com/u/pepeizq/games/stats?sort=recent_playtime&order=desc&page=3

#nullable disable

using Herramientas;
using Microsoft.VisualBasic;
using pepeizqs_deals_web.Areas.Identity.Data;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace APIs.GOG
{
	public static class Cuenta
	{
		public static async Task<string> UsuarioId(string usuario)
		{
			if (string.IsNullOrEmpty(usuario) == false)
			{
				usuario = usuario.Replace("https://www.gog.com/u/", null);
				usuario = usuario.Replace("http://www.gog.com/u/", null);

				if (usuario.Contains("?") == true)
				{
					int int1 = usuario.IndexOf("?");
					usuario = usuario.Remove(int1, usuario.Length - int1);
				}

				usuario = usuario.Replace("/", null);

				string html = await Decompiladores.Estandar("https://www.gog.com/u/" + usuario + "/wishlist");

				if (string.IsNullOrEmpty(html) == false)
				{
					if (html.Contains("gog-user=") == true)
					{
						int int1 = html.IndexOf("gog-user=");
						
						if (int1 > -1)
						{
							string temp1 = html.Remove(0, int1 + 10);

							int int2 = temp1.IndexOf(Strings.ChrW(34));
							string temp2 = temp1.Remove(int2, temp1.Length - int2);
							BaseDatos.Errores.Insertar.Mensaje("test", temp2);
							return temp2;
						}
					}
				}
			}

			return null;
		}

		public static async Task<string> BuscarJuegos(string usuario)
		{
			if (string.IsNullOrEmpty(usuario) == false)
			{
				usuario = usuario.Replace("https://www.gog.com/u/", null);
				usuario = usuario.Replace("http://www.gog.com/u/", null);

				if (usuario.Contains("?") == true)
				{
					int int1 = usuario.IndexOf("?");
					usuario = usuario.Remove(int1, usuario.Length - int1);
				}

				usuario = usuario.Replace("/", null);

				string html = await Decompiladores.Estandar("https://www.gog.com/u/" + usuario + "/games/stats?page=1");

				if (string.IsNullOrEmpty(html) == false)
				{
					GOGCuenta cuenta = JsonSerializer.Deserialize<GOGCuenta>(html);

					if (cuenta != null)
					{
						int limite = cuenta.Paginas;

						if (limite > 0)
						{
							string juegos = string.Empty;

							int i = 1;
							while (i < limite + 1)
							{
								string html2 = await Decompiladores.Estandar("https://www.gog.com/u/" + usuario + "/games/stats?page=" + i.ToString());

								if (string.IsNullOrEmpty(html2) == false)
								{
									GOGCuenta cuenta2 = JsonSerializer.Deserialize<GOGCuenta>(html2);

									if (cuenta2 != null)
									{
										foreach (var juego in cuenta2.Datos.Juegos)
										{
											if (string.IsNullOrEmpty(juegos) == false)
											{
												juegos = juegos + "," + juego.Datos.Id;
											}
											else
											{
												juegos = juego.Datos.Id;
											}
										}
									}
								}

								i += 1;
							}

							if (string.IsNullOrEmpty(juegos) == false)
							{
								return juegos;
							}
						}
					}
				}
			}

			return null;
		}

		public static async Task<string> BuscarDeseados(string usuarioId)
		{
			if (string.IsNullOrEmpty(usuarioId) == false)
			{
				string html = await Decompiladores.Estandar("https://embed.gog.com/public_wishlist/" + usuarioId + "/search?hiddenFlag=0&mediaType=0&page=1&sortBy=date_added");
			
				if (string.IsNullOrEmpty(html) == false)
				{
					GOGDeseados deseados = JsonSerializer.Deserialize<GOGDeseados>(html);

					if (deseados != null)
					{
						int limite = deseados.Paginas;

						if (limite > 0)
						{
							string juegos = string.Empty;

							int i = 1;
							while (i < limite + 1)
							{
								string html2 = await Decompiladores.Estandar("https://embed.gog.com/public_wishlist/" + usuarioId + "/search?hiddenFlag=0&mediaType=0&page=" + i.ToString() + "&sortBy=date_added");

								if (string.IsNullOrEmpty(html2) == false)
								{
									GOGDeseados deseados2 = JsonSerializer.Deserialize<GOGDeseados>(html2);

									if (deseados2 != null)
									{
										foreach (var juego in deseados2.Juegos)
										{
											if (string.IsNullOrEmpty(juegos) == false)
											{
												juegos = juegos + "," + juego.Id.ToString();
											}
											else
											{
												juegos = juego.Id.ToString();
											}
										}
									}
								}

								i += 1;
							}

							if (string.IsNullOrEmpty(juegos) == false)
							{
								return juegos;
							}
						}
					}
				}
			}

			return null;
		}
	}

	public class GOGCuenta
	{
		[JsonPropertyName("page")]
		public int Pagina { get; set; }

		[JsonPropertyName("pages")]
		public int Paginas { get; set; }

		[JsonPropertyName("_embedded")]
		public GOGCuentaDatos Datos { get; set; }
	}

	public class GOGCuentaDatos
	{
		[JsonPropertyName("items")]
		public List<GOGCuentaJuego> Juegos { get; set; }
	}

	public class GOGCuentaJuego
	{
		[JsonPropertyName("game")]
		public GOGCuentaJuegoDatos Datos { get; set; }
	}

	public class GOGCuentaJuegoDatos
	{
		[JsonPropertyName("id")]
		public string Id { get; set; }
	}

	//-----------------------------------------------

	public class GOGDeseados
	{
		[JsonPropertyName("page")]
		public int Pagina { get; set; }

		[JsonPropertyName("totalPages")]
		public int Paginas { get; set; }

		[JsonPropertyName("products")]
		public List<GOGDeseadosJuego> Juegos { get; set; }
	}

	public class GOGDeseadosJuego
	{
		[JsonPropertyName("id")]
		public int Id { get; set; }
	}
}
