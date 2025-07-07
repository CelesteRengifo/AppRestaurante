using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppRestaurante.Modelos.Clases_pedidos;


namespace AppRestaurante.Modelos
{
    public class PedidoReporte
    {
        public int Id { get; set; }
        public Plato Plato { get; set; } // Aquí sí viene el objeto
        public int Cantidad { get; set; }
        public DateTime Fecha { get; set; }
        public string Estado { get; set; }
    }
}
