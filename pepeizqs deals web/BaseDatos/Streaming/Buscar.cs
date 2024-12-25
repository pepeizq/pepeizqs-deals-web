#nullable disable

using Herramientas;
using Juegos;
using Microsoft.Data.SqlClient;
using System.Text.Json;

namespace BaseDatos.Streaming
{
    public static class Buscar
    {
        public static List<JuegoDRM> GeforceNOWDRMs(int idJuego, SqlConnection conexion = null)
        {
            if (conexion == null)
            {
                conexion = Herramientas.BaseDatos.Conectar();
            }
            else
            {
                if (conexion.State != System.Data.ConnectionState.Open)
                {
                    conexion = Herramientas.BaseDatos.Conectar();
                }
            }

            List<JuegoDRM> listaDRMs = new List<JuegoDRM>();

            using (conexion)
            {
                string busqueda = "SELECT drms FROM streaminggeforcenow WHERE idJuego = " + idJuego.ToString();

                using (SqlCommand comando = new SqlCommand(busqueda, conexion))
                {
                    SqlDataReader lector = comando.ExecuteReader();

                    using (lector)
                    {
                        if (lector.Read() == true)
                        {
                            if (lector.IsDBNull(0) == false)
                            {
                                string drmsTexto = lector.GetString(0);
                                List<string> drms = JsonSerializer.Deserialize<List<string>>(drmsTexto);

                                foreach (var drm in drms)
                                {
                                    listaDRMs.Add(JuegoDRM2.Traducir(drm));
                                }
                            }
                        }
                    }
                }
            }

            return listaDRMs;
        }

        public static bool AmazonLuna(int idJuego, SqlConnection conexion = null)
        {
			if (conexion == null)
			{
				conexion = Herramientas.BaseDatos.Conectar();
			}
			else
			{
				if (conexion.State != System.Data.ConnectionState.Open)
				{
					conexion = Herramientas.BaseDatos.Conectar();
				}
			}

			string busqueda = "SELECT nombre FROM streamingamazonluna WHERE idJuego = " + idJuego.ToString();

			using (SqlCommand comando = new SqlCommand(busqueda, conexion))
			{
				SqlDataReader lector = comando.ExecuteReader();

				using (lector)
				{
					if (lector.Read() == true)
					{
                        return true;
					}
				}
			}

            return false;
		}
    }
}
