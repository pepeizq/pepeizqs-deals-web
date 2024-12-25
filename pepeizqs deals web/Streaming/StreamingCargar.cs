namespace Streaming2
{
    public enum StreamingTipo
    {
        GeforceNOW,
        AmazonLuna
    }

    public class StreamingCargar
    {
        public static List<Streaming> GenerarListado()
        {
            List<Streaming> streaming = new List<Streaming>
            {
                APIs.GeforceNOW.Streaming.Generar(),
                APIs.GOG.Streaming.Generar()
            };

            return streaming;
        }

    }
}
