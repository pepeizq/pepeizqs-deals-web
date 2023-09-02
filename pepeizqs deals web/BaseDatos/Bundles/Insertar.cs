#nullable disable

using Microsoft.Data.SqlClient;
using Newtonsoft.Json;

namespace BaseDatos.Bundles
{
	public static class Insertar
	{
		public static void Ejecutar(Bundles2.Bundle bundle) 
		{
			SqlConnection conexion = Herramientas.BaseDatos.Conectar();

			using (conexion)
			{
				conexion.Open();

				string sqlInsertar = "INSERT INTO bundles " +
					"(bundleTipo, nombre, tienda, imagen, enlace, fechaEmpieza, fechaTermina, juegos, tiers, pick) VALUES " +
					"(@bundleTipo, @nombre, @tienda, @imagen, @enlace, @fechaEmpieza, @fechaTermina, @juegos, @tiers, @pick) ";

				using (SqlCommand comando = new SqlCommand(sqlInsertar, conexion))
				{
					comando.Parameters.AddWithValue("@bundleTipo", bundle.Tipo);
					comando.Parameters.AddWithValue("@nombre", bundle.NombreBundle);
					comando.Parameters.AddWithValue("@tienda", bundle.NombreTienda);
					comando.Parameters.AddWithValue("@imagen", bundle.ImagenBundle);
					comando.Parameters.AddWithValue("@enlace", bundle.Enlace);
					comando.Parameters.AddWithValue("@fechaEmpieza", bundle.FechaEmpieza.ToString());
					comando.Parameters.AddWithValue("@fechaTermina", bundle.FechaTermina.ToString());
					comando.Parameters.AddWithValue("@juegos", JsonConvert.SerializeObject(bundle.Juegos));
					comando.Parameters.AddWithValue("@tiers", JsonConvert.SerializeObject(bundle.Tiers));
					comando.Parameters.AddWithValue("@pick", bundle.Pick.ToString());

					comando.ExecuteNonQuery();
					try
					{
						
					}
					catch
					{

					}
				}

				if (bundle.Juegos != null)
				{
					int id = 0;
					string sqlBuscarId = "SELECT * FROM bundles WHERE id = (SELECT MAX(id) FROM bundles)";

					using (SqlCommand comando = new SqlCommand(sqlBuscarId, conexion))
					{
						using (SqlDataReader lector = comando.ExecuteReader())
						{
							while (lector.Read())
							{
								id = lector.GetInt32(0);
							}
						}
					}

					if (id > 0) 
					{
						if (bundle.Juegos.Count > 0)
						{
							foreach (var juego in bundle.Juegos)
							{
								global::Juegos.Juego juego2 = Juegos.Buscar.UnJuego(juego.JuegoId);

								if (juego2 != null)
								{
									global::Juegos.JuegoBundle juegoBundle = new global::Juegos.JuegoBundle();
									juegoBundle.DRM = juego.DRM;
									juegoBundle.JuegoId = int.Parse(juego.JuegoId);
									juegoBundle.Tier = juego.Tier;
									juegoBundle.BundleId = id;
									juegoBundle.FechaEmpieza = bundle.FechaEmpieza;
									juegoBundle.FechaTermina = bundle.FechaTermina;
									juegoBundle.Imagen = juego.Imagen;
									juegoBundle.Nombre = juego.Nombre;
									juegoBundle.Enlace = bundle.Enlace;
									juegoBundle.Tipo = bundle.Tipo;

									if (juego2.Bundles == null)
									{
										juego2.Bundles = new List<global::Juegos.JuegoBundle>();
									}

									juego2.Bundles.Add(juegoBundle);

									string sqlActualizarJuego = "UPDATE juegos " +
											"SET bundles=@bundles WHERE id=@id";

									using (SqlCommand comando = new SqlCommand(sqlActualizarJuego, conexion))
									{
										comando.Parameters.AddWithValue("@id", juego.JuegoId);
										comando.Parameters.AddWithValue("@bundles", JsonConvert.SerializeObject(juego2.Bundles));

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
						}
					}					
				}		
			}

			conexion.Dispose();
		}
	}
}
