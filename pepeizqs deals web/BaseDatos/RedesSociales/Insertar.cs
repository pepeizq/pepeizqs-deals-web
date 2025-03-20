#nullable disable

using Juegos;
using Microsoft.Data.SqlClient;

namespace BaseDatos.RedesSociales
{
	public static class Insertar
	{
		public static void Ejecutar(int id, JuegoPrecio juego, string tipo, SqlConnection conexion = null)
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

			string añadirCodigo1 = null;
			string añadirCodigo2 = null;

			if (juego.CodigoDescuento > 0 && string.IsNullOrEmpty(juego.CodigoTexto) == false)
			{
				añadirCodigo1 = ", codigoDescuento, codigoTexto";
				añadirCodigo2 = ", @codigoDescuento, @codigoTexto";
			}

			string sqlAñadir = "INSERT INTO redesSocialesPosteador " +
					"(enlace, idJuego, descuento, precio, tipo, tienda" + añadirCodigo1 + ") VALUES " +
					"(@enlace, @idJuego, @descuento, @precio, @tipo, @tienda" + añadirCodigo2 + ") ";

			using (SqlCommand comando = new SqlCommand(sqlAñadir, conexion))
			{
				comando.Parameters.AddWithValue("@enlace", juego.Enlace);
				comando.Parameters.AddWithValue("@idJuego", id);
				comando.Parameters.AddWithValue("@descuento", juego.Descuento);
				comando.Parameters.AddWithValue("@precio", juego.Precio);
				comando.Parameters.AddWithValue("@tipo", tipo);
				comando.Parameters.AddWithValue("@tienda", juego.Tienda);

				if (juego.CodigoDescuento > 0 && string.IsNullOrEmpty(juego.CodigoTexto) == false)
				{
					comando.Parameters.AddWithValue("@codigoDescuento", juego.CodigoDescuento);
					comando.Parameters.AddWithValue("@codigoTexto", juego.CodigoTexto);
				}

				try
				{
					comando.ExecuteNonQuery();
				}
				catch (Exception ex)
				{
					Errores.Insertar.Mensaje("Añadir Redes Sociales Posteador", ex);
				}
			}
		}
	}
}
