using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using AppRestaurante.Modelos.Clases_pedidos;
using System.Text.Json;
using AppRestaurante.Modelos; 

namespace AppRestaurante.Servicios
{
    public class PedidoService
    {
        private readonly HttpClient _httpClient;

        public PedidoService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://lp5-backend.jmtqu4.easypanel.host/api/")
            };
        }

        public async Task<List<PedidoReporte>> ObtenerPedidosAsync()
        {
            var pedidos = new List<PedidoReporte>();
            int page = 1;
            bool hayMas = true;

            var platosResponse = await _httpClient.GetAsync("platos/");
            if (!platosResponse.IsSuccessStatusCode)
                throw new Exception("Error al obtener platos");

            var platosJson = await platosResponse.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var platosResponseData = JsonSerializer.Deserialize<RespuestaPlatos>(platosJson, options);
            var platos = platosResponseData?.Results ?? new List<Plato>();

            while (hayMas)
            {
                var response = await _httpClient.GetAsync($"pedidos/?page={page}");
                if (!response.IsSuccessStatusCode) break;

                var pedidosJson = await response.Content.ReadAsStringAsync();
                var datos = JsonSerializer.Deserialize<RespuestaPedidosReporteDTO>(pedidosJson, options);

                foreach (var dto in datos.Results)
                {
                    var plato = platos.FirstOrDefault(p => p.Id == dto.Plato);
                    pedidos.Add(new PedidoReporte
                    {
                        Id = dto.Id,
                        Cantidad = dto.Cantidad,
                        Estado = dto.Estado,
                        Fecha = dto.Fecha,
                        Plato = plato ?? new Plato { Id = dto.Plato, Name = $"Plato #{dto.Plato}" }
                    });
                }

                hayMas = datos.Next != null;
                page++;
            }

            return pedidos;
        }

    }
}
