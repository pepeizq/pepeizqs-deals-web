#nullable disable

using Herramientas;
using Juegos;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Data.SqlClient;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace APIs.EA
{
    public class Tienda
    {
        public static Tiendas2.Tienda Generar()
        {
            Tiendas2.Tienda tienda = new Tiendas2.Tienda
            {
                Id = "ea",
                Nombre = "EA",
                ImagenLogo = "/imagenes/tiendas/ea_logo.webp",
                Imagen300x80 = "/imagenes/tiendas/ea_300x80.webp",
                ImagenIcono = "/imagenes/tiendas/ea_icono.webp",
                Color = "#ff4747",
                AdminEnseñar = true,
                AdminInteractuar = true
            };

            return tienda;
        }

        public static async Task BuscarOfertas(SqlConnection conexion, IDecompiladores decompilador)
        {
			BaseDatos.Admin.Actualizar.Tiendas(Tienda.Generar().Id, DateTime.Now, "0 ofertas detectadas", conexion);

			int juegos2 = 0;

			string html = await Decompiladores.Estandar("https://api3.origin.com/supercat/GB/en_GB/supercat-PCWIN_MAC-GB-en_GB.json.gz");

			if (string.IsNullOrEmpty(html) == false)
			{
				EABD basedatos = JsonSerializer.Deserialize<EABD>(html);

				if (basedatos != null)
				{
					string superIds = string.Empty;
					int i = 0;
					int total = 0;

					foreach (var juegoEA in basedatos.Juegos)
					{
						superIds = superIds + juegoEA.Id + ",";
						i += 1;

						if (i == 100)
						{
							total += i;
							i = 0;

							superIds = superIds.Remove(superIds.Length - 1, 1);

							string html2 = await Decompiladores.Estandar("https://api1.origin.com/supercarp/rating/offers/anonymous?country=ES&locale=es_ES&pid=&currency=EUR&offerIds=" + superIds);
							AñadirPrecios(html2, basedatos.Juegos, juegos2, conexion);
							superIds = string.Empty;
						}

						if ((total + 1) == basedatos.Juegos.Count)
						{
							if (superIds.Length > 0)
							{
								superIds = superIds.Remove(superIds.Length - 1, 1);

								string html2 = await Decompiladores.Estandar("https://api1.origin.com/supercarp/rating/offers/anonymous?country=ES&locale=es_ES&pid=&currency=EUR&offerIds=" + superIds);
								AñadirPrecios(html2, basedatos.Juegos, juegos2, conexion);
							}
						}
					}
				}
			}
		}

        private static void AñadirPrecios(string html2, List<EABDJuego> basedatos, int juegos2, SqlConnection conexion, ViewDataDictionary objeto = null)
        {
            if (html2 != null)
            {
                StringReader stream = new StringReader(html2);
                XmlSerializer xml = new XmlSerializer(typeof(EAPrecio1));
                EAPrecio1 precio1 = (EAPrecio1)xml.Deserialize(stream);

                foreach (var precioEA in precio1.Precio2)
                {
                    if (precioEA.Precio3 != null) 
                    {
                        decimal precioRebajado = decimal.Parse(precioEA.Precio3.PrecioRebajado);
                        decimal precioBase = decimal.Parse(precioEA.Precio3.PrecioBase);

                        int descuento = Calculadora.SacarDescuento(precioBase, precioRebajado);

                        if (descuento > 0)
                        {
                            foreach (var juegobd in basedatos)
                            {
                                if (precioEA.Id == juegobd.Id)
                                {
                                    string nombre = WebUtility.HtmlDecode(juegobd.i18n.Titulo);

                                    string enlace = "https://www.origin.com/store" + juegobd.Enlace;

                                    string imagen = juegobd.ImagenServidor + juegobd.i18n.ImagenGrande;

                                    JuegoDRM drm = JuegoDRM.EA;

                                    JuegoPrecio oferta = new JuegoPrecio
                                    {
                                        Nombre = nombre,
                                        Enlace = enlace,
                                        Imagen = imagen,
                                        Moneda = JuegoMoneda.Euro,
                                        Precio = precioRebajado,
                                        Descuento = descuento,
                                        Tienda = Generar().Id,
                                        DRM = drm,
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
										BaseDatos.Admin.Actualizar.Tiendas(Tienda.Generar().Id, DateTime.Now, juegos2.ToString() + " ofertas detectadas", conexion);
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

    #region Clases

    public class EABD
    {
        [JsonPropertyName("offers")]
        public List<EABDJuego> Juegos { get; set; }
    }

    public class EABDJuego 
    {
        [JsonPropertyName("offerId")]
        public string Id { get; set; }

        [JsonPropertyName("itemName")]
        public string Titulo { get; set; }

        [JsonPropertyName("offerPath")]
        public string Enlace { get; set; }

        [JsonPropertyName("i18n")]
        public EABDJuegoi18n i18n { get; set; }

        [JsonPropertyName("imageServer")]
        public string ImagenServidor { get; set; }

		[JsonPropertyName("vault")]
		public EABDJuegoSuscripcion Suscripcion { get; set; }

		[JsonPropertyName("premiumVault")]
		public EABDJuegoSuscripcion SuscripcionPremium { get; set; }

		[JsonPropertyName("originDisplayType")]
		public string Tipo { get; set; }

		[JsonPropertyName("mdmItemType")]
		public string Tipo2 { get; set; }
	}

    public class EABDJuegoi18n
    {
        [JsonPropertyName("displayName")]
        public string Titulo { get; set; }

        [JsonPropertyName("packArtMedium")]
        public string ImagenPequeña { get; set; }

        [JsonPropertyName("packArtLarge")]
        public string ImagenGrande { get; set; }
    }

    public class EABDJuegoSuscripcion
    {
        [JsonPropertyName("path")]
        public string Enlace { get; set; }

		[JsonPropertyName("vaultEndDate")]
		public string FechaAcaba { get; set; }
	}


	[XmlRoot("offerRatingResults")]
    public class EAPrecio1
    {
        [XmlElement("offer")]
        public List<EAPrecio2> Precio2 { get; set; }
    }

    public class EAPrecio2
    {
        [XmlElement("offerId")]
        public string Id { get; set; }

        [XmlElement("rating")]
        public EAPrecio3 Precio3 { get; set; }
    }

    public class EAPrecio3
    {
        [XmlElement("finalTotalAmount")]
        public string PrecioRebajado { get; set; }

        [XmlElement("originalTotalPrice")]
        public string PrecioBase { get; set; }
    }

    #endregion
}
