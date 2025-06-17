using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using AppRestaurante.Modelos;

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
