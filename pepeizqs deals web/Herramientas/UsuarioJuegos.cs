#nullable disable

using Juegos;
using pepeizqs_deals_web.Areas.Identity.Data;

namespace Herramientas
{
	public static class UsuarioJuegos
	{
		public static UsuarioListadosJuegos Cargar(Usuario usuario)
		{
			UsuarioListadosJuegos listados = new UsuarioListadosJuegos();

			if (string.IsNullOrEmpty(usuario.SteamGames) == false)
			{
				listados.Steam = Herramientas.Listados.Generar(usuario.SteamGames);
			}

			if (string.IsNullOrEmpty(usuario.GogGames) == false)
			{
				listados.Gog = Herramientas.Listados.Generar(usuario.GogGames);
			}

			if (string.IsNullOrEmpty(usuario.AmazonGames) == false)
			{
				listados.Amazon = Herramientas.Listados.Generar(usuario.AmazonGames);
			}

			if (string.IsNullOrEmpty(usuario.EpicGames) == false)
			{
				listados.Epic = Herramientas.Listados.Generar(usuario.EpicGames);
			}

			if (string.IsNullOrEmpty(usuario.UbisoftGames) == false)
			{
				listados.Ubisoft = Herramientas.Listados.Generar(usuario.UbisoftGames);
			}

			if (string.IsNullOrEmpty(usuario.EaGames) == false)
			{
				listados.Ea = Herramientas.Listados.Generar(usuario.EaGames);
			}

			return listados;
		}

		public static bool ComprobarSiTiene(UsuarioListadosJuegos listados, Juego juego, JuegoDRM drm = JuegoDRM.NoEspecificado)
		{
			if (juego != null && listados != null)
			{
				if (juego.IdSteam > 0)
				{
					bool drmValidoSteam = false;

					if (drm == JuegoDRM.NoEspecificado || drm == JuegoDRM.Steam)
					{
						drmValidoSteam = true;
					}

					if (drmValidoSteam == true)
					{
						if (listados.Steam != null)
						{
							if (listados.Steam.Count > 0)
							{
								if (juego.Tipo == JuegoTipo.Game)
								{
									foreach (var juegoUsuario in listados.Steam)
									{
										if (juegoUsuario == juego.IdSteam.ToString())
										{
											return true;
										}
									}
								}
							}
						}
					}
				}
				
				if (juego.IdGog > 0)
				{
					bool drmValidoGog = false;

					if (drm == JuegoDRM.NoEspecificado || drm == JuegoDRM.GOG)
					{
						drmValidoGog = true;
					}

					if (drmValidoGog == true)
					{
						if (listados.Gog != null)
						{
							if (listados.Gog.Count > 0)
							{
								if (juego.Tipo == JuegoTipo.Game)
								{
									foreach (var juegoUsuario in listados.Gog)
									{
										if (juegoUsuario == juego.IdGog.ToString())
										{
											return true;
										}
									}
								}
							}
						}
					}
				}
				
				if (string.IsNullOrEmpty(juego.IdAmazon) == false)
				{
					bool drmValidoAmazon = false;

					if (drm == JuegoDRM.NoEspecificado || drm == JuegoDRM.Amazon)
					{
						drmValidoAmazon = true;
					}

					if (drmValidoAmazon == true)
					{
						if (listados.Amazon != null)
						{
							if (listados.Amazon.Count > 0)
							{
								if (juego.Tipo == JuegoTipo.Game)
								{
									foreach (var juegoUsuario in listados.Amazon)
									{
										if (juegoUsuario == juego.IdAmazon.ToString())
										{
											return true;
										}
									}
								}
							}
						}
					}
				}

				if (string.IsNullOrEmpty(juego.ExeEpic) == false)
				{
					bool drmValidoEpic = false;

					if (drm == JuegoDRM.NoEspecificado || drm == JuegoDRM.Epic)
					{
						drmValidoEpic = true;
					}

					if (drmValidoEpic == true)
					{
						if (listados.Epic != null)
						{
							if (listados.Epic.Count > 0)
							{
								if (juego.Tipo == JuegoTipo.Game)
								{
									foreach (var juegoUsuario in listados.Epic)
									{
										if (juegoUsuario == juego.ExeEpic)
										{
											return true;
										}
									}
								}
							}
						}
					}
				}

				if (string.IsNullOrEmpty(juego.ExeUbisoft) == false)
				{
					bool drmValidoUbisoft = false;

					if (drm == JuegoDRM.NoEspecificado || drm == JuegoDRM.Ubisoft)
					{
						drmValidoUbisoft = true;
					}

					if (drmValidoUbisoft == true)
					{
						if (listados.Ubisoft != null)
						{
							if (listados.Ubisoft.Count > 0)
							{
								if (juego.Tipo == JuegoTipo.Game)
								{
									foreach (var juegoUsuario in listados.Ubisoft)
									{
										if (juegoUsuario == juego.ExeUbisoft)
										{
											return true;
										}
									}
								}
							}
						}
					}
				}

				if (string.IsNullOrEmpty(juego.ExeEA) == false)
				{
					bool drmValidoEa = false;

					if (drm == JuegoDRM.NoEspecificado || drm == JuegoDRM.EA)
					{
						drmValidoEa = true;
					}

					if (drmValidoEa == true)
					{
						if (listados.Ea != null)
						{
							if (listados.Ea.Count > 0)
							{
								if (juego.Tipo == JuegoTipo.Game)
								{
									foreach (var juegoUsuario in listados.Ea)
									{
										if (juegoUsuario == juego.ExeEA)
										{
											return true;
										}
									}
								}
							}
						}
					}
				}
			}

			return false;
		}
	}

	public class UsuarioListadosJuegos
	{
		public List<string> Steam { get; set; }
		public List<string> Gog { get; set; }
		public List<string> Amazon { get; set; }
		public List<string> Epic { get; set; }
		public List<string> Ubisoft { get; set; }
		public List<string> Ea { get; set; }
	}
}
