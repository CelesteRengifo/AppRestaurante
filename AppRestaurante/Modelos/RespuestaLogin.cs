using Newtonsoft.Json;


namespace AppRestaurante.Modelos
{
    public class RespuestaLogin
    {
        [JsonProperty("actualizar")]
        public string refresh { get; set; }

        [JsonProperty("access")]
        public string access { get; set; }


        public int user_id { get; set; }
        public string username { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string rol { get; set; }
    }
}
