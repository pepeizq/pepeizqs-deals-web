#nullable disable

using Microsoft.Data.SqlClient;

namespace BaseDatos.Avisos
{
    public static class Buscar
    {
        public static Aviso Ejecutar(string idioma, SqlConnection conexion = null)
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

			if (idioma == "es-ES")
            {
                idioma = "es";
            }
            else if (idioma != "es")
            {
                idioma = "en";
            }

            string sqlBuscar = "SELECT * FROM avisos WHERE id=@id";

            using (SqlCommand comando = new SqlCommand(sqlBuscar, conexion))
            {
                comando.Parameters.AddWithValue("@id", idioma);

                using (SqlDataReader lector = comando.ExecuteReader())
                {
                    if (lector.Read())
                    {
                        if (lector.IsDBNull(1) == false)
                        {
                            try
                            {
								Aviso aviso = new Aviso
								{
									Mensaje = lector.GetString(1),
									Fecha = DateTime.Parse(lector.GetString(2)),
									Enlace = lector.GetString(3)
								};

								return aviso;
							}
                            catch { }                           
                        }
                    }
                }
            }

            return null;
        }
    }

    public class Aviso
    {
        public string Mensaje;
        public DateTime Fecha;
        public string Enlace;
    }
}
