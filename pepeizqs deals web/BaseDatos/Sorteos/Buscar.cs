#nullable disable

using Microsoft.Data.SqlClient;
using Newtonsoft.Json;

namespace BaseDatos.Sorteos
{
	public static class Buscar
	{
		public static List<Sorteos2.Sorteo> Todos()
		{
			List<Sorteos2.Sorteo> sorteos = new List<Sorteos2.Sorteo>();

			SqlConnection conexion = Herramientas.BaseDatos.Conectar();

			using (conexion)
			{
				string busqueda = "SELECT * FROM sorteos";

				using (SqlCommand comando = new SqlCommand(busqueda, conexion))
				{
					using (SqlDataReader lector = comando.ExecuteReader())
					{
						while (lector.Read())
						{
							Sorteos2.Sorteo sorteo = new Sorteos2.Sorteo();
							sorteo.Id = lector.GetInt32(0);
							sorteo.JuegoId = lector.GetInt32(1);
							sorteo.GrupoId = lector.GetString(2);
							sorteo.Clave = lector.GetString(3);

                            if (lector.IsDBNull(4) == false)
                            {
                                if (lector.GetString(4) != null)
                                {
                                    try
                                    {
                                        sorteo.Participantes = JsonConvert.DeserializeObject<List<string>>(lector.GetString(4));
                                    }
                                    catch { }
                                }
                            }

							sorteo.FechaTermina = DateTime.Parse(lector.GetString(5));
                            sorteo.GanadorId = lector.GetString(6);

							sorteos.Add(sorteo);
						}
					}
				}
			}

			return sorteos;
		}

        public static Sorteos2.Sorteo Uno(string id)
        {
            SqlConnection conexion = Herramientas.BaseDatos.Conectar();

            using (conexion)
            {
                string busqueda = "SELECT * FROM sorteos WHERE id=@id";

                using (SqlCommand comando = new SqlCommand(busqueda, conexion))
                {
                    comando.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            Sorteos2.Sorteo sorteo = new Sorteos2.Sorteo();
                            sorteo.Id = lector.GetInt32(0);
                            sorteo.JuegoId = lector.GetInt32(1);
                            sorteo.GrupoId = lector.GetString(2);
                            sorteo.Clave = lector.GetString(3);

                            if (lector.IsDBNull(4) == false)
                            {
                                if (lector.GetString(4) != null)
                                {
                                    try
                                    {
                                        sorteo.Participantes = JsonConvert.DeserializeObject<List<string>>(lector.GetString(4));
                                    }
                                    catch { }
                                }
                            }

                            sorteo.FechaTermina = DateTime.Parse(lector.GetString(5));
                            sorteo.GanadorId = lector.GetString(6);

                            return sorteo;
                        }
                    }
                }
            }

            return null;
        }
    }
}
