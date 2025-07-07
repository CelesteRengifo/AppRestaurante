using AppRestaurante.Modelos;
using AppRestaurante.Modelos.Clases_pedidos;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;

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

        public class PlatoResponse
        {
            public int Count { get; set; }
            public string? Next { get; set; }
            public string? Previous { get; set; }
            public List<Plato> Results { get; set; } = new();
        }

        public async Task<List<Plato>> ObtenerPlatosAsync()
        {
            var todosLosPlatos = new List<Plato>();
            string url = $"{_baseUrl}/platos_del_dia/";

            var opciones = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            while (!string.IsNullOrEmpty(url))
            {
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Error al obtener platos: {response.StatusCode}\n{error}");
                }

                var json = await response.Content.ReadAsStringAsync();
                var pagina = System.Text.Json.JsonSerializer.Deserialize<PlatoResponse>(json, opciones);

                if (pagina?.Results != null)
                {
                    foreach (var p in pagina.Results)
                    {
                        if (!string.IsNullOrWhiteSpace(p.Imagen) && !p.Imagen.StartsWith("http"))
                        {
                            p.Imagen = "https://lp5-backend.jmtqu4.easypanel.host" + p.Imagen;
                        }

                        todosLosPlatos.Add(p);
                    }
                }

                url = pagina?.Next;
            }

            return todosLosPlatos;
        }

        public async Task EnviarPedidoAsync(Pedidos pedido)
        {
            var opciones = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false
            };

            var json = System.Text.Json.JsonSerializer.Serialize(pedido, opciones);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_baseUrl}/pedidos/", content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al enviar pedido: {response.StatusCode}\n{error}");
            }
        }

        private class MesaListResponse
        {
            public List<Mesas> Results { get; set; }
        }

        public async Task<List<Mesas>> ObtenerMesasAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/mesas/");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();

                var opciones = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var datos = System.Text.Json.JsonSerializer.Deserialize<MesaListResponse>(json, opciones);
                return datos?.Results ?? new List<Mesas>();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener mesas desde la API: " + ex.Message);
            }
        }

        public async Task SubirPlatoAsync(string nombre, int precio, Stream imagenStream, string nombreArchivo)
        {
            var form = new MultipartFormDataContent();

            // Agrega los campos de texto
            form.Add(new StringContent(nombre), "name");
            form.Add(new StringContent(precio.ToString(System.Globalization.CultureInfo.InvariantCulture)), "precio");

            // Agrega la imagen como archivo
            var imagenContent = new StreamContent(imagenStream);
            imagenContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg"); // o "image/png" según el tipo

            form.Add(imagenContent, "imagen", nombreArchivo);

            // Envía la solicitud POST
            var response = await _httpClient.PostAsync($"{_baseUrl}/platos/", form);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al subir plato: {response.StatusCode}\n{error}");
            }
        }
    }
}
