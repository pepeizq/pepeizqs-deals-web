using Microsoft.Data.SqlClient;
using pepeizqs_deals_web.Areas.Identity.Data;

namespace Herramientas
{
	public static class Deseados
	{
		public static void ActualizarJuegoConUsuarios(Juegos.Juego juego, Juegos.JuegoDRM drm, Usuario usuario, bool estado)
		{
			SqlConnection conexion = BaseDatos.Conectar();

			using (conexion)
			{
				List<Juegos.JuegoUsuariosInteresados> usuariosInteresados = juego.UsuariosInteresados;

				if (usuariosInteresados == null)
				{
					usuariosInteresados = new List<Juegos.JuegoUsuariosInteresados>();
				}

				if (usuariosInteresados.Count > 0)
				{
					if (estado == false)
					{
						int posicion = -1;

						for (int i = 0; i < usuariosInteresados.Count; i += 1)
						{
							if (usuariosInteresados[i].DRM == drm && usuariosInteresados[i].UsuarioId == usuario.Id)
							{
								posicion = i;
							}
						}

						if (posicion >= 0)
						{
							usuariosInteresados.RemoveAt(posicion);
						}
					}
					else
					{
						Juegos.JuegoUsuariosInteresados nuevo = new Juegos.JuegoUsuariosInteresados();
						nuevo.UsuarioId = usuario.Id;
						nuevo.DRM = drm;

						bool añadir = true;

						foreach (var usuario2 in usuariosInteresados)
						{
							if (usuario2.DRM == drm && usuario2.UsuarioId == usuario.Id)
							{
								añadir = false;
							}
						}

						if (añadir == true)
						{
							usuariosInteresados.Add(nuevo);
						}
					}
				}
				else
				{
					Juegos.JuegoUsuariosInteresados nuevo = new Juegos.JuegoUsuariosInteresados();
					nuevo.UsuarioId = usuario.Id;
					nuevo.DRM = drm;

					usuariosInteresados.Add(nuevo);
				}

				global::BaseDatos.Juegos.Actualizar.UsuariosInteresados(juego, conexion, usuariosInteresados);
			}

			conexion.Dispose();
		}
	}
}
