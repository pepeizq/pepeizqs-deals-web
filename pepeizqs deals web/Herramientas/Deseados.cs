#nullable disable

using Juegos;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using pepeizqs_deals_web.Areas.Identity.Data;

namespace Herramientas
{
	public static class Deseados
	{
		public static void ActualizarJuegoConUsuarios(Juego juego, JuegoDRM drm, Usuario usuario, bool estado)
		{
			SqlConnection conexion = BaseDatos.Conectar();

			using (conexion)
			{
				List<JuegoUsuariosInteresados> usuariosInteresados = juego.UsuariosInteresados;

				if (usuariosInteresados == null)
				{
					usuariosInteresados = new List<JuegoUsuariosInteresados>();
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
						JuegoUsuariosInteresados nuevo = new JuegoUsuariosInteresados();
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
					JuegoUsuariosInteresados nuevo = new JuegoUsuariosInteresados();
					nuevo.UsuarioId = usuario.Id;
					nuevo.DRM = drm;

					usuariosInteresados.Add(nuevo);
				}

				global::BaseDatos.Juegos.Actualizar.UsuariosInteresados(juego, conexion, usuariosInteresados);
			}
		}

		public static bool ComprobarSiEsta(Usuario usuario, Juego juego, JuegoDRM drm)
		{
			bool estado = false;
			List<JuegoDeseado> deseados = new List<JuegoDeseado>();

			if (usuario.Wishlist != null)
			{
				deseados = JsonConvert.DeserializeObject<List<JuegoDeseado>>(usuario.Wishlist);
			}

			if (deseados.Count > 0)
			{
				foreach (var deseado in deseados)
				{
					if (juego.Id == int.Parse(deseado.IdBaseDatos))
					{
						if (drm == deseado.DRM)
						{
							estado = true;
							break;
						}
					}
				}
			}

			return estado;
		}
	}
}
