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
            var pedidosResponse = await _httpClient.GetAsync("pedidos/");
            var platosResponse = await _httpClient.GetAsync("platos/");

            if (!pedidosResponse.IsSuccessStatusCode || !platosResponse.IsSuccessStatusCode)
                throw new Exception("Error al obtener pedidos o platos");

            var pedidosJson = await pedidosResponse.Content.ReadAsStringAsync();
            var platosJson = await platosResponse.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var pedidosDTO = JsonSerializer.Deserialize<RespuestaPedidosReporteDTO>(pedidosJson, options)?.Results ?? new();
            var platosResponseData = JsonSerializer.Deserialize<RespuestaPlatos>(platosJson, options);
            var platos = platosResponseData?.Results ?? new List<Plato>();


            // Convertir DTO a PedidoReporte con Plato real
            var pedidos = pedidosDTO.Select(dto =>
            {
                var plato = platos.FirstOrDefault(p => p.Id == dto.Plato);
                return new PedidoReporte
                {
                    Id = dto.Id,
                    Cantidad = dto.Cantidad,
                    Estado = dto.Estado,
                    Fecha = dto.Fecha,
                    Plato = plato ?? new Plato { Id = dto.Plato, Name = $"Plato #{dto.Plato}" }
                };
            }).ToList();

            return pedidos;
        }
    }
}
