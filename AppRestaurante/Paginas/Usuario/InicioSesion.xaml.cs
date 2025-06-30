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

        // Crear objeto con los datos del formulario
        var datos = new LoginRequest
        {
            username = entryUsuario.Text?.Trim(),
            password = entryClave.Text
        };

        // Validar campos
        if (string.IsNullOrWhiteSpace(datos.username) || string.IsNullOrWhiteSpace(datos.password))
        {
            await DisplayAlert("Error", "Debe ingresar usuario y contraseña", "OK");
            LimpiarCampos();
            return;
        }

        // Llamar al servicio
        var loginInfo = await _authService.LoginAsync(datos);

        if (loginInfo != null)
        {
            // Mostrar mensaje de bienvenida
            await DisplayAlert("Bienvenido", $"Hola {loginInfo.first_name}", "OK");

            // Guardar datos persistentes
            Preferences.Set("accessToken", loginInfo.access);
            Preferences.Set("usuario", loginInfo.username);
            Preferences.Set("rol", loginInfo.rol);

            // Mostrar el rol detectado (opcional)
            await DisplayAlert("Rol detectado", $"Rol: {loginInfo.rol}", "OK");

            // Redirigir según el rol
            await RedirigirPorRol(loginInfo.rol);

            LimpiarCampos();
        }
        else
        {
            await DisplayAlert("Error", "Credenciales incorrectas", "OK");
            LimpiarCampos();
        }

    }
    private void btnVerClave_Clicked(object sender, EventArgs e)
    {
        mostrarClave = !mostrarClave;
        entryClave.IsPassword = !mostrarClave;

        btnVerClave.Source = mostrarClave ? "ocultar.png" : "ver.png";
    }


    private async Task RedirigirPorRol(string rol)
    {
        switch (rol.ToLower())
        {
            case "administrador":
                await Shell.Current.GoToAsync("//AdministradorPage");
                break;
            case "mesero":
                await Shell.Current.GoToAsync("//MeseroPage");
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