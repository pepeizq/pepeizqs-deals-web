#nullable disable

using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using pepeizqs_deals_web.Areas.Identity.Data;

namespace Herramientas.Ficheros
{
	public static class Amazon
	{
		public static async Task<int> Cargar(IBrowserFile fichero, Usuario usuario, UserManager<Usuario> UserManager)
		{
			int importados = 0;

			int maximoTamaño = 268435456; //256 mb;
			byte[] buffer = new byte[fichero.Size];

			Herramientas.Ficheros.LecturaPerezosa stream = new Herramientas.Ficheros.LecturaPerezosa(fichero, maximoTamaño);
			StreamContent contenido = new StreamContent(stream);

			string ubicacion = Path.GetFullPath("./wwwroot/otros/amazon-" + usuario.Id + ".sqlite");
			await File.WriteAllBytesAsync(ubicacion, await contenido.ReadAsByteArrayAsync());

			List<string> listadoIds = new List<string>();

			using (SqliteConnection conexion = new SqliteConnection("Data Source=" + ubicacion))
			{
				conexion.Open();

				SqliteCommand comando = conexion.CreateCommand();
				comando.CommandText = "SELECT * FROM dbset";

				using (SqliteDataReader lector = comando.ExecuteReader())
				{
					while (lector.Read())
					{
						if (lector.IsDBNull(0) == false)
						{
							if (string.IsNullOrEmpty(lector.GetString(0)) == false)
							{
								listadoIds.Add(lector.GetString(0));
							}
						}
					}
				}
			}

			if (listadoIds.Count > 0)
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

				foreach (string id in listadoIds)
				{
					global::BaseDatos.Plataformas.Buscar.Amazon(id, null, conexion);

					if (string.IsNullOrEmpty(textoIds) == true)
					{
						textoIds = id;
					}
					else
					{
						textoIds = textoIds + "," + id;
					}

					importados += 1;
				}

				if (usuario != null)
				{
					usuario.AmazonGames = textoIds;
					usuario.AmazonLastImport = DateTime.Now;

					try
					{
						await UserManager.UpdateAsync(usuario);
					}
					catch
					{
						global::BaseDatos.Errores.Insertar.Mensaje("Cuenta Amazon Juegos", usuario.Id);
					}
				}
			}

			return importados;
		}
	}
}
