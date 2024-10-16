#nullable disable

using Microsoft.Data.SqlClient;
using Newtonsoft.Json;

namespace BaseDatos.Bundles
{
	public static class Buscar
	{
		public static Bundles2.Bundle Cargar(SqlDataReader lector)
		{
            Bundles2.Bundle bundle = new Bundles2.Bundle
            {
                Id = lector.GetInt32(0),
                Tipo = Bundles2.BundlesCargar.DevolverBundle(int.Parse(lector.GetString(1))).Tipo,
                NombreBundle = lector.GetString(2),
                NombreTienda = lector.GetString(3),
                ImagenBundle = lector.GetString(4),
                Enlace = lector.GetString(5),
                FechaEmpieza = Convert.ToDateTime(lector.GetString(6)),
                FechaTermina = Convert.ToDateTime(lector.GetString(7)),
                Juegos = JsonConvert.DeserializeObject<List<Bundles2.BundleJuego>>(lector.GetString(8)),
                Tiers = JsonConvert.DeserializeObject<List<Bundles2.BundleTier>>(lector.GetString(9)),
                Pick = Convert.ToBoolean(lector.GetString(10))
            };

            if (lector.IsDBNull(11) == false)
            {
                bundle.ImagenNoticia = lector.GetString(11);
            }

			return bundle;
        }

		public static List<Bundles2.Bundle> Actuales(SqlConnection conexion = null)
		{
			List<Bundles2.Bundle> bundles = new List<Bundles2.Bundle>();

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

			using (conexion)
			{
				string busqueda = "SELECT * FROM bundles WHERE GETDATE() BETWEEN fechaEmpieza AND fechaTermina";

				using (SqlCommand comando = new SqlCommand(busqueda, conexion))
				{
					using (SqlDataReader lector = comando.ExecuteReader())
					{
						while (lector.Read())
						{
							bundles.Add(Cargar(lector));
						}
					}
				}
			}

			if (bundles.Count > 0)
			{
				bundles.Reverse();
			}

			return bundles;
		}

		public static List<Bundles2.Bundle> Año(string año, SqlConnection conexion = null)
		{
			List<Bundles2.Bundle> bundles = new List<Bundles2.Bundle>();

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

			using (conexion)
			{
				string busqueda = "SELECT * FROM bundles WHERE YEAR(fechaEmpieza) = " + año + " AND GETDATE() > fechaTermina";

				using (SqlCommand comando = new SqlCommand(busqueda, conexion))
				{
					using (SqlDataReader lector = comando.ExecuteReader())
					{
						while (lector.Read())
						{
							bundles.Add(Cargar(lector));
						}
					}
				}
			}

			if (bundles.Count > 0)
			{
				bundles.Reverse();
			}

			return bundles;
		}

		public static List<Bundles2.Bundle> UnTipo(string tipo, Herramientas.Tiempo tiempo)
		{
			List<Bundles2.Bundle> bundles = new List<Bundles2.Bundle>();

			SqlConnection conexion = Herramientas.BaseDatos.Conectar();

			using (conexion)
			{
				string busqueda = "SELECT * FROM bundles";

				using (SqlCommand comando = new SqlCommand(busqueda, conexion))
				{
					using (SqlDataReader lector = comando.ExecuteReader())
					{
						while (lector.Read())
						{
							Bundles2.Bundle bundle = Cargar(lector);

                            if (tipo != "0")
							{
								if (bundle.Tipo == Bundles2.BundlesCargar.DevolverBundle(tipo).Tipo)
								{
									if (tiempo == Herramientas.Tiempo.Atemporal)
									{
										bundles.Add(bundle);
									}
									else if (tiempo == Herramientas.Tiempo.Actual)
									{
										if (DateTime.Now >= bundle.FechaEmpieza && DateTime.Now <= bundle.FechaTermina)
										{
											bundles.Add(bundle);
										}
									}
									else if (tiempo == Herramientas.Tiempo.Pasado)
									{
										if (DateTime.Now > bundle.FechaTermina)
										{
											bundles.Add(bundle);
										}
									}
								}
							}												
						}
					}
				}
			}

			return bundles;
		}

		public static Bundles2.Bundle UnBundle(int bundleId)
		{
			SqlConnection conexion = Herramientas.BaseDatos.Conectar();

			using (conexion)
			{
				string busqueda = "SELECT * FROM bundles WHERE id=@id";

				using (SqlCommand comando = new SqlCommand(busqueda, conexion))
				{
					comando.Parameters.AddWithValue("@id", bundleId);

					using (SqlDataReader lector = comando.ExecuteReader())
					{
						while (lector.Read())
						{
							return Cargar(lector);
						}
					}
				}
			}

			return null;
		}

        public static List<Bundles2.Bundle> Ultimos(string cantidad)
        {
            List<Bundles2.Bundle> bundles = new List<Bundles2.Bundle>();

            SqlConnection conexion = Herramientas.BaseDatos.Conectar();

            using (conexion)
            {
                string busqueda = "SELECT TOP " + cantidad + " * FROM bundles ORDER BY id DESC";

                using (SqlCommand comando = new SqlCommand(busqueda, conexion))
                {
                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            bundles.Add(Cargar(lector));
                        }
                    }
                }
            }

            return bundles;
        }
    }
}
