using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppRestaurante.Modelos
{
    public class RespuestaUsuarios
    {
        public int count { get; set; }
        public string next { get; set; }
        public string previous { get; set; }
        public List<Usuario> results { get; set; }
    }
}
