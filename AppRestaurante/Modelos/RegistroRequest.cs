﻿namespace AppRestaurante.Modelos
{
    public class RegistroRequest
    {
        public string username { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public int rol_id { get; set; }
    }
}
