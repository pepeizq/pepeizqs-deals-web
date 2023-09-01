using Juegos;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;

namespace BaseDatos.Bundles
{
	public static class Buscar
	{
		public static List<Bundles2.Bundle> Todos(Herramientas.Tiempo tiempo)
		{
			List<Bundles2.Bundle> bundles = new List<Bundles2.Bundle>();

			SqlConnection conexion = Herramientas.BaseDatos.Conectar();

			using (conexion)
			{
				conexion.Open();

				string busqueda = "SELECT * FROM bundles";

				using (SqlCommand comando = new SqlCommand(busqueda, conexion))
				{
					using (SqlDataReader lector = comando.ExecuteReader())
					{
						while (lector.Read())
						{
							Bundles2.Bundle bundle = new Bundles2.Bundle();
							bundle.Id = lector.GetInt32(0);
							bundle.Tipo = Bundles2.BundlesCargar.DevolverBundle(int.Parse(lector.GetString(1))).Tipo;
							bundle.Nombre = lector.GetString(2);
							bundle.Tienda = lector.GetString(3);
							bundle.Imagen = lector.GetString(4);
							bundle.Enlace = lector.GetString(5);
							bundle.FechaEmpieza = Convert.ToDateTime(lector.GetString(6));
							bundle.FechaTermina = Convert.ToDateTime(lector.GetString(7));
							bundle.Juegos = JsonConvert.DeserializeObject<List<Bundles2.BundleJuego>>(lector.GetString(8));
							bundle.Tiers = JsonConvert.DeserializeObject<List<Bundles2.BundleTier>>(lector.GetString(9));
							bundle.Pick = Convert.ToBoolean(lector.GetString(10));

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

			conexion.Dispose();

			return bundles;
		}
	}
}
