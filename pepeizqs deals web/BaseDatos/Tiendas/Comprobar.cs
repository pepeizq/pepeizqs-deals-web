#nullable disable

using Juegos;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Data.SqlClient;

namespace BaseDatos.Tiendas
{
	public static class Comprobar
	{
		public static async void Steam(JuegoPrecio oferta, JuegoAnalisis analisis, ViewDataDictionary objeto, SqlConnection conexion)
		{
			Juego juego = JuegoCrear.Generar();

			bool insertar = false;
			bool actualizar = false;

			string idSteam = string.Empty;

			if (oferta.Enlace.Contains("https://store.steampowered.com/app/") == true)
			{
				idSteam = APIs.Steam.Juego.LimpiarID(oferta.Enlace);
			}

			if (string.IsNullOrEmpty(idSteam) == false)
			{
				int numeroId = 0;

				try
				{
					numeroId = int.Parse(idSteam);
				}
				catch
				{

				}

				if (numeroId > 0)
				{
					string buscarJuego = "SELECT * FROM juegos WHERE idSteam=@idSteam";

					using (SqlCommand comando = new SqlCommand(buscarJuego, conexion))
					{
						comando.Parameters.AddWithValue("@idSteam", idSteam);

						using (SqlDataReader lector = comando.ExecuteReader())
						{
							if (lector.Read() == false)
							{
								try
								{
									juego = await APIs.Steam.Juego.CargarDatos(idSteam);
								}
								catch
								{

								}

								if (juego != null)
								{
									if (juego.PrecioActualesTiendas == null)
									{
										juego.PrecioActualesTiendas = new List<JuegoPrecio>();
										juego.PrecioMinimosHistoricos = new List<JuegoPrecio>();
									}

									if (juego.PrecioActualesTiendas.Count == 0)
									{
										juego.PrecioActualesTiendas.Add(oferta);
										juego.PrecioMinimosHistoricos.Add(oferta);
									}

									insertar = true;
								}
								else
								{
									insertar = false;
								}
							}
							else
							{
								juego = Juegos.Buscar.Cargar(juego, lector);

								actualizar = true;
							}
						}
					}

					if (analisis != null && juego != null)
					{
						if (string.IsNullOrEmpty(analisis.Cantidad) == false && string.IsNullOrEmpty(analisis.Porcentaje) == false)
						{
							JuegoAnalisis nuevoAnalisis = new JuegoAnalisis();
							nuevoAnalisis.Cantidad = analisis.Cantidad;
							nuevoAnalisis.Porcentaje = analisis.Porcentaje;

							juego.Analisis = nuevoAnalisis;
						}						
					}

					if (insertar == true && actualizar == false)
					{
						bool insertar2 = true;
						string buscarJuego2 = "SELECT * FROM juegos WHERE idSteam=@idSteam";

						if (conexion.State == System.Data.ConnectionState.Closed)
						{
							conexion = Herramientas.BaseDatos.Conectar();
						}

						using (SqlCommand comando = new SqlCommand(buscarJuego2, conexion))
						{
							comando.Parameters.AddWithValue("@idSteam", juego.IdSteam);

							using (SqlDataReader lector = comando.ExecuteReader())
							{
								if (lector.Read() == true)
								{
									insertar2 = false;
								}
							}
						}

						if (insertar2 == true)
						{
							Juegos.Insertar.Ejecutar(juego, conexion);
						}					
					}

					if (actualizar == true && insertar == false)
					{
						bool actualizarAPI = false;

						DateTime fechaComprobacion = Convert.ToDateTime(juego.FechaSteamAPIComprobacion);
						fechaComprobacion = fechaComprobacion.AddDays(35);

						if (fechaComprobacion < DateTime.Now)
						{
							juego = await ActualizarDatosAPI(juego);
						}

						Juegos.Precios.Actualizar(juego, oferta, conexion, actualizarAPI);
					}
				}
			}
		}

