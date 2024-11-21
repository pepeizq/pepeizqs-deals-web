//https://store.steampowered.com/search/results/?query&start=350&count=50&dynamic_data=&sort_by=Price_ASC&force_infinite=1&supportedlang=english&specials=1&hidef2p=1&ndl=1&infinite=1&ignore_preferences=1
//https://api.steampowered.com/IStoreQueryService/Query/v1/?input_json={%22query%22:{%22filters%22:{%22tagids_must_match%22:[{%22tagids%22:[%229%22]}]}},%22context%22:{%22language%22:%22english%22,%22country_code%22:%22US%22,%22steam_realm%22:%221%22},%22data_request%22:{%22include_basic_info%22:true}}
//https://store.steampowered.com/saleaction/ajaxgetdeckappcompatibilityreport?nAppID=1868140&l=spanish&cc=ES
//https://api.steampowered.com/ISteamUserStats/GetNumberOfCurrentPlayers/v1/?appid=730
//https://api.steampowered.com/IStoreService/GetAppList/v1/?key=[devkey]&max_results=50000
//https://store.steampowered.com/appreviews/730?json=1

#nullable disable

using Herramientas;
using Juegos;
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace APIs.Steam
{
	public static class Tienda
	{
        public static Tiendas2.Tienda Generar()
		{
			Tiendas2.Tienda tienda = new Tiendas2.Tienda
			{
				Id = "steam",
				Nombre = "Steam",
				ImagenLogo = "/imagenes/tiendas/steam_logo.webp",
				Imagen300x80 = "/imagenes/tiendas/steam_300x80.webp",
				ImagenIcono = "/imagenes/tiendas/steam_icono.ico",
				Color = "#2e4460",
                AdminEnseñar = true,
				AdminInteractuar = true
			};

			return tienda;
		}

        public static string Referido(string enlace)
        {
            return enlace + "?curator_clanid=33500256";
        }

		public static async Task BuscarOfertas(SqlConnection conexion, IDecompiladores decompilador, bool mirarOfertas)
		{
			BaseDatos.Tiendas.Admin.Actualizar(Tienda.Generar().Id, DateTime.Now, "0 ofertas detectadas", conexion);

			string añadirDeck = string.Empty;
			int deck = 0;

			if (DateTime.Now.Hour > 0 && DateTime.Now.Hour < 10)
			{
				Random rnd = new Random();
				int azar = rnd.Next(1, 2);

				if (azar == 1)
				{
					añadirDeck = "&deck_compatibility=3";
					deck = 3;
				}

				if (azar == 2)
				{
					añadirDeck = "&deck_compatibility=2";
					deck = 2;
				}
			}

			int juegos = 0;

			int arranque = 0;
			int tope = 100000;

			//if (mirarOfertas == true)
			//{
			//	arranque = int.Parse(BaseDatos.Tiendas.Admin.CargarValorAdicional(Generar().Id, conexion));

			//	tope = int.Parse(BaseDatos.Tiendas.Admin.CargarValorAdicional2(Generar().Id, conexion));

			//	if (tope == 0)
			//	{
			//		tope = 100000;
			//	}

			//	if (arranque >= (tope - 300))
			//	{
			//		arranque = 0;
			//	}
			//}

			for (int i = arranque; i < tope; i += 50)
			{
				string html = null;

				if (mirarOfertas == true)
				{				
					string html2 = await Decompiladores.Estandar("https://store.steampowered.com/search/results/?query&start=" + i.ToString() + "&count=50&dynamic_data=&force_infinite=1&supportedlang=english&specials=1&hidef2p=1&ndl=1&infinite=1&ignore_preferences=1&l=english" + añadirDeck);

					try
					{
						SteamQueryAPI datos = JsonSerializer.Deserialize<SteamQueryAPI>(html2);

						if (datos != null)
						{
							html = datos.Html;
						}

						tope = datos.Total;
					}
					catch { }

					if (html == null)
					{
						html = html2;
					}

					if (tope == 100000)
					{
						int int1 = html2.IndexOf("total_count");

						if (int1 != -1)
						{
							string temp1 = html.Remove(0, int1);

							int int2 = temp1.IndexOf(":");
							string temp2 = temp1.Remove(0, int2 + 1);

							int int3 = temp2.IndexOf(",");
							string temp3 = temp2.Remove(int3, temp2.Length - int3);

							tope = int.Parse(temp3.Trim());
						}
					}
				}
				else
				{
					string html2 = await Decompiladores.Estandar("https://store.steampowered.com/search/results/?query&start=" + i.ToString() + "&count=50&dynamic_data=&force_infinite=1&supportedlang=english&hidef2p=1&ndl=1&infinite=1&l=english");

					try
					{
						SteamQueryAPI datos = JsonSerializer.Deserialize<SteamQueryAPI>(html2);

						if (datos != null)
						{
							html = datos.Html;
						}
					}
					catch { }

					if (html == null)
					{
						html = html2;
					}
				}

				if (string.IsNullOrEmpty(html) == false)
				{
					try
					{
                        if (html.Contains("<!-- List Items -->") == true)
                        {
                            int int1 = html.IndexOf("<!-- List Items -->");
                            html = html.Remove(0, int1);

                            int int2 = html.IndexOf("<!-- End List Items -->");
                            html = html.Remove(int2, html.Length - int2);

                            int j = 0;
                            while (j < 50)
                            {
                                if (html.Contains("<a href=" + Strings.ChrW(34) + "https://store.steampowered.com/") == true)
                                {
                                    int int3 = html.IndexOf("<a href=" + Strings.ChrW(34) + "https://store.steampowered.com/");
                                    string temp3 = html.Remove(0, int3 + 5);

                                    html = temp3;

                                    int int4 = temp3.IndexOf("</a>");
                                    string temp4 = temp3.Remove(int4, temp3.Length - int4);

                                    int int5 = temp4.IndexOf("<span class=" + Strings.ChrW(34) + "title" + Strings.ChrW(34) + ">");
                                    string temp5 = temp4.Remove(0, int5);

                                    int int6 = temp5.IndexOf("</span>");
                                    string temp6 = temp5.Remove(int6, temp5.Length - int6);

                                    int5 = temp6.IndexOf(">");
                                    temp6 = temp6.Remove(0, int5 + 1);

                                    string titulo = temp6.Trim();
                                    titulo = WebUtility.HtmlDecode(titulo);

                                    int int7 = temp4.IndexOf("https://");
                                    string temp7 = temp4.Remove(0, int7);

                                    int int8 = temp7.IndexOf("?");
                                    string temp8 = temp7.Remove(int8, temp7.Length - int8);

                                    string enlace = temp8.Trim();

                                    if (enlace.Contains("https://store.steampowered.com/app/") == true)
                                    {
                                        enlace = "https://store.steampowered.com/app/" + Juego.LimpiarID(enlace);
                                    }

                                    int int9 = temp4.IndexOf("<img src=");
                                    string temp9 = temp4.Remove(0, int9 + 10);

                                    int int10 = temp9.IndexOf("?");
                                    string temp10 = temp9.Remove(int10, temp9.Length - int10);

                                    string imagen = temp10.Trim();

                                    JuegoAnalisis analisis = new JuegoAnalisis
                                    {
                                        Cantidad = "0",
                                        Porcentaje = "0"
                                    };

                                    if (enlace.Contains("https://store.steampowered.com/app/") == true)
                                    {
                                        int int11 = temp4.IndexOf("data-tooltip-html=");

                                        if (int11 != -1)
                                        {
                                            string temp11 = temp4.Remove(0, int11);

                                            int int12 = temp11.IndexOf("%");
                                            string temp12 = temp11.Remove(int12, temp11.Length - int12);

                                            temp12 = temp12.Remove(0, temp12.Length - 2);
                                            temp12 = temp12.Trim();

                                            if (temp12.Contains(";") == true)
                                            {
                                                temp12 = temp12.Replace(";", "0");
                                            }

                                            if (temp12 == "00")
                                            {
                                                temp12 = "100";
                                            }

                                            string porcentaje = temp12;

                                            int int13 = temp4.IndexOf("data-tooltip-html=");
                                            string temp13 = temp4.Remove(0, int13);

                                            int int14 = temp13.IndexOf("user reviews");
                                            string temp14 = temp13.Remove(int14, temp13.Length - int14);

                                            int14 = temp14.IndexOf("of the");
                                            temp14 = temp14.Remove(0, int14 + 6);

                                            string cantidad = temp14.Trim();

                                            if (cantidad.Length > 1)
                                            {
                                                analisis.Cantidad = cantidad;
                                                analisis.Porcentaje = porcentaje;
                                            }
                                        }
                                    }

                                    if (analisis.Cantidad.Length > 1)
                                    {
                                        int int11 = temp4.IndexOf("data-discount=" + Strings.ChrW(34));

                                        if (int11 != -1)
                                        {
                                            string temp11 = temp4.Remove(0, int11);

                                            int11 = temp11.IndexOf(Strings.ChrW(34));
                                            temp11 = temp11.Remove(0, int11 + 1);

                                            int int12 = temp11.IndexOf(Strings.ChrW(34));
                                            string temp12 = temp11.Remove(int12, temp11.Length - int12);

                                            int descuento = 0;

                                            if (int12 != -1)
                                            {
                                                temp12 = temp12.Replace("-", null);
                                                temp12 = temp12.Replace("%", null);

                                                descuento = int.Parse(temp12.Trim());
                                            }

                                            if (descuento >= 0)
                                            {
                                                int int13 = temp4.IndexOf(Strings.ChrW(34) + "discount_final_price" + Strings.ChrW(34));
                                                string temp13 = temp4.Remove(0, int13);

                                                int13 = temp13.IndexOf(Strings.ChrW(34) + ">");
                                                temp13 = temp13.Remove(0, int13 + 2);

                                                int int14 = temp13.IndexOf("</div>");
                                                string temp14 = temp13.Remove(int14, temp13.Length - int14);

                                                if (temp14 != null)
                                                {
                                                    temp14 = temp14.Replace("--", "00");
                                                    temp14 = temp14.Replace(",", ".");
                                                    temp14 = temp14.Replace("€", null);
                                                }

                                                bool precioFormato = true;

                                                if (temp14.Contains("Free") == true)
                                                {
                                                    precioFormato = false;
                                                }
                                                else if (temp14.Length == 0)
                                                {
                                                    precioFormato = false;
                                                }

                                                if (precioFormato == true)
                                                {
                                                    decimal precio = decimal.Parse(temp14.Trim());

                                                    List<string> etiquetas = new List<string>();

                                                    if (temp4.Contains("data-ds-tagids") == true)
                                                    {
                                                        int int15 = temp4.IndexOf("data-ds-tagids");
                                                        string temp15 = temp4.Remove(0, int15);

                                                        int int16 = temp15.IndexOf("[");
                                                        string temp16 = temp15.Remove(0, int16 + 1);

                                                        int int17 = temp16.IndexOf("]");
                                                        string temp17 = temp16.Remove(int17, temp16.Length - int17);

                                                        etiquetas = Herramientas.Listados.Generar(temp17);
                                                    }

                                                    JuegoPrecio oferta = new JuegoPrecio
                                                    {
                                                        Nombre = titulo,
                                                        Imagen = imagen,
                                                        Tienda = "steam",
                                                        DRM = JuegoDRM.Steam,
                                                        Descuento = descuento,
                                                        Precio = precio,
                                                        Moneda = JuegoMoneda.Euro,
                                                        Enlace = enlace,
                                                        FechaDetectado = DateTime.Now,
                                                        FechaActualizacion = DateTime.Now
                                                    };

                                                    try
                                                    {
                                                        BaseDatos.Tiendas.Comprobar.Steam(oferta, analisis, etiquetas, conexion, deck);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        BaseDatos.Errores.Insertar.Mensaje(Tienda.Generar().Id, ex, conexion);
                                                    }

                                                    juegos += 1;

                                                    try
                                                    {
                                                        BaseDatos.Tiendas.Admin.Actualizar(Tienda.Generar().Id, DateTime.Now, juegos.ToString() + " ofertas detectadas", conexion, i.ToString(), tope.ToString());
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

                                j += 1;
                            }
                        }
                    }
					catch (Exception ex)
					{
						BaseDatos.Errores.Insertar.Mensaje("Steam Ofertas", ex);
					}
				}
			}
		}
	}

	public class SteamQueryAPI
	{
		[JsonPropertyName("results_html")]
		public string Html { get; set; }

		[JsonPropertyName("total_count")]
		public int Total { get; set; }
	}
}
