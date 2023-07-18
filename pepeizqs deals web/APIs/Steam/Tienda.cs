#nullable disable

using Herramientas;
using Juegos;
using Microsoft.VisualBasic;
using System.Net;

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
				ImagenLogo = "/imagenes/tiendas/steam_logo.png",
				Imagen300x80 = "/imagenes/tiendas/steam_300x80.png",
				ImagenIcono = "/imagenes/tiendas/steam_icono.ico",
				Color = "#2e4460"
			};

			return tienda;
		}

		public static async Task<List<JuegoPrecio>> BuscarOfertas()
		{
			List<JuegoPrecio> ofertas = new List<JuegoPrecio>();

            //int numPaginas = await GenerarNumPaginas("https://store.steampowered.com/search/?sort_by=Price_ASC&specials=1&page=1&l=english");
            int numPaginas = 20;

            if (numPaginas > 0) 
            {
                int i = 1;
                while (i <= numPaginas) 
                {
                    string html = await Decompiladores.Estandar("https://store.steampowered.com/search/?cc=fr&sort_by=Price_ASC&specials=1&page=" + i.ToString() + "&l=english");

                    if (html != null)
                    {
                        if (html.Contains("<!-- List Items -->") == false)
                        {
                            if (i < numPaginas - 10)
                            {
                                i -= 1;
                            }
                            else
                            {
                                break;
                            }
                        }
                        else
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

                                    int int11 = temp4.IndexOf(Strings.ChrW(34) + "discount_pct" + Strings.ChrW(34));

                                    if (int11 != -1)
                                    {
                                        string temp11 = temp4.Remove(0, int11);

                                        int11 = temp11.IndexOf(">");
                                        temp11 = temp11.Remove(0, int11 + 1);

                                        int int12 = temp11.IndexOf("</div>");
                                        string temp12 = temp11.Remove(int12, temp11.Length - int12);

                                        int descuento = 0;

                                        if (int12 != -1)
                                        {
                                            temp12 = temp12.Replace("-", null);
                                            temp12 = temp12.Replace("%", null);

                                            descuento = int.Parse(temp12.Trim());
                                        }

                                        if (descuento > 0)
                                        {
											int int13 = temp4.IndexOf(Strings.ChrW(34) + "discount_final_price" + Strings.ChrW(34));
											string temp13 = temp4.Remove(0, int13);

											int13 = temp13.IndexOf(Strings.ChrW(34) + ">");
											temp13 = temp13.Remove(0, int13 + 2);

											int int14 = temp13.IndexOf("</div>");
											string temp14 = temp13.Remove(int14, temp13.Length - int14);

                                            if (temp14 != null)
                                            {
												if (temp14.Contains("--") == true)
												{
													temp14 = temp14.Replace("--", "00");
												}

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
                                                    FechaDetectado = DateTime.Now
												};

												ofertas.Add(oferta);
											}
										}                                       
                                    }
								}

                                j += 1;
                            }
                        }
                    }

                    i += 1;
                }
            }

			return ofertas;
		}

        public static async Task<int> GenerarNumPaginas(string enlace)
        {
            int numPaginas = 0;

            string html = await Decompiladores.Estandar(enlace);

            if (html != null)
            {
                if (html.Contains("<div class=" + Strings.ChrW(34) + "search_pagination_right" + Strings.ChrW(34) + ">") == true)
                {
                    int int1 = html.IndexOf("<div class=" + Strings.ChrW(34) + "search_pagination_right" + Strings.ChrW(34) + ">");
                    string temp1 = html.Remove(0, int1);

                    int int2 = temp1.IndexOf("</div>");
                    string temp2 = temp1.Remove(int2, temp1.Length - int2);

                    if (temp2.Contains("<a href=") == true)
                    {
                        int i = 0;
                        while (i < 10)
                        {
                            if (temp2.Contains("<a href=") == true)
                            {
                                int int3 = temp2.IndexOf("<a href=");
                                string temp3 = temp2.Remove(0, int3 + 3);

                                temp2 = temp3;

                                int int4 = temp3.IndexOf(">");
                                string temp4 = temp3.Remove(0, int4 + 1);

                                int int5 = temp4.IndexOf("</a>");
                                string temp5 = temp4.Remove(int5, temp4.Length - int5);

                                if (temp5.Contains("&gt;") == false)
                                {
									if (temp5.Contains("&lt;") == false)
									{
                                        if (int.Parse(temp5.Trim()) > numPaginas)
                                        {
                                            numPaginas = int.Parse(temp5.Trim());
                                        }
									}
								}
                            }
							i += 1;
                        }
                    }


                }
            }

            if (numPaginas == 0) 
            {
                numPaginas = 300;
            }

            return numPaginas;
        }
	}
}
