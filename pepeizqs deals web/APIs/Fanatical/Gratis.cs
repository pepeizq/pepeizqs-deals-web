namespace APIs.Fanatical
{
    public static class Gratis
    {
        public static Gratis2.Gratis Generar()
        {
            Gratis2.Gratis fanatical = new Gratis2.Gratis
            {
                Id = Gratis2.GratisTipo.Fanatical,
                Nombre = "Fanatical",
                Imagen = "/imagenes/tiendas/fanatical_300x80.webp",
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
    }
}