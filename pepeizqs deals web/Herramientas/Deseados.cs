﻿#nullable disable

using Juegos;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using pepeizqs_deals_web.Areas.Identity.Data;
using System.Text.Json;

namespace Herramientas
{
	public static class Deseados
	{
		public static void ActualizarJuegoConUsuarios(int idJuego, List<JuegoUsuariosInteresados> usuariosInteresados, JuegoDRM drm, Usuario usuario, bool estado)
		{
			SqlConnection conexion = BaseDatos.Conectar();

			using (conexion)
			{
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

				global::BaseDatos.Juegos.Actualizar.UsuariosInteresados(idJuego, conexion, usuariosInteresados);
			}
		}

		public static bool ComprobarSiEsta(string deseadosSteamEnBruto, string deseadosWebEnBruto, string deseadosGogEnBruto, Juego juego, JuegoDRM drm = JuegoDRM.NoEspecificado, bool usarIdMaestra = false)
		{
			if (usarIdMaestra == true && juego.IdMaestra == 0)
			{
				juego.IdMaestra = juego.Id;
			}

			if (drm == JuegoDRM.Steam || drm == JuegoDRM.NoEspecificado)
			{
				if (juego.IdSteam > 0)
				{
					List<string> deseadosSteam = new List<string>();

					if (string.IsNullOrEmpty(deseadosSteamEnBruto) == false)
					{
						deseadosSteam = Listados.Generar(deseadosSteamEnBruto);
					}

					if (deseadosSteam != null)
					{
						if (deseadosSteam.Count > 0)
						{
							foreach (var deseado in deseadosSteam)
							{
								if (juego.IdSteam > 0)
								{
									if (juego.IdSteam.ToString() == deseado && (drm == JuegoDRM.Steam || drm == JuegoDRM.NoEspecificado))
									{
										return true;
									}
								}
							}
						}
					}
				}
			}

			if (drm == JuegoDRM.GOG || drm == JuegoDRM.NoEspecificado)
			{
				if (juego.IdGog > 0)
				{
					List<string> deseadosGog = new List<string>();

					if (string.IsNullOrEmpty(deseadosGogEnBruto) == false)
					{
						deseadosGog = Listados.Generar(deseadosGogEnBruto);
					}

					if (deseadosGog != null)
					{
						if (deseadosGog.Count > 0)
						{
							foreach (var deseado in deseadosGog)
							{
								if (juego.IdGog.ToString() == deseado && (drm == JuegoDRM.GOG || drm == JuegoDRM.NoEspecificado))
								{
									return true;
								}
							}
						}
					}
				}
			}

			if (juego.Id > 0)
			{
				List<JuegoDeseado> deseadosWeb = new List<JuegoDeseado>();

				if (string.IsNullOrEmpty(deseadosWebEnBruto) == false)
				{
					deseadosWeb = JsonSerializer.Deserialize<List<JuegoDeseado>>(deseadosWebEnBruto);
				}

				if (deseadosWeb != null)
				{
					if (deseadosWeb.Count > 0)
					{
						foreach (var deseado in deseadosWeb)
						{
							if (usarIdMaestra == false)
							{
								if (juego.Id == int.Parse(deseado.IdBaseDatos))
								{
									if (drm == deseado.DRM || drm == JuegoDRM.NoEspecificado)
									{
										return true;
									}
								}
							}
							else
							{
								if (juego.IdMaestra == int.Parse(deseado.IdBaseDatos))
								{
									if (drm == deseado.DRM || drm == JuegoDRM.NoEspecificado)
									{
										return true;
									}
								}
							}
						}
					}
				}
			}
			
			return false;
		}

		public static bool ComprobarSiEsta(string deseadosWebEnBruto, Juego juego, JuegoDRM drm, bool usarIdMaestra = false)
		{
			List<JuegoDeseado> deseadosWeb = new List<JuegoDeseado>();

			if (string.IsNullOrEmpty(deseadosWebEnBruto) == false)
			{
				deseadosWeb = JsonSerializer.Deserialize<List<JuegoDeseado>>(deseadosWebEnBruto);
			}

			if (deseadosWeb != null)
			{
				if (deseadosWeb.Count > 0)
				{
					foreach (var deseado in deseadosWeb)
					{
						if (usarIdMaestra == false)
						{
							if (juego.Id == int.Parse(deseado.IdBaseDatos))
							{
								if (drm == deseado.DRM)
								{
									return true;
								}
							}
						}
						else
						{
							if (juego.IdMaestra == int.Parse(deseado.IdBaseDatos))
							{
								if (drm == deseado.DRM)
								{
									return true;
								}
							}
						}
					}
				}
			}

			return false;
		}

		public static async Task<List<JuegoRazorUsuario>> CambiarEstado(List<JuegoRazorUsuario> usuarioTieneDesea, UserManager<Usuario> UserManager, Usuario usuario, Juego juego, bool estado, JuegoDRM drm)
		{
			List<JuegoDeseado> deseados = new List<JuegoDeseado>();

			if (string.IsNullOrEmpty(usuario.Wishlist) == false)
			{
				deseados = JsonSerializer.Deserialize<List<JuegoDeseado>>(usuario.Wishlist);
			}

			ActualizarJuegoConUsuarios(juego.Id, juego.UsuariosInteresados, drm, usuario, estado);

			if (estado == true)
			{
				bool añadir = true;

				if (deseados.Count > 0)
				{
					foreach (var deseado in deseados)
					{
						if (int.Parse(deseado.IdBaseDatos) == juego.Id && deseado.DRM == drm)
						{
							añadir = false;
						}
					}
				}

				if (añadir == true)
				{
					JuegoDeseado deseado = new JuegoDeseado();
					deseado.IdBaseDatos = juego.Id.ToString();
					deseado.DRM = drm;

					deseados.Add(deseado);

					if (usuarioTieneDesea != null)
					{
						JuegoRazorUsuario deseado2 = new JuegoRazorUsuario();
						deseado2.DRM = drm;
						deseado2.Desea = true;
						usuarioTieneDesea.Add(deseado2);
					}
				}

				usuario.Wishlist = JsonSerializer.Serialize(deseados);

				await UserManager.UpdateAsync(usuario);
			}
			else
			{
				int posicion = -1;

				if (deseados.Count > 0)
				{
					for (int i = 0; i < deseados.Count; i += 1)
					{
						if (int.Parse(deseados[i].IdBaseDatos) == juego.Id && deseados[i].DRM == drm)
						{
							posicion = i;
						}
					}
				}

				if (posicion >= 0)
				{
					deseados.RemoveAt(posicion);
				}

				usuario.Wishlist = JsonSerializer.Serialize(deseados);

				await UserManager.UpdateAsync(usuario);

				if (usuarioTieneDesea != null)
				{
					foreach (var desea in usuarioTieneDesea)
					{
						if (desea.DRM == drm)
						{
							desea.Desea = false;
						}
					}
				}
			}

			return usuarioTieneDesea;
		}
	}
}
