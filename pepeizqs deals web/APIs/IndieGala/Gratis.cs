namespace APIs.IndieGala
{
    public static class Gratis
    {
        public static Gratis2.Gratis Generar()
        {
            Gratis2.Gratis indiegala = new Gratis2.Gratis
            {
                Tipo = Gratis2.GratisTipo.IndieGala,
                Nombre = "IndieGala",
                ImagenLogo = "/imagenes/tiendas/indiegala_logo.webp",
                ImagenIcono = "/imagenes/tiendas/indiegala_icono.ico",
                DRMDefecto = Juegos.JuegoDRM.Steam,
                DRMEnseñar = true
            };

            DateTime fechaIndiegala = DateTime.Now;
            fechaIndiegala = fechaIndiegala.AddDays(2);

            indiegala.FechaSugerencia = fechaIndiegala;

            return indiegala;
        }

        public static string Referido(string enlace)
        {
            return enlace + "?ref=pepeizq";
        }
    }
}
