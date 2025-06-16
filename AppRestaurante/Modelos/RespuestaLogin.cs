using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppRestaurante.Modelos
{
    public class RespuestaLogin
    {
        public string access { get; set; }
        public string refresh { get; set; }
        public int user_id { get; set; }
        public string username { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string rol { get; set; }
    }
}
