#nullable disable

using Bundles2;
using Gratis2;
using Suscripciones2;

namespace Herramientas
{
	public static class EnlaceAcortador
	{
		private static readonly char[] _chars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
		private static Dictionary<string, string> diccionario { get; set; } = [];

		private static string GenerarCodigo()
		{
			return string.Create(6, _chars, (codigoCorto, charsEstado) => Random.Shared.GetItems(charsEstado, codigoCorto));
		}

		public static string AcortarEnlace(string enlaceLargo)
		{
			if (diccionario != null)
			{
				foreach (var pareja in diccionario)
				{
					if (pareja.Value == enlaceLargo)
					{
						return pareja.Key;
					}
				}
			}

			string codigoCorto;

			while (true)
			{
				codigoCorto = GenerarCodigo();

				if (diccionario.TryAdd(codigoCorto, enlaceLargo))
				{
					break;
				}
			}

			return codigoCorto;
		}

		#nullable enable

		public static string? AlargarEnlace(string codigoCorto)
		{
			if (diccionario.TryGetValue(codigoCorto, out string? enlaceLargo))
			{
				return enlaceLargo;
			}

			return default;
		}

		#nullable disable

		private static string dominio = "https://pepeizqdeals.com";

		public static string Generar(string enlace, string tienda = null, bool usuarioPatreon = false)
		{
			bool usarReferido = true;

			if (usuarioPatreon == true)
			{
				usarReferido = false;
			}

			if (usarReferido == true)
			{
				if (tienda != null)
				{
					if (tienda == APIs.Steam.Tienda.Generar().Id)
					{
						enlace = APIs.Steam.Tienda.Referido(enlace);
					}
					else if (tienda == APIs.GamersGate.Tienda.Generar().Id)
					{
						enlace = APIs.GamersGate.Tienda.Referido(enlace);
					}
					else if (tienda == APIs.Humble.Tienda.Generar().Id || tienda == APIs.Humble.Tienda.GenerarChoice().Id)
					{
						enlace = APIs.Humble.Tienda.Referido(enlace);
					}
					else if (tienda == APIs.Gamesplanet.Tienda.GenerarUk().Id ||
						tienda == APIs.Gamesplanet.Tienda.GenerarFr().Id ||
						tienda == APIs.Gamesplanet.Tienda.GenerarDe().Id ||
						tienda == APIs.Gamesplanet.Tienda.GenerarUs().Id)
					{
						enlace = APIs.Gamesplanet.Tienda.Referido(enlace);
					}
					else if (tienda == APIs.Fanatical.Tienda.Generar().Id)
					{
						enlace = APIs.Fanatical.Tienda.Referido(enlace);
					}
					else if (tienda == APIs.GreenManGaming.Tienda.Generar().Id || tienda == APIs.GreenManGaming.Tienda.GenerarGold().Id)
					{
						enlace = APIs.GreenManGaming.Tienda.Referido(enlace);
					}
					else if (tienda == APIs.IndieGala.Tienda.Generar().Id)
					{
						enlace = APIs.IndieGala.Tienda.Referido(enlace);
					}
					else if (tienda == APIs.WinGameStore.Tienda.Generar().Id)
					{
						enlace = APIs.WinGameStore.Tienda.Referido(enlace);
					}
					else if (tienda == APIs.DLGamer.Tienda.Generar().Id)
					{
						enlace = APIs.DLGamer.Tienda.Referido(enlace);
					}
					else if (tienda == APIs.JoyBuggy.Tienda.Generar().Id)
					{
						enlace = APIs.JoyBuggy.Tienda.Referido(enlace);
					}
					else if (tienda == APIs._2Game.Tienda.Generar().Id)
					{
						enlace = APIs._2Game.Tienda.Referido(enlace);
					}
					else if (tienda == APIs.GameBillet.Tienda.Generar().Id)
					{
						enlace = APIs.GameBillet.Tienda.Referido(enlace);
					}
					else if (tienda == APIs.Playsum.Tienda.Generar().Id)
					{
						enlace = APIs.Playsum.Tienda.Referido(enlace);
					}
				}
			}
			
			return dominio + "/link/" + AcortarEnlace(enlace) + "/";
		}

		public static string Generar(string enlace, BundleTipo tipo, bool usuarioPatreon = false)
		{
			bool usarReferido = true;

			if (usuarioPatreon == true)
			{
				usarReferido = false;
			}

			if (usarReferido == true)
			{
				if (tipo == BundleTipo.HumbleBundle)
				{
					enlace = APIs.Humble.Bundle.Referido(enlace);
				}
				else if (tipo == BundleTipo.Fanatical)
				{
					enlace = APIs.Fanatical.Bundle.Referido(enlace);
				}
			}

			//----------------------------------------

			return dominio + "/link/" + AcortarEnlace(enlace) + "/";
		}

		public static string Generar(string enlace, GratisTipo tipo, bool usuarioPatreon = false)
		{
			bool usarReferido = true;

			if (usuarioPatreon == true)
			{
				usarReferido = false;
			}

			if (usarReferido == true)
			{
				if (tipo == APIs.Steam.Gratis.Generar().Tipo)
				{
					enlace = APIs.Steam.Gratis.Referido(enlace);
				}
				else if (tipo == APIs.Fanatical.Gratis.Generar().Tipo)
				{
					enlace = APIs.Fanatical.Gratis.Referido(enlace);
				}
				else if (tipo == APIs.Humble.Gratis.Generar().Tipo)
				{
					enlace = APIs.Humble.Gratis.Referido(enlace);
				}
				else if (tipo == APIs.IndieGala.Gratis.Generar().Tipo)
				{
					enlace = APIs.IndieGala.Gratis.Referido(enlace);
				}
			}

			//----------------------------------------

			return dominio + "/link/" + AcortarEnlace(enlace) + "/";
		}

		public static string Generar(string enlace, SuscripcionTipo tipo, bool usuarioPatreon = false)
		{
			bool usarReferido = true;

			if (usuarioPatreon == true)
			{
				usarReferido = false;
			}

			if (usarReferido == true)
			{
				if (tipo == SuscripcionTipo.HumbleChoice)
				{
					enlace = APIs.Humble.Suscripcion.Referido(enlace);
				}
			}

			//----------------------------------------

			return dominio + "/link/" + AcortarEnlace(enlace) + "/";
		}
	}
}
