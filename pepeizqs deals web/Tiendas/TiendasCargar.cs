#nullable disable

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
				APIs.GOG.Tienda.Generar(),
                APIs.IndieGala.Tienda.Generar(),
                APIs.WinGameStore.Tienda.Generar(),
				APIs.EA.Tienda.Generar(),
				APIs.DLGamer.Tienda.Generar(),
				APIs.Battlenet.Tienda.Generar()
			};

			return tiendas;
		}

        public static async Task AdminTiendas(string id)
        {
			SqlConnection conexion = Herramientas.BaseDatos.Conectar();

			using (conexion)
			{
				if (id == APIs.Steam.Tienda.Generar().Id)
				{
					await APIs.Steam.Tienda.BuscarOfertas(conexion, true);
				}
				else if (id == APIs.GamersGate.Tienda.Generar().Id)
				{
					await APIs.GamersGate.Tienda.BuscarOfertas(conexion);
				}
				else if (id == APIs.Gamesplanet.Tienda.GenerarUk().Id)
				{
					await APIs.Gamesplanet.Tienda.BuscarOfertasUk(conexion);
				}
				else if (id == APIs.Gamesplanet.Tienda.GenerarFr().Id)
				{
					await APIs.Gamesplanet.Tienda.BuscarOfertasFr(conexion);
				}
				else if (id == APIs.Gamesplanet.Tienda.GenerarDe().Id)
				{
					await APIs.Gamesplanet.Tienda.BuscarOfertasDe(conexion);
				}
				else if (id == APIs.Gamesplanet.Tienda.GenerarUs().Id)
				{
					await APIs.Gamesplanet.Tienda.BuscarOfertasUs(conexion);
				}
				else if (id == APIs.Fanatical.Tienda.Generar().Id)
				{
					await APIs.Fanatical.Tienda.BuscarOfertas(conexion);
				}
				else if (id == APIs.GreenManGaming.Tienda.Generar().Id)
				{
					await APIs.GreenManGaming.Tienda.BuscarOfertas(conexion);
				}
				else if (id == APIs.GOG.Tienda.Generar().Id)
				{
					await APIs.GOG.Tienda.BuscarOfertas(conexion);
				}
				else if (id == APIs.IndieGala.Tienda.Generar().Id)
				{
					await APIs.IndieGala.Tienda.BuscarOfertas(conexion);
				}
				else if (id == APIs.WinGameStore.Tienda.Generar().Id)
				{
					await APIs.WinGameStore.Tienda.BuscarOfertas(conexion);
				}
				else if (id == APIs.EA.Tienda.Generar().Id)
				{
					await APIs.EA.Tienda.BuscarOfertas(conexion);
				}
				else if (id == APIs.DLGamer.Tienda.Generar().Id)
				{
					await APIs.DLGamer.Tienda.BuscarOfertas(conexion);
				}
				else if (id == APIs.Battlenet.Tienda.Generar().Id)
				{
					await APIs.Battlenet.Tienda.BuscarOfertas(conexion);
				}
			}

			conexion.Dispose();
        }

		public static async Task TareasGestionador(SqlConnection conexion, string id)
		{
			if (id == APIs.Steam.Tienda.Generar().Id)
			{
				await APIs.Steam.Tienda.BuscarOfertas(conexion, true);
			}
			else if (id == APIs.GamersGate.Tienda.Generar().Id)
			{
				await APIs.GamersGate.Tienda.BuscarOfertas(conexion);
			}
			else if (id == APIs.Gamesplanet.Tienda.GenerarUk().Id)
			{
				await APIs.Gamesplanet.Tienda.BuscarOfertasUk(conexion);
			}
			else if (id == APIs.Gamesplanet.Tienda.GenerarFr().Id)
			{
				await APIs.Gamesplanet.Tienda.BuscarOfertasFr(conexion);
			}
			else if (id == APIs.Gamesplanet.Tienda.GenerarDe().Id)
			{
				await APIs.Gamesplanet.Tienda.BuscarOfertasDe(conexion);
			}
			else if (id == APIs.Gamesplanet.Tienda.GenerarUs().Id)
			{
				await APIs.Gamesplanet.Tienda.BuscarOfertasUs(conexion);
			}
			else if (id == APIs.Fanatical.Tienda.Generar().Id)
			{
				await APIs.Fanatical.Tienda.BuscarOfertas(conexion);
			}
			else if (id == APIs.GreenManGaming.Tienda.Generar().Id)
			{
				await APIs.GreenManGaming.Tienda.BuscarOfertas(conexion);
			}
			else if (id == APIs.GOG.Tienda.Generar().Id)
			{
				await APIs.GOG.Tienda.BuscarOfertas(conexion);
			}
			else if (id == APIs.IndieGala.Tienda.Generar().Id)
			{
				await APIs.IndieGala.Tienda.BuscarOfertas(conexion);
			}
			else if (id == APIs.WinGameStore.Tienda.Generar().Id)
			{
				await APIs.WinGameStore.Tienda.BuscarOfertas(conexion);
			}
			else if (id == APIs.EA.Tienda.Generar().Id)
			{
				await APIs.EA.Tienda.BuscarOfertas(conexion);
			}
			else if (id == APIs.DLGamer.Tienda.Generar().Id)
			{
				await APIs.DLGamer.Tienda.BuscarOfertas(conexion);
			}
			else if (id == APIs.Battlenet.Tienda.Generar().Id)
			{
				await APIs.Battlenet.Tienda.BuscarOfertas(conexion);
			}
		}
    }
}
