using AppRestaurante.Modelos;
using AppRestaurante.Servicios;

namespace AppRestaurante.Paginas.Usuario;

public partial class InicioSesion : ContentPage
{
    private readonly AuthService _authService;
    private bool mostrarClave = false;

    public InicioSesion()
    {
        InitializeComponent();
        _authService = new AuthService();
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        // Animación del botón
        await btnLogin.ScaleTo(0.95, 100);
        await btnLogin.ScaleTo(1, 100);

        var username = entryUsuario.Text?.Trim();
        var password = entryClave.Text;

        // Validación de campos vacíos
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            await DisplayAlert("Campos vacíos", "Por favor complete todos los campos.", "OK");
            return;
        }

        // Validación de longitud mínima
        if (username.Length < 3)
        {
            await DisplayAlert("Usuario inválido", "El nombre de usuario debe tener al menos 3 caracteres.", "OK");
            return;
        }

        if (password.Length < 4)
        {
            await DisplayAlert("Contraseña inválida", "La contraseña debe tener al menos 4 caracteres.", "OK");
            return;
        }

        // Crear objeto para enviar al servicio
        var datos = new LoginRequest
        {
            username = username,
            password = password
        };

        // Llamar al servicio
        var loginInfo = await _authService.LoginAsync(datos);

        if (loginInfo != null)
        {
            // Guardar datos persistentes
            Preferences.Set("accessToken", loginInfo.access);
            Preferences.Set("usuario", loginInfo.username);
            Preferences.Set("rol", loginInfo.rol);
            Preferences.Set("first_name", loginInfo.first_name);
            Preferences.Set("last_name", loginInfo.last_name);

            // Redirigir según el rol
            await RedirigirPorRol(loginInfo.rol, loginInfo.first_name, loginInfo.last_name, loginInfo.access);

            LimpiarCampos();
        }
        else
        {
            await DisplayAlert("Acceso denegado", "Usuario o contraseña incorrectos.", "Intentar de nuevo");
            entryClave.Text = string.Empty; // solo limpiamos clave
            entryClave.Focus();
        }
    }

    private void btnVerClave_Clicked(object sender, EventArgs e)
    {
        mostrarClave = !mostrarClave;
        entryClave.IsPassword = !mostrarClave;
        btnVerClave.Source = mostrarClave ? "ocultar.png" : "ver.png";
    }

    private async Task RedirigirPorRol(string rol, string firstName, string lastName, string token)
    {
        switch (rol.ToLower())
        {
            case "administrador":
                await Shell.Current.GoToAsync("//AdministradorPage");
                break;
            case "mesero":
                await Navigation.PushAsync(new Paginas.Logueo.MeseroPage(firstName, lastName, token));
                break;
            case "cocinero":
                await Shell.Current.GoToAsync("//CocineroPage");
                break;
            default:
                await DisplayAlert("Error", $"Rol desconocido: {rol}", "OK");
                break;
        }
    }

    void LimpiarCampos()
    {
        entryUsuario.Text = string.Empty;
        entryClave.Text = string.Empty;
        mostrarClave = false;
        entryClave.IsPassword = true;
        btnVerClave.Source = "ver.png";
    }
}
