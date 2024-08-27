#nullable disable

using BaseDatos.Publishers;
using Herramientas;
using Microsoft.VisualBasic;

namespace APIs.Steam
{
    public static class Curator
    {
        public static async Task<Publisher> SacarPublisher(string id)
        {
            string html = await Decompiladores.Estandar("https://store.steampowered.com/publisher/" + id);

            if (string.IsNullOrEmpty(html) == false)
            {
                Publisher publisher = new Publisher
                {
                    Id = id
                };

                if (html.Contains("<title>") == true)
                {
                    int int1 = html.IndexOf("<title>");
                    string temp1 = html.Remove(0, int1 + 7);

                    int int2 = temp1.IndexOf("</title>");
                    string temp2 = temp1.Remove(int2, temp1.Length - int2);

                    temp2 = temp2.Replace("Steam Publisher:", null);

                    publisher.Nombre = temp2.Trim();
                }

                if (publisher.Nombre == "Steam Search")
                {
                    BaseDatos.Errores.Insertar.Mensaje("Publisher no encontrado", id);
                    return null;
                }
                else
                {
                    if (html.Contains("name=" + Strings.ChrW(34) + "Description" + Strings.ChrW(34)) == true)
                    {
                        int int1 = html.IndexOf("name=" + Strings.ChrW(34) + "Description" + Strings.ChrW(34));
                        string temp1 = html.Remove(0, int1 + 10);

                        int int2 = temp1.IndexOf("content=" + Strings.ChrW(34));
                        string temp2 = temp1.Remove(0, int2 + 9);

                        int int3 = temp2.IndexOf(Strings.ChrW(34));
                        string temp3 = temp2.Remove(int3, temp2.Length - int3);

                        publisher.Descripcion = temp3.Trim();
                    }

                    if (html.Contains("property=" + Strings.ChrW(34) + "og:image" + Strings.ChrW(34)) == true)
                    {
                        int int1 = html.IndexOf("property=" + Strings.ChrW(34) + "og:image" + Strings.ChrW(34));
                        string temp1 = html.Remove(0, int1 + 10);

                        int int2 = temp1.IndexOf("content=" + Strings.ChrW(34));
                        string temp2 = temp1.Remove(0, int2 + 9);

                        int int3 = temp2.IndexOf(Strings.ChrW(34));
                        string temp3 = temp2.Remove(int3, temp2.Length - int3);

                        publisher.Imagen = temp3.Trim();
                    }

                    return publisher;
                }               
            }

            return null;
        }
    }
}
