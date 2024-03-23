#nullable disable

using BaseDatos.Tiendas;
using Microsoft.Data.SqlClient;

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

                try
                {
                    List<Herramientas.CorreoConId> correos = Herramientas.Correos.ComprobarNuevosCorreos();

                    if (correos.Count > 0)
                    {
                        Admin.ActualizarDato(conexion, "correos", correos.Count.ToString());
                    }
                    else
                    {
                        Admin.ActualizarDato(conexion, "correos", "0");
                    }
                }
                catch (Exception ex)
                {
                    BaseDatos.Errores.Insertar.Ejecutar("Tarea - Admin", ex, conexion);
                }

                try
                {
                    List<BaseDatos.Pendientes.Pendiente> pendientes = BaseDatos.Pendientes.Buscar.Todos(conexion);

                    if (pendientes.Count > 0)
                    {
                        Admin.ActualizarDato(conexion, "pendientes", pendientes.Count.ToString());
                    }
                    else
                    {
                        Admin.ActualizarDato(conexion, "pendientes", "0");
                    }
                }
                catch (Exception ex)
                {
                    BaseDatos.Errores.Insertar.Ejecutar("Tarea - Admin", ex, conexion);
                }

                try
                {
                    List<BaseDatos.Errores.Error> errores = BaseDatos.Errores.Buscar.Todos(conexion);

                    if (errores.Count > 0)
                    {
                        Admin.ActualizarDato(conexion, "errores", errores.Count.ToString());
                    }
                    else
                    {
                        Admin.ActualizarDato(conexion, "errores", "0");
                    }
                }
                catch (Exception ex)
                {
                    BaseDatos.Errores.Insertar.Ejecutar("Tarea - Admin", ex, conexion);
                }

                try
                {
                    List<Juegos.Juego> dlcs = BaseDatos.Juegos.Buscar.DLCs(null, conexion, false);

                    if (dlcs.Count > 0)
                    {
                        Admin.ActualizarDato(conexion, "dlcs", dlcs.Count.ToString());
                    }
                    else
                    {
                        Admin.ActualizarDato(conexion, "dlcs", "0");
                    }
                }
                catch (Exception ex)
                {
                    BaseDatos.Errores.Insertar.Ejecutar("Tarea - Admin", ex, conexion);
                }
            }
		}
	}
}
