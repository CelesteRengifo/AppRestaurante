using AppRestaurante.Modelos;
using Newtonsoft.Json;
using System.Text;

namespace AppRestaurante.Servicios
{
    public class RolService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "http://lp5-backend.jmtqu4.easypanel.host/api";

        public RolService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<Rol>> ObtenerRolesAsync()
        {
            var respuesta = await _httpClient.GetAsync($"{_baseUrl}/roles/");
            if (respuesta.IsSuccessStatusCode)
            {
                var json = await respuesta.Content.ReadAsStringAsync();
                var datos = JsonConvert.DeserializeObject<RespuestaRoles>(json);
                return datos.results;
            }

            return new List<Rol>();
        }

        public async Task<string> RegistrarRolAsync(Rol rol)
        {
            var contenido = new StringContent(
                JsonConvert.SerializeObject(rol),
                Encoding.UTF8,
                "application/json"
            );

            var respuesta = await _httpClient.PostAsync($"{_baseUrl}/roles/", contenido);

            if (respuesta.IsSuccessStatusCode)
                return null;

            var errorJson = await respuesta.Content.ReadAsStringAsync();
            return errorJson;
        }
        public async Task<bool> EliminarRolAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_baseUrl}/roles/{id}/");
            return response.IsSuccessStatusCode;
        }
        public async Task<bool> ActualizarRolAsync(Rol rol)
        {
            var contenido = new StringContent(
                JsonConvert.SerializeObject(rol),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PutAsync($"{_baseUrl}/roles/{rol.id}/", contenido);
            return response.IsSuccessStatusCode;
        }


    }
}
