using AppRestaurante.Modelos;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace AppRestaurante.Servicios
{
    public class UsuarioService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "http://lp5-backend.jmtqu4.easypanel.host/api";

        public UsuarioService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<Usuario>> ObtenerUsuariosAsync()
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/users/");
            if (!response.IsSuccessStatusCode) return new List<Usuario>();

            var json = await response.Content.ReadAsStringAsync();
            var datos = JsonConvert.DeserializeObject<RespuestaUsuarios>(json); // Usa tu modelo contenedor
            return datos.results ?? new List<Usuario>(); // Devuelve la lista dentro de 'results'
        }
        public async Task<bool> CambiarMiContrasenaAsync(string oldPassword, string newPassword)
        {
            var token = await SecureStorage.GetAsync("access_token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var datos = new
            {
                old_password = oldPassword,
                new_password = newPassword
            };

            var contenido = new StringContent(JsonConvert.SerializeObject(datos), Encoding.UTF8, "application/json");
            var respuesta = await _httpClient.PostAsync($"{_baseUrl}/cambiar_la_contraseña/", contenido);

            return respuesta.IsSuccessStatusCode;
        }

        public async Task<bool> ActualizarUsuarioAsync(Usuario usuario)
        {
            var contenido = new StringContent(JsonConvert.SerializeObject(usuario), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"{_baseUrl}/users/{usuario.id}/", contenido);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> EliminarUsuarioAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_baseUrl}/users/{id}/");
            return response.IsSuccessStatusCode;
        }
    }
}
