﻿//https://store.ubisoft.com/on/demandware.store/Sites-es_ubisoft-Site/es_ES/Product-GetAvailabilityAndOwnership?pid=60c30d870d253c1914049e94
//https://xely3u4lod-dsn.algolia.net/1/indexes/es_custom_MFE/query?x-algolia-agent=Algolia%20for%20JavaScript%20(4.8.5)%3B%20Browser&x-algolia-application-id=XELY3U4LOD&x-algolia-api-key=5638539fd9edb8f2c6b024b49ec375bd
//{"query":"5b06a3994e0165fa45ffdcdf"} <-- tienda id


#nullable disable

using Herramientas;
using Juegos;
using Microsoft.Data.SqlClient;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace APIs.Ubisoft
{
    public static class Tienda
    {
        public static Tiendas2.Tienda Generar()
        {
            Tiendas2.Tienda tienda = new Tiendas2.Tienda
            {
                Id = "ubisoft",
                Nombre = "Ubisoft Store",
                ImagenLogo = "/imagenes/tiendas/ubisoft2_logo.webp",
                Imagen300x80 = "/imagenes/tiendas/ubisoft2_300x80.webp",
                ImagenIcono = "/imagenes/tiendas/ubisoft_icono.webp",
                Color = "#0a0a0a",
                AdminEnseñar = true,
                AdminInteractuar = true
            };

            return tienda;
        }

        public static async Task BuscarOfertas(SqlConnection conexion, IDecompiladores decompilador)
        {
			BaseDatos.Admin.Actualizar.Tiendas(Generar().Id, DateTime.Now, 0, conexion);

            //HttpClient cliente = new HttpClient();
            //cliente.BaseAddress = new Uri("https://store.ubisoft.com/");
            //cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //         cliente.DefaultRequestHeaders.Add("X-Algolia-Api-Key", "9258b782262f815cdfee54a00cf69d02");
            //         cliente.DefaultRequestHeaders.Add("X-Algolia-Application-Id", "AVCVYSEJS1");

            //         HttpResponseMessage respuesta = await cliente.GetAsync("https://avcvysejs1-dsn.algolia.net/1/indexes/products_en-us_default?&query=&hitsPerPage=1000");

            //string html = string.Empty;

            //try
            //{
            //	html = await respuesta.Content.ReadAsStringAsync();
            //}
            //catch { }

            //if (string.IsNullOrEmpty(html) == false)
            //{
            //	BaseDatos.Errores.Insertar.Mensaje("test", html);
            //}

            string html = await Decompiladores.Estandar("https://daisycon.io/datafeed/?media_id=350618&standard_id=1&language_code=es&locale_id=17&type=JSON&program_id=14538&html_transform=none&rawdata=false&encoding=utf8&general=false");

            if (string.IsNullOrEmpty(html) == false)
            {
                UbisoftDatos datos = JsonSerializer.Deserialize<UbisoftDatos>(html);

                if (datos != null)
                {
                    if (datos.Datos != null)
                    {
                        if (datos.Datos.Programas != null)
                        {
                            if (datos.Datos.Programas.Count > 0)
                            {
                                if (datos.Datos.Programas[0].Productos != null)
                                {
                                    if (datos.Datos.Programas[0].Productos.Count > 0)
                                    {
                                        int juegos2 = 0;

                                        foreach (UbisoftDatosProducto juego in datos.Datos.Programas[0].Productos)
                                        {
                                            int descuento = Calculadora.SacarDescuento(juego.Datos.PrecioBase, juego.Datos.PrecioRebajado);

                                            if (descuento > 0)
                                            {
                                                string nombre = WebUtility.HtmlDecode(juego.Datos.Nombre);

                                                string enlace = "https://store.ubisoft.com/" + juego.Datos.Id + ".html";

                                                string imagen = "vacio";

                                                if (juego.Datos.Imagenes != null)
                                                {
                                                    if (juego.Datos.Imagenes.Count > 0)
                                                    {
                                                        imagen = juego.Datos.Imagenes[0].Enlace;

                                                        if (imagen.Contains("?") == true)
                                                        {
                                                            int int1 = imagen.IndexOf("?");
                                                            imagen = imagen.Remove(int1, imagen.Length - int1);
                                                        }
                                                    }
                                                }

                                                JuegoPrecio oferta = new JuegoPrecio
                                                {
                                                    Nombre = nombre,
                                                    Enlace = enlace,
                                                    Imagen = imagen,
                                                    Moneda = JuegoMoneda.Euro,
                                                    Precio = juego.Datos.PrecioRebajado,
                                                    Descuento = descuento,
                                                    Tienda = Generar().Id,
                                                    DRM = JuegoDRM.Ubisoft,
                                                    FechaDetectado = DateTime.Now,
                                                    FechaActualizacion = DateTime.Now
                                                };

                                                try
                                                {
                                                    BaseDatos.Tiendas.Comprobar.Resto(oferta, conexion);
                                                }
                                                catch (Exception ex)
                                                {
                                                    BaseDatos.Errores.Insertar.Mensaje(Tienda.Generar().Id, ex, conexion);
                                                }

                                                juegos2 += 1;

                                                try
                                                {
                                                    BaseDatos.Admin.Actualizar.Tiendas(Tienda.Generar().Id, DateTime.Now, juegos2, conexion);
                                                }
                                                catch (Exception ex)
                                                {
                                                    BaseDatos.Errores.Insertar.Mensaje(Tienda.Generar().Id, ex, conexion);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public static async void BuscarJuego(string tiendaId)
        {
            HttpClient cliente = new HttpClient();
            cliente.BaseAddress = new Uri("https://store.ubisoft.com/");
            cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpRequestMessage peticion = new HttpRequestMessage(HttpMethod.Post, "https://xely3u4lod-dsn.algolia.net/1/indexes/eu_custom_MFE/query?x-algolia-agent=Algolia%20for%20JavaScript%20(4.8.5)%3B%20Browser&x-algolia-application-id=XELY3U4LOD&x-algolia-api-key=5638539fd9edb8f2c6b024b49ec375bd");
            peticion.Content = new StringContent("{\"query\":\"" + tiendaId + "\"}",
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
                BaseDatos.Errores.Insertar.Mensaje("test", html);
            }
        }
	}

    public class UbisoftDatos
    {
        [JsonPropertyName("datafeed")]
        public UbisoftDatosProgramas Datos { get; set; }
    }

    public class UbisoftDatosProgramas
    {
        [JsonPropertyName("programs")]
        public List<UbisoftDatosProductos> Programas { get; set; }
    }

    public class UbisoftDatosProductos
    {
        [JsonPropertyName("products")]
        public List<UbisoftDatosProducto> Productos { get; set; }
    }

    public class UbisoftDatosProducto
    {
        [JsonPropertyName("product_info")]
        public UbisoftDatosProductoInfo Datos { get; set; }
    }

    public class UbisoftDatosProductoInfo
    {
        [JsonPropertyName("description")]
        public string Nombre { get; set; }

        [JsonPropertyName("sku")]
        public string Id { get; set; }

        [JsonPropertyName("price")]
        public decimal PrecioRebajado { get; set; }

        [JsonPropertyName("price_old")]
        public decimal PrecioBase { get; set; }

        [JsonPropertyName("images")]
        public List<UbisoftDatosProductoInfoImagen> Imagenes { get; set; }
    }

    public class UbisoftDatosProductoInfoImagen
    {
        [JsonPropertyName("location")]
        public string Enlace { get; set; }
    }
}
