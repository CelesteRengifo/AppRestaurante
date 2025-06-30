using System.Text.Json.Serialization;

namespace AppRestaurante.Modelos
{
    public class Insumo
    {
        [JsonPropertyName("id")]
        public int id { get; set; }

        [JsonPropertyName("name")]
        public string name { get; set; }

        [JsonPropertyName("stock")]
        public decimal stock { get; set; }

        [JsonPropertyName("precio")]
        public decimal precio { get; set; }

        [JsonPropertyName("unidadMedida")]
        public string unidadMedida { get; set; }

    }
}
