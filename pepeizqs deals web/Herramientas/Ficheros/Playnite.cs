#nullable disable

using Juegos;
using LiteDB;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using pepeizqs_deals_web.Areas.Identity.Data;

namespace Herramientas.Ficheros
{
	public static class Playnite
	{
		public static async Task<int> Cargar(JuegoDRM drm, IBrowserFile fichero, Usuario usuario, UserManager<Usuario> UserManager)
		{
			int importados = 0;

			int maximoTamaño = 268435456; //256 mb
			byte[] buffer = new byte[fichero.Size];

			LecturaPerezosa stream = new LecturaPerezosa(fichero, maximoTamaño);
			StreamContent contenido = new StreamContent(stream);

			string ubicacion = Path.GetFullPath("./wwwroot/otros/playnite-" + usuario.Id + ".db");
			await File.WriteAllBytesAsync(ubicacion, await contenido.ReadAsByteArrayAsync());

			List<PlayniteJuego> listadoJuegos = new List<PlayniteJuego>();

			using (LiteDatabase bd = new LiteDatabase(ubicacion))
			{
				ILiteCollection<PlayniteBDJuego> consulta = bd.GetCollection<PlayniteBDJuego>("game");

				List<PlayniteBDJuego> resultados = consulta.Query().ToList();

				foreach (PlayniteBDJuego resultado in resultados)
				{
					if (resultado.PluginId.ToString().ToLower() == "402674cd-4af6-4886-b6ec-0e695bfa0688" && drm == JuegoDRM.Amazon)
					{
						PlayniteJuego juego = new PlayniteJuego
						{
							Nombre = resultado.Name,
							Id = resultado.GameId
						};

						listadoJuegos.Add(juego);
					}

					if (resultado.PluginId.ToString().ToLower() == "00000002-dbd1-46c6-b5d0-b1ba559d10e4" && drm == JuegoDRM.Epic)
					{
						PlayniteJuego juego = new PlayniteJuego
						{
							Nombre = resultado.Name,
							Id = resultado.GameId
						};

						listadoJuegos.Add(juego);
					}

					if (resultado.PluginId.ToString().ToLower() == "c2f038e5-8b92-4877-91f1-da9094155fc5" && drm == JuegoDRM.Ubisoft)
					{
						PlayniteJuego juego = new PlayniteJuego
						{
							Nombre = resultado.Name,
							Id = resultado.GameId
						};

						listadoJuegos.Add(juego);
					}

					if (resultado.PluginId.ToString().ToLower() == "85dd7072-2f20-4e76-a007-41035e390724" && drm == JuegoDRM.EA)
					{
						PlayniteJuego juego = new PlayniteJuego
						{
							Nombre = resultado.Name,
							Id = resultado.GameId
						};

						listadoJuegos.Add(juego);
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

				foreach (var juego in listadoJuegos)
				{
					if (drm == JuegoDRM.Amazon)
					{
						global::BaseDatos.Plataformas.Buscar.Amazon(juego.Id, juego.Nombre, conexion);
					}

					if (drm == JuegoDRM.Epic)
					{
						global::BaseDatos.Plataformas.Buscar.Epic(juego.Id, juego.Nombre, conexion);
					}

					if (drm == JuegoDRM.Ubisoft)
					{
						global::BaseDatos.Plataformas.Buscar.Ubisoft(juego.Id, juego.Nombre, conexion);
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

					if (drm == JuegoDRM.Epic)
					{
						usuario.EpicGames = textoIds;
						usuario.EpicGamesLastImport = DateTime.Now;
					}

					if (drm == JuegoDRM.Ubisoft)
					{
						usuario.UbisoftGames = textoIds;
						usuario.UbisoftLastImport = DateTime.Now;
					}

					try
					{
						await UserManager.UpdateAsync(usuario);
					}
					catch
					{
						global::BaseDatos.Errores.Insertar.Mensaje("Cuenta Playnite Juegos", usuario.Id);
					}
				}
			}

			return importados;
		}
	}

	public class PlayniteJuego
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
    }

    public class PlayniteBDJuego
    {
        public string GameId { get; set; }
        public string Name { get; set; }
        public object PluginId { get; set; }
    }
}