		private static async Task<Juego> ActualizarDatosAPI(Juego juego)
		{
			if (juego.IdSteam > 0)
			{
                Juego nuevoJuego = null;
				
				try
				{
                    nuevoJuego = await APIs.Steam.Juego.CargarDatos(juego.IdSteam.ToString());
                }
				catch { }
				
				if (nuevoJuego != null)
				{
                    juego.Nombre = nuevoJuego.Nombre;
                    juego.Media = nuevoJuego.Media;

					if (juego.Tipo == JuegoTipo.DLC)
					{
						if (string.IsNullOrEmpty(juego.Maestro) == true)
						{
							juego.Maestro = nuevoJuego.Maestro;
						}						
					}
					
					juego.FechaSteamAPIComprobacion = DateTime.Now;
					juego.Categorias = nuevoJuego.Categorias;
					juego.Generos = nuevoJuego.Generos;
                }
			}

			return juego;
		}

		public static void Resto(JuegoPrecio oferta, ViewDataDictionary objeto, SqlConnection conexion, string idGog = null, string slugGOG = null, string slugEpic = null)
		{
			bool encontrado = false;

			string buscarJuegos = "DECLARE @ids NVARCHAR(MAX);\r\n\r\nSET @ids = (SELECT idJuegos FROM tienda" + oferta.Tienda + " WHERE enlace='" + oferta.Enlace + "' AND descartado='no'); \r\n\r\nDECLARE @pos INT; \r\nDECLARE @nextpos INT; \r\nDECLARE @valuelen INT; \r\nDECLARE @tabla TABLE (numero int NOT NULL); \r\n\r\nSELECT @pos = 0, @nextpos = 1; \r\n\r\nWHILE @nextpos > 0\r\nBEGIN\r\n    SELECT @nextpos = charindex(',', @ids, @pos + 1)\r\n    SELECT @valuelen = CASE WHEN @nextpos > 0\r\n                            THEN @nextpos\r\n                            ELSE len(@ids) + 1\r\n                        END - @pos - 1\r\nIF (convert(int, substring(@ids, @pos + 1, @valuelen))) != 0 BEGIN\r\n    INSERT @tabla (numero)\r\n        VALUES (convert(int, substring(@ids, @pos + 1, @valuelen))) END\r\n    SELECT @pos = @nextpos;\r\nEND\r\n\r\nSELECT * FROM juegos WHERE id IN (SELECT numero FROM @tabla);";

			//string buscarJuegos = "DECLARE @ids NVARCHAR(MAX); " + Environment.NewLine + 
			//	"SET @ids = (SELECT idJuegos FROM tienda" + oferta.Tienda + " WHERE enlace='" + oferta.Enlace + "' AND descartado='no'); " + Environment.NewLine +
			//	"DECLARE @pos INT; " + Environment.NewLine +
			//	"DECLARE @nextpos INT; " + Environment.NewLine +
			//	"DECLARE @valuelen INT; " + Environment.NewLine +
			//	"DECLARE @tabla TABLE (numero int NOT NULL); " + Environment.NewLine +
			//	"SELECT @pos = 0, @nextpos = 1; " + Environment.NewLine +
			//	"WHILE @nextpos > 0 " + Environment.NewLine +
			//	"BEGIN " + Environment.NewLine +
			//	"SELECT @nextpos = charindex(',', @ids, @pos + 1) " + Environment.NewLine +
			//	"SELECT @valuelen = CASE WHEN @nextpos > 0 " + Environment.NewLine +
			//	"THEN @nextpos " + Environment.NewLine +
			//	"ELSE len(@ids) + 1 " + Environment.NewLine +                       
			//	"END - @pos - 1 " + Environment.NewLine +
			//	"    IF convert(int, substring(@ids, @pos + 1, @valuelen)) != 0 BEGIN\r\n IF @ids != 0 BEGIN   INSERT @tabla (numero)\r\n        VALUES (convert(int, substring(@ids, @pos + 1, @valuelen)))\r\n END;  END;    SELECT @pos = @nextpos;\r\nEND\r\n\r\nSELECT * FROM juegos WHERE id IN (SELECT numero FROM @tabla);";

			using (SqlCommand comandoBuscar = new SqlCommand(buscarJuegos, conexion))
			{
				using (SqlDataReader lector = comandoBuscar.ExecuteReader())
				{
					while (lector.Read() == true)
					{
						encontrado = true;

						Juego juego = Juegos.Buscar.Cargar(JuegoCrear.Generar(), lector);

						if (juego.PrecioActualesTiendas == null)
						{
							juego.PrecioActualesTiendas = new List<JuegoPrecio>();
							juego.PrecioMinimosHistoricos = new List<JuegoPrecio>();
						}

						if (juego.PrecioActualesTiendas.Count == 0)
						{
							juego.PrecioActualesTiendas.Add(oferta);
							juego.PrecioMinimosHistoricos.Add(oferta);
						}

						if (juego.IdGog == 0)
						{
							if (string.IsNullOrEmpty(idGog) == false)
							{
								juego.IdGog = int.Parse(idGog);
								juego.SlugGOG = slugGOG;
							}
						}
						else
						{
							if (string.IsNullOrEmpty(slugGOG) == false)
							{
								juego.SlugGOG = slugGOG;
							}
						}

						if (string.IsNullOrEmpty(slugEpic) == false)
						{
							juego.SlugEpic = slugEpic;
						}

						Juegos.Precios.Actualizar(juego, oferta, conexion, false);
					}
				}
			}

			if (encontrado == false)
			{
				string sqlAñadir = "INSERT INTO tienda" + oferta.Tienda + " " +
					"(enlace, nombre, imagen, idJuegos, descartado) VALUES " +
					"(@enlace, @nombre, @imagen, @idJuegos, @descartado)";

				using (SqlCommand comandoInsertar = new SqlCommand(sqlAñadir, conexion))
				{
					comandoInsertar.Parameters.AddWithValue("@enlace", oferta.Enlace);
					comandoInsertar.Parameters.AddWithValue("@nombre", oferta.Nombre);
					comandoInsertar.Parameters.AddWithValue("@imagen", oferta.Imagen);
					comandoInsertar.Parameters.AddWithValue("@idJuegos", "0");
					comandoInsertar.Parameters.AddWithValue("@descartado", "no");

					try
					{
						comandoInsertar.ExecuteNonQuery();
					}
					catch (Exception ex)
					{
						Errores.Insertar.Ejecutar("Insertar Tienda", ex);
					}
				}
			}






			//	//Buscar en tabla tienda
			//	List<int> listaIds = new List<int>();
			//string buscarTienda = "SELECT idJuegos FROM tienda" + oferta.Tienda + " WHERE enlace=@enlace";

			//         using (SqlCommand comandoBuscar = new SqlCommand(buscarTienda, conexion))
			//{
			//             comandoBuscar.Parameters.AddWithValue("@enlace", oferta.Enlace);

			//             using (SqlDataReader lector = comandoBuscar.ExecuteReader())
			//	{
			//		if (lector.Read() == true)
			//		{
			//			string tempIds = lector.GetString(0);

			//			if (string.IsNullOrEmpty(tempIds) == false)
			//			{
			//				int i = 0;
			//				while (i < 100)
			//				{
			//					if (tempIds.Contains(",") == true)
			//					{
			//						int int1 = tempIds.IndexOf(",");
			//						string temp1 = tempIds.Remove(int1, tempIds.Length - int1);

			//						listaIds.Add(int.Parse(temp1));

			//						tempIds = tempIds.Remove(0, int1 + 1);
			//					}
			//					else
			//					{
			//						listaIds.Add(int.Parse(tempIds));
			//						break;
			//					}

			//					i += 1;
			//				}
			//			}						
			//		}
			//	}
			//}

			////Insertar en tabla tienda o actualizar juego
			//if (listaIds.Count == 0)
			//{
			//	int idBuscarJuego = 0;

			//	string buscarNombre = "SELECT * FROM juegos WHERE nombreCodigo=@nombreCodigo";

			//	using (SqlCommand comandoBuscar2 = new SqlCommand(buscarNombre, conexion))
			//	{
			//		comandoBuscar2.Parameters.AddWithValue("@nombreCodigo", Herramientas.Buscador.LimpiarNombre(oferta.Nombre));

			//		using (SqlDataReader lector = comandoBuscar2.ExecuteReader())
			//		{
			//			if (lector.Read() == true)
			//			{
			//				idBuscarJuego = lector.GetInt32(0);
			//			}
			//		}
			//	}

			//	string sqlAñadir = "INSERT INTO tienda" + oferta.Tienda + " " +
			//		"(enlace, nombre, imagen, idJuegos, descartado) VALUES " +
			//		"(@enlace, @nombre, @imagen, @idJuegos, @descartado)";

			//	using (SqlCommand comandoInsertar = new SqlCommand(sqlAñadir, conexion))
			//	{
			//		comandoInsertar.Parameters.AddWithValue("@enlace", oferta.Enlace);
			//		comandoInsertar.Parameters.AddWithValue("@nombre", oferta.Nombre);
			//		comandoInsertar.Parameters.AddWithValue("@imagen", oferta.Imagen);
			//		comandoInsertar.Parameters.AddWithValue("@idJuegos", idBuscarJuego);
			//		comandoInsertar.Parameters.AddWithValue("@descartado", "no");

			//		try
			//		{
			//			comandoInsertar.ExecuteNonQuery();
			//		}
			//		catch (Exception ex)
			//		{
			//			Errores.Insertar.Ejecutar("Insertar Tienda", ex);
			//		}
			//	}
			//}
			//else if (listaIds.Count > 0)
			//{
			//	foreach (int id in listaIds)
			//	{
			//		if (id > 0)
			//		{
			//			string buscarJuego = "SELECT * FROM juegos WHERE id=@id";

			//			using (SqlCommand comandoBuscar3 = new SqlCommand(buscarJuego, conexion))
			//			{
			//				comandoBuscar3.Parameters.AddWithValue("@id", id);

			//				using (SqlDataReader lector = comandoBuscar3.ExecuteReader())
			//				{
			//					if (lector.Read() == true)
			//					{
			//                                 Juego juego = JuegoCrear.Generar();

			//						juego = Juegos.Buscar.Cargar(juego, lector);

			//						if (juego.PrecioActualesTiendas == null)
			//						{
			//							juego.PrecioActualesTiendas = new List<JuegoPrecio>();
			//							juego.PrecioMinimosHistoricos = new List<JuegoPrecio>();
			//						}

			//						if (juego.PrecioActualesTiendas.Count == 0)
			//						{
			//							juego.PrecioActualesTiendas.Add(oferta);
			//							juego.PrecioMinimosHistoricos.Add(oferta);
			//						}

			//						if (juego.IdGog == 0)
			//						{
			//							if (string.IsNullOrEmpty(idGog) == false)
			//							{
			//								juego.IdGog = int.Parse(idGog);
			//								juego.SlugGOG = slugGOG;
			//							}
			//						}
			//						else
			//						{
			//                                     if (string.IsNullOrEmpty(slugGOG) == false)
			//                                     {
			//								juego.SlugGOG = slugGOG;
			//							}
			//						}

			//                                 if (string.IsNullOrEmpty(slugEpic) == false)
			//						{
			//							juego.SlugEpic = slugEpic;
			//						}

			//                                 Juegos.Precios.Actualizar(juego, oferta, conexion, false);
			//					}
			//				}
			//			}
			//		}
			//		else
			//		{
			//			string buscarNombre = "SELECT * FROM juegos WHERE nombreCodigo=@nombreCodigo";

			//			SqlCommand comandoBuscar2 = new SqlCommand(buscarNombre, conexion);

			//			using (comandoBuscar2)
			//			{
			//				comandoBuscar2.Parameters.AddWithValue("@nombreCodigo", Herramientas.Buscador.LimpiarNombre(oferta.Nombre));

			//				using (SqlDataReader lector = comandoBuscar2.ExecuteReader())
			//				{
			//					if (lector.Read() == true)
			//					{
			//						int idBuscarJuego2 = lector.GetInt32(0);

			//						string sqlActualizar = "UPDATE tienda" + oferta.Tienda + " " +
			//								"SET idJuegos=@idJuegos WHERE enlace=@enlace";

			//						using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
			//						{
			//							comando.Parameters.AddWithValue("@idJuegos", idBuscarJuego2);
			//							comando.Parameters.AddWithValue("@enlace", oferta.Enlace);

			//							try
			//							{
			//                                         comando.ExecuteNonQuery();
			//                                     }
			//							catch
			//							{

			//							}
			//						}
			//					}
			//				}
			//			}
			//		}
			//	}
			//}
		}
	}
}
