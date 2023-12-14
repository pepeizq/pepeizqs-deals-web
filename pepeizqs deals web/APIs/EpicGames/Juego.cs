#nullable disable

using Herramientas;
using Newtonsoft.Json;

namespace APIs.EpicGames
{
    public static class Juego
    {
        public static async Task<Juegos.Juego> CargarDatos(string enlace)
        {
            string html = await Decompiladores.Estandar("https://store-content-ipv4.ak.epicgames.com/api/en-US/content/products/" + enlace);

            if (html != null)
            {
                EpicJuegoAPI datos = JsonConvert.DeserializeObject<EpicJuegoAPI>(html);

                if (datos != null) 
                {
                    Juegos.Juego juego = Juegos.JuegoCrear.Generar();

                    juego.Nombre = datos.Nombre;

                    return juego;
                }
            }

            return null;
        }

        public static bool Detectar(string enlace)
        {
            bool resultado = false;

            if (enlace.Contains("https://store.epicgames.com/es-ES/p/") == true)
            {
                resultado = true;
            }
            else if (enlace.Contains("https://store.epicgames.com/p/") == true)
            {
                resultado = true;
            }

            return resultado;
        }

        public static string LimpiarSlug(string enlace)
        {
            enlace = enlace.Replace("https://store.epicgames.com/es-ES/p/", null);
            enlace = enlace.Replace("https://store.epicgames.com/p/", null);

            if (enlace.Contains("/") == true)
            {
                int int1 = enlace.IndexOf("/");
                enlace = enlace.Remove(int1, enlace.Length - int1);
            }

            if (enlace.Contains("?") == true)
            {
                int int1 = enlace.IndexOf("?");
                enlace = enlace.Remove(int1, enlace.Length - int1);
            }

            string id = enlace.Trim();

            return id;
        }
    }

    #region Clases

    public class EpicJuegoAPI
    {
        [JsonProperty("productName")]
        public string Nombre { get; set; }
    }

    #endregion
}