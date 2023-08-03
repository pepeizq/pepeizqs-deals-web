using Quartz;

namespace Herramientas
{
    public class CronGestionador : IJob
    {
        public Task Execute(IJobExecutionContext contexto)
        {
			int orden = global::BaseDatos.Tiendas.Admin.CronLeerOrden();
		
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
					APIs.Fanatical.Tienda.BuscarOfertas();
				}
				else if (orden == 7)
				{
					Divisas.Ejecutar();
				}
			}
			catch (Exception ex)
			{
				throw new JobExecutionException(ex, refireImmediately: true)
				{
					UnscheduleFiringTrigger = true,
					UnscheduleAllTriggers = true
				};
			}

			orden += 1;

            if (orden == 8)
            {
                orden = 0;
            }

			global::BaseDatos.Tiendas.Admin.CronAumentarOrden(orden);

            return Task.FromResult(true);
        }
    }
}
