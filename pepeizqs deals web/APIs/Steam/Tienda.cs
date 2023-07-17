#nullable disable

using Herramientas;
using Juegos;
using Microsoft.VisualBasic;

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

            int numPaginas = await GenerarNumPaginas("https://store.steampowered.com/search/?sort_by=Price_ASC&specials=1&page=1&l=english");

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
