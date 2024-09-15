namespace APIs.Humble
{
    public static class Gratis
    {
        public static Gratis2.Gratis Generar()
        {
            Gratis2.Gratis humble = new Gratis2.Gratis
            {
                Tipo = Gratis2.GratisTipo.Humble,
                Nombre = "Humble Store",
                ImagenLogo = "/imagenes/tiendas/humblestore_300x80.webp",
                ImagenIcono = "/imagenes/tiendas/humblestore_icono.ico",
                DRMDefecto = Juegos.JuegoDRM.Steam,
                DRMEnseñar = true
            };

            DateTime fechaHumble = DateTime.Now;
            fechaHumble = fechaHumble.AddDays(7);

            humble.FechaSugerencia = fechaHumble;

            return humble;
        }

        public static string Referido(string enlace)
        {
            enlace = enlace + "?partner=pepeizq";

            enlace = enlace.Replace(":", "%3A");
            enlace = enlace.Replace("/", "%2F");
            enlace = enlace.Replace("/", "%2F");
            enlace = enlace.Replace("?", "%3F");
            enlace = enlace.Replace("=", "%3D");

            return "https://humblebundleinc.sjv.io/c/1382810/2059850/25796?u=" + enlace;
        }
    }
}
