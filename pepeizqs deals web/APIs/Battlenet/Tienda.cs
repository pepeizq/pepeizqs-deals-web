namespace APIs.Battlenet
{
    public class Tienda
    {
        public static Tiendas2.Tienda Generar()
        {
            Tiendas2.Tienda tienda = new Tiendas2.Tienda
            {
                Id = "battlenet",
                Nombre = "Battlenet Store",
                ImagenLogo = "/imagenes/tiendas/battlenet_logo.webp",
                Imagen300x80 = "/imagenes/tiendas/battlenet_300x80.webp",
                ImagenIcono = "/imagenes/tiendas/battlenet_icono.webp",
                Color = "#005aad",
                AdminEnseñar = true,
                AdminInteractuar = true
            };

            return tienda;
        }
    }
}
