#nullable disable

using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace APIs.GeforceNOW
{
    public static class Streaming
    {
        public static Streaming2.Streaming Generar()
        {
            Streaming2.Streaming geforcenow = new Streaming2.Streaming
            {
                Id = Streaming2.StreamingTipo.GeforceNOW,
                Nombre = "Geforce NOW",
                ImagenLogo = "/imagenes/streaming/geforcenow_logo.webp",
                ImagenIcono = "/imagenes/streaming/geforcenow_icono.webp"
            };

            return geforcenow;
        }

        public static async Task Buscar(SqlConnection conexion)
        {
            BaseDatos.Admin.Actualizar.Tiendas("geforcenow", DateTime.Now, "0", conexion);

            int cantidad = 0;
            string cadena = string.Empty;

            int i = 0;
            while (i < 10)
            {
                HttpClient cliente = new HttpClient();
                cliente.BaseAddress = new Uri("https://www.nvidia.com/");
                cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string cadenaFinal = string.Empty;
                if (string.IsNullOrEmpty(cadena) == false)
                {
                    cadenaFinal = "after:" + Strings.ChrW(34) + cadena + Strings.ChrW(34) + " ";
                }

                HttpRequestMessage peticion = new HttpRequestMessage(HttpMethod.Post, "https://api-prod.nvidia.com/services/gfngames/v1/gameList");
                peticion.Content = new StringContent("{ apps(country:\"US\" language:\"en_US\" " + cadenaFinal + ") {\r\n        numberReturned\r\n        pageInfo {\r\n          endCursor\r\n          hasNextPage\r\n        }\r\n        items {\r\n        title\r\n        sortName\r\n        \r\n      variants{\r\n        appStore\r\n        publisherName\r\n          }\r\n        }\r\n}}",
                                                    Encoding.UTF8, "application/json");

                HttpResponseMessage respuesta = await cliente.SendAsync(peticion);

                string html = string.Empty;

                try
                {
                    html = await respuesta.Content.ReadAsStringAsync();
                }
                catch { }

                if (string.IsNullOrEmpty(html) == false)
                {
                    GeforceNOWDatos datos = JsonSerializer.Deserialize<GeforceNOWDatos>(html);

                    if (datos != null)
                    {
                        if (datos.Datos != null)
                        {
                            if (datos.Datos.Info != null)
                            {
                                if (datos.Datos.Info.SiguientePagina != null)
                                {
                                    if (datos.Datos.Info.SiguientePagina.Existe == true)
                                    {
                                        cadena = datos.Datos.Info.SiguientePagina.Cadena;
                                    }
                                    else
                                    {
                                        i = 100;
                                    }
                                }

                                foreach (var juego in datos.Datos.Info.Juegos)
                                {
                                    List<string> drms = new List<string>();

                                    foreach (var drm in juego.DRMs)
                                    {
                                        drms.Add(drm.DRM);
                                    }

                                    DateTime fecha = DateTime.Now;
                                    fecha = fecha + TimeSpan.FromDays(1);

                                    bool encontrado = false;

                                    string sqlBuscar = "SELECT * FROM streaminggeforcenow WHERE nombreCodigo=@nombreCodigo";

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

                                    using (SqlCommand comando = new SqlCommand(sqlBuscar, conexion))
                                    {
                                        comando.Parameters.AddWithValue("@nombreCodigo", Herramientas.Buscador.LimpiarNombre(juego.Nombre, true));

                                        using (SqlDataReader lector = comando.ExecuteReader())
                                        {
                                            encontrado = lector.Read();

                                            cantidad += 1;
											BaseDatos.Admin.Actualizar.Tiendas("geforcenow", DateTime.Now, cantidad.ToString(), conexion);
                                        }
                                    }

                                    if (encontrado == true)
                                    {
                                        string sqlActualizar = "UPDATE streaminggeforcenow " +
																"SET fecha=@fecha WHERE nombreCodigo=@nombreCodigo";

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

                                        using (SqlCommand comandoActualizar = new SqlCommand(sqlActualizar, conexion))
                                        {
                                            comandoActualizar.Parameters.AddWithValue("@nombreCodigo", Herramientas.Buscador.LimpiarNombre(juego.Nombre, true));
                                            comandoActualizar.Parameters.AddWithValue("@fecha", fecha);

                                            try
                                            {
                                                comandoActualizar.ExecuteNonQuery();
                                            }
                                            catch
                                            {

                                            }
                                        }
                                    }
                                    else
                                    {
                                        string sqlInsertar = "INSERT INTO streaminggeforcenow " +
                                                            "(nombreCodigo, nombre, drms, fecha) VALUES " +
                                                            "(@nombreCodigo, @nombre, @drms, @fecha) ";

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

                                        using (SqlCommand comandoInsertar = new SqlCommand(sqlInsertar, conexion))
                                        {
                                            comandoInsertar.Parameters.AddWithValue("@nombreCodigo", Herramientas.Buscador.LimpiarNombre(juego.Nombre, true));
                                            comandoInsertar.Parameters.AddWithValue("@nombre", juego.Nombre);
                                            comandoInsertar.Parameters.AddWithValue("@drms", JsonSerializer.Serialize(drms));
                                            comandoInsertar.Parameters.AddWithValue("@fecha", fecha);

                                            try
                                            {
                                                comandoInsertar.ExecuteNonQuery();
                                            }
                                            catch (Exception ex)
                                            {
                                                BaseDatos.Errores.Insertar.Mensaje("Insertar Geforce NOW " + juego.Nombre, ex);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        
                    }
                }

                i += 1;
            }          
        }
    }

    public class GeforceNOWDatos
    {
        [JsonPropertyName("data")]
        public GeforceNOWDatosApps Datos { get; set; }
    }

    public class GeforceNOWDatosApps
    {
        [JsonPropertyName("apps")]
        public GeforceNOWDatosInfo Info { get; set; }
    }

    public class GeforceNOWDatosInfo
    {
        [JsonPropertyName("pageInfo")]
        public GeforceNOWDatosInfoPagina SiguientePagina { get; set; }

        [JsonPropertyName("items")]
        public List<GeforceNOWDatosInfoJuego> Juegos { get; set; }
    }

    public class GeforceNOWDatosInfoPagina
    {
        [JsonPropertyName("endCursor")]
        public string Cadena { get; set; }

        [JsonPropertyName("hasNextPage")]
        public bool Existe { get; set; }
    }

    public class GeforceNOWDatosInfoJuego
    {
        [JsonPropertyName("title")]
        public string Nombre { get; set; }

        [JsonPropertyName("variants")]
        public List<GeforceNOWDatosInfoJuegoDRM> DRMs { get; set; }
    }

    public class GeforceNOWDatosInfoJuegoDRM
    {
        [JsonPropertyName("appStore")]
        public string DRM { get; set; }
    }
}
