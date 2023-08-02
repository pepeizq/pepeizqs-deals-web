using Quartz;

namespace Herramientas
{
    public class CronGestionador : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            int orden = BaseDatos.Tiendas.Admin.CronLeerOrden();

            try
            {
				if (orden == 0)
				{
					APIs.Steam.Tienda.BuscarOfertas();
				}
				else if (orden == 1)
				{
					APIs.GamersGate.Tienda.BuscarOfertas();
				}
				else if (orden == 2)
				{
					APIs.Gamesplanet.Tienda.BuscarOfertasUk();
				}
				else if (orden == 3)
				{
					APIs.Gamesplanet.Tienda.BuscarOfertasFr();
				}
				else if (orden == 4)
				{
					APIs.Gamesplanet.Tienda.BuscarOfertasDe();
				}
				else if (orden == 5)
				{
					APIs.Gamesplanet.Tienda.BuscarOfertasUs();
				}
				else if (orden == 6)
				{
					Divisas.Ejecutar();
				}
			}
            catch
            {

            }
            
            orden += 1;

            if (orden == 7)
            {
                orden = 0;
            }

            BaseDatos.Tiendas.Admin.CronAumentarOrden(orden);

            return Task.FromResult(true);
        }
    }
}
