#nullable disable

using Herramientas;
using Microsoft.Data.SqlClient;

namespace Tiendas2
{
	public static class TiendasCargar
	{
		public static List<Tienda> GenerarListado()
		{
			List<Tienda> tiendas = new List<Tienda>
			{
				APIs.Steam.Tienda.Generar(),
				APIs.GamersGate.Tienda.Generar(),
				APIs.Humble.Tienda.Generar(),
				APIs.Humble.Tienda.GenerarChoice(),
				APIs.Gamesplanet.Tienda.GenerarUk(),
				APIs.Gamesplanet.Tienda.GenerarFr(),
				APIs.Gamesplanet.Tienda.GenerarDe(),
				APIs.Gamesplanet.Tienda.GenerarUs(),
				APIs.Fanatical.Tienda.Generar(),
				APIs.GreenManGaming.Tienda.Generar(),
				APIs.GreenManGaming.Tienda.GenerarGold(),
				APIs.GOG.Tienda.Generar(),
                APIs.IndieGala.Tienda.Generar(),
                APIs.WinGameStore.Tienda.Generar(),
				APIs.EA.Tienda.Generar(),
				APIs.DLGamer.Tienda.Generar(),
				APIs.Battlenet.Tienda.Generar(),
				APIs.JoyBuggy.Tienda.Generar(),
				APIs.Voidu.Tienda.Generar(),
				APIs.EpicGames.Tienda.Generar(),
				APIs._2Game.Tienda.Generar(),
				APIs.GameBillet.Tienda.Generar(),
				APIs.Ubisoft.Tienda.Generar()
            };

			return tiendas;
		}

        public static async Task AdminTiendas(string id, IDecompiladores decompilador)
        {
			SqlConnection conexion = Herramientas.BaseDatos.Conectar();

			using (conexion)
			{
				if (id == APIs.Steam.Tienda.Generar().Id)
				{
					await APIs.Steam.Tienda.BuscarOfertas(conexion, decompilador, true);
				}
				else if (id == APIs.GamersGate.Tienda.Generar().Id)
				{
					await APIs.GamersGate.Tienda.BuscarOfertas(conexion, decompilador);
				}
				else if (id == APIs.Humble.Tienda.Generar().Id)
				{
					await APIs.Humble.Tienda.BuscarOfertas(conexion);
				}
				else if (id == APIs.Gamesplanet.Tienda.GenerarUk().Id)
				{
					await APIs.Gamesplanet.Tienda.BuscarOfertasUk(conexion, decompilador);
				}
				else if (id == APIs.Gamesplanet.Tienda.GenerarFr().Id)
				{
					await APIs.Gamesplanet.Tienda.BuscarOfertasFr(conexion, decompilador);
				}
				else if (id == APIs.Gamesplanet.Tienda.GenerarDe().Id)
				{
					await APIs.Gamesplanet.Tienda.BuscarOfertasDe(conexion, decompilador);
				}
				else if (id == APIs.Gamesplanet.Tienda.GenerarUs().Id)
				{
					await APIs.Gamesplanet.Tienda.BuscarOfertasUs(conexion, decompilador);
				}
				else if (id == APIs.Fanatical.Tienda.Generar().Id)
				{
					await APIs.Fanatical.Tienda.BuscarOfertas(conexion, decompilador);
				}
				else if (id == APIs.GreenManGaming.Tienda.Generar().Id)
				{
					await APIs.GreenManGaming.Tienda.BuscarOfertas(conexion, decompilador);
				}
				else if (id == APIs.GreenManGaming.Tienda.GenerarGold().Id)
				{
					await APIs.GreenManGaming.Tienda.BuscarOfertasGold(conexion, decompilador);
				}
				else if (id == APIs.GOG.Tienda.Generar().Id)
				{
					await APIs.GOG.Tienda.BuscarOfertas(conexion, decompilador);
				}
				else if (id == APIs.IndieGala.Tienda.Generar().Id)
				{
					await APIs.IndieGala.Tienda.BuscarOfertas(conexion, decompilador);
				}
				else if (id == APIs.WinGameStore.Tienda.Generar().Id)
				{
					await APIs.WinGameStore.Tienda.BuscarOfertas(conexion, decompilador);
				}
				else if (id == APIs.EA.Tienda.Generar().Id)
				{
					await APIs.EA.Tienda.BuscarOfertas(conexion, decompilador);
				}
				else if (id == APIs.DLGamer.Tienda.Generar().Id)
				{
					await APIs.DLGamer.Tienda.BuscarOfertas(conexion, decompilador);
				}
				else if (id == APIs.Battlenet.Tienda.Generar().Id)
				{
					await APIs.Battlenet.Tienda.BuscarOfertas(conexion, decompilador);
				}
                else if (id == APIs.JoyBuggy.Tienda.Generar().Id)
                {
                    await APIs.JoyBuggy.Tienda.BuscarOfertas(conexion, decompilador);
                }
				else if (id == APIs.Voidu.Tienda.Generar().Id)
				{
					await APIs.Voidu.Tienda.BuscarOfertas(conexion, decompilador);
				}
				else if (id == APIs.EpicGames.Tienda.Generar().Id)
				{
					await APIs.EpicGames.Tienda.BuscarOfertas(conexion, decompilador);
				}
                else if (id == APIs._2Game.Tienda.Generar().Id)
                {
                    await APIs._2Game.Tienda.BuscarOfertas(conexion, decompilador);
                }
				else if (id == APIs.GameBillet.Tienda.Generar().Id)
				{
					await APIs.GameBillet.Tienda.BuscarOfertas(conexion, decompilador);
				}
                else if (id == APIs.Ubisoft.Tienda.Generar().Id)
                {
                    await APIs.Ubisoft.Tienda.BuscarOfertas(conexion, decompilador);
                }
				else if (id == APIs.Allyouplay.Tienda.Generar().Id)
				{
					await APIs.Allyouplay.Tienda.BuscarOfertas(conexion, decompilador);
				}
			}
        }

