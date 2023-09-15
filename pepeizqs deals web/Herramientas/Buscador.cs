#nullable disable

using Juegos;
using Microsoft.Data.SqlClient;

namespace Herramientas
{
	public static class Buscador
	{
		public static string LimpiarNombre(string nombre)
		{
			if (nombre != null)
			{
				List<string> caracteres = new List<string>
				{
					":", ",", ".", "®", "™", "_", "-", ">", "<", ";", "(", ")", "[", "]", "=", "?", "¿", "'", "¡", "!", "&", "|",
					"/", "\\", "{", "}", "#", "´", " "
				};

				foreach (string caracter in caracteres)
				{
					nombre = nombre.Replace(caracter, null);
				}

				nombre = nombre.ToLower();
				nombre = nombre.Trim();

				return nombre;
			}

			return null;
		}

		public static void ActualizarCodigos()
		{
			SqlConnection conexion = BaseDatos.Conectar();

			using (conexion)
			{
				List<Juego> juegos = global::BaseDatos.Juegos.Buscar.Todos(conexion);

				if (juegos != null)
				{
					foreach (var juego in juegos)
					{
						global::BaseDatos.Juegos.Actualizar.UnParametro(juego, conexion);
					}
				}
			}

			conexion.Dispose();
		}
	}
}
