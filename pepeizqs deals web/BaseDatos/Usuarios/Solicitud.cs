#nullable disable

using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using pepeizqs_deals_web.Areas.Identity.Componentes;
using pepeizqs_deals_web.Areas.Identity.Data;

namespace BaseDatos.Usuarios
{
	public static class Solicitud
	{
		public static void Insertar(string idUsuario, string correo, string usuarioSteam, string avatarSteam, string nicknameSteam, 
            string mensaje, string cantidad, string fecha, SqlConnection conexion)
		{
            string sqlInsertar = "INSERT INTO solicitudes " +
                "(id, correo, usuarioSteam, avatarSteam, nicknameSteam, bundleMensaje, cantidadMensaje, fechaMensaje, aprobado) VALUES " +
                "(@id, @correo, @usuarioSteam, @avatarSteam, @nicknameSteam, @bundleMensaje, @cantidadMensaje, @fechaMensaje, @aprobado) ";

            using (SqlCommand comando = new SqlCommand(sqlInsertar, conexion))
            {
                comando.Parameters.AddWithValue("@id", idUsuario);
                comando.Parameters.AddWithValue("@correo", correo);
                comando.Parameters.AddWithValue("@usuarioSteam", usuarioSteam);
                comando.Parameters.AddWithValue("@avatarSteam", avatarSteam);
                comando.Parameters.AddWithValue("@nicknameSteam", nicknameSteam);
                comando.Parameters.AddWithValue("@bundleMensaje", mensaje);
                comando.Parameters.AddWithValue("@cantidadMensaje", cantidad);
                comando.Parameters.AddWithValue("@fechaMensaje", fecha);
				comando.Parameters.AddWithValue("@aprobado", "false");

				try
                {
                    comando.ExecuteNonQuery();
                }
                catch
                {

                }
            }
        }

        public static bool ComprobarEstadoUsuario(string idUsuario, SqlConnection conexion)
        {
			string busqueda = "SELECT * FROM solicitudes WHERE id=@id";

			using (SqlCommand comando = new SqlCommand(busqueda, conexion))
            {
				comando.Parameters.AddWithValue("@id", idUsuario);

                using (SqlDataReader lector = comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        if (lector.IsDBNull(0) == false)
                        {
                            if (string.IsNullOrEmpty(lector.GetString(0)) == false)
                            {
                                if (idUsuario == lector.GetString(0))
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

        public static List<SolicitudGrupo> DevolverTodo(SqlConnection conexion)
        {
            List<SolicitudGrupo> solicitudes = new List<SolicitudGrupo>();

			string busqueda = "SELECT * FROM solicitudes WHERE aprobado='false'";

			using (SqlCommand comando = new SqlCommand(busqueda, conexion))
			{
				using (SqlDataReader lector = comando.ExecuteReader())
				{
					while (lector.Read())
					{
						SolicitudGrupo nuevaSolicitud = new SolicitudGrupo
						{
							IdUsuario = lector.GetString(0),
                            Correo = lector.GetString(1),
                            UsuarioSteam = lector.GetString(2),
                            AvatarSteam = lector.GetString(3),
                            BundleMensaje = lector.GetString(4),
                            CantidadMensaje = lector.GetString(5),
                            FechaMensaje = DateTime.Parse(lector.GetString(6)),
                            NicknameSteam = lector.GetString(7),
                            Aprobado = lector.GetString(8)
						};

                        solicitudes.Add(nuevaSolicitud);
					}
				}
			}

            return solicitudes;
		}

        public static void AprobarUsuario(string idUsuario, SqlConnection conexion)
        {
            string sqlActualizar = "UPDATE solicitudes " +
                   "SET aprobado=@aprobado WHERE Id=@Id";

            using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
            {
                comando.Parameters.AddWithValue("@id", idUsuario);
                comando.Parameters.AddWithValue("@aprobado", "true");

                try
                {
                    comando.ExecuteNonQuery();
                }
                catch
                {

                }
            }
        }

		public static void BorrarUsuario(string idUsuario, SqlConnection conexion)
		{
            string sqlBorrar = "DELETE FROM solicitudes " +
                  "WHERE Id=@Id";

            using (SqlCommand comando = new SqlCommand(sqlBorrar, conexion))
            {
                comando.Parameters.AddWithValue("@id", idUsuario);

                try
                {
                    comando.ExecuteNonQuery();
                }
                catch
                {

                }
            }
        }
	}

    public class SolicitudGrupo
    {
        public string IdUsuario;
        public string Correo;
        public string UsuarioSteam;
        public string AvatarSteam;
        public string BundleMensaje;
        public string CantidadMensaje;
        public DateTime FechaMensaje;
        public string NicknameSteam;
        public string Aprobado;
    }
}
