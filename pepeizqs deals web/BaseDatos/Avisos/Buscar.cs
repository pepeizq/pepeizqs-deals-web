﻿#nullable disable

using Microsoft.Data.SqlClient;

namespace BaseDatos.Avisos
{
    public static class Buscar
    {
        public static Aviso Ejecutar(string idioma, SqlConnection conexion)
        {
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
                            Aviso aviso = new Aviso
                            {
                                Mensaje = lector.GetString(1),
                                Fecha = DateTime.Parse(lector.GetString(2))
                            };

                            return aviso;
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
    }
}
