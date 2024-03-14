namespace APIs.SEGA
{
    public static class Gratis
    {
        public static Gratis2.Gratis Generar()
        {
            Gratis2.Gratis sega = new Gratis2.Gratis
            {
                Tipo = Gratis2.GratisTipo.SEGA,
                Nombre = "SEGA",
                ImagenLogo = "/imagenes/tiendas/sega_300x80.webp",
                ImagenIcono = "/imagenes/tiendas/sega_icono.webp",
                DRMDefecto = Juegos.JuegoDRM.Steam,
                DRMEnseñar = true
            };

            DateTime fechaSega = DateTime.Now;
            fechaSega = fechaSega.AddDays(7);

            sega.FechaSugerencia = fechaSega;

            return sega;
        }
    }
}
