using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using AppRestaurante.Modelos;
using Microsoft.Maui.Storage;


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
            var json = await response.Content.ReadAsStringAsync();

            Console.WriteLine("Código: " + response.StatusCode);
            Console.WriteLine("Respuesta: " + json);

            if (response.IsSuccessStatusCode)
            {
                var loginData = JsonConvert.DeserializeObject<RespuestaLogin>(json);

                // Verifica antes de guardar
                if (loginData != null && loginData.access != null && loginData.refresh != null)
                {
                    await SecureStorage.SetAsync("access_token", loginData.access);
                    await SecureStorage.SetAsync("refresh_token", loginData.refresh);
                }
                else
                {
                    Console.WriteLine("Error: Token vacío o null");
                }

                return loginData;
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
