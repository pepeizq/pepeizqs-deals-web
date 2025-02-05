#nullable disable

using Juegos;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using pepeizqs_deals_web.Areas.Identity.Data;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Herramientas.Ficheros
{
	public static class HeroicGames
	{
		public static async Task<int> Cargar(JuegoDRM drm, IBrowserFile fichero, Usuario usuario, UserManager<Usuario> UserManager)
		{
			int importados = 0;

			int maximoTamaño = 268435456; //256 mb
			byte[] buffer = new byte[fichero.Size];

			LecturaPerezosa stream = new LecturaPerezosa(fichero, maximoTamaño);
			StreamContent contenido = new StreamContent(stream);

			string contenido2 = await contenido.ReadAsStringAsync();

			List<HeroicGamesJuego> listadoJuegos = new List<HeroicGamesJuego>();

			if (string.IsNullOrEmpty(contenido2) == false)
			{
				List<HeroicGamesBD> bd = JsonSerializer.Deserialize<List<HeroicGamesBD>>(contenido2);

				if (bd != null)
				{
					if (bd.Count > 0)
					{
						foreach (HeroicGamesBD registro in bd)
						{
							if (drm == JuegoDRM.Amazon && registro.Tipo.Contains("com.amazon") == true)
							{
								HeroicGamesJuego juego = new HeroicGamesJuego
								{
									Nombre = registro.Producto.Nombre,
									Id = registro.Producto.Id
								};

								juego.Id = juego.Id.Replace("amzn1.resource.", null);

								listadoJuegos.Add(juego);
							}
						}
					}
				}
			}

			if (listadoJuegos.Count > 0)
			{
				SqlConnection conexion = new SqlConnection();

				if (conexion == null)
				{
					conexion = Herramientas.BaseDatos.Conectar();
				}
				else
				{
					if (conexion.State != System.Data.ConnectionState.Open)
					{
						conexion = Herramientas.BaseDatos.Conectar();
					}
				}

				string textoIds = string.Empty;

				foreach (HeroicGamesJuego juego in listadoJuegos)
				{
					if (drm == JuegoDRM.Amazon)
					{
						global::BaseDatos.Plataformas.Buscar.Amazon(juego.Id, juego.Nombre, conexion);
					}

					if (string.IsNullOrEmpty(textoIds) == true)
					{
						textoIds = juego.Id;
					}
					else
					{
						textoIds = textoIds + "," + juego.Id;
					}

					importados += 1;
				}

				if (usuario != null)
				{
					if (drm == JuegoDRM.Amazon)
					{
						usuario.AmazonGames = textoIds;
						usuario.AmazonLastImport = DateTime.Now;
					}

					try
					{
						await UserManager.UpdateAsync(usuario);
					}
					catch
					{
						global::BaseDatos.Errores.Insertar.Mensaje("Cuenta Heroic Juegos", usuario.Id);
					}
				}
			}

			return importados;
		}
	}

	public class HeroicGamesBD
	{
		[JsonPropertyName("product")]
		public HeroicGamesDBProducto Producto { get; set; }

		[JsonPropertyName("__type")]
		public string Tipo { get; set; }
	}

	public class HeroicGamesDBProducto
	{
		[JsonPropertyName("id")]
		public string Id { get; set; }

		[JsonPropertyName("title")]
		public string Nombre { get; set; }
	}

	public class HeroicGamesJuego
	{
		public string Id { get; set; }
		public string Nombre { get; set; }
	}
}
