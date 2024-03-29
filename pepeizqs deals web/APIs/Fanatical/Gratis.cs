namespace APIs.Fanatical
{
    public static class Gratis
    {
        public static Gratis2.Gratis Generar()
        {
            Gratis2.Gratis fanatical = new Gratis2.Gratis
            {
                Tipo = Gratis2.GratisTipo.Fanatical,
                Nombre = "Fanatical",
                ImagenLogo = "/imagenes/tiendas/fanatical_300x80.webp",
				ImagenIcono = "/imagenes/tiendas/fanatical_icono.ico",
				DRMDefecto = Juegos.JuegoDRM.Steam,
                DRMEnseñar = true
            };

            DateTime fechaFanatical = DateTime.Now;
            fechaFanatical = fechaFanatical.AddDays(2);

            fanatical.FechaSugerencia = fechaFanatical;

            return fanatical;
        }

        public static string Referido(string enlace)
        {
            return enlace + "?ref=pepeizq";
        }

        public static Gratis2.Gratis GenerarAntiguo()
        {
            Gratis2.Gratis fanatical = new Gratis2.Gratis
            {
                Tipo = Gratis2.GratisTipo.BundleStars,
                Nombre = "BundleStars",
                ImagenLogo = "/imagenes/tiendas/bundlestars_300x80.webp",
                ImagenIcono = "/imagenes/tiendas/bundlestars_icono.webp",
                DRMDefecto = Juegos.JuegoDRM.Steam,
                DRMEnseñar = true
            };

            DateTime fechaFanatical = DateTime.Now;
            fechaFanatical = fechaFanatical.AddDays(2);

            fanatical.FechaSugerencia = fechaFanatical;

            return fanatical;
        }
    }
}