		public static async Task TareasGestionador(SqlConnection conexion, string id, IDecompiladores decompilador = null)
		{
			if (id == APIs.Steam.Tienda.Generar().Id)
			{
				await APIs.Steam.Tienda.BuscarOfertas(conexion, decompilador, true);
			}
			else if (id == APIs.GamersGate.Tienda.Generar().Id)
			{
				await APIs.GamersGate.Tienda.BuscarOfertas(conexion, decompilador);
			}
			else if (id == APIs.Humble.Tienda.Generar().Id)
			{
				await APIs.Humble.Tienda.BuscarOfertas(conexion);
			}
			else if (id == APIs.Gamesplanet.Tienda.GenerarUk().Id)
			{
				await APIs.Gamesplanet.Tienda.BuscarOfertasUk(conexion, decompilador);
			}
			else if (id == APIs.Gamesplanet.Tienda.GenerarFr().Id)
			{
				await APIs.Gamesplanet.Tienda.BuscarOfertasFr(conexion, decompilador);
			}
			else if (id == APIs.Gamesplanet.Tienda.GenerarDe().Id)
			{
				await APIs.Gamesplanet.Tienda.BuscarOfertasDe(conexion, decompilador);
			}
			else if (id == APIs.Gamesplanet.Tienda.GenerarUs().Id)
			{
				await APIs.Gamesplanet.Tienda.BuscarOfertasUs(conexion, decompilador);
			}
			else if (id == APIs.Fanatical.Tienda.Generar().Id)
			{
				await APIs.Fanatical.Tienda.BuscarOfertas(conexion, decompilador);
			}
			else if (id == APIs.GreenManGaming.Tienda.Generar().Id)
			{
				await APIs.GreenManGaming.Tienda.BuscarOfertas(conexion, decompilador);
			}
			else if (id == APIs.GreenManGaming.Tienda.GenerarGold().Id)
			{
				await APIs.GreenManGaming.Tienda.BuscarOfertasGold(conexion, decompilador);
			}
			else if (id == APIs.GOG.Tienda.Generar().Id)
			{
				await APIs.GOG.Tienda.BuscarOfertas(conexion, decompilador);
			}
			else if (id == APIs.IndieGala.Tienda.Generar().Id)
			{
				await APIs.IndieGala.Tienda.BuscarOfertas(conexion, decompilador);
			}
			else if (id == APIs.WinGameStore.Tienda.Generar().Id)
			{
				await APIs.WinGameStore.Tienda.BuscarOfertas(conexion, decompilador);
			}
			else if (id == APIs.EA.Tienda.Generar().Id)
			{
				await APIs.EA.Tienda.BuscarOfertas(conexion, decompilador);
			}
			else if (id == APIs.DLGamer.Tienda.Generar().Id)
			{
				await APIs.DLGamer.Tienda.BuscarOfertas(conexion, decompilador);
			}
			else if (id == APIs.Battlenet.Tienda.Generar().Id)
			{
				await APIs.Battlenet.Tienda.BuscarOfertas(conexion, decompilador);
			}
            else if (id == APIs.JoyBuggy.Tienda.Generar().Id)
            {
                await APIs.JoyBuggy.Tienda.BuscarOfertas(conexion, decompilador);
            }
			else if (id == APIs.Voidu.Tienda.Generar().Id)
			{
				await APIs.Voidu.Tienda.BuscarOfertas(conexion, decompilador);
			}
			else if (id == APIs.EpicGames.Tienda.Generar().Id)
			{
				await APIs.EpicGames.Tienda.BuscarOfertas(conexion, decompilador);
			}
            else if (id == APIs._2Game.Tienda.Generar().Id)
            {
                await APIs._2Game.Tienda.BuscarOfertas(conexion, decompilador);
            }
			else if (id == APIs.GameBillet.Tienda.Generar().Id)
			{
				await APIs.GameBillet.Tienda.BuscarOfertas(conexion, decompilador);
			}
            else if (id == APIs.Ubisoft.Tienda.Generar().Id)
            {
                await APIs.Ubisoft.Tienda.BuscarOfertas(conexion, decompilador);
            }
			else if (id == APIs.Allyouplay.Tienda.Generar().Id)
			{
				await APIs.Allyouplay.Tienda.BuscarOfertas(conexion, decompilador);
			}
		}
    }
}
