#nullable disable

using Microsoft.Data.SqlClient;

namespace BaseDatos.RedesSociales
{
	public static class Buscar
	{
		public static void PendientesPosteo(SqlConnection conexion = null)
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

			List<string> enlacesBorrar = new List<string>();

			string busqueda = "SELECT * FROM redesSocialesPosteador";

			using (SqlCommand comando = new SqlCommand(busqueda, conexion))
			{
				using (SqlDataReader lector = comando.ExecuteReader())
				{
					while (lector.Read() == true)
					{
						if (lector.IsDBNull(0) == false && lector.IsDBNull(1) == false && lector.IsDBNull(2) == false && lector.IsDBNull(3) == false && lector.IsDBNull(4) == false && lector.IsDBNull(5) == false)
						{
							string enlace = lector.GetString(0);
							enlacesBorrar.Add(enlace);

							int idJuego = lector.GetInt32(1);

							int descuento = lector.GetInt32(2);

							string precio = lector.GetString(3);

							string tipo = lector.GetString(4);

							string tienda = lector.GetString(5);

							string codigoDescuento = string.Empty;

							if (lector.IsDBNull(6) == false)
							{
								codigoDescuento = lector.GetString(6);
							}

							string codigoTexto = string.Empty;

							if (lector.IsDBNull(7) == false)
							{
								codigoTexto = lector.GetString(6);
							}

							Herramientas.RedesSociales.Reddit.Postear(enlace, idJuego, descuento, precio, tipo, tienda, codigoDescuento, codigoDescuento);
						}
					}
				}
			}

			if (enlacesBorrar.Count > 0)
			{
				foreach (var enlace in enlacesBorrar)
				{
					string borrar = "DELETE FROM redesSocialesPosteador WHERE enlace=@enlace";
					
					using (SqlCommand comando = new SqlCommand(borrar, conexion))
					{
						comando.Parameters.AddWithValue("@enlace", enlace);

						comando.ExecuteNonQuery();
					}
				}
			}
		}
	}
}
