using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using AppRestaurante.Modelos;

namespace AppRestaurante.Servicios
{
    internal class InsumoService
    {
        private readonly HttpClient _httpClient;

        public InsumoService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://lp5-backend.jmtqu4.easypanel.host/api/")
            };
        }

        public async Task<bool> AgregarInsumoAsync(Insumo insumo)
        {
            var json = System.Text.Json.JsonSerializer.Serialize(insumo);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("insumos/", content);

            var respuestaTexto = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Respuesta del backend:");
            Console.WriteLine(respuestaTexto);

            return response.IsSuccessStatusCode;
        }

        public async Task<List<Insumo>> ObtenerInsumosAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("insumos/");
                if (response.IsSuccessStatusCode)
                {
                    var resultado = await response.Content.ReadFromJsonAsync<ApiResultado<Insumo>>();
                    return resultado?.results ?? new List<Insumo>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener insumos: " + ex.Message);
            }
            return new List<Insumo>();
        }

        public async Task<bool> EliminarInsumoAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"insumos/{id}/");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar insumo {id}: " + ex.Message);
                return false;
            }
        }

        public class ApiResultado<T>
        {
            public int count { get; set; }
            public List<T> results { get; set; }
        }
    }
}
