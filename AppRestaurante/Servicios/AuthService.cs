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
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "http://lp5-backend.jmtqu4.easypanel.host/api";

        public AuthService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<RespuestaLogin> LoginAsync(LoginRequest datos)
        {
            var contenido = new StringContent(
                JsonConvert.SerializeObject(datos),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync($"{_baseUrl}/login/", contenido);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<RespuestaLogin>(json);
            }

            return null;
        }
        public async Task<string> RegistrarUsuarioAsync(RegistroRequest datos)
        {
            var contenido = new StringContent(
                JsonConvert.SerializeObject(datos),
                Encoding.UTF8,
                "application/json"
            );

            var respuesta = await _httpClient.PostAsync($"{_baseUrl}/register/", contenido);

            if (respuesta.IsSuccessStatusCode)
                return null;

            return await respuesta.Content.ReadAsStringAsync();
        }
    }

}
