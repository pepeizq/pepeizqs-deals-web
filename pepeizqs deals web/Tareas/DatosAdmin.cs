#nullable disable

using BaseDatos.Tiendas;
using Microsoft.Data.SqlClient;

namespace Tareas
{
	public class DatosAdmin
	{
		public static async Task CorreosDeals(SqlConnection conexion)
		{
			await Task.Delay(1000);

			TimeSpan tiempoSiguiente = TimeSpan.FromMinutes(30);

			if (Admin.ComprobarTareaUso(conexion, "correos", tiempoSiguiente) == true)
			{
				Admin.ActualizarTareaUso(conexion, "correos", DateTime.Now);

                try
                {
                    List<Herramientas.CorreoConId> correosDeals = Herramientas.Correos.ComprobarNuevosCorreos(0);

                    if (correosDeals.Count > 0)
                    {
                        Admin.ActualizarDato(conexion, "correos", correosDeals.Count.ToString());
                    }
                    else
                    {
                        Admin.ActualizarDato(conexion, "correos", "0");
                    }
                }
                catch (Exception ex)
                {
                    BaseDatos.Errores.Insertar.Ejecutar("Tarea - Correos Deals", ex, conexion);
                }
            }
		}

        public static async Task CorreosApps(SqlConnection conexion)
        {
            await Task.Delay(1000);

            TimeSpan tiempoSiguiente = TimeSpan.FromMinutes(30);

            if (Admin.ComprobarTareaUso(conexion, "correos2", tiempoSiguiente) == true)
            {
                Admin.ActualizarTareaUso(conexion, "correos2", DateTime.Now);

                try
                {
                    List<Herramientas.CorreoConId> correosApps = Herramientas.Correos.ComprobarNuevosCorreos(1);

                    if (correosApps.Count > 0)
                    {
                        Admin.ActualizarDato(conexion, "correos2", correosApps.Count.ToString());
                    }
                    else
                    {
                        Admin.ActualizarDato(conexion, "correos2", "0");
                    }
                }
                catch (Exception ex)
                {
                    BaseDatos.Errores.Insertar.Ejecutar("Tarea - Correos Apps", ex, conexion);
                }
            }
        }

        public static async Task Pendientes(SqlConnection conexion)
        {
            await Task.Delay(1000);

            TimeSpan tiempoSiguiente = TimeSpan.FromMinutes(30);

            if (Admin.ComprobarTareaUso(conexion, "pendientes", tiempoSiguiente) == true)
            {
                Admin.ActualizarTareaUso(conexion, "pendientes", DateTime.Now);

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
                    BaseDatos.Errores.Insertar.Ejecutar("Tarea - Pendientes", ex, conexion);
                }
            }
        }

        public static async Task Errores(SqlConnection conexion)
        {
            await Task.Delay(1000);

            TimeSpan tiempoSiguiente = TimeSpan.FromMinutes(30);

            if (Admin.ComprobarTareaUso(conexion, "errores", tiempoSiguiente) == true)
            {
                Admin.ActualizarTareaUso(conexion, "errores", DateTime.Now);

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
                    BaseDatos.Errores.Insertar.Ejecutar("Tarea - Errores", ex, conexion);
                }
            }
        }

        public static async Task DLCs(SqlConnection conexion)
        {
            await Task.Delay(1000);

            TimeSpan tiempoSiguiente = TimeSpan.FromMinutes(30);

            if (Admin.ComprobarTareaUso(conexion, "dlcs", tiempoSiguiente) == true)
            {
                Admin.ActualizarTareaUso(conexion, "dlcs", DateTime.Now);

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
                    BaseDatos.Errores.Insertar.Ejecutar("Tarea - DLCs", ex, conexion);
                }
            }
        }

        public static async Task Solicitudes(SqlConnection conexion)
        {
            await Task.Delay(1000);

            TimeSpan tiempoSiguiente = TimeSpan.FromMinutes(30);

            if (Admin.ComprobarTareaUso(conexion, "solicitudes", tiempoSiguiente) == true)
            {
                Admin.ActualizarTareaUso(conexion, "solicitudes", DateTime.Now);

                try
                {
                    List<BaseDatos.Usuarios.SolicitudGrupo> solicitudes = BaseDatos.Usuarios.Solicitud.DevolverTodo(conexion);

                    if (solicitudes.Count > 0)
                    {
                        Admin.ActualizarDato(conexion, "solicitudes", solicitudes.Count.ToString());
                    }
                    else
                    {
                        Admin.ActualizarDato(conexion, "solicitudes", "0");
                    }
                }
                catch (Exception ex)
                {
                    BaseDatos.Errores.Insertar.Ejecutar("Tarea - Solicitudes", ex, conexion);
                }
            }
        }
	}
}
