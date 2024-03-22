#nullable disable

using BaseDatos.Tiendas;
using Microsoft.Data.SqlClient;
using MimeKit;

namespace Tareas
{
	public class DatosAdmin
	{
		public static async Task Ejecutar(SqlConnection conexion)
		{
			await Task.Delay(1000);

			TimeSpan tiempoSiguiente = TimeSpan.FromMinutes(30);

			if (Admin.ComprobarTareaUso(conexion, "datos", tiempoSiguiente) == true)
			{
				Admin.ActualizarTareaUso(conexion, "datos", DateTime.Now);

				List<Herramientas.CorreoConId> correos = new List<Herramientas.CorreoConId>();

				correos = Herramientas.Correos.ComprobarNuevosCorreos();

				Admin.ActualizarDato(conexion, "correos", correos.Count.ToString());

				int pendientes = BaseDatos.Pendientes.Buscar.Todos(conexion).Count;

                Admin.ActualizarDato(conexion, "pendientes", pendientes.ToString());

				int errores = BaseDatos.Errores.Buscar.Todos(conexion).Count;

                Admin.ActualizarDato(conexion, "errores", errores.ToString());

                int dlcs = BaseDatos.Juegos.Buscar.DLCs(null, conexion, false).Count;

                Admin.ActualizarDato(conexion, "dlcs", dlcs.ToString());
            }
		}
	}
}
